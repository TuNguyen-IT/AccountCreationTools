using AutomationTesting.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomationTesting.Extensions
{
    public class StringGenerators
    {
        public static string GenerateRandomName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }

        public static string GenerateRandomEmail(int length, string domain = "gmail")
        {
            return string.Format("{0}@{1}.com", GenerateRandomAlphabetString(length), domain);
        }

        /// <summary>
        /// Gets a string from the English alphabet at random
        /// </summary>
        public static string GenerateRandomAlphabetString(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyz";
            var rnd = new Random(Guid.NewGuid().GetHashCode());

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rnd.Next(allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
