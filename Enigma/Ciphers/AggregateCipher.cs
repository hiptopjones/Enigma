using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class AggregateCipher : ICipher
    {
        private LinkedList<ICipher> _cipherChain;

        public AggregateCipher()
        {
            _cipherChain = new LinkedList<ICipher>();
        }

        public void AddCipher(ICipher cipher)
        {
            _cipherChain.AddLast(cipher);
        }

        public string Decode(string input)
        {
            string output = input;

            foreach (ICipher cipher in _cipherChain.Reverse())
            {
                output = cipher.Decode(input);
                input = output;
            }

            return output;
        }

        public string Encode(string input)
        {
            string output = input;

            foreach (ICipher cipher in _cipherChain)
            {
                output = cipher.Encode(input);
                input = output;
            }

            return output;
        }
    }
}
