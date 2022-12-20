using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerProject.Library.Extractor
{
    internal static class CustomStringMethod
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
            txt = txt.Trim();
            string result = String.Empty;
            bool isstartoftext = true;
            foreach(var letter in txt)
            {
                if (!Char.IsLetterOrDigit(letter) || isstartoftext == true)
                    continue;
                isstartoftext = false;
                result = result + letter;               
            }

            return result;
        }

        public static string TrimNumberStart(this string txt)
        {
            txt = txt.Trim();
            string result = String.Empty;
            bool isstartoftext = true;
            foreach (var letter in txt)
            {
                if (Char.IsDigit(letter) || isstartoftext == true)
                    continue;
                isstartoftext = false;
                result = result + letter;
            }

            return result;
        }

        public static string GetStartNumber(this string txt)
        {
            txt = txt.Trim();
            string result = String.Empty;
            foreach (var letter in txt)
            {
                if (Char.IsDigit(letter))
                {
                    result = result + letter;
                    continue;
                }
                break;
            }

            return result;
        }
    }
}
