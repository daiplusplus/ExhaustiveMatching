namespace ExhaustiveMatching.Analyzer.Enums.Tests
{
    public static class CodeContext
    {
        public static string Basic(string args, string body)
        {
            const string context = @"using System; // DayOfWeek
using System.ComponentModel; // InvalidEnumArgumentException

class TestClass
{{
    void TestMethod({0})
    {{{1}
    }}
}}";
            return string.Format(context, args, body);
        }

        public static string CoinFlip(string args, string body)
        {
            const string context = @"using System; // DayOfWeek
using System.ComponentModel; // InvalidEnumArgumentException

class TestClass
{{
    void TestMethod({0})
    {{{1}
    }}
}}

enum CoinFlip {{ Heads, Tails }}";
            return string.Format(context, args, body);
        }
    }
}