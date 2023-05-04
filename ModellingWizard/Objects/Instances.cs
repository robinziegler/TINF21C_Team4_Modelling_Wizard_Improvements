using ModellingWizard.Objects.Libaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aml.Engine;
using Aml.Engine.CAEX;
using System.ComponentModel.Design;
using Aml.Engine.AmlObjects;
using Windows.ApplicationModel.DataTransfer;

namespace ModellingWizard.Objects
{
    public class Instances
    {
        /* App Settings */
        public static int CurrentTheme {  get; set; }
        public static bool ExpertMode { get; set; } = false;
        public static bool UseCAEX3 { get; set; } = false;

        /* Configuration Data */
        public static Objects.Libaries.Libary System_Unit_Libs { get; set; } = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item3;
        public static Objects.Libaries.Libary RoleClassLib { get; set; } = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item1;
        public static Objects.Libaries.Libary InterfacesLib { get; set; } = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item2;

        public static void LibReload()
        {
             System_Unit_Libs = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item3;
             RoleClassLib = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item1;
             InterfacesLib = Processes.Libary.Load.LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full").Item2;
        }

        /* File Data */
        public static AutomationMLContainer CurrentFile { get; set; }
        public static string FileName { get; set; } = "New.amlx";
        public static Objects.Libaries.Libary Loaded_RoleClass_Data { get; set; }
        public static Objects.Libaries.Libary Loaded_Interfaces_Data { get; set; }
        public static List<Objects.Attachments.AttachmentObject> Attachments { get; set; } = new();
        public static Objects.Libaries.Libary Loaded_System_Unit_Libs { get; set; }
    }
}
