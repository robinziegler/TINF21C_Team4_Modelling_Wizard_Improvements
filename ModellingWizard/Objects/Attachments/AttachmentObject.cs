using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Objects.Attachments
{
    public class AttachmentObject
    {
        public string Title { get; set; }
        public string Base64Content { get; set; }
        public string UUID { get; set; }

        public AttachmentObject() 
        {
            UUID = Guid.NewGuid().ToString();
        }
    }
}
