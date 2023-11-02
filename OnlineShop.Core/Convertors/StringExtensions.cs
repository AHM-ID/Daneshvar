using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Convertors
{
    public static class StringExtensions
    {
        public static string EmailFix(this string value)
        {
            return value.Trim().ToLower();
        }
    }
}
