using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;

class Example
{
    static void Main()
    {
        string asciiString = "This string contains the unicode character Pi (\u03a0)";

        // Create two different encodings.
        Encoding ascii = Encoding.ASCII;
        Encoding unicode = Encoding.Unicode;

        // Convert the string into a byte array.
        byte[] asciiBytes = ascii.GetBytes(asciiString);

        // Perform the conversion from one encoding to the other.
        byte[] unicodeBytes = Encoding.Convert(ascii, unicode, asciiBytes);

        // Convert the new byte[] into a char[] and then into a string.
        char[] asciiChars = new char[unicode.GetCharCount(asciiBytes, 0, unicodeBytes.Length)];
        unicode.GetChars(unicodeBytes, 0, unicodeBytes.Length, asciiChars, 0);
        string uniString = new string(asciiChars);

        // Display the strings created before and after the conversion.
        Console.WriteLine("Original string: {0}", asciiString);
        Console.WriteLine("Ascii converted string: {0}", uniString);
        Console.ReadLine();
    }
}