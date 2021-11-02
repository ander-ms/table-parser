using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class TestField
    {
        [TestCase("abc",0,"abc",3)]
        [TestCase("a c", 0, "a",1)]
        [TestCase("a  c", 0, "a", 1)]
        [TestCase("a \"c", 0, "a", 1)]
        [TestCase("\"a \"c", 0, "", 0)]
        [TestCase("a ", 0, "a", 1)]
        [TestCase(" a", 0, "a", 2)]
        [TestCase("  a", 0, "a", 3)]
        public void TestFieldTests(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = Field.ReadField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class Field
    {
        public static Token ReadField(string line, int startIndex)
        {
            if (line.Length < startIndex) return new Token("", startIndex, 0);
            int fieldLength = 0;
            while (line.Length>fieldLength+startIndex && line[fieldLength+startIndex]==' ') fieldLength++;
            for (int i = startIndex+fieldLength; i < line.Length; i++)
            {
                if (line[i] == '\'' || line[i] == '\"'||line[i]==' ') break;
                fieldLength++;
            }
            var result = line.Substring(startIndex, fieldLength).Trim();
            return new Token(result,startIndex, fieldLength);

        }
    }
}
