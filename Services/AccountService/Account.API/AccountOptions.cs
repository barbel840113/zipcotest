using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.API
{
    public class AccountOptions
    {
        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
    }
}
