using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Codecs
{
    public class RadixCodec : ICodec
    {
        private int _radix;
        private int _padding;

        public RadixCodec(int radix, int padding)
        {
            _radix = radix;
            _padding = padding;
        }

        public string Encode(string input)
        {
            // This creates zero-padded strings for each input character (separated by spaces)
            StringBuilder output = new StringBuilder(input.Length * (_padding + 1));

            foreach (char c in input)
            {
                output.Append(BaseConverter.ConvertBase10ToBase(c, _radix).PadLeft(_padding, '0'));
                output.Append(' ');
            }

            return output.ToString().Trim();
        }

        public string Decode(string input)
        {
            StringBuilder output = new StringBuilder(input.Length / _padding);

            string[] groups = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string g in groups)
            {
                output.Append((char)BaseConverter.ConvertBaseToBase10(g, _radix));
            }

            return output.ToString();
        }
    }
}
