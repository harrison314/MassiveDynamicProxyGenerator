using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    /// <summary>
    /// Class representes korean alphabet.
    /// </summary>
    internal class KoreanAlphabet
    {
        private static char[] alphabet;

        /// <summary>
        /// Gets the alphabet.
        /// </summary>
        /// <value>
        /// The alphabet.
        /// </value>
        public static char[] Alphabet
        {
            get
            {
                return alphabet;
            }
        }

        static KoreanAlphabet()
        {
            alphabet = string.Concat("ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ",
 "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ",
 "ㅣㅏㅓㅜㅗ",
 "ㅂ빕밥법븝붑봅",
 "ㅈ집잡접즙줍좁",
 "ㄷ딥답덥듭둡돕",
 "ㄱ깁갑겁급굽곱",
 "ㅅ십삽섭습숩솝",
 "ㅁ밉맙멉믑뭅몹",
 "ㄴ닙납넙늡눕놉",
 "ㅎ힙합헙흡훕홉",
 "ㄹ립랍럽릅룹롭").ToCharArray().Distinct().ToArray();
        }

        /// <summary>
        /// Generates the string.
        /// </summary>
        /// <param name="numberRepresentation">The number representation.</param>
        /// <returns>Number representation.</returns>
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
        /// <param name="randomSourse">The random sourse.</param>
        /// <param name="lenght">The lenght.</param>
        /// <returns>Text representation.</returns>
        public static string GenerateRandom(Random randomSourse, int lenght)
        {
            char[] array = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                int index = randomSourse.Next(0, alphabet.Length);
                array[i] = alphabet[index];
            }

            return new string(array);
        }
    }
}
