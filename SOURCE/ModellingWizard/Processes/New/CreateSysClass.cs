using Aml.Engine.CAEX;
using ModellingWizard.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.New
{
    public class CreateSysClass
    {
        public static void Execute()
        {
            Instances.Loaded_System_Unit_Libs = new();
            Instances.Loaded_RoleClass_Data = new();
            Instances.Loaded_Interfaces_Data = new();
            Instances.Attachments = new();

            //Add standard static SystemUnitClass
            Instances.Loaded_System_Unit_Libs.Name = "";

            var MainLib = new Objects.Libaries.Libary();
            MainLib.Name = Objects.Instances.System_Unit_Libs.Name;
            MainLib.SubObjects.Add((Objects.Libaries.Libary)((Objects.Libaries.Libary)Objects.Instances.System_Unit_Libs.SubObjects.Find(x => x.Name == "AutomationMLComponentStandardRCL")).SubObjects.Find(x => x.Name == "AutomationComponent"));

            Instances.Loaded_System_Unit_Libs = (Objects.Libaries.Libary)MainLib;
            Objects.Instances.LibReload();
        }
    }
}
