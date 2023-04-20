using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModellingWizard.Objects
{
    public class MyTreNode:TreeViewNode
    {
        public Objects.Libaries.Libary lib { get; set; }
    }
}
