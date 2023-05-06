using Aml.Engine.Adapter;
using Aml.Engine.AmlObjects;
using Aml.Engine.AmlObjects.Extensions;
using Aml.Engine.CAEX;
using Aml.Engine.Services;
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
using System.Xaml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using static Aml.Engine.AmlObjects.AutomationMLContainer;

namespace ModellingWizard.Processes.Save
{
    public class Save
    {
        public static void SaveFile(string path, string filename, string fileType )
        {
            if(fileType.ToLower() == ".aml")
            {
                SaveAsAML(path, false);
            }
            else if(fileType.ToLower() == ".amlx")
            {
                SaveAsAML(path, true);
            }
        }

        private static void SaveAsAML(string path, bool AMLX)
        {
            var doc = CAEXDocument.New_CAEXDocument(Instances.UseCAEX3 ? CAEXDocument.CAEXSchema.CAEX2_15 : CAEXDocument.CAEXSchema.CAEX3_0);
            if(Instances.CurrentFile != null )
            {
                IEnumerable<PackagePart> rootParts = Instances.CurrentFile.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);
                PackagePart part = rootParts.First();
                doc = CAEXDocument.LoadFromStream(part.GetStream());
            }
           

            GetAllRoleClasses(doc);
            GetAllInterfaces(doc);
            GetAllSystemUnitClasses(doc);

            if (AMLX)
            {
                AutomationMLContainer AMLContainer = new(path, FileMode.OpenOrCreate);
               // IEnumerable<PackagePart> rootParts = AMLContainer.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);



                string fileName = path.Split('.')[0].Split("/").Last().Split(@"\").Last();

                AMLContainer.AddRoot(doc.SaveToStream(true), PackUriHelper.CreatePartUri(new Uri("/" + fileName + "-root.aml", UriKind.Relative)));
                AMLContainer.Save();
            }
            else
            {
                doc.SaveToFile(path, true);
            }
        }

        /* Convert of Role Classes */
        private static void GetAllRoleClasses(CAEXDocument doc)
        {
            doc.CAEXFile.RoleClassLib.Remove();

            RoleClassLibType roleClassLib = doc.CAEXFile.RoleClassLib.Append("Test");
            Instances.Loaded_RoleClass_Data.SubObjects.ForEach(obj =>
            {
                var x = roleClassLib.RoleClass.Append(obj.Name);
                AddSubRoleClasses(x, obj.SubObjects);
                obj.Attributes.ForEach(attr =>
                {
                    var at = x.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);
                    
                });
            });
        }

        private static void AddSubRoleClasses(RoleFamilyType roleClass, List<Objects.Libaries.Libary> lib )
        {
            lib.ForEach(subLib =>
            {
                var rc = roleClass.RoleClass.Append(subLib.Name);

                AddSubRoleClasses(rc, subLib.SubObjects);
                subLib.Attributes.ForEach(attr =>
                {
                    var at = rc.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);
                });

            });
        }

        private static void AddAttributes(AttributeType attr, List<Objects.Libaries.LibaryObject> attrebutes)
        {
            attrebutes.ForEach(subAttr =>
            {
                var at = attr.New_Attribute(subAttr.Name);
                at.Value = subAttr.Value;
                at.DefaultValue = subAttr.Default;
                at.Unit = subAttr.Unit;
                at.AttributeDataType = subAttr.DataType;
                at.Description = subAttr.Description;
                at.Copyright = subAttr.CopyRight;
                //at.AttributePath = attr.AttributePath;
                //at.RefSemantic = attr.RefSemanticList
                //at.Referenced
                //at.RefBase
                at.ID = subAttr.ID;
                //at.SupporesToleClassType
                AddAttributes(at, subAttr.SubAttrebutes);
            });
        }

        /* Convert of Interfaces */
        private static void GetAllInterfaces(CAEXDocument doc)
        {
            doc.CAEXFile.InterfaceClassLib.Remove();

            InterfaceClassLibType interfaceClassLib = doc.CAEXFile.InterfaceClassLib.Append("Test");
            Instances.Loaded_RoleClass_Data.SubObjects.ForEach(obj =>
            {
                var x = interfaceClassLib.InterfaceClass.Append(obj.Name);
                AddSubInterfaceslasses(x, obj.SubObjects);
                obj.Attributes.ForEach(attr =>
                {
                    var at = x.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);

                });
            });
        }

        private static void AddSubInterfaceslasses(InterfaceFamilyType interfaceClass, List<Objects.Libaries.Libary> lib)
        {
            lib.ForEach(subLib =>
            {
                var rc = interfaceClass.InterfaceClass.Append(subLib.Name);

                AddSubInterfaceslasses(rc, subLib.SubObjects);
                subLib.Attributes.ForEach(attr =>
                {
                    var at = rc.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);
                });

            });
        }

        /* Convert of SystemUnit */
        private static void GetAllSystemUnitClasses(CAEXDocument doc)
        {
            doc.CAEXFile.SystemUnitClassLib.Remove();

            SystemUnitClassLibType sucClassLib = doc.CAEXFile.SystemUnitClassLib.Append("ComponentSystemUnitClassLib");
            Instances.Loaded_RoleClass_Data.SubObjects.ForEach(obj =>
            {
                var x = sucClassLib.SystemUnitClass.Append(obj.Name);
                AddSubSUCClasses(x, obj.SubObjects);
                obj.Attributes.ForEach(attr =>
                {
                    var at = x.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);

                });
            });
        }

        private static void AddSubSUCClasses(SystemUnitFamilyType sucClass, List<Objects.Libaries.Libary> lib)
        {
            lib.ForEach(subLib =>
            {
                var rc = sucClass.SystemUnitClass.Append(subLib.Name);

                AddSubSUCClasses(rc, subLib.SubObjects);
                subLib.Attributes.ForEach(attr =>
                {
                    var at = rc.New_Attribute(attr.Value);
                    at.Value = attr.Value;
                    at.DefaultValue = attr.Default;
                    at.Unit = attr.Unit;
                    at.AttributeDataType = attr.DataType;
                    at.Description = attr.Description;
                    at.Copyright = attr.CopyRight;
                    //at.AttributePath = attr.AttributePath;
                    //at.RefSemantic = attr.RefSemanticList
                    //at.Referenced
                    //at.RefBase
                    at.ID = attr.ID;
                    //at.SupporesToleClassType

                    AddAttributes(at, attr.SubAttrebutes);
                });

            });
        }
    }
}
