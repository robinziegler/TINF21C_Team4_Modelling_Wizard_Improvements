using ModellingWizard.Objects.Libaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Processes.GeneralFunctions
{
    public static class Stringfunctions
    {
        public static string StringifyLibAtrebutes(List<LibaryObject> atrebutes)
        {
            string result = "";
            foreach (var item in atrebutes){

                result += "," + item.Name + "|" + item.Value + "|" + item.Default + "|" + item.Unit + "|" + item.DataType + "|" + item.Semantic; 
            }

            return result;
        }

        public static List<LibaryObject> LibraryObjektListFromAtrebiteStrings(string atrebutes)
        {



            return null; 
        }
    }
}
