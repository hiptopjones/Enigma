using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class RotationCipher : ICipher
    {
        private int _letterRotation;

        public RotationCipher(int letterRotation)
        {
            _letterRotation = letterRotation;
        }

        public string Encode(string input)
        {
            return Rotate(input, _letterRotation);
        }

        public string Decode(string input)
        {
            return Rotate(input, (26 - _letterRotation));
        }

        private static string Rotate(string input, int letterRotationCount)
        {
            StringBuilder output = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (Char.IsLetter(c))
                {
                    char referenceLetter = Char.IsUpper(c) ? 'A' : 'a';
                    int letterIndex = c - referenceLetter;
                    int rotatedIndex = (letterIndex + letterRotationCount) % 26;
                    char rotatedCharacter = (char)(referenceLetter + rotatedIndex);
                    output.Append(rotatedCharacter);
                }
                else
                {
                    // Anything that's not a letter goes through unchanged
                    output.Append(c);
                }
            }

            return output.ToString();

        }
    }
}
