using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PizzaHAL
{
    public static class ExtensionMethods
    {
        public static String GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static void CenterText(String text)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}\n", text));
        }
        public static void CenterText(String text, String text2)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 4) + (text.Length / 2)) + "}" +
                "{1," + ((Console.WindowWidth / 2) + (text2.Length / 2)) + "}\n", text, text2));

            /* String space = new string(' ', (WINDOW_SPACE/2 - text.Length) / 2);
            Console.Write(space + text + space);
            space = new string(' ', (WINDOW_SPACE/2 - text2.Length) / 2);
            Console.Write(space + text2 + space + "\n"); */

        }
        public static void CenterTextNoNewLine(String text)
        {
            Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}", text));
        }

        public static String FirstCharToUpper(String text)
        {
            return text[0].ToString().ToUpper() + text.Substring(1);
        }
    }
}
