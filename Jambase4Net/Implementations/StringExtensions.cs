using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jambase4Net
{
    internal static class StringExtensions
    {
        private static readonly Regex REGEX = new Regex("[\n\r\t]");

        public static String EscapeWhiteSpace(this String str)
        {
            return REGEX.Replace(str, String.Empty);
        }
    }
}
