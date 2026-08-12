#!/usr/bin/env python3
"""
Extract TMDb watch providers into a JSON format suitable for generating WatchProvider.cs.

Usage:
    python extract_watch_providers.py --api-key YOUR_API_KEY [--output providers.json]

The output JSON has the structure:
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
"""

import argparse
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

    # Step 6 – fallback
    if not member:
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
                 include_channels: bool) -> dict[str, Any]:
    output: dict[str, dict] = {}

    for pid, provider in sorted(providers.items()):
        name = provider["provider_name"]
        is_channel = is_channel_addon(name)

        if not include_channels and is_channel:
            continue

        group = detect_group(name) or "_ungrouped"

        if group not in output:
            output[group] = {}

        existing = set(output[group].keys())
        member = to_member_name(name, group if group != "_ungrouped" else "", existing)

        output[group][member] = {
            "id": pid,
            "name": name,
            "is_channel": is_channel,
            "display_priority": provider.get("display_priority", 9999),
        }

    return output


# ---------------------------------------------------------------------------
# CLI
# ---------------------------------------------------------------------------

def main() -> None:
    parser = argparse.ArgumentParser(
        description="Extract TMDb watch providers to JSON for WatchProvider.cs generation."
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

    output = build_output(all_providers, args.include_channels)

    grouped = sum(len(v) for k, v in output.items() if k != "_ungrouped")
    ungrouped = len(output.get("_ungrouped", {}))
    print(f"  {grouped} providers grouped into known brands, "
          f"{ungrouped} ungrouped", file=sys.stderr)

    result = json.dumps(output, indent=2, ensure_ascii=False)

    if args.output == "-":
        print(result)
    else:
        with open(args.output, "w", encoding="utf-8") as fh:
            fh.write(result)
        print(f"Written to {args.output}", file=sys.stderr)


if __name__ == "__main__":
    main()
