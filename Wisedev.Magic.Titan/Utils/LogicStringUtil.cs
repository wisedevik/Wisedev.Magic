using System.Text;

namespace Wisedev.Magic.Titam.Utils;

public static class LogicStringUtil
{
    private const string STRING_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static byte[] GetBytes(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    public static string RandomString(int length)
    {
        char[] result = new char[length];
        Random rand = new Random();

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = STRING_CHARACTERS[rand.Next(STRING_CHARACTERS.Length)];
        }

        return new string(result);
    }
}
