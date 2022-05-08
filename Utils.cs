using System;
using System.Collections.Generic;
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

    }
}
