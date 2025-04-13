/*namespace Wisedev.Magic.Titan.Utils;

public class String : IDisposable
{
    private char[] m_chars;

    public String(char[] chars)
    {
        m_chars = chars ?? throw new ArgumentNullException(nameof(chars));
    }

    public String(string str)
    {
        if (str == null) throw new ArgumentNullException(nameof (str));

        m_chars = str.ToCharArray();
    }

    public int Length => m_chars.Length;

    public char this[int index]
    {
        get
        {
            return m_chars[index];
        }
        set
        {
            m_chars[index] = value;
        }
    }

    public int IndexOf(char ch)
    {
        for (int i = 0; i < m_chars.Length; i++)
        {
            if (m_chars[i] == ch)
                return i;
        }

        return -1;
    }

    public bool EqualsIgnoreCase(String s)
    {
        if (s == null || Length != s.Length)
            return false;

        for (int i = 0; i < Length; i++)
        {
            if (char.ToLower(m_chars[i]) != char.ToLower(s.m_chars[i]))
                return false;
        }
        return true;
    }

    public String Substring(int startIdx, int len)
    {
        if (startIdx < 0 || startIdx >= m_chars.Length
            || len < 0 || startIdx + len > m_chars.Length)
            throw new ArgumentOutOfRangeException();

        char[] substring = new char[len];
        Array.Copy(m_chars, startIdx, substring, 0 ,len);

        return new String(substring);
    }

    // TODO: rewrite ..
    public static String operator +(String a, String b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (b == null) throw new ArgumentNullException(nameof(b));

        char[] combinedChars = new char[a.Length + b.Length];
        Array.Copy(a.m_chars, combinedChars, a.Length);
        Array.Copy(b.m_chars, 0, combinedChars, a.Length, b.Length);

        return new String(combinedChars);
    }

    public override string ToString()
    {
        return new string(m_chars);
    }

    public bool EqualsIgnoreCase(object other)
    {
        if (other == null)
            return false;

       String otherMyString;

        if (other is string otherString)
        {
            otherMyString = new String(otherString);
        }
        else if (other is String myString)
        {
            otherMyString = myString;
        }
        else
        {
            return false;
        }

        if (Length != otherMyString.Length)
            return false;

        for (int i = 0; i < Length; i++)
        {
            if (char.ToLower(m_chars[i]) != char.ToLower(otherMyString.m_chars[i]))
                return false;
        }

        return true;
    }

    public void Dispose()
    {
        m_chars = null;
    }

    ~String()
    {
        Dispose();
    }
}
*/