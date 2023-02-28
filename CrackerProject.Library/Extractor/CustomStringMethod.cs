using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrackerProject.Library.Extractor
{
    public static class CustomStringMethod
    {
        public static bool IsStartWithNumber(this string txt)
        {
            txt = txt.Trim();
            return Char.IsDigit(txt[0]);
        }

        public static bool IsStartWithLetter(this string txt)
        {
            txt = txt.Trim();
            return Char.IsLetter(txt[0]);
        }

        public static bool IsStartWithWord(this string txt, string word)
        {
            txt= txt.Trim();
            if(txt.Substring(0, word.Length).Trim().ToUpper() == word.ToUpper())
            {
                return true;
            }
            return false;
        }

        public static string TrimSymbolStart(this string txt)
        {
            // Use a regular expression to match any non-letter or non-digit characters at the start of the string
            string pattern = @"^[^a-zA-Z0-9]+";
            return Regex.Replace(txt, pattern, "").TrimStart();
        }
        public static string TrimSymbol(this string txt)
        {
            // Use a regular expression to match any non - letter or non-digit characters at the start or end of the string
        string pattern = @"^[^a-zA-Z0-9]+|[^a-zA-Z0-9]+$";
            return Regex.Replace(txt, pattern, "");
        }

        public static string TrimNumberStart(this string txt)
        {
            // Use a regular expression to match any digit characters at the start of the string
            string pattern = @"^\d+";
            return Regex.Replace(txt, pattern, "").TrimStart();
        }

        public static string GetStartNumber(this string txt)
        {
            // Use a regular expression to match a number at the start of the string
            string pattern = @"^\d+";
            Match match = Regex.Match(txt, pattern);
            if (match.Success)
            {
                // Return the matched value as a string
                return match.Value;
            }
            else
            {
                // Return an empty string if no match was found
                return "";
            }
        }
    }
}
