using ABI.Windows.ApplicationModel.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModellingWizard.Processes.GeneralFunctions
{
    public static class SeartchforGuid
    {
        public static Objects.Libaries.Libary SeartchLibrary(string guid, Objects.Libaries.Libary lib)
        {

             foreach(Objects.Libaries.Libary sublib in lib.SubObjects)
             {
                if(sublib != null)
                {
                    if (sublib.myGuid == guid)
                        return sublib;
                    else
                    {
                        Objects.Libaries.Libary res = SeartchLibrary(guid, sublib);
                        if (res != null)
                            return res;
                    }
                }
             }
             if(lib.myGuid == guid)
             {
                return lib;
            }
             return null;
            
        }

    }
}
