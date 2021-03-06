using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceASPNetCore.Data.Options
{
    public class DatabaseOptions
    {
        public string Server { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DatabaseName { get; set; }

        public bool IsWindowsAuthentication { get; set; }
    }
}
