using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        [TestCase ("a b",new[] { "a","b"})]
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }
        [TestCase("text", new[] { "text" })]
        [TestCase("\"\"", new[] { "" })]
        [TestCase("\"text\"", new[] { "text" })]
        [TestCase("\"text", new[] { "text" })]
        [TestCase("\'text\'", new[] { "text" })]
        [TestCase("\'text", new[] { "text" })]
        [TestCase("\'\"text\"\'", new[] { "\"text\"" })]
        [TestCase("\'\\\'text\\\'\'", new[] { "\'text\'" })]
        [TestCase("\"\'text\'\"", new[] { "\'text\'" })]
        [TestCase("\"a\" 'b'", new[] { "a", "b" })]
        [TestCase(@"'' ""bcd ef"" 'x y'", new[] { "", "bcd ef", "x y" })]
        [TestCase("\\", new[] { "\\" })]
        [TestCase("\"a \\\"c\\\"\"", new[] { "a \"c\"" })]
        [TestCase("\'a \\\'c\\\'\'", new[] { "a \'c\'" })]
        [TestCase("\'a \\\"c\\\"\'", new[] { "a \"c\"" })]
        [TestCase(@"""\'\'""", new[] { "\'\'" })]
        [TestCase("\\ ", new[] { "\\" })]
        [TestCase("'\\\\' ", new[] { "\\" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("\"hello\" \'world\'", new[] { "hello", "world" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase("a'bc", new[] { "a", "bc" })]
        [TestCase("'bc ", new[] { "bc " })]
        [TestCase("", new string[0])]
        [TestCase("'hello'  world", new[] { "hello", "world" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            
            Test(input, expectedOutput);
        }

        
    }

    public class FieldsParserTask
    {
        
        public static List<Token> ParseLine(string line)
        {
            int startIndex = 0;
            var result = new List<Token>(); // сокращенный синтаксис для инициализации коллекции.
            while (line.Length>startIndex) 
            {
                if (line[startIndex] == ' ') startIndex = SkipSpaces(line,startIndex);
                if (line.Length <= startIndex) break;
                var token = line[startIndex] == '\'' || line[startIndex] == '\"' ? ReadQuotedField(line, startIndex) : ReadField(line, startIndex);
                
                if (token.Length!=0) result.Add(token);
                startIndex = token.GetIndexNextToToken();
            }
            return result;
        }
        public static int SkipSpaces(string line, int startIndex)
        {
            int countOfSpaces = 0;
            while (line.Length > countOfSpaces + startIndex && line[countOfSpaces + startIndex] == ' ') countOfSpaces++;
            return startIndex + countOfSpaces;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            return Field.ReadField(line,startIndex);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}