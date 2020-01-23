using System;
using System.Linq;

namespace Expenses.UnitTests.TestExtensions
{
    public static class StringExtensions
    {
        private static Random random = new Random();

        public static string RandomString(this string value, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
