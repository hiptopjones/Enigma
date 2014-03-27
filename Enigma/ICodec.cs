using System;
namespace Enigma
{
    interface ICodec
    {
        string Decode(string input);
        string Encode(string input);
    }
}
