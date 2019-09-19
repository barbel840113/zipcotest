using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API
{
    public class UserManagementOptions
    {
        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
    }
}
