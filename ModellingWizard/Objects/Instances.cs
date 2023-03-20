using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Objects
{
    internal class Instances
    {
        /* Configuration Data */
        public static Objects.Libaries.LibaryFile RoleClassLib { get; set; } = Processes.Libaries.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full");
        public static bool ExpertMode { get; set; } = true;

        /* File Data */

    }
}
