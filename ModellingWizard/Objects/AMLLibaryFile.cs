using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModellingWizard.Objects
{
    public class AMLLibaryFile
    {
        public Dictionary<string, List<Libaries.LibaryObject>> Dictionary_RoleClass_Attributes { get; set; }
        public Dictionary<string, List<List<Libaries.LibaryObject>>> Dictionary_Interface_Attributes { get; set; } 

        public XmlNode FirstLib { get; set; }
    }
}
