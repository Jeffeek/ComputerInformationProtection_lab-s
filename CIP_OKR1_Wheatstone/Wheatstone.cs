﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CIP_OKR1_Wheatstone
{
    public class Wheatstone
    {
        private readonly string _firstKey;
        private readonly string _secondKey;
        private readonly char[,] _firstMap;
        private readonly char[,] _secondMap;
        private readonly string _alphabet;
        private readonly int _alphabetFactor;

        public Wheatstone(string firstKey, string secondKey, string alphabet = null)
        {
            _alphabet = alphabet ?? "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ,.";
            _firstKey = String.Concat(firstKey.Distinct());
            _secondKey = String.Concat(secondKey.Distinct());
            _alphabetFactor = (int)Math.Ceiling(Math.Sqrt(_alphabet.Length));
            _firstMap = new char[_alphabetFactor, _alphabetFactor];
            _secondMap = new char[_alphabetFactor, _alphabetFactor];

            var newAlphabet1 = _firstKey.Aggregate(_alphabet, (current, t) => current.Replace(t.ToString(), ""));
            var newAlphabet2 = _secondKey.Aggregate(_alphabet, (current, t) => current.Replace(t.ToString(), ""));

            newAlphabet1 = _firstKey + newAlphabet1;
            newAlphabet2 = _secondKey + newAlphabet2;

            var index = 0;
            for (var i = 0; i < _alphabetFactor; i++)
            {
                for (var j = 0; j < _alphabetFactor; j++)
                {
                    if (index >= newAlphabet1.Length) break;
                    _firstMap[i, j] = newAlphabet1[index];
                    index++;
                }

                if (index >= newAlphabet1.Length) break;
            }

            index = 0;
            for (var i = 0; i < _alphabetFactor; i++)
            {
                for (var j = 0; j < _alphabetFactor; j++)
                {
                    if (index >= newAlphabet2.Length) break;
                    _secondMap[i, j] = newAlphabet2[index];
                    index++;
                }

                if (index >= newAlphabet1.Length) break;
            }
        }

        public string Encrypt(string text)
        {
            text = text.ToLower();

            var encryptedResult = "";

            if (text.Length % 2 != 0)
                text += ' ';

            var length = text.Length / 2;
            var k = 0;
            var bigram = new char[length, 2];
            var kryptoBigram = new char[length, 2];

            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    bigram[i, j] = text[k];
                    k++;
                }
            }

            var step = 0;
            while (step < length)
            {
                var cortege1 = FindIndexes(bigram[step, 0], _firstMap);
                var cortege2 = FindIndexes(bigram[step, 1], _secondMap);

                if (cortege1.Item1 == cortege2.Item1)
                {
                    cortege1.Item2++;
                    cortege2.Item2++;
                    if (cortege1.Item2 == _alphabetFactor)
                        cortege1.Item2 = 0;
                    if (cortege2.Item2 == _alphabetFactor)
                        cortege2.Item2 = 0;
                }

                kryptoBigram[step, 0] = _firstMap[cortege1.Item1, cortege2.Item2];
                kryptoBigram[step, 1] = _secondMap[cortege2.Item1, cortege1.Item2];

                step++;
            }


            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    encryptedResult += kryptoBigram[i, j].ToString();
                }
            }

            return encryptedResult;
        }

        public string Decrypt(string text)
        {
            var recryptedResult = "";

            var length = text.Length / 2;
            var k = 0;

            var bigram = new char[length, 2];
            var cryptoBigram = new char[length, 2];

            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    bigram[i, j] = text[k];
                    k++;
                }
            }

            var step = 0;
            while (step < length)
            {
                var cortege1 = FindIndexes(bigram[step, 0], _firstMap);
                var cortege2 = FindIndexes(bigram[step, 1], _secondMap);

                if (cortege1.Item1 == cortege2.Item1)
                {
                    cortege1.Item2--;
                    cortege2.Item2--;
                    if (cortege1.Item2 == -1)
                        cortege1.Item2 = _alphabetFactor - 1;
                    if (cortege2.Item2 == -1)
                        cortege2.Item2 = _alphabetFactor - 1;
                }

                cryptoBigram[step, 0] = _firstMap[cortege1.Item1, cortege2.Item2];
                cryptoBigram[step, 1] = _secondMap[cortege2.Item1, cortege1.Item2];

                step++;
            }


            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    recryptedResult += cryptoBigram[i, j].ToString();
                }
            }


            return recryptedResult;
        }

        private (int, int) FindIndexes(char symbol, char[,] matrix)
        {
            for (var i = 0; i < _alphabetFactor; i++)
            {
                for (var j = 0; j < _alphabetFactor; j++)
                {
                    if (symbol == matrix[i, j])
                    {
                        return (i, j);
                    }
                }
            }

            throw new Exception();
        }
    }
}
