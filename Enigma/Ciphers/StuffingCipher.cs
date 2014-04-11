using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    // Called a stuffing cipher because it stuffs random letters inbetween the legitimate characters
    class StuffingCipher : ICipher
    {
        private int _padSize;
        private Random _random;

        public StuffingCipher(int padSize)
        {
            _padSize = padSize;
            _random = new Random();
        }

        public string Encode(string input)
        {
            // Maximum size is input * 2 for pad size of 1
            StringBuilder output = new StringBuilder(input.Length * (_padSize + 1) + 2);

            // Start with a random letter
            output.Append(GetRandomLetter());

            // Add characters with _padSize random letters between them
            foreach (char c in input)
            {
                // Deal only in letters and numbers, no punctuation or whitespace
                if (Char.IsLetterOrDigit(c))
                {
                    output.Append(Char.ToUpper(c));

                    for (int i = 0; i < _padSize; i++)
                    {
                        output.Append(GetRandomLetter());
                    }
                }
            }

            return output.ToString();
        }

        public string Decode(string input)
        {
            // No complete decode possible, since we lost spaces and punctuation
            StringBuilder output = new StringBuilder(input.Length);

            // The first character is garbage
            input = input.Substring(1);

            int count = 0;
            foreach (char c in input)
            {
                // Grab every (_padSize + 1)th character for the output string
                if ((count % (_padSize + 1)) == 0)
                {
                    output.Append(c);
                }

                count++;
            }

            return output.ToString();
        }

        private char GetRandomLetter()
        {
            return (char)_random.Next('A', 'Z');
        }
    }
}
