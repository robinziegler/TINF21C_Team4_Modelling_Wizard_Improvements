using Aml.Engine.Adapter;
using Aml.Engine.AmlObjects;
using Aml.Engine.AmlObjects.Extensions;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using ModellingWizard.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;

namespace ModellingWizard.Processes.Save
{
    public class Save
    {
        public static void SaveFile(string path, string filename, string fileType )
        {
            if(fileType.ToLower() == ".aml")
            {
                SaveAsAML(path);
            }
        }

        private static void SaveAsAML(string path)
        {
            var x = Instances.CurrentFile;
            IEnumerable<PackagePart> rootParts = Instances.CurrentFile.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);
            PackagePart part = rootParts.First();
            CAEXDocument doc = CAEXDocument.LoadFromStream(part.GetStream());

            GetAllRoleClasses(doc.CAEXFile.RoleClassLib);
            GetAllInterfaces(doc.CAEXFile.InterfaceClassLib);
            GetAllSystemUnits(doc.CAEXFile.SystemUnitClassLib);

            doc.SaveToFile(path, true);           
        }

        /* Convert of Role Classes */
        private static CAEXSequenceOfCAEXObjects<RoleClassLibType> GetAllRoleClasses(CAEXSequenceOfCAEXObjects<RoleClassLibType> input)
        {
            input.Remove();



            return input;
        }


        /* Convert of Interfaces */
        private static CAEXSequenceOfCAEXObjects<InterfaceClassLibType> GetAllInterfaces(CAEXSequenceOfCAEXObjects<InterfaceClassLibType> input)
        {
            input.Remove();

            return input;
        }

        /* Convert of SystemUnit */
        private static CAEXSequenceOfCAEXObjects<SystemUnitClassLibType> GetAllSystemUnits(CAEXSequenceOfCAEXObjects<SystemUnitClassLibType> input)
        {
            input.Remove();
            var x = new SystemUnitClassLibType(new System.Xml.Linq.XElement(input.ElementName));
            x.Insert(GetSystemUnits(Objects.Instances.Loaded_System_Unit_Libs));
            input.Insert(x);
            return input;
        }

        private static SystemUnitFamilyType GetSystemUnits(Objects.Libaries.Libary lib)
        {
            SystemUnitFamilyType SUCL = new SystemUnitFamilyType(new System.Xml.Linq.XElement(lib.Name));
            /* General Data */
            SUCL.Name = lib.Name;

            /* SubObjects */
            foreach(Objects.Libaries.Libary libary in lib.SubObjects)
            {
                SUCL.SystemUnitClass.Insert(GetSystemUnits(libary));
            }

            /* Attributes */
            foreach(Objects.Libaries.LibaryObject attribute in lib.Attributes)
            {
                SUCL.Attribute.Insert(LoadSUCLAttributes(attribute));
            }
            return SUCL;
        }

        private static AttributeType LoadSUCLAttributes(Objects.Libaries.LibaryObject lib)
        {
            AttributeType attr = new(new System.Xml.Linq.XElement(lib.Name));

            attr.Name = lib.Name;
            attr.Value = lib.Value;
            attr.DefaultValue = lib.Default;
            attr.Unit = lib.Unit;
            attr.AttributeDataType = lib.DataType;
            attr.Description = lib.Description;
            attr.Copyright = lib.CopyRight;
            //attr.AttributePath = lib.AttributePath;
            //attr.RefSemantic.Insert(lib.RefSemanticList);
            //attr.ReferencedClassName.Inse = lib.RefBaseClassPath;
            attr.ID = lib.ID;
            //attr.CAEXPath = cla
            lib.SubAttrebutes.ForEach(a =>
            {
                attr.Attribute.Insert(LoadSUCLAttributes(a));
            });
            return attr;
        }
    }
}
