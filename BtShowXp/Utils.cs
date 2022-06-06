using BattleTech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BtShowXp
{
    public static class Utils
    {

        public static decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }


        /// <summary>
        /// The full path to this mod.  No trailing slash
        /// </summary>
        /// <returns></returns>
        public static string GetModPath()
        {
            return Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
        }

        public static string GetStackTrace()
        {

            StringBuilder sb = new StringBuilder();

            // Create a StackTrace that captures
            // filename, line number, and column
            // information for the current thread.
            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);

                sb.AppendLine($"Method: {sf.GetMethod()} Line: {sf.GetFileLineNumber()}");
            }

            return sb.ToString();
        }

        public static bool IsMethodInStack(string methodName)
        {
            // Create a StackTrace that captures
            // filename, line number, and column
            // information for the current thread.
            StackTrace st = new StackTrace(true);

            return st.GetFrames().Any(x => x.GetMethod().Name == methodName);
        }
    }
}
