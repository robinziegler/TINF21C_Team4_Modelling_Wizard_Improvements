using Aml.Engine.Adapter;
using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.RoleClassLib
{
    public class Load
    {
        public static Objects.Libaries.Libary LoadLib(byte[] binary, string name)
        {
            Objects.Libaries.Libary Lib = new();
            CAEXDocument doc = CAEXDocument.LoadFromBinary(binary);
            Lib.Name = name;
            foreach (var classLibType in doc.CAEXFile.RoleClassLib)
            {
                Objects.Libaries.Libary SubLib = new();
                SubLib.Name = classLibType.Name;

                foreach (var classLib in classLibType.RoleClass)
                {
                    SubLib.SubObjects.Add(LoadSubLibs(classLib));
                }

                Lib.SubObjects.Add(SubLib);
            }
            return Lib;
        }

        private static Objects.Libaries.Libary LoadSubLibs(RoleFamilyType input)
        {
            Objects.Libaries.Libary SubLib = new()
            {
                Name = input.Name,
                Attributes = CheckForAttributes(input)
            };
            foreach (var classLib in input.RoleClass)
            {
                SubLib.SubObjects.Add(LoadSubLibs(classLib));
            }

            return SubLib;
        }

        private static List<Objects.Libaries.LibaryObject> CheckForAttributes(RoleFamilyType classType)
        {
            List<Objects.Libaries.LibaryObject> libAttributes = new();
            foreach (var attribute in classType.Attribute)
            {
                libAttributes.Add(new()
                {
                    Name = attribute.Name,
                    Value = attribute.Value,
                    Default = attribute.DefaultValue,
                    Unit = attribute.Unit,
                    DataType = attribute.AttributeDataType,
                    Description = attribute.Description,
                    CopyRight = attribute.Copyright,
                    AttributePath = attribute.AttributePath,
                    RefSemanticList = attribute.RefSemantic,
                    ReferencedClassName = classType.ReferencedClassName,
                    RefBaseClassPath = classType.RefBaseClassPath,
                    ID = classType.ID,
                    SupportesRoleClassType = classType.CAEXPath()
                });
            }
            return libAttributes;
        }
    }
}
