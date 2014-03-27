using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    public class BaseConverter
    {
        public static int ConvertBaseToBase10(string sourceText, int sourceRadix)
        {
            int base10 = 0;

            if (sourceRadix == 10)
            {
                if (!Int32.TryParse(sourceText, out base10))
                {
                    throw new ArgumentException("Invalid digit specified for given base.");
                }
            }
            else
            {
                for (int i = 0; i < sourceText.Length; i++)
                {
                    int currentPositionalValue = (int)Math.Pow(sourceRadix, i);

                    int characterIndex = sourceText.Length - i - 1;
                    int digitValue = GetDigitValueFromChar(sourceText[characterIndex], sourceRadix);

                    base10 += digitValue * currentPositionalValue;
                }
            }

            return base10;
        }

        public static string ConvertBase10ToBase(int source, int targetRadix)
        {
            StringBuilder builder = new StringBuilder();

            if (source == 0)
            {
                return "0";
            }

            int currentPowerOfRadix = 0;
            int lastPowerOfRadix = -1;

            while (true)
            {
                currentPowerOfRadix = GetNearestPowerOfRadix(source, targetRadix);

                // Fill in any zeros between the last position and the new position
                if (lastPowerOfRadix > currentPowerOfRadix)
                {
                    builder.Append(new string('0', lastPowerOfRadix - currentPowerOfRadix - 1));
                }

                // Figure out the digit value, which is how many times the current positional
                // value goes evenly into the source number
                int currentPositionalValue = (int)Math.Pow(targetRadix, currentPowerOfRadix);
                int digitValue = source / currentPositionalValue;

                // Add the new character to the target string
                builder.Append(GetCharFromDigitValue(digitValue, targetRadix));

                // Subtract the digit's positional value from the source
                source -= (int)(currentPositionalValue * digitValue);
                if (source == 0)
                {
                    if (currentPowerOfRadix > 0)
                    {
                        builder.Append(new string('0', currentPowerOfRadix));
                    }

                    break;
                }

                lastPowerOfRadix = currentPowerOfRadix;
            }

            return builder.ToString();
        }

        private static int GetDigitValueFromChar(char c, int radix)
        {
            int value = 0;

            if (Char.IsDigit(c))
            {
                value = (int)(c - '0');
            }
            else if (radix <= 36)
            {
                // + 10 to account for numeric digits
                value = (int)(Char.ToUpper(c) - 'A') + 10;
            }
            else
            {
                throw new ArgumentException("Unsupported base specified.");
            }

            if (value >= radix)
            {
                throw new ArgumentException("Invalid digit specified for given base.");
            }

            return value;
        }

        private static char GetCharFromDigitValue(int digitValue, int radix)
        {
            if (digitValue >= radix)
            {
                throw new ArgumentException("Invalid digit value specified for given base.");
            }

            if (digitValue < 10)
            {
                return (char)('0' + digitValue);
            }
            else if (radix <= 36)
            {
                // Must -10 to account for numeric digits
                return (char)('A' + digitValue - 10);
            }
            else
            {
                throw new ArgumentException("Unsupported base specified.");
            }
        }

        private static int GetNearestPowerOfRadix(int source, int radix)
        {
            int powerOfRadix = 0;

            while (true)
            {
                int test = (int)Math.Pow(radix, powerOfRadix);
                if (test > source)
                {
                    break;
                }

                powerOfRadix++;
            }

            return (powerOfRadix - 1);
        }

    }
}
