using ExhaustiveMatching.Analyzer.Semantics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExhaustiveMatching.Analyzer
{
    internal static class ExpressionAnalyzer
    {
        public static SwitchStatementKind SwitchStatementKindForThrown(SyntaxNodeAnalysisContext context, ExpressionSyntax thrownExpression)
        {
            var exceptionType = context.SemanticModel.GetTypeInfo(thrownExpression, context.CancellationToken).Type;
            if (exceptionType == null || exceptionType.TypeKind == TypeKind.Error)
                return new SwitchStatementKind(isExhaustive: false, throwsInvalidEnum: false);

            var isExhaustive = exceptionType.IsAConfiguredExhaustiveExceptionType(context);
            var isInvalidEnumArgumentException = exceptionType.IsInvalidEnumArgumentException();

            return new SwitchStatementKind(isExhaustive, isInvalidEnumArgumentException);
        }
    }
}
