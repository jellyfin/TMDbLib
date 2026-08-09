using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TMDbLib.SourceGenerators;

/// <summary>
/// Emits, for every enum marked with <c>[TolerantEnum]</c>:
/// <list type="bullet">
///   <item>a <c>GetDescription()</c> extension method returning the <c>[EnumValue]</c> string (or the member name), and</item>
///   <item>optionally a <c>JsonConverter&lt;T&gt;</c> that parses TMDb's string/number forms tolerantly.</item>
/// </list>
/// Both are plain switch statements over compile-time constants, so the library needs no
/// runtime reflection over enum fields or their attributes.
/// </summary>
[Generator]
public sealed class TolerantEnumGenerator : IIncrementalGenerator
{
    private const string MarkerAttributeName = "TMDbLib.Utilities.TolerantEnumAttribute";
    private const string ValueAttributeName = "TMDbLib.Utilities.EnumValueAttribute";
    private const string ConverterNamespace = "TMDbLib.Utilities.Converters";

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var models = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                MarkerAttributeName,
                static (node, _) => node is EnumDeclarationSyntax,
                static (ctx, _) => Describe(ctx))
            .Where(static m => m is not null)
            .Select(static (m, _) => m!);

        context.RegisterSourceOutput(models.Collect(), static (spc, all) => Emit(spc, all));
    }

    private static EnumModel? Describe(GeneratorAttributeSyntaxContext ctx)
    {
        if (ctx.TargetSymbol is not INamedTypeSymbol symbol || symbol.TypeKind != TypeKind.Enum)
        {
            return null;
        }

        // [TolerantEnum(GenerateJsonConverter = false)] opts out of the converter (for enums that
        // only ever appear in URLs, never in JSON).
        var generateConverter = true;
        foreach (var arg in ctx.Attributes[0].NamedArguments)
        {
            if (arg.Key == "GenerateJsonConverter" && arg.Value.Value is bool b)
            {
                generateConverter = b;
            }
        }

        var members = new List<EnumMemberModel>();
        foreach (var member in symbol.GetMembers().OfType<IFieldSymbol>())
        {
            if (!member.IsConst || member.ConstantValue is null)
            {
                continue;
            }

            string? enumValue = null;
            foreach (var attribute in member.GetAttributes())
            {
                if (attribute.AttributeClass?.ToDisplayString() != ValueAttributeName)
                {
                    continue;
                }

                // [EnumValue(null)] is used to mean "no mapping" - treat it as absent.
                if (attribute.ConstructorArguments.Length == 1)
                {
                    enumValue = attribute.ConstructorArguments[0].Value as string;
                }
            }

            members.Add(new EnumMemberModel(
                member.Name,
                enumValue,
                Convert.ToInt64(member.ConstantValue)));
        }

        if (members.Count == 0)
        {
            return null;
        }

        // Mirrors the old TolerantEnumConverter fallback: prefer a member called "Unknown",
        // otherwise the first declared member.
        var fallback = members.FirstOrDefault(m => string.Equals(m.Name, "Unknown", StringComparison.OrdinalIgnoreCase))
                       ?? members[0];

        var accessible = IsExternallyVisible(symbol);

        return new EnumModel(
            symbol.Name,
            symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            accessible,
            generateConverter,
            fallback.Name,
            members.ToImmutableArray());
    }

    private static bool IsExternallyVisible(ITypeSymbol symbol)
    {
        for (ISymbol? current = symbol; current is not null; current = current.ContainingType)
        {
            if (current.DeclaredAccessibility != Accessibility.Public)
            {
                return false;
            }
        }

        return true;
    }

    private static void Emit(SourceProductionContext context, ImmutableArray<EnumModel> models)
    {
        if (models.IsDefaultOrEmpty)
        {
            return;
        }

        var ordered = models.OrderBy(m => m.FullyQualifiedName, StringComparer.Ordinal).ToArray();

        EmitDescriptions(context, ordered);

        var withConverters = ordered.Where(m => m.GenerateJsonConverter).ToArray();

        foreach (var model in withConverters)
        {
            EmitConverter(context, model);
        }

        EmitRegistration(context, withConverters);
    }

    /// <summary>
    /// Emits the registration helper. The converters are attached through
    /// <c>JsonSerializerOptions.Converters</c> rather than <c>[JsonConverter]</c> on each enum,
    /// because System.Text.Json's own source generator runs in this same compilation and cannot
    /// see types produced by this one (it would report SYSLIB1220 and fall back to reflection).
    /// </summary>
    private static void EmitRegistration(SourceProductionContext context, IReadOnlyList<EnumModel> models)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("// Mapping every declared member necessarily names obsolete ones too.");
        sb.AppendLine("#pragma warning disable CS0612, CS0618");
        sb.AppendLine();
        sb.AppendLine("using System.Text.Json;");
        sb.AppendLine();
        sb.AppendLine($"namespace {ConverterNamespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Registers the source-generated tolerant enum converters.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class TmdbEnumConverters");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Adds a converter for every TMDb enum to the supplied options.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"options\">The options to add the converters to.</param>");
        sb.AppendLine("    public static void RegisterAll(JsonSerializerOptions options)");
        sb.AppendLine("    {");

        foreach (var model in models)
        {
            sb.AppendLine($"        options.Converters.Add(new {model.Name}JsonConverter());");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource("TmdbEnumConverters.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private static void EmitDescriptions(SourceProductionContext context, IReadOnlyList<EnumModel> models)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("// Mapping every declared member necessarily names obsolete ones too.");
        sb.AppendLine("#pragma warning disable CS0612, CS0618");
        sb.AppendLine();
        sb.AppendLine("namespace TMDbLib.Utilities;");
        sb.AppendLine();
        sb.AppendLine("public static partial class EnumExtensions");
        sb.AppendLine("{");

        foreach (var model in models)
        {
            var accessibility = model.IsExternallyVisible ? "public" : "internal";

            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// Gets the TMDb wire string for a <see cref=\"{XmlRef(model.FullyQualifiedName)}\"/> value.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    /// <param name=\"value\">The enum value.</param>");
            sb.AppendLine("    /// <returns>The mapped string, or the member name when no mapping is declared.</returns>");
            sb.AppendLine($"    {accessibility} static string GetDescription(this {model.FullyQualifiedName} value)");
            sb.AppendLine("    {");
            sb.AppendLine("        return value switch");
            sb.AppendLine("        {");

            foreach (var member in DistinctByValue(model.Members).Where(m => m.EnumValue is not null))
            {
                sb.AppendLine($"            {model.FullyQualifiedName}.{member.Name} => {Literal(member.EnumValue!)},");
            }

            // Members without a mapping (and undeclared [Flags] combinations) keep the old
            // behaviour of falling back to ToString().
            sb.AppendLine("            _ => value.ToString(),");
            sb.AppendLine("        };");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("}");

        context.AddSource("EnumExtensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private static void EmitConverter(SourceProductionContext context, EnumModel model)
    {
        var converterName = model.Name + "JsonConverter";

        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("// Mapping every declared member necessarily names obsolete ones too.");
        sb.AppendLine("#pragma warning disable CS0612, CS0618");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Text.Json;");
        sb.AppendLine("using System.Text.Json.Serialization;");
        sb.AppendLine("using TMDbLib.Utilities;");
        sb.AppendLine();
        sb.AppendLine($"namespace {ConverterNamespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Converts <see cref=\"{XmlRef(model.FullyQualifiedName)}\"/> to and from JSON, tolerating");
        sb.AppendLine("/// unrecognised values the way TMDb's API requires.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"internal sealed class {converterName} : JsonConverter<{model.FullyQualifiedName}>");
        sb.AppendLine("{");

        // --- Read ---
        sb.AppendLine("    /// <inheritdoc />");
        sb.AppendLine($"    public override {model.FullyQualifiedName} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (reader.TokenType == JsonTokenType.String)");
        sb.AppendLine("        {");
        sb.AppendLine("            var text = reader.GetString();");
        sb.AppendLine("            if (!string.IsNullOrEmpty(text))");
        sb.AppendLine("            {");

        // 1. Declared [EnumValue] strings win, in declaration order.
        foreach (var member in model.Members.Where(m => m.EnumValue is not null))
        {
            sb.AppendLine($"                if (string.Equals(text, {Literal(member.EnumValue!)}, StringComparison.OrdinalIgnoreCase))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    return {model.FullyQualifiedName}.{member.Name};");
            sb.AppendLine("                }");
            sb.AppendLine();
        }

        // 2. Then member names, as Enum.TryParse(ignoreCase: true) used to do.
        foreach (var member in model.Members)
        {
            sb.AppendLine($"                if (string.Equals(text, {Literal(member.Name)}, StringComparison.OrdinalIgnoreCase))");
            sb.AppendLine("                {");
            sb.AppendLine($"                    return {model.FullyQualifiedName}.{member.Name};");
            sb.AppendLine("                }");
            sb.AppendLine();
        }

        // 3. Then a numeric string, which Enum.TryParse also accepted.
        sb.AppendLine("                if (int.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var parsedText)");
        sb.AppendLine("                    && TryFromNumber(parsedText, out var fromText))");
        sb.AppendLine("                {");
        sb.AppendLine("                    return fromText;");
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine("        else if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var number))");
        sb.AppendLine("        {");
        sb.AppendLine("            if (TryFromNumber(number, out var fromNumber))");
        sb.AppendLine("            {");
        sb.AppendLine("                return fromNumber;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine($"        return {model.FullyQualifiedName}.{model.FallbackMemberName};");
        sb.AppendLine("    }");
        sb.AppendLine();

        // --- Write ---
        sb.AppendLine("    /// <inheritdoc />");
        sb.AppendLine($"    public override void Write(Utf8JsonWriter writer, {model.FullyQualifiedName} value, JsonSerializerOptions options)");
        sb.AppendLine("    {");
        sb.AppendLine("        writer.WriteStringValue(value.GetDescription());");
        sb.AppendLine("    }");
        sb.AppendLine();

        // --- Defined-value check, replacing Enum.IsDefined ---
        sb.AppendLine($"    private static bool TryFromNumber(int number, out {model.FullyQualifiedName} value)");
        sb.AppendLine("    {");
        sb.AppendLine("        switch (number)");
        sb.AppendLine("        {");

        foreach (var member in DistinctByValue(model.Members))
        {
            sb.AppendLine($"            case {member.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}:");
            sb.AppendLine($"                value = {model.FullyQualifiedName}.{member.Name};");
            sb.AppendLine("                return true;");
        }

        sb.AppendLine("            default:");
        sb.AppendLine("                value = default;");
        sb.AppendLine("                return false;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource($"{converterName}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    /// <summary>
    /// Enum members may share a constant value (aliases); switch labels may not. The last
    /// declaration wins, matching the dictionary the old reflection-based cache built.
    /// </summary>
    private static IEnumerable<EnumMemberModel> DistinctByValue(ImmutableArray<EnumMemberModel> members)
    {
        var seen = new Dictionary<long, EnumMemberModel>();
        foreach (var member in members)
        {
            seen[member.Value] = member;
        }

        return members.Where(m => ReferenceEquals(seen[m.Value], m));
    }

    private static string Literal(string value)
    {
        return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }

    private static string XmlRef(string fullyQualifiedName)
    {
        return fullyQualifiedName.StartsWith("global::", StringComparison.Ordinal)
            ? fullyQualifiedName.Substring("global::".Length)
            : fullyQualifiedName;
    }

    private sealed record EnumMemberModel(string Name, string? EnumValue, long Value);

    private sealed record EnumModel(
        string Name,
        string FullyQualifiedName,
        bool IsExternallyVisible,
        bool GenerateJsonConverter,
        string FallbackMemberName,
        ImmutableArray<EnumMemberModel> Members);
}
