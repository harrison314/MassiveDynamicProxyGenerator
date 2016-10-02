﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Utils
{
    internal static class RusianAplhabet
    {
        private static char[] alphabet;

        static RusianAplhabet()
        {
            string sourse = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            alphabet = sourse.ToCharArray();
        }

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
