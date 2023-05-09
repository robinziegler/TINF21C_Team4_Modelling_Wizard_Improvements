using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Aml.Editor.Plugin;
using Aml.Engine.Adapter;
using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using ModellingWizard.Objects;
using ModellingWizard.Objects.Attachments;
using ModellingWizard.Objects.Libaries;
using ModellingWizard.Processes.Libary;
using ModellingWizard.UIs.SubPages;
using ModellingWizard.Processes.GeneralFunctions;

namespace ModellingWizard.Processes.Open
{
    public class Open
    {
        public static (Objects.Libaries.Libary, Objects.Libaries.Libary) OpenFiles(byte[] binary, string name, string filepath)
        {
            if (name.EndsWith(".edz"))
            {
                ConverterAML converterAML = new ConverterAML();

                //add path to generate amlx file
                converterAML._pathAMLDestinationDirectory = Path.GetDirectoryName(name);
                //function of class to export .edz file to .amlx
                converterAML.exportStart(name);
                //get path to amlx file generated
                string AMLXFile = converterAML._pathAMLDestinationDirectory + "\\" + converterAML._AMLXFileName;
                //send path to function to open amlx file generated
                Console.WriteLine(AMLXFile);
            }
            if (name.EndsWith(".amlx"))
            {
                // Load the amlx container from the given filepath
                AutomationMLContainer amlx = new AutomationMLContainer(filepath);

                /* Load Attachments */
                //string tempPath = Path.GetTempPath();
                //amlx.ExtractAllFiles(tempPath);

                //string[] files = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);
                //files.ToList().ForEach(fileName =>
                //{
                // // Byte[] bytes = File.ReadAllBytes(fileName);
                // var OpenedFile = File.Open(fileName, FileMode.Open);
                // attachment = new AttachmentObject();
                //attachment.Title = OpenedFile.Name;
                //attachment.Base64Content = Convert.ToBase64String(OpenedFile());
                //Instances.Attachments.Add(attachment);
                //});

                // Get the root path -> main .aml file
                IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                // We expect the aml to only have one root part
                if (rootParts.First() != null)
                {

                    PackagePart part = rootParts.First();

                    // load the aml file as an CAEX document
                    CAEXDocument doc = CAEXDocument.LoadFromStream(part.GetStream());
                    amlx.Close();
                    // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                    // since we expect only one device per aml(x) file, return after on is found
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
                            SubLib.SubObjects.Add(Load.LoadRoleClassSubLibs(classLib));
                        }

                        RoleClassLib.SubObjects.Add(SubLib);
                    }

                    /* Load interfaces from libary */
                    Objects.Libaries.Libary InterfacesLib = new();

                    InterfacesLib.Name = name;
                    foreach (var classLibType in doc.CAEXFile.InterfaceClassLib)
                    {
                        Objects.Libaries.Libary SubLib = new()
                        {
                            Name = classLibType.Name
                        };

                        foreach (var classLib in classLibType.InterfaceClass)
                        {
                            SubLib.SubObjects.Add(Load.LoadInterfaceSubLibs(classLib));
                        }
                        InterfacesLib.SubObjects.Add(SubLib);
                    }
                    Objects.Libaries.Libary SystemLib = new();
                    foreach (var classLibType in doc.CAEXFile.SystemUnitClassLib)
                    {
                        Objects.Libaries.Libary SubLib = new()
                        {
                            Name = classLibType.Name
                        };

                        foreach (var classLib in classLibType.SystemUnitClass)
                        {
                            SubLib.SubObjects.Add(LoadSystemUnitSublibs(classLib));
                        }
                        SystemLib.SubObjects.Add(SubLib);
                    }


                    Instances.Loaded_RoleClass_Data = RoleClassLib;
                    Instances.Loaded_Interfaces_Data = InterfacesLib;
                    Instances.Loaded_System_Unit_Libs = SystemLib;
                    return (RoleClassLib, InterfacesLib);
                }
                return (null, null);
            }
            else
            {
                (Objects.Libaries.Libary RoleClassLib, Objects.Libaries.Libary InterfacesLib, Objects.Libaries.Libary SystemUnitClassLib) = Libary.Load.LoadLib(binary, name);
                Instances.Loaded_RoleClass_Data = RoleClassLib;
                Instances.Loaded_Interfaces_Data = InterfacesLib;
                return (RoleClassLib, InterfacesLib);
            }
                
        }
        public static Objects.Libaries.Libary LoadSystemUnitSublibs(SystemUnitFamilyType input)
        {
            Objects.Libaries.Libary SubLib = new()
            {
                Name = input.Name,
                Attributes = CheckForSystemUnitAttributes(input)
            };
            foreach (var classLib in input.SystemUnitClass)
            {
                SubLib.SubObjects.Add(LoadSystemUnitSublibs(classLib));
            }

            return SubLib;
        }
        private static List<Objects.Libaries.LibaryObject> CheckForSystemUnitAttributes(SystemUnitFamilyType classType)
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
                    SupportesRoleClassType = classType.CAEXPath(),
                    SubAttrebutes = CheckForsubAttributes(attribute.getAttributeField())
                });
            }
            return libAttributes;
        }
        private static List<Objects.Libaries.LibaryObject> CheckForsubAttributes(IEnumerable<AttributeType> atrr)
        {
            List<Objects.Libaries.LibaryObject> libAttributes = new();
            foreach (var attribute in atrr)
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
                    ID = attribute.ID,
                    SupportesRoleClassType = attribute.CAEXPath(),
                    SubAttrebutes = CheckForsubAttributes(attribute.getAttributeField())
                });
            }
            return libAttributes;
        }
        /*
        public static Objects.Attachments.AttachmentObject LoadAttatchments(AttachmentObject input)
        {
            Objects.Attachments.AttachmentObject SubLib = new()
            {
                Title = input.Title,
                Base64Content = input.Base64Content,
            };
            foreach (var classLib in input.InterfaceClass)
            {
                SubLib.SubObjects.Add(LoadInterfaceSubLibs(classLib));
            }

            return SubLib;
        }
        */
    }
}
