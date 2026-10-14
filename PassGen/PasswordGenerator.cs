using System;
using System.Security.Cryptography;
using System.Text;

namespace PassGen
{
    public class PasswordGenerator
    {
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DigitChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()_-+=[{]};:<>|./?";

        public string Generate(int length, bool useLower, bool useUpper, bool useDigits, bool useSpecials)
        {
            StringBuilder charPool = new StringBuilder();
            if (useLower) charPool.Append(LowercaseChars);
            if (useUpper) charPool.Append(UppercaseChars);
            if (useDigits) charPool.Append(DigitChars);
            if (useSpecials) charPool.Append(SpecialChars);

            if (charPool.Length == 0)
                throw new ArgumentException("Необходимо выбрать хотя бы один набор символов!");

            StringBuilder password = new StringBuilder();

            // Используем криптографически стойкий генератор чисел взамен старого Random
            for (int i = 0; i < length; i++)
            {
                int randomIndex = RandomNumberGenerator.GetInt32(0, charPool.Length);
                password.Append(charPool[randomIndex]);
            }

            return password.ToString();
        }
        
        public int CheckStrength(string password, bool useLower, bool useUpper, bool useDigits, bool useSpecials)
        {
            if (string.IsNullOrEmpty(password)) return 0;

            // Расчет размера используемого алфавита (пула символов)
            int poolSize = 0;
            if (useLower) poolSize += LowercaseChars.Length;
            if (useUpper) poolSize += UppercaseChars.Length;
            if (useDigits) poolSize += DigitChars.Length;
            if (useSpecials) poolSize += SpecialChars.Length;

            if (poolSize == 0) return 0;

            // Расчет информационной энтропии пароля: E = L * log2(N)
            double entropy = password.Length * Math.Log2(poolSize);

            // Градации надежности на основе энтропии (стандартные требования ИБ)
            if (entropy < 40) return 1;      // Слабый пароль (брутфорсится мгновенно)
            if (entropy < 60) return 2;      // Средний пароль
            return 3;                        // Надежный пароль (устойчив к современным атакам)
        }
    }
}
