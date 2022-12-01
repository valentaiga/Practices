using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Practices.LeetCode;

public class KataTests
{
    [Theory]
    [InlineData("apples, pears\ngrapes\nbananas", "apples, pears # and bananas\ngrapes\nbananas !apples",
        new[] { "#", "!" })]
    [InlineData("a\nc\nd", "a #b\nc\nd $e f g", new[] { "#", "$" })]
    public void Codewars_StripComments_4kyu(string result, string text, string[] commentSymbols)
    {
        /*
         * Complete the solution so that it strips all text that follows any of a set of comment markers passed in.
         * Any whitespace at the end of the line should also be stripped out.
         */
        var symbols = commentSymbols.Select(char.Parse).ToArray();
        var sb = new StringBuilder();
        var buffer = new StringBuilder();
        var skip = false;

        var span = new ReadOnlySpan<char>(text.ToCharArray());
        foreach (var c in span)
        {
            if (symbols.Contains(c))
            {
                skip = true;
            }

            if (c == '\n')
            {
                sb.Append(buffer.ToString().TrimEnd());
                sb.Append(c);
                buffer.Clear();
                skip = false;
            }
            else if (!skip)
            {
                buffer.Append(c);
            }
        }

        if (buffer.Length > 0) sb.Append(buffer.ToString().TrimEnd());

        Assert.Equal(result, sb.ToString());
    }

    [Theory]
    [InlineData("aabb", new[] { "aabb", "abab", "abba", "baab", "baba", "bbaa" })]
    [InlineData("ab", new[] { "ab", "ba" })]
    [InlineData("a", new[] { "a" })]
    public void Codewars_SinglePermutations_4kyu(string s, string[] result)
    {
        /*
         * In this kata you have to create all permutations of a non empty input string and remove duplicates, if present.
         * This means, you have to shuffle all letters from the input in all possible orders.
         */
        var r = Perm(s).Distinct().ToList();
        Assert.All(r, x => Assert.Contains(x, result));
        Assert.Equal(result.Length, r.Count);

        IEnumerable<string> Perm(string text)
        {
            if (text.Length == 1)
            {
                yield return text;
                yield break;
            }

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var subText = text.Remove(i, 1);
                foreach (var temp in Perm(subText))
                {
                    yield return $"{c}{temp}";
                }
            }
        }

        IEnumerable<string> Permutations(string text)
        {
            if (text.Length == 1)
            {
                yield return text;
                yield break;
            }

            foreach (var buffer in text)
            {
                foreach (var value in Permutations(text[1..]))
                {
                    yield return $"{buffer}{value}";
                }
            }
        }
    }

    [Theory]
    [InlineData("HEY JUDE", ".... . -.--   .--- ..- -.. .")]
    [InlineData("SOS", "... --- ...")]
    [InlineData("SOS", "...---...")]
    public void Codewars_MorseCodeDecode_6kyu(string result, string morseCode)
    {
        /*
         * In this kata you have to write a simple Morse code decoder. 
         * While the Morse code is now mostly superseded by voice and digital data communication channels,
         *      it still has its use in some applications around the world.
         */

        Dictionary<string, string> code = new()
        {
            { ".-", "A" },
            { "-...", "B" },
            { "-.-.", "C" },
            { "-..", "D" },
            { ".", "E" },
            { "..-.", "F" },
            { "--.", "G" },
            { "....", "H" },
            { "..", "I" },
            { ".---", "J" },
            { "-.-", "K" },
            { ".-..", "L" },
            { "--", "M" },
            { "-.", "N" },
            { ".--.", "P" },
            { "---", "O" },
            { "--.-", "Q" },
            { ".-.", "R" },
            { "...", "S" },
            { "-", "T" },
            { "..-", "U" },
            { "...-", "V" },
            { ".--", "W" },
            { "-..-", "X" },
            { "-.--", "Y" },
            { "--..", "Z" },
            { ".----", "1" },
            { "..---", "2" },
            { "...--", "3" },
            { "....-", "4" },
            { ".....", "5" },
            { "-....", "6" },
            { "--...", "7" },
            { "---..", "8" },
            { "----.", "9" },
            { "-----", "0" },
            { "  ", " " },
            { "-.-.--", "!" },
            { ".-.-.-", "." },
            { "...---...", "SOS" },
        };

        var words = morseCode.Split("  ")
            .Select(x => x.Split(" ").Where(s => s != string.Empty).ToArray());

        var sb = new StringBuilder();
        foreach (var word in words)
        {
            var lastCharIsSpace = false;

            foreach (var str in word)
            {
                var c = code[str];
                if (lastCharIsSpace && c == " ")
                {
                    continue;
                }

                if (c == " ")
                    lastCharIsSpace = true;

                sb.Append(c);
            }

            if (word.Length > 0)
                sb.Append(" ");
        }

        Assert.Equal(result, sb.ToString().Trim());
    }

    [Theory]
    [InlineData(0, 10, 1000)]
    [InlineData(6, 4, 2)]
    [InlineData(9, 9, 7)]
    public void Codewars_GetLastDigit_5kyu(int result, BigInteger n1, BigInteger n2)
    {
        var num1 = (int)(n1 % 10);
        var res = -1;

        if (n2.IsZero) res = 1;
        else
            switch (num1)
            {
                case 0:
                    res = 0;
                    break;
                case 1:
                    res = 1;
                    break;
                case 2:
                {
                    var mod = (int)(n2 % 4); // 2^1=2 2^2=4 2^3=8 2^4=6 2^5=2 ---> b % 4 == 0
                    res = mod switch
                    {
                        0 => 6,
                        1 => 2,
                        2 => 4,
                        3 => 8
                    };
                    break;
                }
                case 3:
                {
                    var mod = (int)(n2 % 4); //3^1=3 3^2=9 3^3=7 3^4=1 3^5=3 ---> b % 4
                    res = mod switch
                    {
                        0 => 1,
                        1 => 3,
                        2 => 9,
                        3 => 7
                    };
                    break;
                }
                case 4:
                {
                    var mod = (int)(n2 % 2);
                    res = mod switch
                    {
                        0 => 6,
                        1 => 4
                    };
                    break;
                }
                case 5:
                    res = 5;
                    break;
                case 6:
                    res = 6;
                    break;
                case 7:
                {
                    var mod = (int)(n2 % 4);
                    res = mod switch
                    {
                        0 => 1,
                        1 => 7,
                        2 => 9,
                        3 => 3
                    };
                    break;
                }
                case 8:
                {
                    var mod = (int)(n2 % 4);
                    res = mod switch
                    {
                        0 => 6,
                        1 => 8,
                        2 => 4,
                        3 => 2
                    };
                    break;
                }
                case 9:
                    res = 9;
                    break;
            }

        Assert.Equal(result, res);
    }
}