using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this String str) {

            if (String.IsNullOrEmpty(str)) {
                throw new ArgumentException($"{nameof(str)} cannot be null.");
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(Char.ToUpper(str[0]));
            for (int i = 1; i < str.Length; i++) {
                builder.Append(Char.ToLower(str[i]));
            }
            return builder.ToString().Trim();
        }
    }
}
