using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace HelseId.Core.BFF.Sample.Api.Extensions
{
    public static class StringExtensions
    {
        public static string FromUrlSafeBase64(this string s)
        {
            return s.Replace('-', '+').Replace('_', '/');
        }

        static Regex phoneNumberRegex = new Regex("^\\s*\\+?\\d[\\d\\s\\(\\)]*\\s*$");

        public static bool IsValidPhoneNumber(this string s)
        {
            return phoneNumberRegex.IsMatch(s);
        }

        public static bool IsValidEmailAddress(this string s)
        {
            try
            {
                var address = new MailAddress(s);
                return address.Address == s;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
