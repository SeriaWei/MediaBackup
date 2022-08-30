using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MediaBackup.IO
{
    internal static class DateParser
    {
        private static Regex _dateRegex = new Regex(@"([1|2]\d{3})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])", RegexOptions.Compiled);
        public static DateTime? Parse(string date)
        {
            if (string.IsNullOrEmpty(date)) return null;

            var match = _dateRegex.Match(date);
            if (!match.Success) return null;

            try
            {
                return new DateTime(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
            }
            catch { return null; }
        }
    }
}
