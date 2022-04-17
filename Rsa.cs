using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSA
{
    public class Rsa
    {
        private int length=512;
        public BigInteger n;
        public BigInteger e;
        public BigInteger f;
        public BigInteger d;
        private string alphabet = "~0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ ,.-!?()\n";
        

        public Rsa() { }


        public Rsa(BigInteger n, BigInteger e, BigInteger f, BigInteger d)
        {
            this.n = n;
            this.e = e;
            this.d = d;
            this.f = f;
        }
        public string RSAEncryptAlgorithm(string text)
        {
            byte[] textByteArray = TextConverter(text);
            BigInteger textBigInt = new BigInteger(textByteArray);
            textBigInt = /*MillerRabin.MyModPow(textBigInt, e, n);*/BigInteger.ModPow(textBigInt, e, n);
            byte[] byteArray = textBigInt.ToByteArray();
            string textToReturn = Convert.ToBase64String(byteArray);
            return textToReturn;
        }

        public string RSADecryptAlgorithm(string text)
        {
            byte[] wordByteArray = Convert.FromBase64String(text);
            BigInteger textBigInt = new BigInteger(wordByteArray);
            textBigInt = /*MillerRabin.MyModPow(textBigInt, d, n);*/BigInteger.ModPow(textBigInt, d, n);
            byte[] byteArray = textBigInt.ToByteArray();
            char[] textToReturn = byteArray.Select(i => alphabet[i]).ToArray();
            return new string(textToReturn);
        }

        private byte[] TextConverter(string text)
        {
            /*Regex.Replace(text, @"[^A-Za-z\s0-9А-Яа-яЁё]+", "");*/

            
            byte[] byteArray = text.Select(i => (byte)alphabet.IndexOf(i)).ToArray();

            return byteArray;

        }

        public void GenerateKeys(int length)
        {
            this.length = length;
            (BigInteger p,BigInteger q) = GeneratePQ();
            n = p * q;
            f = (p - 1) * (q - 1);
            e = GenerateE();
            d = GenerateD();
        }

        private (BigInteger p, BigInteger q) GeneratePQ()
        {
            BigInteger p, q;
            do
            {

                p = MillerRabin.getRandom(length);


            } while (!MillerRabin.MRTest(p, length));
            do
            {

                q = MillerRabin.getRandom(length);


            } while (!MillerRabin.MRTest(q, length));
            return (p, q);
        }
        private BigInteger GenerateE()
        {
            BigInteger e;
            int localLength = (length * 2) / 3;
            do
            {
                e = MillerRabin.getRandom(localLength);
            } while (BigInteger.GreatestCommonDivisor(f,e)!=1);
            return e;
        }

        private BigInteger GenerateD()
        {
            d=ExtendedGCD(e,f);


            if (d < 0)
                d = d + f;
            return d;

        }

        BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (b < a)
            {
                BigInteger t = a;
                a = b;
                b = t;
            }

            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            BigInteger gcd = ExtendedGCD(b % a, a, out x, out y);

            BigInteger newY = x;
            BigInteger newX = y - (b / a) * x;

            x = newX;
            y = newY;
            return gcd;
        }

        BigInteger ExtendedGCD(BigInteger a, BigInteger b)
        {
            List<BigInteger> wholeParts = new List<BigInteger>();
            while (a % b != 0)
            {
                wholeParts.Add(a / b);
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }

            BigInteger x = 0, y = 1;
            for (int i = 0; i < wholeParts.Count; i++)
            {
                BigInteger temp = x;
                x = y;
                y = temp - y * wholeParts[wholeParts.Count - i - 1];
            }
            return x;
        }

        /*private string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }*/
    }
}
