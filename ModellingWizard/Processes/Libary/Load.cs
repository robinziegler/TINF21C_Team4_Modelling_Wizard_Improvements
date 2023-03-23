using Aml.Engine.Adapter;
using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.Libary
{
    public class Load
    {
        public static (Objects.Libaries.Libary, Objects.Libaries.Libary) LoadLib(byte[] binary, string name)
        {
            CAEXDocument doc = CAEXDocument.LoadFromBinary(binary);

            /* Load role classes  from libary */
            Objects.Libaries.Libary RoleClassLib = new();
            RoleClassLib.Name = name;
            foreach (var classLibType in doc.CAEXFile.RoleClassLib)
            {
                Objects.Libaries.Libary SubLib = new()
                {
                    Name = classLibType.Name
                };

                foreach (var classLib in classLibType.RoleClass)
                {
                    SubLib.SubObjects.Add(LoadRoleClassSubLibs(classLib));
                }

                RoleClassLib.SubObjects.Add(SubLib);
            }

            /* Load interfaces from libary */
            Objects.Libaries.Libary InterfacesLib = new();
            InterfacesLib.Name = name;
            foreach(var classLibType in doc.CAEXFile.InterfaceClassLib)
            {
                Objects.Libaries.Libary SubLib = new()
                {
                    Name = classLibType.Name
                };

                foreach(var classLib in classLibType.InterfaceClass)
                {
                    SubLib.SubObjects.Add(LoadInterfaceSubLibs(classLib));
                }
                InterfacesLib.SubObjects.Add(SubLib);
            }

            return (RoleClassLib, InterfacesLib);
        }

        private static Objects.Libaries.Libary LoadRoleClassSubLibs(RoleFamilyType input)
        {
            Objects.Libaries.Libary SubLib = new()
            {
                Name = input.Name,
                Attributes = CheckForRoleClassAttributes(input)
            };
            foreach (var classLib in input.RoleClass)
            {
                SubLib.SubObjects.Add(LoadRoleClassSubLibs(classLib));
            }

            return SubLib;
        }

        private static Objects.Libaries.Libary LoadInterfaceSubLibs(InterfaceFamilyType input)
        {
            Objects.Libaries.Libary SubLib = new()
            {
                Name = input.Name,
                Attributes = CheckForInterfaceAttributes(input)
            };
            foreach (var classLib in input.InterfaceClass)
            {
                SubLib.SubObjects.Add(LoadInterfaceSubLibs(classLib));
            }

            return SubLib;
        }

        private static List<Objects.Libaries.LibaryObject> CheckForRoleClassAttributes(RoleFamilyType classType)
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

        private static List<Objects.Libaries.LibaryObject> CheckForInterfaceAttributes(InterfaceFamilyType classType)
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
