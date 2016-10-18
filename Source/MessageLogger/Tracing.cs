using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger
{
    public class Tracing
    {
        public static string FormatException(Exception ex)
        {
            StringBuilder sb = new StringBuilder(1024);

            sb.AppendFormat("\nException: {0}. ", ex.Message);
            sb.AppendFormat("Callstack: {0}.\n", ex.StackTrace);

            ExplodeInnerException(ex, ref sb);

            return sb.ToString();
        }

        public static string FormatException(Exceptions.BaseException ex)
        {
            StringBuilder sb = new StringBuilder(1024);

            sb.AppendFormat("\nCode: {0}. Exception: {1}. Callstack: {2}.\n", ex.ErrorCode, ex.Message, ex.StackTrace);
            ExplodeInnerException(ex, ref sb);

            return sb.ToString();
        }

        private static void ExplodeInnerException(Exception ex, ref StringBuilder sb)
        {
            // Lógica desenvolvida em conjunto com o Roger
            if (ex.InnerException == null)
                return;

            sb.AppendLine("InnerException:");
            sb.AppendFormat("Exception: {0}. ", ex.Message);
            sb.AppendFormat("Callstack: {0}.\n", ex.StackTrace);

            ExplodeInnerException(ex.InnerException, ref sb);
        }
    }
}
