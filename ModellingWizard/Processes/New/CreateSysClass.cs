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

           
            Objects.Instances.LibReload();
        }

        private static void Add()
        {
            /* Manufacturer */
            Instances.Loaded_System_Unit_Libs.Attributes.Add(new()
            {
                Name = "Manufacturer",
                Value = "",
                Default = "",
                Unit = "",
                Semantic = "",
                Reference = "",
                Description = "",
                CopyRight = "",
                AttributePath = "",
                RefBaseClassPath = "",
                ID = "",
                ReferencedClassName = "",
                //RefSemanticList = new CAEXSequence<RefSemanticType>(),
                SupportesRoleClassType = "",
                DataType = "",
                SubAttrebutes = null
            });

            Instances.Loaded_System_Unit_Libs.SubObjects.Add(new()
            {


            });
        }
    }
}
