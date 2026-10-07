using System;
using System.Text;

namespace PassGen

    public class PasswordGenerator
    {
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DigitChars = "0123456789";
        private const string SpecialChars = "!@#$%^&*()_-+=[{]};:<>|./?";

        private readonly Random _random = new Random();

        public string Generate(int length, bool useLower, bool useUpper, bool useDigits, bool useSpecials)
        {
            StringBuilder charPool = new StringBuilder();

            if (useLower) charPool.Append(LowercaseChars);
            if (useUpper) charPool.Append(UppercaseChars);
            if (useDigits) charPool.Append(DigitChars);
            if (useSpecials) charPool.Append(SpecialChars);

            if (charPool.Length == 0)
            {
                throw new ArgumentException("Необходимо выбрать хотя бы один набор символов!");
            }

            StringBuilder password = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randomIndex = _random.Next(0, charPool.Length);
                password.Append(charPool[randomIndex]);
            }

            return password.ToString();
        }

        public int CheckStrength(string password, bool useLower, bool useUpper, bool useDigits, bool useSpecials)
        {
            if (string.IsNullOrEmpty(password)) return 0;

            int score = 0;
            if (useLower) score++;
            if (useUpper) score++;
            if (useDigits) score++;
            if (useSpecials) score++;

            if (password.Length < 8 && score > 1) score--;

            return score;
        }
    }
}
