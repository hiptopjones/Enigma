using System;
namespace Enigma
{
    interface ICipher
    {
        string Decode(string input);
        string Encode(string input);
    }
}
