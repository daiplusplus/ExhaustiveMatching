using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;

using ExhaustiveMatching.Analyzer.Semantics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExhaustiveMatching.Analyzer
{
    internal static class AnalysisContextExtensions
    {
        private const string EM0001_TYPE_NAMES_CONFIG_KEY = "dotnet_diagnostic.EM0001.exception_type_names";

        internal const string INVALID_ENUM_ARGUMENT_EXCEPTION = nameof(System.ComponentModel.InvalidEnumArgumentException);

        private static readonly ImmutableHashSet<string> _defaultExceptionTypes = new[]
        {
            "ExhaustiveMatchFailedException",
            nameof(System.ComponentModel.InvalidEnumArgumentException),
            nameof(System.ArgumentOutOfRangeException)
        }
            .ToImmutableHashSet(StringComparer.Ordinal);

        private static readonly ConcurrentDictionary<string/*configValue*/,ImmutableHashSet<string>/*typeNames*/> _configuredExceptionTypes = new ConcurrentDictionary<string,ImmutableHashSet<string>>();

        public static ImmutableHashSet<string> GetConfiguredExhaustiveEnumExceptionTypeNames(this SyntaxNodeAnalysisContext context)
        {
            // https://www.mytechramblings.com/posts/configure-roslyn-analyzers-using-editorconfig/
            AnalyzerConfigOptions config = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Node.SyntaxTree);
            if (config.TryGetValue(EM0001_TYPE_NAMES_CONFIG_KEY, out string? typeNames) && !string.IsNullOrWhiteSpace(typeNames))
            {
                return _configuredExceptionTypes.GetOrAdd(typeNames,SplitTypeNamesFactory);
            }
            else
            {
                return _defaultExceptionTypes;
            }
        }

        private static readonly char[] _splitSeparators = new char[] { '|' };

        private static ImmutableHashSet<string> SplitTypeNamesFactory(string configValue)
        {
            // HACK: For now (because I'm unsure what syntax to use: e.g. C# vs. Type.FullName vs. XML-doc cref, I'll use pipe characters to separate type names:
            string[] typeNames = configValue.Split(_splitSeparators, StringSplitOptions.RemoveEmptyEntries);
            return typeNames.ToImmutableHashSet(StringComparer.Ordinal);
        }
    }

    internal static class ConfiguredTypeSymbolExtensions
    {
        public static bool IsAConfiguredExhaustiveExceptionType(this ITypeSymbol type, SyntaxNodeAnalysisContext context)
        {
            var configuredTypeNames = context.GetConfiguredExhaustiveEnumExceptionTypeNames();

            return
                configuredTypeNames.Contains(type.Name) ||
                configuredTypeNames.Contains(type.MetadataName) ||
                configuredTypeNames.Contains(type.GetFullName());
        }

        public static bool IsInvalidEnumArgumentException(this ITypeSymbol type)
        {
            return StringComparer.Ordinal.Equals(type.Name, AnalysisContextExtensions.INVALID_ENUM_ARGUMENT_EXCEPTION);
        }
    }
}
