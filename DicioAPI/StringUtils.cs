using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DicioAPI
{
    internal static class StringUtils
    {
        public static string PrepareToSearch(this string word){           

            var normalizedString = word.ToLower().Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }            

            return Regex.Replace(stringBuilder.ToString().Normalize(NormalizationForm.FormC), "[^a-z]", "");
        }
    }
}