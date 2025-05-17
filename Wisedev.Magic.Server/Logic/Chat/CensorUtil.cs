namespace Wisedev.Magic.Server.Logic.Chat;
 
public static class CensorUtil
{
    private static HashSet<string> _badWords;

    static CensorUtil()
    {
        LoadBadWords();
    }

    public static string FilterMessage(string message)
    {
        string[] array = message.Split(' ');

        for (int i = 0; i < array.Length; i++)
        {
            string str = array[i];
            string toLower = str.ToLower();

            if (_badWords.Contains(toLower))
                array[i] = "***";
        }

        return string.Join(" ", array);
    }

    public static bool HasBadWords(string message)
    {
        string[] array = message.Split(' ');
        for (int i = 0; i < array.Length; i++)
        {
            string str = array[i];
            string toLower = str.ToLower();
            if (_badWords.Contains(toLower))
                return true;
        }
        return false;
    }

    private static void LoadBadWords()
    {
        _badWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            string[] lines = File.ReadAllLines("assets/censor/bad_words.txt");
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _badWords.Add(line.Trim());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading bad words: {ex.Message}");
        }
    }
}
