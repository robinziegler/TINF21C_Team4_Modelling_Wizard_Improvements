using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using ModellingWizard.Objects;
using ModellingWizard.Objects.Attachments;
using ModellingWizard.Processes.Libary;

namespace ModellingWizard.Processes.Open
{
    public class Open
    {
        public static (Objects.Libaries.Libary, Objects.Libaries.Libary) OpenFiles(byte[] binary, string name, string filepath)
        {
            if (name.EndsWith(".amlx"))
            {
                // Load the amlx container from the given filepath
                AutomationMLContainer amlx = new AutomationMLContainer(filepath);

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

                    Instances.Loaded_RoleClass_Data = RoleClassLib;
                    Instances.Loaded_Interfaces_Data = InterfacesLib;
                    
                    return (RoleClassLib, InterfacesLib);
                }
                return (null, null);
            }
            else
            {
                (Objects.Libaries.Libary RoleClassLib, Objects.Libaries.Libary InterfacesLib) = Libary.Load.LoadLib(binary, name);
                Instances.Loaded_RoleClass_Data = RoleClassLib;
                Instances.Loaded_Interfaces_Data = InterfacesLib;
                return (RoleClassLib, InterfacesLib);
            }
                
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
