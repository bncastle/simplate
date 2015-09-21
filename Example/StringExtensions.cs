using System.Linq;
using System.Text;

namespace Template_parser
{
    static class StringExtensions
    {
        static string Caseify(string text, bool uppercaseFirstChar = false)
        {
            text = text.Trim();
            StringBuilder st = new StringBuilder(text);

            if (uppercaseFirstChar) st[0] = char.ToUpper(st[0]);
            else st[0] = char.ToLower(st[0]);

            for (int i = 1; i < st.Length; i++)
            {
                if (char.IsWhiteSpace(st[i]) && i + 1 < st.Length)
                {
                    st[i + 1] = char.ToUpper(st[i + 1]);
                    st.Remove(i, 1);
                }
            }
            return st.ToString();
        }

        public static string ToPascalCase(this string text)
        {
            return Caseify(text, true);
        }

        public static string ToCamelCase(this string text)
        {
            return Caseify(text, false);
        }

        public static string MakeCodeSafe(this string text)
        {
            text = text.Replace('-', '_');
            if (char.IsDigit(text[0])) return '_' + text;
            return text;
        }

        public static string ToDefineCase(this string text)
        {
            text = text.Trim();

            //Insert underscores where appropriate
            text = string.Concat(text.Select((c, i) =>
            {
                if (i > 0 && char.IsUpper(c) && char.IsLower(text[i - 1])) return "_" + c.ToString();
                else if (char.IsWhiteSpace(c)) return "_";
                else return c.ToString();
            }).ToArray());

            text = MakeCodeSafe(text);
            return text.ToUpper();
        }
    }
}
