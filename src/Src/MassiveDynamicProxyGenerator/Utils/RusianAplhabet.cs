using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Class represented rusian alphabet.
    /// </summary>
    internal static class RusianAplhabet
    {
        private static char[] alphabet;

        static RusianAplhabet()
        {
            string sourse = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            alphabet = sourse.ToCharArray();
        }

        /// <summary>
        /// Generates the string.
        /// </summary>
        /// <param name="numberRepresentation">The number representation.</param>
        /// <returns>String representation of number.</returns>
        public static string GenerateString(int numberRepresentation)
        {
            int z = alphabet.Length;

            if (numberRepresentation == 0)
            {
                return new string(alphabet[0], 1);
            }

            List<char> characters = new List<char>();

            int number = numberRepresentation;
            if (numberRepresentation < 0)
            {
                characters.Add('ㅡ');
            }

            while (number > 0)
            {
                char newChar = alphabet[number % z];
                number = number / z;
                characters.Add(newChar);
            }

            characters.Reverse();

            return new string(characters.ToArray());
        }

        /// <summary>
        /// Generates the random.
        /// </summary>
        /// <param name="randomSource">The random source.</param>
        /// <param name="length">The length.</param>
        /// <returns>String representation of number.</returns>
        public static string GenerateRandom(Random randomSource, int length)
        {
            char[] array = new char[length];
            for (int i = 0; i < length; i++)
            {
                int index = randomSource.Next(0, alphabet.Length);
                array[i] = alphabet[index];
            }

            return new string(array);
        }
    }
}
