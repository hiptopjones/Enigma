using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Ciphers
{
    class MorseCodeCipher : ICipher
    {
        static Dictionary<char, string> EncodeMap;
        static Dictionary<string, char> DecodeMap;

        static MorseCodeCipher()
        {
            EncodeMap = new Dictionary<char, string>
            {
                {'A', ".-"},
                {'B', "-..."},
                {'C', "-.-."},
                {'D', "-.."},
                {'E', "."},
                {'F', "..-."},
                {'G', "--."},
                {'H', "...."},
                {'I', ".."},
                {'J', ".---"},
                {'K', "-.-"},
                {'L', ".-.."},
                {'M', "--"},
                {'N', "-."},
                {'O', "---"},
                {'P', ".--."},
                {'Q', "--.-"},
                {'R', ".-."},
                {'S', "..."},
                {'T', "-"},
                {'U', "..-"},
                {'V', "...-"},
                {'W', ".--"},
                {'X', "-..-"},
                {'Y', "-.--"},
                {'Z', "--.."},
                {'1', ".----"},
                {'2', "..---"},
                {'3', "...--"},
                {'4', "....-"},
                {'5', "....."},
                {'6', "-...."},
                {'7', "--..."},
                {'8', "---.."},
                {'9', "----."},
                {'0', "-----"},
                {'.', ".-.-.-"},
                {',', "--..--"},
                {'?', "..--.."},
                {':', "---..."},
                {';', "-.-.-."},
                {'-', "-....-"},
                {'/', "-..-."},
                {'"', ".-..-."},
                {'\'', ".----."},
                {'(', "-.--."},
                {')', "-.--.-"},
                {'=', "--...-"},
                {'+', ".-.-."},
                {'$', "...-..-"},
                {'_', "..--.-"},
            };

            DecodeMap = new Dictionary<string,char>();
            foreach (var key in EncodeMap.Keys)
            {
                var value = EncodeMap[key];
                DecodeMap[value] = key;
            }
        }

        public MorseCodeCipher()
        {
        }

        public string Encode(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (char c in input)
            {
                string code;
                if (EncodeMap.TryGetValue(Char.ToUpper(c), out code))
                {
                    builder.Append(code);
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }

        public string Decode(string input)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string code in input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                char c;
                if (DecodeMap.TryGetValue(code, out c))
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}
