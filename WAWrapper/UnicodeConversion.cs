using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WAWrapper
{
    class UnicodeConversion
    {
        public static string Convert(string input)
        {
            var result = input;
            foreach (Match match in new Regex("\\\\:30..").Matches(input))
            {
                var temp = match.Value.Replace(':', 'u');
                temp = Decode(temp);
                result = result.Replace(match.Value, temp);
            }
            result = Decode(result);
            return result;
        }

        private static string Decode(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }
}
