using System.IO;

namespace vzr_common
{
    public static class TextExtensions
    {
        public static string ReadFile(this string path)
        {
            return File.ReadAllText(path);
        }

        public static void Write(this string text, string path)
        {
            using (var file = new StreamWriter(path))
            {
                file.Write(text);
            }
        }
    }
}