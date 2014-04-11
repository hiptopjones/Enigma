using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class MapCipher : ICipher
    {
        // Parameters:
        //    char: character to encode/decode
        //    bool: true if this is an encode operation, false for decode
        // Returns:
        //    string: what the input character maps to
        private Func<char, bool, string> _mapper;

        public MapCipher(Func<char, bool, string> mapper)
        {
            _mapper = mapper;
        }

        public string Decode(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char c in input)
            {
                builder.Append(_mapper(c, false));
            }

            return builder.ToString();
        }

        public string Encode(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char c in input)
            {
                builder.Append(_mapper(c, true));
            }

            return builder.ToString();
        }
    }
}
