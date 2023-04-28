using Aml.Engine.CAEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Objects.Libaries
{
    public class Libary : ICloneable 
    {
        public string myGuid { get; set; }    
        public string Name { get; set; }
        public List<LibaryObject> Attributes { get; set; }
        public List<Libary> SubObjects { get; set; }

        public Libary()
        {
            Attributes = new();
            SubObjects = new();
            myGuid = Guid.NewGuid().ToString();
        } 

        public Object Clone()
        {
            foreach(Libary obj in SubObjects)
            {
                obj.MemberwiseClone();
            }
            return this.MemberwiseClone();
        }
    }

    public class LibaryObject
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public string Unit { get; set; }
        public string Semantic { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string CopyRight { get; set; }
        public string AttributePath { get; set; }
        public string RefBaseClassPath { get; set; }
        public string ID { get; set; }
        public string ReferencedClassName { get; set; }
        public CAEXSequence<RefSemanticType> RefSemanticList { get; set; }
        public string SupportesRoleClassType { get; set; }
        public string DataType { get; set; }
        public List<LibaryObject> SubAttrebutes { get; set; }
    }
}
