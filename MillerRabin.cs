using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    public static class MillerRabin
    {
        //Миллер-Рабин, true - вероятно простое, false - составное
        public static bool MRTest(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n % 2 == 0)
                return false;
            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                Random rnd = new Random();
                byte[] bytarr = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rnd.NextBytes(bytarr);
                    a = new BigInteger(bytarr);
                }
                while (a < 2 || a >= n - 2);
                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = MyModPow(a, t, n);
                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;
                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = MyModPow(x, 2, n);
                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;
                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            // вернуть "вероятно простое"
            return true;
        }

        public static BigInteger MyModPow(BigInteger Number, BigInteger Pow, BigInteger Mod)
        {
            BigInteger Result = 1;
            BigInteger Bit = Number % Mod;

            while (Pow > 0)
            {
                if ((Pow & 1) == 1)
                {
                    Result *= Bit;
                    Result %= Mod;
                }
                Bit *= Bit;
                Bit %= Mod;
                //битовый сдвиг, вправо - целочисленное деление на 2, влево - умножение на 2
                Pow >>= 1;
            }
            return Result;
        }

        public static BigInteger Log2n(BigInteger n)
        {
            if (n < 1)
            {
                return -1;
            }
            BigInteger count = 0;
            BigInteger num = 1;
            while (num <= n)
            {
                count++;
                num *= 2;
            }
            return count;
        }



        public static BigInteger getRandom(int length)
        {
            BigInteger min = minimum(length);
            BigInteger max = maximum(length);

            BigInteger num;
            Random random = new Random();
            BitArray bitarr = new BitArray(length);
            byte[] bytearr = ToBytes(bitarr).ToArray();
            do
            {
                random.NextBytes(bytearr);
                num = new BigInteger(bytearr.Concat(new byte[] { 0 }).ToArray());

            } while (num > max || num < min);
            return num;


            
        }
        public static IEnumerable<byte> ToBytes(this BitArray bits, bool MSB = false)
        {
            int bitCount = 7;
            int outByte = 0;

            foreach (bool bitValue in bits)
            {
                if (bitValue)
                    outByte |= MSB ? 1 << bitCount : 1 << (7 - bitCount);
                if (bitCount == 0)
                {
                    yield return (byte)outByte;
                    bitCount = 8;
                    outByte = 0;
                }
                bitCount--;
            }
            // Последний частично декодированный байт
            if (bitCount < 7)
                yield return (byte)outByte;
        }

        //minimum
        private static BigInteger minimum(int length)
        {
            BitArray b = new BitArray(length);
            b.SetAll(false);
            b.Set(length - 1, true);

            byte[] m = b.ToBytes().ToArray();
            BigInteger res = new BigInteger(m.Concat(new byte[] { 0 }).ToArray());
            return res;
        }

        //maximum
        private static BigInteger maximum(int length)
        {
            BitArray b = new BitArray(length);
            b.SetAll(true);
            byte[] m = b.ToBytes().ToArray();
            BigInteger res = new BigInteger(m.Concat(new byte[] { 0 }).ToArray());
            return res;
        }
    
    }
}
