using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace CMcG.CommonwealthBank.Logic
{
    public static class Extensions
    {
        public static string Replace(this string instance, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(instance, pattern, evaluator);
        }

        public static string RemoveAllCaps(this string instance)
        {
            if (instance.Length < 2)
                return instance.ToUpper();

            string converted = instance.Substring(0, 1).Trim();

            for (int i = 1; i < instance.Length; i++)
                converted += (instance[i - 1] != ' ') ?  instance[i].ToString().ToLower() : instance[i].ToString();

            return converted;
        }

        public static string ToVerboseString(this Exception ex)
        {
            var sb = new StringBuilder("Date and Time: ")
                           .AppendLine(DateTime.Now.ToString())
                           .AppendLine("------------------------");
            ToVerboseString(sb, ex);
            return sb.ToString();
        }

        static void ToVerboseString(StringBuilder sb, Exception ex, int level = 0)
        {
            var pad     = string.Empty.PadLeft(level * 3);
            var format  = pad + "{0}:\r\n" + pad + "{1}\r\n" + pad + "------------------------\r\n";

            sb.AppendFormat(format, "Message",        ex.Message ?? string.Empty)
              .AppendFormat(format, "Exception Type", ex.GetType().Name)
              .AppendFormat(format, "StackTrace",     ex.StackTrace != null ? ex.StackTrace.Replace("\r\n", "\r\n" + pad) : string.Empty)
              .AppendFormat(format, "Data",           ex.Data != null ? ex.Data.ToString() : string.Empty);

            if (ex.InnerException != null)
            {
                sb.Append(pad).AppendLine("Inner Exception:");
                ToVerboseString(sb, ex.InnerException, level + 1);
            }
        }

        public static string ToFormattedString(this TimeSpan ts)
        {
            int days = (int)ts.TotalDays;
            var str  = string.Empty;

            if (days > 0)
                str += days + (days == 1 ? " day, " : " days, ");

            if (ts.Hours > 0 || days > 0)
                str += ts.Hours + (ts.Hours == 1 ? " hour, " : " hours, ");

            str += ts.Minutes + (ts.Minutes == 1 ? " minute" : " minutes");

            return str;
        }

        public static string GetFriendlyName(this MemberInfo instance)
        {
            var desc = instance.GetAttribute<DescriptionAttribute>();
            if (desc != null)
                return desc.Description;

            var name = instance.Name;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && !char.IsLower(name[i]) && char.IsLower(name[i - 1]))
                    result.Append(' ');

                result.Append(name[i]);
            }
            return result.ToString();
        }

        public static TAttribute GetAttribute<TAttribute>(this MemberInfo instance, bool inherit = false)
            where TAttribute : Attribute
        {
            return (TAttribute)instance.GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault();
        }
    }
}
