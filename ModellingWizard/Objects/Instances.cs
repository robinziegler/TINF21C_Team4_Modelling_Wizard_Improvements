﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Objects
{
    internal class Instances
    {
        /* App Settings */
        public static bool ExpertMode { get; set; } = true;

        /* Configuration Data */
        public static Objects.Libaries.Libary RoleClassLib { get; set; } = Processes.RoleClassLib.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full");
        public static Objects.Libaries.Libary InterfacesLib { get; set; } = Processes.RoleClassLib.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full");

        /* File Data */
        public static Objects.Libaries.Libary Loaded_RoleClass_Data { get; set; }
        public static Objects.Libaries.Libary Loaded_Interfaces_Data { get; set; }
    }
}