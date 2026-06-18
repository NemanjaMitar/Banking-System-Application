using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Model
{
    public enum Country
    {
        RS, // Serbia  — IBAN length 22, BBAN 18
        DE, // Germany — IBAN length 22, BBAN 18

    }



    public static class IbanGenerator
    {
        private static readonly Dictionary<Country, int> BbanLength =
            new Dictionary<Country, int>
            {
                { Country.RS, 18 },
                { Country.DE, 18 }
            };

        private static string BuildGermanBban(string bankCode, long accountNumber)
        {
            string bank = bankCode.PadLeft(8, '0');                      // 8-digit BLZ
            string acct = accountNumber.ToString().PadLeft(10, '0');     // ensure >= 10
            if (acct.Length > 10)
                acct = acct.Substring(acct.Length - 10);                 // keep last 10
            return bank + acct;                                          // 8 + 10 = 18
        }


        public static string Generate(Country country, string bankCode, long accountNumber)
        {
            string bban;
            switch (country)
            {
                case Country.RS:
                    bban = BuildSerbianBban(bankCode, accountNumber);
                    break;

                case Country.DE:
                    bban = BuildGermanBban(bankCode, accountNumber);
                    break;
                default:
                    throw new NotSupportedException(
                        "BBAN format for " + country + " not implemented.");
            }

            if (bban.Length != BbanLength[country])
                throw new ArgumentException(
                    "BBAN for " + country + " must be " + BbanLength[country] +
                    " digits, got " + bban.Length + ".");

            string checkDigits = ComputeCheckDigits(country.ToString(), bban);
            return country.ToString() + checkDigits + bban;
        }

        private static string BuildSerbianBban(string bankCode, long accountNumber)
        {
            string bank = bankCode.PadLeft(3, '0');
            string account = accountNumber.ToString().PadLeft(13, '0');
            string partial = bank + account;

            int control = 98 - Mod97(partial + "00");
            return partial + control.ToString().PadLeft(2, '0'); // 18 total
        }

        private static string ComputeCheckDigits(string countryCode, string bban)
        {
            string rearranged = bban + countryCode + "00";
            int remainder = Mod97(ToNumericString(rearranged));
            int check = 98 - remainder;
            return check.ToString().PadLeft(2, '0');
        }

        private static string ToNumericString(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input.ToUpperInvariant())
            {
                if (char.IsDigit(c))
                    sb.Append(c);
                else if (c >= 'A' && c <= 'Z')      // was: c is >= 'A' and <= 'Z'
                    sb.Append((c - 'A' + 10).ToString());
                else
                    throw new ArgumentException("Invalid IBAN character: '" + c + "'");
            }
            return sb.ToString();
        }

        private static int Mod97(string digits)
        {
            int remainder = 0;
            foreach (char c in digits)
                remainder = (remainder * 10 + (c - '0')) % 97;
            return remainder;
        }

        public static bool IsValid(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban)) return false;
            iban = iban.Replace(" ", "").ToUpperInvariant();
            if (iban.Length < 5) return false;

            string rearranged = iban.Substring(4) + iban.Substring(0, 4);
            return Mod97(ToNumericString(rearranged)) == 1;
        }
    }
}