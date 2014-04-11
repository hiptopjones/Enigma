using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class RadixCipher : ICipher
    {
        private int _radix;
        private int _padding;

        public RadixCipher(int radix, int padding)
        {
            _radix = radix;
            _padding = padding;
        }

        public string Encode(string input)
        {
            // This creates zero-padded strings for each input character
            StringBuilder output = new StringBuilder(input.Length * _padding);

            foreach (char c in input)
            {
                output.Append(BaseConverter.ConvertBase10ToBase(c, _radix).PadLeft(_padding, '0'));
            }

            return output.ToString().Trim();
        }

        public string Decode(string input)
        {
            StringBuilder output = new StringBuilder(input.Length / _padding);

            // Break up into groups of _padding chars, and convert each one in turn
            StringBuilder group = new StringBuilder();
            int count = 0;
            foreach (char c in input)
            {
                group.Append(c);
                count++;

                if ((count % _padding) == 0)
                {
                    output.Append((char)BaseConverter.ConvertBaseToBase10(group.ToString(), _radix));
                    group.Clear();
                }
            }

            return output.ToString();
        }
    }
}
