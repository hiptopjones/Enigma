using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class GroupingCipher : ICipher
    {
        private int _groupSize;
        private Random _random;

        public GroupingCipher(int groupSize)
        {
            _groupSize = groupSize;
            _random = new Random();
        }

        public string Encode(string input)
        {
            // If short of making all full groups, pad with random lettters
            int lastGroupSize = (input.Length % _groupSize);
            if (lastGroupSize > 0)
            {
                input += GetRandomLetters(_groupSize - lastGroupSize);
            }

            // Maximum size is input * 2 for groups of size 1
            StringBuilder output = new StringBuilder(input.Length * 2);

            // Break the input up into groups of _groupSize
            int count = 0;
            foreach (char c in input)
            {
                output.Append(c);
                count++;

                if ((count % _groupSize) == 0)
                {
                    output.Append(' ');
                }
            }

            return output.ToString().Trim();
        }

        public string Decode(string input)
        {
            // No full reconstruction is possible, just remove spaces
            StringBuilder output = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (c != ' ')
                {
                    output.Append(c);
                }
            }

            return output.ToString();
        }

        private string GetRandomLetters(int count)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                builder.Append((char)_random.Next('A', 'Z'));
            }

            return builder.ToString();
        }
    }
}
