using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger
{
    public class Base64Encoder
    {
        public static string DecodeString(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }

        public static string EncodeString(string plainString)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainString);
            string encodedString = Convert.ToBase64String(data);

            return encodedString;
        }
    }
}
