using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Generators
{
    public class UserCodeGenerator
    {
        public static string UniqueCodeGenerator()
        {
            return Guid.NewGuid().ToString().Replace("-" , "");
        }
    }
}
