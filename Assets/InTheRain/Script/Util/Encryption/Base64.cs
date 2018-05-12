using System.Text;

namespace VNFramework
{
    public class Base64
    {
        static public string Encode(byte[] inData)
        {
            return System.Convert.ToBase64String(inData);
        }

        static public byte[] Decode(string inData)
        {
            return System.Convert.FromBase64String(inData);
        }

        static public string EncodeString(string toEncode)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(toEncode);
            return System.Convert.ToBase64String(bytesToEncode);
        }

        static public string DecodeString(string toEncode)
        {
            byte[] decodeBytes = System.Convert.FromBase64String(toEncode);
            return Encoding.UTF8.GetString(decodeBytes);
        }
    }
}
