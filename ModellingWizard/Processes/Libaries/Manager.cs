using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.Libaries
{
    class Manager
    {
        public static void LoadAvailableLibaries()
        {
            Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full");
        }
    }
}
