using System.ComponentModel;

namespace ExhaustiveMatching.Analyzer
{
    /// <summary>
    /// Full names for types the analyzer uses the metadata for.
    /// </summary>
    /// <remarks>Types in the ExhaustiveMatching assembly are not referenced to prevent needing to
    /// distribute that assembly as part of the analyzer in addition to the actual dependencies.</remarks>
    internal static class TypeNames
    {
        public const string ExhaustiveMatchFailedException = "ExhaustiveMatching.ExhaustiveMatchFailedException";
        public const string ClosedAttribute                = "ExhaustiveMatching.ClosedAttribute";
        public const string InvalidEnumArgumentException   = "System.ComponentModel." + nameof(InvalidEnumArgumentException);
        public const string Nullable                       = "System.Nullable`1"; // i.e. `typeof(System.Nullable<>).FullName`
    }
}
