#!/usr/bin/env python3
"""
Extract TMDb watch providers, either as JSON or as the WatchProvider.cs source.

Usage:
    # inspect the grouping as JSON
    python extract_watch_providers.py --api-key YOUR_API_KEY [--output providers.json]

    # regenerate the C# constants (overwrites the file; do not hand-edit it)
    python extract_watch_providers.py --api-key YOUR_API_KEY --format cs \
        --output ../TMDbLib/Objects/Discover/WatchProvider.cs

The JSON output has the structure:
    {
      "GroupName": {
        "MemberName": { "id": 8, "name": "Netflix", "is_channel": false,
                        "display_priority": 0 }
      },
      ...
      "_ungrouped": {
        "SomeService123": { "id": 123, "name": "Some Obscure Service", "is_channel": false }
      }
    }

The "_ungrouped" key collects providers that did not match any known brand group.

With --format cs the previous output is read back first: any constant whose
provider ID has disappeared from the API is re-emitted as [Obsolete] rather than
deleted, so regenerating never breaks a caller.
"""

import argparse
import datetime
import json
import re
import sys
import unicodedata
import urllib.request
import urllib.error
from typing import Any

TMDB_API_BASE = "https://api.themoviedb.org/3"

# ---------------------------------------------------------------------------
# Channel-add-on suffix patterns
#
# When a provider name ends with one of these suffixes, we strip it before
# detecting the primary brand.  This ensures "BritBox Amazon Channel" is
# grouped under BritBox, not under Amazon.
# ---------------------------------------------------------------------------
_CHANNEL_SUFFIXES_RAW: list[str] = [
    r"\s+amazon channels?\b.*",
    r"\s+apple\s*tv\s+channels?\b.*",
    r"\s+roku\s+premium\s+channels?\b.*",
    # A few add-ons omit the word "Channel" ("Acorn TV Apple TV").  Require a
    # preceding word so the host platform's own entry ("Apple TV") is kept.
    r"(?<=\S)\s+apple\s*tv\s*$",
]
_COMPILED_SUFFIXES = [re.compile(p, re.IGNORECASE) for p in _CHANNEL_SUFFIXES_RAW]

# Patterns that flag a provider as a channel add-on (used only for the
# is_channel metadata field, after grouping has already occurred).
_CHANNEL_FLAG_PATTERNS = [re.compile(p, re.IGNORECASE) for p in [
    r"amazon channels?\b",
    r"apple\s*tv\s+channels?\b",
    r"roku\s+premium\s+channels?\b",
    r"(?<=\S)\s+apple\s*tv\s*$",
]]

# ---------------------------------------------------------------------------
# Brand grouping rules
#
# Each entry maps a canonical group name (→ C# class name) to a list of
# regex patterns matched against the provider name (case-insensitive).
# ORDER MATTERS: the first matching group wins.
# ---------------------------------------------------------------------------
BRAND_GROUPS: list[tuple[str, list[str]]] = [
    ("Netflix",          [r"\bnetflix\b"]),
    ("Disney",           [r"\bdisney"]),
    ("Max",              [r"\bhbo\s*max\b", r"^\s*max\s*$", r"^\s*max\s+\("]),
    ("Hulu",             [r"\bhulu\b"]),
    ("Apple",            [r"\bapple\s*tv\b"]),
    ("Paramount",        [r"\bparamount\b"]),
    ("Amazon",           [r"\bamazon\b", r"\bfreevee\b", r"\bprime\s*video\b"]),
    ("Google",           [r"\bgoogle\s*play\b", r"\byoutube\b"]),
    ("FandangoAtHome",   [r"\bfandango\s+at\s+home\b", r"\bvudu\b"]),
    ("Sky",              [r"\bsky\b", r"\bnow\s*tv\b", r"\bnow\s+cinema\b",
                          r"\bskyshowtime\b", r"^\s*wow\s*$"]),
    ("Peacock",          [r"\bpeacock\b"]),
    ("Crunchyroll",      [r"\bcrunchyroll\b"]),
    ("Tubi",             [r"\btubi\b"]),
    ("PlutoTV",          [r"\bpluto\s*tv\b", r"^\s*pluto\s*$"]),
    ("Starz",            [r"\bstarz"]),
    ("MGMPlus",          [r"\bmgm\b", r"\bmgm\+", r"\bepix\b"]),
    ("AMCPlus",          [r"\bamc\s*\+", r"\bamc\s*plus\b", r"\bamc\s*channels?\b"]),
    ("DiscoveryPlus",    [r"\bdiscovery\s*\+", r"\bdiscovery\s*plus\b"]),
    ("MUBI",             [r"\bmubi\b"]),
    ("CriterionChannel", [r"\bcriterion\b"]),
    ("Shudder",          [r"\bshudder\b"]),
    ("BritBox",          [r"\bbritbox\b"]),
    ("CuriosityStream",  [r"\bcuriosity\s*stream\b"]),
    ("RokuChannel",      [r"\bthe\s+roku\s+channel\b", r"\broku\s+channel\b"]),
    ("Plex",             [r"\bplex\b"]),
    ("Kanopy",           [r"\bkanopy\b"]),
    ("Hoopla",           [r"\bhoopla\b"]),
    ("AcornTV",          [r"\bacorn\s*tv\b"]),
    ("SundanceNow",      [r"\bsundance\s*now\b"]),
    ("Viaplay",          [r"\bviaplay\b"]),
    ("Crave",            [r"\bcrave\b"]),
    ("Stan",             [r"^\s*stan\s*$"]),
    ("BBCiPlayer",       [r"\bbbc\s+iplayer\b"]),
    ("ITVX",             [r"\bitvx\b"]),
    ("Channel4",         [r"\bchannel\s*4\b"]),
    ("CanalPlus",        [r"\bcanal\s*\+", r"\bcanal\s*plus\b"]),
    ("RTLPlus",          [r"\brtl\s*\+", r"\brtl\s*plus\b"]),
    ("Joyn",             [r"\bjoyn\b"]),
    ("RakutenTV",        [r"\brakuten\b"]),
    ("Hayu",             [r"\bhayu\b"]),
    ("HIDIVE",           [r"\bhidive\b"]),
    ("Zee5",             [r"\bzee\s*5\b"]),
    ("JioHotstar",       [r"\bjiohotstar\b", r"\bhotstar\b", r"\bjio\b"]),
    ("Philo",            [r"\bphilo\b"]),
    ("FuboTV",           [r"\bfubo"]),
    ("ESPN",             [r"\bespn\b"]),
    ("Showmax",          [r"\bshowmax\b"]),
    ("UNext",            [r"\bu-?next\b"]),
    ("SonyLiv",          [r"\bsony\s*liv\b"]),
    ("Globoplay",        [r"\bgloboplay\b"]),
    ("ViX",              [r"^\s*vix\s*$"]),
    ("IQIYI",            [r"\biqiyi\b"]),
]

_COMPILED_BRANDS: list[tuple[str, list[re.Pattern]]] = [
    (group, [re.compile(p, re.IGNORECASE) for p in patterns])
    for group, patterns in BRAND_GROUPS
]


def strip_channel_suffix(name: str) -> str:
    """Remove trailing 'Amazon Channel', 'Apple TV Channel', etc. from a name."""
    for pattern in _COMPILED_SUFFIXES:
        if not pattern.search(name):
            # Test the pattern, not the result: several TMDb names carry a
            # trailing space, and comparing the stripped result to the original
            # would treat that whitespace alone as a successful match.
            continue

        stripped = pattern.sub("", name).strip()
        if stripped:
            return stripped

    return name.strip()


def is_channel_addon(name: str) -> bool:
    return any(p.search(name) for p in _CHANNEL_FLAG_PATTERNS)


def detect_group(name: str) -> str | None:
    # For channel add-ons, strip the suffix first so the *primary* brand wins.
    base = strip_channel_suffix(name)

    for group, patterns in _COMPILED_BRANDS:
        if any(p.search(base) for p in patterns):
            return group

    # No fallback to the full name here: for a channel add-on the suffix always
    # names the *host* platform ("… Amazon Channel"), so matching against the
    # unstripped name would file every unknown brand under Amazon/Apple/Roku.
    # An unrecognised base brand belongs in "_ungrouped" instead.
    return None


# ---------------------------------------------------------------------------
# Member-name generation
# ---------------------------------------------------------------------------

# Words whose capitalisation we want to normalise in member names.
_WORD_MAP: dict[str, str] = {
    "tv": "TV",
    "hbo": "HBO",
    "hd": "HD",
    "bbc": "BBC",
    "itv": "ITV",
    "nbc": "NBC",
    "amc": "AMC",
    "mgm": "MGM",
    "rtl": "RTL",
    "espn": "ESPN",
    "ifc": "IFC",
    "tnt": "TNT",
}

# Noise tokens to drop from names when building a member identifier.
# Tokenisation splits on non-alphanumerics, so only alphanumeric tokens reach it.
_DROP_TOKENS: set[str] = {"the", "a", "an"}

# Substitutions applied (in order) on the *raw* provider name before
# tokenisation.  Each maps a regex → replacement string.
# More-specific phrases must come BEFORE less-specific ones (e.g. "Standard
# with Ads" before "with Ads") so the longer pattern matches first.
_NAME_SUBS: list[tuple[re.Pattern, str]] = [
    (re.compile(r"\bamazon\s+channels?\b", re.I), "AmazonChannel"),
    (re.compile(r"\bapple\s*tv\s+channels?\b", re.I), "AppleTVChannel"),
    (re.compile(r"\broku\s+premium\s+channels?\b", re.I), "RokuChannel"),
    (re.compile(r"\bstandard\s+with\s+ads\b", re.I), "StandardWithAds"),
    (re.compile(r"\bbasic\s+with\s+ads\b", re.I), "BasicWithAds"),
    (re.compile(r"\bfree\s+with\s+ads\b", re.I), "FreeWithAds"),
    (re.compile(r"\bwith\s+ads\b", re.I), "WithAds"),
    (re.compile(r"\bpremium\s+plus\b", re.I), "PremiumPlus"),
    # "A&E" → "AE": an ampersand between single letters is part of the brand,
    # not a separator, so glue the letters together before tokenisation.
    (re.compile(r"(?<=\b[A-Za-z])\s*&\s*(?=[A-Za-z]\b)"), ""),
    # "AMC+" → "AMCPlus". Must run last so the phrase rules above still see the
    # original spelling. Tokenisation would otherwise drop the '+' entirely and
    # "AMC+ Amazon Channel" could not be matched against the "AMCPlus" group.
    (re.compile(r"\+"), "Plus"),
]


def _normalise_word(token: str) -> str:
    low = token.lower()
    if low in _WORD_MAP:
        return _WORD_MAP[low]
    if low in _DROP_TOKENS:
        return ""
    # Preserve tokens that are already PascalCase/camelCase (produced by
    # substitutions above), so "AmazonChannel" is not mangled to "Amazonchannel".
    if any(c.isupper() for c in token[1:]):
        return token
    return token.capitalize()


def _squash(text: str) -> str:
    """Reduce a name to comparable form: lowercase, alphanumerics only."""
    return re.sub(r"[^a-z0-9]", "", text.lower())


def _strip_group_prefix(tokens: list[str], group_name: str) -> list[str]:
    """
    Drop the leading tokens that spell out the group name.

    Comparison happens on squashed forms, so the group name may be spread over
    several tokens ("MGM Plus" vs. group "MGMPlus") or be glued into one
    ("ParamountPlus" vs. group "Paramount", where only "Plus" is left over).
    """
    goal = _squash(group_name)
    if not goal or not tokens:
        return tokens

    acc = ""
    cut = 0
    remainder = None

    for index, token in enumerate(tokens):
        squashed = _squash(token)
        combined = acc + squashed

        if combined == goal:  # exact match wins over any partial one
            cut, remainder = index + 1, None
            break

        if goal.startswith(combined):  # partial: keep consuming tokens
            acc, cut = combined, index + 1
            continue

        if index == 0 and squashed.startswith(goal):
            # The group name is glued into the first token: keep the leftover.
            remainder, cut = token[len(goal):], 1

        break

    tokens = tokens[cut:]
    return [remainder, *tokens] if remainder else tokens


def to_member_name(provider_name: str, group_name: str,
                   existing: set[str]) -> str:
    """
    Derive a C#-style PascalCase identifier for a provider within its group.

    Strategy:
    1. Apply known string substitutions.
    2. Fold accents so "Pokémon" survives tokenisation as one word.
    3. Tokenise on non-alphanumeric characters.
    4. Strip the leading group name (group=Netflix, "Netflix Kids" → "Kids").
    5. Drop noise words, normalise case.
    6. If the result is empty or just the group prefix, use "Standard".
    7. De-duplicate with an "Alt" suffix when needed.
    """
    name = provider_name

    # Step 1 – apply substitutions
    for pattern, repl in _NAME_SUBS:
        name = pattern.sub(repl, name)

    # Step 2 – fold accents; tokenising would otherwise split "Pokémon"
    # into "Pok" + "mon" and yield "PokMon".
    name = "".join(
        c for c in unicodedata.normalize("NFKD", name)
        if not unicodedata.combining(c)
    )

    # Step 3 – tokenise
    tokens = re.split(r"[^a-zA-Z0-9]+", name)
    tokens = [t for t in tokens if t]

    # Step 4 – strip the group name
    tokens = _strip_group_prefix(tokens, group_name)

    # Step 5 – normalise
    parts = [_normalise_word(t) for t in tokens]
    parts = [p for p in parts if p]

    member = "".join(parts)

    # Step 6 – fallback.  Also catches names that survive step 4 still spelling
    # out the group ("The Roku Channel" keeps its noise word past the prefix
    # strip, then normalises back to "RokuChannel"): C# rejects a member sharing
    # its enclosing type's name (CS0542), and the repetition says nothing anyway.
    if not member or (group_name and _squash(member) == _squash(group_name)):
        member = "Standard"

    # Clean up: remove any remaining leading/trailing underscores or digits
    if member and member[0].isdigit():
        member = "Provider" + member

    # Step 7 – de-duplicate. TMDb ships several providers under one name
    # (two "Amazon Prime Video" entries, for instance); mark the later ones
    # as alternates rather than baking the volatile provider ID into the
    # identifier.
    if member in existing:
        base = member
        alternate = 1
        while member in existing:
            member = f"{base}Alt" if alternate == 1 else f"{base}Alt{alternate}"
            alternate += 1

    return member


# ---------------------------------------------------------------------------
# API helpers
# ---------------------------------------------------------------------------

def fetch_providers(api_key: str, media_type: str) -> list[dict]:
    url = f"{TMDB_API_BASE}/watch/providers/{media_type}?api_key={api_key}&language=en-US"
    try:
        with urllib.request.urlopen(url) as resp:
            data = json.loads(resp.read().decode())
            return data.get("results", [])
    except urllib.error.HTTPError as exc:
        print(f"HTTP {exc.code} fetching {media_type} providers: {exc.reason}", file=sys.stderr)
        sys.exit(1)
    except urllib.error.URLError as exc:
        print(f"Network error: {exc.reason}", file=sys.stderr)
        sys.exit(1)


def merge_providers(movie: list[dict], tv: list[dict]) -> dict[int, dict]:
    merged: dict[int, dict] = {}
    for provider in movie + tv:
        pid = provider["provider_id"]
        if pid not in merged:
            merged[pid] = provider
    return merged


# ---------------------------------------------------------------------------
# Output builder
# ---------------------------------------------------------------------------

def build_output(providers: dict[int, dict],
                 include_channels: bool,
                 reserved: dict[str, set[str]] | None = None) -> dict[str, Any]:
    """
    Group the live providers and assign each a C# member name.

    `reserved` maps a group to member names that must not be handed to a live
    provider — the names of retired providers we still emit as obsolete
    constants.  Reusing such a name for a different ID would silently repoint a
    published constant, so live providers yield and take an "Alt" suffix instead.
    """
    output: dict[str, dict] = {}
    reserved = reserved or {}

    for pid, provider in sorted(providers.items()):
        name = provider["provider_name"]
        is_channel = is_channel_addon(name)

        if not include_channels and is_channel:
            continue

        group = detect_group(name) or "_ungrouped"

        if group not in output:
            output[group] = {}

        existing = set(output[group].keys()) | reserved.get(group, set())
        member = to_member_name(name, group if group != "_ungrouped" else "", existing)

        output[group][member] = {
            "id": pid,
            "name": name,
            "is_channel": is_channel,
            "display_priority": provider.get("display_priority", 9999),
        }

    return output


# ---------------------------------------------------------------------------
# Previous-output parser
#
# TMDb silently drops providers from watch/providers.  Deleting the matching
# constants would break callers, so on every regeneration we read back the file
# we are about to overwrite: any constant whose ID is gone from the API is
# carried forward and marked [Obsolete].  Their member names and doc comments
# come from the previous file, because the API can no longer supply either.
# ---------------------------------------------------------------------------

_CS_CLASS_RE = re.compile(r"^\s{4}public static class (\w+)")
_CS_CONST_RE = re.compile(r"^\s{8}public const int (\w+) = (\d+);")
_CS_DOC_RE = re.compile(r"^\s{8}/// <summary>(.*)</summary>\s*$")
_CS_OBSOLETE_RE = re.compile(r"^\s+\[Obsolete\((.*)\)\]\s*$")


def parse_previous_cs(text: str) -> dict[str, list[dict]]:
    """
    Recover the {group: [{member, id, doc, obsolete}]} shape from generated C#.

    Only what we need to re-emit a retired constant verbatim is extracted; the
    parser is deliberately tied to this script's own output formatting.
    """
    groups: dict[str, list[dict]] = {}
    group = None
    doc = None
    obsolete = None

    for line in text.splitlines():
        match = _CS_CLASS_RE.match(line)
        if match:
            group = match.group(1)
            groups.setdefault(group, [])
            doc = obsolete = None
            continue

        match = _CS_DOC_RE.match(line)
        if match:
            doc = match.group(1)
            continue

        match = _CS_OBSOLETE_RE.match(line)
        if match:
            obsolete = match.group(1)
            continue

        match = _CS_CONST_RE.match(line)
        if match and group:
            groups[group].append({
                "member": match.group(1),
                "id": int(match.group(2)),
                "doc": doc,
                "obsolete": obsolete,
            })
            doc = obsolete = None

    return {g: members for g, members in groups.items() if members}


def collect_retired(previous: dict[str, list[dict]],
                    live_ids: set[int]) -> dict[str, list[dict]]:
    """Pick out the previously-emitted constants whose IDs the API has dropped."""
    retired: dict[str, list[dict]] = {}

    for group, members in previous.items():
        for entry in members:
            if entry["id"] in live_ids:
                continue
            retired.setdefault(group, []).append(entry)

    return retired


# ---------------------------------------------------------------------------
# C# emitter
#
# Renders the grouped output as TMDbLib/Objects/Discover/WatchProvider.cs.
# Everything here is derived from the API response and the BRAND_GROUPS table,
# so the file can be regenerated from scratch on every refresh; do not hand-edit
# the generated source — change this script instead.
# ---------------------------------------------------------------------------

CS_OBSOLETE_MESSAGE = ("No longer returned by TMDb's watch/providers endpoint. "
                       "Will be removed in a future version.")

CS_HEADER = """namespace TMDbLib.Objects.Discover;

/// <summary>
/// Watch provider IDs for use with Discover filtering. Availability varies by region; combine with <c>WhereWatchRegionIs()</c>.
/// </summary>
/// <remarks>
/// IDs represent base platform providers; channel variants (e.g. "Paramount+ Amazon Channel") have separate IDs. Last updated {date}.
/// </remarks>
public static class WatchProvider
{{
"""


def _xml_escape(text: str) -> str:
    """Escape the three characters that are not legal in XML doc-comment text."""
    return (text.replace("&", "&amp;")
                .replace("<", "&lt;")
                .replace(">", "&gt;"))


def _doc_summary(provider_name: str) -> str:
    """Render a provider name as a one-line XML doc summary."""
    name = _xml_escape(provider_name.strip())
    # TMDb names carry no trailing period of their own; add one for prose.
    return name if name.endswith(".") else name + "."


def _group_sort_key(group: str) -> tuple[str, str]:
    # Case-insensitive so the listing reads alphabetically to a human ("AcornTV"
    # before "AMCPlus"); the exact spelling breaks ties for a stable order.
    return group.lower(), group


def _merge_members(live: dict[str, dict],
                   retired: list[dict]) -> list[dict]:
    """
    Combine a group's live and retired members into one ID-ascending list.

    Retired members keep the member name, doc comment and [Obsolete] argument
    they were emitted with, so regenerating never renames a published constant.
    """
    members = [
        {
            "member": member,
            "id": data["id"],
            "doc": _doc_summary(data["name"]),
            "obsolete": None,
        }
        for member, data in live.items()
    ]

    for entry in retired:
        members.append({
            "member": entry["member"],
            "id": entry["id"],
            "doc": entry["doc"] or _doc_summary(entry["member"]),
            # Preserve an existing message verbatim so re-runs are idempotent.
            "obsolete": entry["obsolete"] or f'"{CS_OBSOLETE_MESSAGE}"',
        })

    return sorted(members, key=lambda m: m["id"])


def render_csharp(output: dict[str, Any], date: str,
                  retired: dict[str, list[dict]] | None = None) -> str:
    """Render the grouped provider output as the WatchProvider.cs source."""
    retired = retired or {}

    # "_ungrouped" is intentionally dropped: those providers matched no brand
    # group, so they have no class to live in.
    groups = (set(output) | set(retired)) - {"_ungrouped"}
    rendered = {
        group: _merge_members(output.get(group, {}), retired.get(group, []))
        for group in groups
    }
    rendered = {group: members for group, members in rendered.items() if members}

    body: list[str] = []
    for index, group in enumerate(sorted(rendered, key=_group_sort_key)):
        members = rendered[group]

        if index:
            body.append("\n")

        # A group whose every provider is gone is obsolete as a whole, so callers
        # get one warning on the class instead of one per constant.
        group_obsolete = all(m["obsolete"] for m in members)

        body.append(f"    /// <summary>\n"
                    f"    /// {group} provider IDs.\n"
                    f"    /// </summary>\n")
        if group_obsolete:
            body.append(f'    [Obsolete("{CS_OBSOLETE_MESSAGE}")]\n')
        body.append(f"    public static class {group}\n"
                    f"    {{\n")

        for member in members:
            body.append(f"        /// <summary>{member['doc']}</summary>\n")
            # Suppressed inside an already-obsolete class: the class-level
            # attribute covers every member, and CS0612 would fire on the All
            # initialiser below for referencing them.
            if member["obsolete"] and not group_obsolete:
                body.append(f"        [Obsolete({member['obsolete']})]\n")
            body.append(f"        public const int {member['member']} = {member['id']};\n\n")

        # Obsolete members stay out of All so iterating it never hits a dead ID
        # (and never trips CS0612 in this generated file).
        listed = [m["member"] for m in members
                  if group_obsolete or not m["obsolete"]]
        body.append(f"        /// <summary>All {group} provider IDs.</summary>\n"
                    f"        public static readonly int[] All = [{", ".join(listed)}];\n"
                    f"    }}\n")

    header = CS_HEADER.format(date=date)
    # System is only referenced by [Obsolete]; omit the using when nothing is.
    if "[Obsolete(" in "".join(body):
        header = "using System;\n\n" + header

    return header + "".join(body) + "}\n"


# ---------------------------------------------------------------------------
# CLI
# ---------------------------------------------------------------------------

def main() -> None:
    parser = argparse.ArgumentParser(
        description="Extract TMDb watch providers as JSON, or generate WatchProvider.cs."
    )
    parser.add_argument(
        "--api-key",
        required=True,
        metavar="KEY",
        help="Your TMDb API v3 key (https://www.themoviedb.org/settings/api)",
    )
    parser.add_argument(
        "--output",
        default="-",
        metavar="FILE",
        help="Output file path (default: stdout)",
    )
    parser.add_argument(
        "--format",
        choices=("json", "cs"),
        default="json",
        help="Output format: 'json' (default) or 'cs' for WatchProvider.cs source",
    )
    parser.add_argument(
        "--date",
        metavar="YYYY-MM-DD",
        help="Value for the 'Last updated' stamp in --format cs (default: today, UTC)",
    )
    parser.add_argument(
        "--previous",
        metavar="FILE",
        help="Existing WatchProvider.cs to read retired providers from "
             "(default: the --format cs output file, when it exists)",
    )
    parser.add_argument(
        "--no-previous",
        dest="use_previous",
        action="store_false",
        default=True,
        help="Do not carry retired providers forward from the previous output",
    )
    parser.add_argument(
        "--no-channels",
        dest="include_channels",
        action="store_false",
        default=True,
        help="Exclude channel add-on providers (e.g. 'Paramount+ Amazon Channel')",
    )
    args = parser.parse_args()

    print("Fetching movie providers…", file=sys.stderr)
    movie_providers = fetch_providers(args.api_key, "movie")
    print(f"  {len(movie_providers)} movie providers", file=sys.stderr)

    print("Fetching TV providers…", file=sys.stderr)
    tv_providers = fetch_providers(args.api_key, "tv")
    print(f"  {len(tv_providers)} TV providers", file=sys.stderr)

    all_providers = merge_providers(movie_providers, tv_providers)
    print(f"  {len(all_providers)} unique providers total", file=sys.stderr)

    # Read back the file we are about to overwrite, so providers TMDb has
    # dropped survive as [Obsolete] constants instead of vanishing.
    retired: dict[str, list[dict]] = {}
    if args.format == "cs" and args.use_previous:
        source = args.previous or (args.output if args.output != "-" else None)
        if source:
            try:
                with open(source, encoding="utf-8") as fh:
                    previous = parse_previous_cs(fh.read())
            except FileNotFoundError:
                print(f"  no previous output at {source}; "
                      f"emitting live providers only", file=sys.stderr)
            else:
                retired = collect_retired(previous, set(all_providers))
                count = sum(len(v) for v in retired.values())
                print(f"  {count} retired provider(s) carried forward from "
                      f"{source}", file=sys.stderr)

    reserved = {group: {e["member"] for e in entries}
                for group, entries in retired.items()}
    output = build_output(all_providers, args.include_channels, reserved)

    grouped = sum(len(v) for k, v in output.items() if k != "_ungrouped")
    ungrouped = len(output.get("_ungrouped", {}))
    print(f"  {grouped} providers grouped into known brands, "
          f"{ungrouped} ungrouped", file=sys.stderr)

    if args.format == "cs":
        stamp = args.date or datetime.datetime.now(datetime.UTC).strftime("%Y-%m-%d")
        result = render_csharp(output, stamp, retired)
    else:
        result = json.dumps(output, indent=2, ensure_ascii=False)

    if args.output == "-":
        sys.stdout.write(result if result.endswith("\n") else result + "\n")
    else:
        with open(args.output, "w", encoding="utf-8") as fh:
            fh.write(result)
        print(f"Written to {args.output}", file=sys.stderr)


if __name__ == "__main__":
    main()
