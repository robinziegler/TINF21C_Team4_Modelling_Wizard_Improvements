using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.Libary
{
    public class LoadStandardLibaries
    {
        public static void Load(Objects.Libaries.LibaryTypes libary)
        {
            switch (libary)
            {
                case Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0:
                    LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0, "AutomationComponentLibrary_v1_0_0");
                    break;
                case Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_CAEX3_BETA:
                    LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_CAEX3_BETA, "AutomationComponentLibrary_v1_0_0_CAEX3_BETA");
                    break;
                case Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full:
                    LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full, "AutomationComponentLibrary_v1_0_0_Full");
                    break;
                case Objects.Libaries.LibaryTypes.AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA:
                    LoadLib(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA, "AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA");
                    break;
                case Objects.Libaries.LibaryTypes.ElectricConnectorLibrary_v1_0_0:
                    LoadLib(Properties.Resources.ElectricConnectorLibrary_v1_0_0, "ElectricConnectorLibrary_v1_0_0");
                    break;
                case Objects.Libaries.LibaryTypes.IndustrialSensorLibrary_v1_0_0:
                    LoadLib(Properties.Resources.IndustrialSensorLibrary_v1_0_0, "IndustrialSensorLibrary_v1_0_0");
                    break;
                default:
                    break;
            }
        }

        private static void LoadLib(byte[] binary, string name)
        {
            var result = Processes.Libary.Load.LoadLib(binary, name);
            Objects.Instances.RoleClassLib = result.Item1;
            Objects.Instances.InterfacesLib = result.Item2;
            Objects.Instances.System_Unit_Libs = result.Item3;
        }
    }
}
