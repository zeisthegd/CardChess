using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Tools
{
    public class Randomizer
    {
        static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string numbers = "0123456789";

        public void GetRandomLetterOnlyString(int length)
        {
            RandomString(length, true, false);
        }

        public void GetRandomNumberOnlyString(int length)
        {
            RandomString(length, false);
        }

        public static string RandomString(int length, bool includeLetter = true, bool includeNumber = true)
        {
            string str = "";
            while (str.Length < length)
            {
                if (includeLetter)
                    str += letters[Random.Range(0, letters.Length)];
                if (includeNumber)
                    str += numbers[Random.Range(0, numbers.Length)];
            }
            return str;
        }

        /// <summary>
        /// Get a random int number Min inclusive, Max exclusive
        /// </summary>
        /// <param name="min">Min inclusive</param>
        /// <param name="max">Max exclusive</param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Return either the first or the second number.
        /// </summary>
        public static int RandomBetween(int numA, int numB)
        {
            return Random.Range(0, 2) == 0 ? numA : numB;
        }

        public static int RandomNumber(IntMinMax intMinMax)
        {
            return Random.Range(intMinMax.Min, intMinMax.Max);
        }

        /// <summary>
        /// Get a random float number Min inclusive, Max inclusive
        /// </summary>
        /// <param name="min">Min inclusive</param>
        /// <param name="max">Max inclusive</param>
        /// <returns></returns>
        public static float RandomNumber(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static float RandomNumber(FloatMinMax floatMinMax)
        {
            return Random.Range(floatMinMax.Min, floatMinMax.Max);
        }
    }
}

