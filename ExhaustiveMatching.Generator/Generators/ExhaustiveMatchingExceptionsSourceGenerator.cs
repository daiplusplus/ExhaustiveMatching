using System;
using System.IO;
using System.Text;
using System.Reflection;

using Microsoft.CodeAnalysis;

namespace ExhaustiveMatching.Analyzer.Enums
{
    internal static class EmbeddedSources
    {
        public static readonly string AttributesAndExceptions = RequireResource(logicalName: "AttributesAndExceptionsCSFile"); // The `LogicalName=""` is defined in $\ExhaustiveMatching.Analyzer.Enums\ExhaustiveMatching.Analyzer.Enums.csproj

        private static string RequireResource(string logicalName)
        {
            using Stream manifestResourceStream = typeof( EmbeddedSources ).Assembly.GetManifestResourceStream( logicalName );
            using StreamReader rdr = new StreamReader( manifestResourceStream, Encoding.UTF8 );

            return rdr.ReadToEnd();
        }
    }

    [Generator]
    public class ExhaustiveMatchingExceptionsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("ExhaustiveMatching.g.cs", EmbeddedSources.AttributesAndExceptions);
        }
    }
}
