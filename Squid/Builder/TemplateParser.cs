using System;
using System.Text;
//+
namespace Squid.Builder
{
    internal static class TemplateParser
    {
        //- ~MatchTemplateToken -//
        static private String MatchTemplateToken(String token, Int32 id, String title, String description)
        {
            String lowerToken = token.ToLower();
            String lowerId = id.ToString().ToLower();
            String lowerTitle = title.ToLower();
            String lowerDescription = description.ToLower();
            if (lowerToken == "id")
            {
                return lowerId;
            }
            else if (lowerToken == "title")
            {
                return title;
            }
            else if (lowerToken == "description")
            {
                return description;
            }
            return String.Empty;
        }

        //- ~ParseLink -//
        static internal String ParseLink(String template, Int32 id, String title, String description)
        {
            StringBuilder s = new StringBuilder();
            Int32 previous = 0;
            Int32 start = -1;
            Int32 end = -1;
            for (Int32 i = 0; i < template.Length; i++)
            {
                if (template[i] == '{')
                {
                    start = i;
                }
                if (start > -1 && template[i] == '}')
                {
                    s.Append(template.Substring(previous, start - previous));
                    end = i;
                    String templateCode = template.Substring(start + 1, end - start - 1);
                    String value = MatchTemplateToken(template.Substring(start + 1, end - start - 1), id, title, description);
                    if (!String.IsNullOrEmpty(value))
                    {
                        s.Append(value);
                    }
                    previous = end + 1;
                    start = -1;
                    end = -1;
                }
            }
            //+
            return s.ToString();
        }
    }
}