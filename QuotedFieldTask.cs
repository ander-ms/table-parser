using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("\"a 'b' 'c' d\"", 0, "a 'b' 'c' d", 13)]
        [TestCase("'\"1\" \"2\" \"3\"'", 0, "\"1\" \"2\" \"3\"", 13)]
        [TestCase("abc \"def g h", 4, "def g h", 8)]
        [TestCase("\"a \\\"c\\\"\"", 0, "a \"c\"", 9)]
        [TestCase(@"'a\\'", 0, @"a\", 5)]
        [TestCase(@"'a\\dcf", 0, @"a\dcf", 7)]
        [TestCase(@"'", 0, @"", 1)]
        [TestCase(@"""a", 0, @"a", 2)]


        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }       
    }

    class QuotedFieldTask
    {
        //public static char [] GetValid
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var kindOfQuoteChar = line[startIndex];
            if (startIndex > line.Length - 1) return new Token("", startIndex, 0);
            if (line.Length == 1) return new Token("", startIndex, 1);
            if (line[startIndex + 1] == kindOfQuoteChar) return new Token("", startIndex, 2);
            if (line.Length == 2) return new Token(line[startIndex + 1].ToString(), startIndex, 2);


            var unparsedToken = new StringBuilder();
            var escapeSymbols = new char[] { '\\', '*', '+', '?', ',', '\'', '\"' };
            var strBuilder = new StringBuilder(line.Substring(startIndex + 1));
            //int tokenLenth = 1;
            int countOfRemovedChars = 1;//include first quote symbol
            int countOfParsedChars = 0;
            for (int i = 0; i < strBuilder.Length - 1; i++)
            {

                if (strBuilder[i] == kindOfQuoteChar)
                {
                    countOfRemovedChars++;
                    break;
                }
                if (strBuilder[i] == '\\' && escapeSymbols.Contains(strBuilder[i + 1]))
                {
                    //var escSymbol = strBuilder[i + 1];
                    strBuilder.Remove(i, 1);
                    countOfRemovedChars++;
                }
                countOfParsedChars++;
                if (i == strBuilder.Length - 2)
                    if (strBuilder[strBuilder.Length - 1] == kindOfQuoteChar)
                    {
                        countOfRemovedChars++;
                    }
                    else countOfParsedChars++;
            }            
            return new Token(strBuilder.ToString().Substring(0, countOfParsedChars), startIndex, countOfParsedChars + countOfRemovedChars);
        }
    }
}
