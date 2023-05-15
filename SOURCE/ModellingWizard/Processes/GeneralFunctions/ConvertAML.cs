using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Reflection;
using SevenZip;
using LumenWorks.Framework.IO.Csv;
using Aml.Engine.AmlObjects;
using System.IO.Packaging;
using System.IO.Compression;
using System.Formats.Asn1;
using ModellingWizard.Objects;

namespace ModellingWizard.Processes.GeneralFunctions
{
    
    public class ConverterAML
    {
        private CAEXDocument _myDoc;
        private string _XMLFileName;
        private string _AMLFileName;
        public string _AMLXFileName { get; set; }
        private string _ComponentPicture;
        private string _componentPictureRoot;
        private string _file2dgm;
        private string _file2dgmRoot;
        private string _file3dgm;
        private string _file3dgmRoot;
        private string _currentCompressedEDZFile;
        private string _sugestFilename;
        private string _appDataMURR = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MURR";
        private string _unzippedTemporaryDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MURR\tmp\extract\";
        private string _templateFileBUSNODE = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MURR\Templates\template_catalog_busnode_v0.4.aml";
        private string _templateFileIOMODULE = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MURR\Templates\template_catalog_iomodule_v0.4.aml";
        private string _templateFileGeneric = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MURR\Templates\template_catalog_device_v0.6.aml";
        public string _pathAMLDestinationDirectory { get; set; }
        private DataTable _functionsDefinitionsDataTable;
        private DataTable _busTypeDataTable;
        private DataTable _manufacturers;


        private InternalElementType _internalElementType_BUSNODE_OR_MODULE_CLEAN;
        private ExternalInterfaceType _internalElementType_CONNECTOR_CLEAN;

        private InternalElementType _internalElementType_BUSNODE_OR_MODULE;
        // private InternalElementType _internalElementType_CONNECTOR;
        private ExternalInterfaceType _internalElementType_CONNECTOR;
        // private ExternalInterfaceType _externalInterfaceType_PIN;
        private List<string> _listOf_ID_PIN;
        private List<DataForCreatePin> _listOf_DataForCreatePin;

        public ConverterAML()
        {
            //Importing functions definitions file
            importFunctionsDefinitionsCSV();
            //Importing bustype file
            importBusTypeCSV();
            //Importing manufacturers file
            importManufacturersCSV();
            //setting path to 7zip lib
            setLibrary7zPath();
        }

        //Sets the location of the 7z libraries needed to unzip the .edz file
        private void setLibrary7zPath()
        {
            var _libraryPath = "";
            if (Environment.Is64BitOperatingSystem)
            {
                _libraryPath = _appDataMURR + @"\x64\7z.dll";
            }
            else
            {
                _libraryPath = _appDataMURR + @"\x86\7z.dll";
            }
            SevenZipBase.SetLibraryPath(_libraryPath);
        }

        //Start exporting .edz file
        public void exportStart(string pathEDZFile)
        {
            _currentCompressedEDZFile = pathEDZFile;
            _sugestFilename = Path.GetFileNameWithoutExtension(_currentCompressedEDZFile);
            _AMLFileName = _sugestFilename + ".aml";
            _AMLXFileName = _sugestFilename + ".amlx";
            extractFileWithDLL7Zip(_currentCompressedEDZFile, _unzippedTemporaryDirectory + _sugestFilename);
            getEDZFileFromTemporaryDirectory(_unzippedTemporaryDirectory + _sugestFilename);
            readXMLAndExportToAML(_XMLFileName);
        }

        //Extract the.edz file
        private void extractFileWithDLL7Zip(string zipPath, string extractPath)
        {
            Stream stream = File.OpenRead(zipPath);
            using (SevenZipExtractor extractor = new SevenZipExtractor(stream))
            {
                extractor.ExtractArchive(extractPath);
            }
        }

        //Get the files needed for the conversion to aml, in the temporary directory
        private void getEDZFileFromTemporaryDirectory(string path)
        {
            _componentPictureRoot = "";
            _file2dgmRoot = "";
            _file3dgmRoot = "";

            string[] filesComponentPicture = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
            if (filesComponentPicture.Length > 0)
            {
                _ComponentPicture = filesComponentPicture[0];
                _componentPictureRoot = Path.GetFileName(_ComponentPicture);
                //string[] filepicture = _ComponentPicture.Split('\\');
                //for (int i = 9; i < filepicture.Length; i++)
                //{
                //    _componentPictureRoot = _componentPictureRoot + "\\" + filepicture[i];
                //}
            }

            string[] files2dgm = Directory.GetFiles(path, "*.ema", SearchOption.AllDirectories);
            if (files2dgm.Length > 0)
            {
                _file2dgm = files2dgm[0];
                _file2dgmRoot = Path.GetFileName(_file2dgm);

                //string[] file2dgm = _file2dgm.Split('\\');
                //for (int i = 9; i < file2dgm.Length; i++)
                //{
                //    _file2dgmRoot = _file2dgmRoot + "\\" + file2dgm[i];
                //}
            }

            string[] files3dgm = Directory.GetFiles(path, "*_3D.ema", SearchOption.AllDirectories);
            if (files3dgm.Length > 0)
            {
                _file3dgm = files3dgm[0];
                _file3dgmRoot = Path.GetFileName(_file3dgm);

                //string[] file3dgm = _file3dgm.Split('\\');
                //for (int i = 9; i < file3dgm.Length; i++)
                //{
                //    _file3dgmRoot = _file3dgmRoot + "\\" + file3dgm[i];
                //}
            }

            string[] files = Directory.GetFiles(path, "*.part.xml", SearchOption.AllDirectories);
            _XMLFileName = files[0];
        }

        //Read XML file and export to AML
        private void readXMLAndExportToAML(string filePath)
        {
            // string pathTest = @"C:\tmp\EDZ FOR TEST\items\partxml\MURR.56526.part.xml";

            XmlReader xmlReader = XmlReader.Create(filePath);

            _internalElementType_BUSNODE_OR_MODULE = _internalElementType_BUSNODE_OR_MODULE_CLEAN;
            _internalElementType_CONNECTOR = _internalElementType_CONNECTOR_CLEAN;
            _listOf_DataForCreatePin = new List<DataForCreatePin>();
            _listOf_ID_PIN = new List<string>();

            string _connectortype = "";
            string _connectorref = "";
            string _partnr = "";
            string _voltage = "";
            string _current = "";
            string _connectionDesignation = "";
            string _functionType = "";
            bool _isNewTerminalNr = true;

            string _econnectorname_current = "";

            //Going through part.xml file and getting the data to generate the AML file
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "part")
                {
                    _partnr = xmlReader.GetAttribute("P_ARTICLE_PARTNR");
                    //      string manufacturer = xmlReader.GetAttribute("P_ARTICLE_MANUFACTURER");
                    string manufacturer = getManufacturer(xmlReader.GetAttribute("P_PART_ADDRESS_LONGNAME"), xmlReader.GetAttribute("P_ARTICLE_MANUFACTURER"));
                    string manwebsite = getManufacturers(manufacturer);
                    string devclass = getValueDevClass(xmlReader.GetAttribute("P_ARTICLE_DESCR1"));
                    string model = getValueModel(xmlReader.GetAttribute("P_ARTICLE_TYPENR"));
                    string prodcode = getValueProdCode(xmlReader.GetAttribute("P_ARTICLE_ORDERNR"), xmlReader.GetAttribute("P_ARTICLE_PARTNR"));
                    string product_permalink = getValueProdPermalink(xmlReader.GetAttribute("P_ARTICLE_MANUFACTURER"), prodcode);
                    string eplanpath = _sugestFilename + ".edz";
                    string componentname = getValueComponentName(xmlReader.GetAttribute("P_ARTICLE_DESCR1"));
                    string weight = getValueWeight(xmlReader.GetAttribute("P_ARTICLE_WEIGHT"));
                    string height = getValueHeight(xmlReader.GetAttribute("P_ARTICLE_HEIGHT"));
                    string width = getValueWidth(xmlReader.GetAttribute("P_ARTICLE_WIDTH"));
                    string length = getValueLength(xmlReader.GetAttribute("P_ARTICLE_DEPTH"));
                    string descshort = getValueDescShort(xmlReader.GetAttribute("P_ARTICLE_NOTE"));
                    string desclong = getValueDescLong(xmlReader.GetAttribute("P_ARTICLE_DESCR2"));

                    string address1 = getValuesAdress1(xmlReader.GetAttribute("P_PART_ADDRESS_NAME1"));
                    string address2 = getValuesAdress2(xmlReader.GetAttribute("P_PART_ADDRESS_STREET"));
                    string zipcode = getValuesZipCode(xmlReader.GetAttribute("P_PART_ADDRESS_ZIPTOWN"));
                    string city = getValuesCity(xmlReader.GetAttribute("P_PART_ADDRESS_TOWN"));
                    string country = getValuesCountry(xmlReader.GetAttribute("P_PART_ADDRESS_STATE"));
                    string contactmail = getValuesEmail(xmlReader.GetAttribute("P_PART_ADDRESS_EMAIL"));
                    string contactphone = getValuesPhone(xmlReader.GetAttribute("P_PART_ADDRESS_PHONE"));

                    string voltage = getValuesVoltage(xmlReader.GetAttribute("P_ARTICLE_VOLTAGE"));
                    string current = getValuesCurrent(xmlReader.GetAttribute("P_ARTICLE_ELECTRICALCURRENT"));
                    string ipcode = string.Empty;
                    string material = string.Empty;


                    _connectortype = getValueConnectortype(xmlReader.GetAttribute("P_ARTICLE_DESCR1"), xmlReader.GetAttribute("P_ARTICLE_DESCR2"), xmlReader.GetAttribute("P_ARTICLE_DESCR3"));
                    _connectorref = getValueConnectorRef(xmlReader.GetAttribute("P_ARTICLE_DESCR1"), xmlReader.GetAttribute("P_ARTICLE_DESCR2"), xmlReader.GetAttribute("P_ARTICLE_DESCR3"));

                    //Sending the data read from the xml to the AML template
                    writeTemplateDeviceGeneric(_partnr, manufacturer, manwebsite, devclass, model, prodcode, product_permalink, eplanpath,
                        componentname, _componentPictureRoot, _file2dgmRoot, _file3dgmRoot, weight, height, width, length, descshort,
                        desclong, address1, address2, zipcode, city, country, contactmail, contactphone, voltage, current, ipcode, material);


                    //if (verifyTypeTemplateBUSNODE(devclass))
                    //{
                    //    writeTemplateBUSNODE(_partnr, manufacturer, manwebsite, devclass, model, prodcode, product_permalink, eplanpath, componentname, _componentPictureRoot, _file2dgmRoot, _file3dgmRoot);
                    //}
                    //else
                    //{
                    //    writeTemplateMODULE(_partnr, manufacturer, manwebsite, devclass, model, prodcode, product_permalink, eplanpath, componentname, _componentPictureRoot, _file2dgmRoot, _file3dgmRoot);
                    //}

                }
                else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "variant")
                {
                    _voltage = xmlReader.GetAttribute("P_ARTICLE_VOLTAGE");
                    _current = xmlReader.GetAttribute("P_ARTICLE_ELECTRICALCURRENT");
                }
                else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "functiontemplate")
                {
                    string econnectorname = xmlReader.GetAttribute("terminalNr");
                    string busType = getBusType(xmlReader.GetAttribute("plcbussystem"));
                    _connectionDesignation = xmlReader.GetAttribute("connectionDesignation");
                    _functionType = getFunctionDefinition(xmlReader.GetAttribute("connectiondescription"), xmlReader.GetAttribute("functiondefcategory"), xmlReader.GetAttribute("functiondefgroup"), xmlReader.GetAttribute("functiondefid"), 5);

                    if (econnectorname != null)
                    {
                        if (_econnectorname_current == "")
                        {
                            _econnectorname_current = econnectorname;
                            _isNewTerminalNr = true;
                        }
                        if (_econnectorname_current != econnectorname)
                        {
                            _econnectorname_current = econnectorname;
                            //   foreach (var pin_id in _listOf_ID_PIN)
                            //    {
                            foreach (var data in _listOf_DataForCreatePin)
                            {
                                //Adding pins to connectors in AML
                                data.externalInterface_id = _internalElementType_CONNECTOR.ID;  //_externalInterfaceType_PIN.ID;
                                addPinInAML(data);
                            }
                            //    }

                            _isNewTerminalNr = true;
                            _listOf_DataForCreatePin.Clear();
                            _listOf_ID_PIN.Clear();
                        }
                        if (_isNewTerminalNr)
                        {
                            //Adding Connectors to AML
                            addConnectorInAML(econnectorname, _connectorref, busType, _connectortype, _voltage, _current, _isNewTerminalNr);
                            _isNewTerminalNr = false;
                        }

                        if (_connectionDesignation != null)
                        {
                            //_listOf_ID_PIN.Add(_externalInterfaceType_PIN.ID);
                            DataForCreatePin dataForCreatePin = new DataForCreatePin(_connectionDesignation, _functionType);
                            _listOf_DataForCreatePin.Add(dataForCreatePin);
                        }


                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "variant")
                {
                    if (_listOf_DataForCreatePin.Count() > 0)
                    {
                        foreach (var pin_id in _listOf_ID_PIN)
                        {
                            foreach (var data in _listOf_DataForCreatePin)
                            {
                                data.externalInterface_id = pin_id;
                                addPinInAML(data);
                            }
                        }

                        _listOf_DataForCreatePin.Clear();
                        _listOf_ID_PIN.Clear();

                    }
                }

            }

            //Save AMLX file
            saveAMLX();
        }

        //Return param of Manufacturers to aml
        private string getManufacturer(string P_PART_ADDRESS_LONGNAME, string P_ARTICLE_PARTNR)
        {
            string manufacturer = String.Empty;

            if (P_PART_ADDRESS_LONGNAME != null)
            {
                manufacturer = P_PART_ADDRESS_LONGNAME;
            }
            else
            {
                manufacturer = P_ARTICLE_PARTNR;
            }

            return manufacturer;
        }
        //Return param of Manufacturers to aml
        private string getManufacturers(string manufacturer)
        {
            for (int i = 0; i < _manufacturers.Rows.Count; i++)
            {
                string value = _manufacturers.Rows[i][1].ToString();
                if (value.Trim() == manufacturer.Trim())
                {
                    return _manufacturers.Rows[i][2].ToString();
                }
            }

            return "Unknown";
        }

        //Return param of ComponentName to aml
        private string getValueComponentName(string P_ARTICLE_DESCR1)
        {
            string[] articles = P_ARTICLE_DESCR1.Split(';');
            foreach (var article in articles)
            {
                if (article.Contains("en_US"))
                {
                    return article.Replace("en_US@", string.Empty);
                }

            }

            return "";

        }

        //Return param of DevClass to aml
        private string getValueDevClass(string P_ARTICLE_DESCR1)
        {
            string[] articles = P_ARTICLE_DESCR1.Split(';');
            foreach (var article in articles)
            {
                if (article.Contains("en_US"))
                {
                    return article.Replace("en_US@", string.Empty);
                }

            }

            return "";

        }

        //Return param of Model to aml
        private string getValueModel(string P_ARTICLE_TYPENR)
        {

            if (P_ARTICLE_TYPENR != null)
            {
                return P_ARTICLE_TYPENR;
            }
            else
            {
                return "";
            }

        }

        //Getting the weight value
        public string getValueWeight(string P_ARTICLE_WEIGHT)
        {
            if (P_ARTICLE_WEIGHT != null)
            {
                return P_ARTICLE_WEIGHT;
            }
            else
            {
                return "";
            }
        }

        //Getting the height value
        public string getValueHeight(string P_ARTICLE_HEIGHT)
        {
            if (P_ARTICLE_HEIGHT != null)
            {
                return P_ARTICLE_HEIGHT;
            }
            else
            {
                return "";
            }
        }

        //Getting the width value
        public string getValueWidth(string P_ARTICLE_WIDTH)
        {
            if (P_ARTICLE_WIDTH != null)
            {
                return P_ARTICLE_WIDTH;
            }
            else
            {
                return "";
            }
        }

        //Getting the length value
        public string getValueLength(string P_ARTICLE_DEPTH)
        {
            if (P_ARTICLE_DEPTH != null)
            {
                return P_ARTICLE_DEPTH;
            }
            else
            {
                return "";
            }
        }

        //Getting the desc short value
        public string getValueDescShort(string P_ARTICLE_NOTE)
        {
            if (P_ARTICLE_NOTE != null)
            {
                return P_ARTICLE_NOTE;
            }
            else
            {
                return "";
            }
        }

        //Getting the desc long value
        public string getValueDescLong(string P_ARTICLE_DESCR2)
        {
            if (P_ARTICLE_DESCR2 != null)
            {
                return P_ARTICLE_DESCR2;
            }
            else
            {
                return "";
            }
        }

        //Getting the adress1 value
        public string getValuesAdress1(string P_PART_ADDRESS_NAME1)
        {
            if (P_PART_ADDRESS_NAME1 != null)
            {
                return P_PART_ADDRESS_NAME1;
            }
            else
            {
                return "";
            }
        }

        //Getting the adress2 value
        public string getValuesAdress2(string P_PART_ADDRESS_STREET)
        {
            if (P_PART_ADDRESS_STREET != null)
            {
                return P_PART_ADDRESS_STREET;
            }
            else
            {
                return "";
            }
        }

        //Getting the zipCode value
        public string getValuesZipCode(string P_PART_ADDRESS_ZIPTOWN)
        {
            if (P_PART_ADDRESS_ZIPTOWN != null)
            {
                return P_PART_ADDRESS_ZIPTOWN;
            }
            else
            {
                return "";
            }
        }

        //Getting the city value
        public string getValuesCity(string P_PART_ADDRESS_TOWN)
        {
            if (P_PART_ADDRESS_TOWN != null)
            {
                return P_PART_ADDRESS_TOWN;
            }
            else
            {
                return "";
            }
        }

        //Getting the country value
        public string getValuesCountry(string P_PART_ADDRESS_STATE)
        {
            if (P_PART_ADDRESS_STATE != null)
            {
                return P_PART_ADDRESS_STATE;
            }
            else
            {
                return "";
            }
        }

        //Getting the email value
        public string getValuesEmail(string P_PART_ADDRESS_EMAIL)
        {
            if (P_PART_ADDRESS_EMAIL != null)
            {
                return P_PART_ADDRESS_EMAIL;
            }
            else
            {
                return "";
            }
        }

        //Getting the phone value
        public string getValuesPhone(string P_PART_ADDRESS_PHONE)
        {
            if (P_PART_ADDRESS_PHONE != null)
            {
                return P_PART_ADDRESS_PHONE;
            }
            else
            {
                return "";
            }
        }

        //Getting the voltage value
        public string getValuesVoltage(string P_ARTICLE_VOLTAGE)
        {
            if (P_ARTICLE_VOLTAGE != null)
            {
                return P_ARTICLE_VOLTAGE;
            }
            else
            {
                return "";
            }
        }

        //Getting the current value
        public string getValuesCurrent(string P_ARTICLE_ELECTRICALCURRENT)
        {
            if (P_ARTICLE_ELECTRICALCURRENT != null)
            {
                return P_ARTICLE_ELECTRICALCURRENT;
            }
            else
            {
                return "";
            }
        }

        //Return param of ProdCode to aml
        private string getValueProdCode(string P_ARTICLE_ORDERNR, string P_ARTICLE_PARTNR)
        {
            string prodcode = String.Empty;

            if (P_ARTICLE_ORDERNR != null)
            {
                prodcode = P_ARTICLE_ORDERNR;
            }
            else
            {
                prodcode = P_ARTICLE_PARTNR;
            }

            return prodcode;
        }

        //Return param of Permalink to aml
        private string getValueProdPermalink(string P_ARTICLE_MANUFACTURER, string prodcode)
        {
            string prodPermalink = "";

            if (P_ARTICLE_MANUFACTURER == "MURR" | P_ARTICLE_MANUFACTURER == "Murrelektronik")
            {
                prodPermalink = "https://shop.murrelektronik.com/" + prodcode + "/";
            }

            return prodPermalink;
        }

        //Return param of Connectortype to aml
        private string getValueConnectortype(string P_ARTICLE_DESCR1, string P_ARTICLE_DESCR2, string P_ARTICLE_DESCR3)
        {

            //string _P_ARTICLE_DESCR1 = "";
            //string _P_ARTICLE_DESCR2 = "";
            //string _P_ARTICLE_DESCR3 = "";


            //_P_ARTICLE_DESCR1 = P_ARTICLE_DESCR1;

            //if (P_ARTICLE_DESCR2 != null)
            //{
            //    _P_ARTICLE_DESCR2 = P_ARTICLE_DESCR2;
            //}

            //if (P_ARTICLE_DESCR3 != null)
            //{
            //    _P_ARTICLE_DESCR3 = P_ARTICLE_DESCR3;
            //}

            //if (_P_ARTICLE_DESCR1.Contains("M8") || _P_ARTICLE_DESCR2.Contains("M8") || _P_ARTICLE_DESCR3.Contains("M8"))
            //{
            //    return "M8";
            //}
            //else if (_P_ARTICLE_DESCR1.Contains("M12") || _P_ARTICLE_DESCR2.Contains("M12") || _P_ARTICLE_DESCR3.Contains("M12"))
            //{
            //    return "M12";
            //}
            //else
            //{
            //    return "ElectricInterface";
            //}

            return "ElectricInterface";
        }

        //Return param of ConnectorRef to aml
        private string getValueConnectorRef(string P_ARTICLE_DESCR1, string P_ARTICLE_DESCR2, string P_ARTICLE_DESCR3)
        {
            //string _P_ARTICLE_DESCR1 = "";
            //string _P_ARTICLE_DESCR2 = "";
            //string _P_ARTICLE_DESCR3 = "";


            //_P_ARTICLE_DESCR1 = P_ARTICLE_DESCR1;

            //if (P_ARTICLE_DESCR2 != null)
            //{
            //    _P_ARTICLE_DESCR2 = P_ARTICLE_DESCR2;
            //}

            //if (P_ARTICLE_DESCR3 != null)
            //{
            //    _P_ARTICLE_DESCR3 = P_ARTICLE_DESCR3;
            //}

            //if (_P_ARTICLE_DESCR1.Contains("M8") || _P_ARTICLE_DESCR2.Contains("M8") || _P_ARTICLE_DESCR3.Contains("M8"))
            //{
            //    return "ConnectorLib_IEC61076/IEC61076-2/M8";
            //}
            //else if (_P_ARTICLE_DESCR1.Contains("M12") || _P_ARTICLE_DESCR2.Contains("M12") || _P_ARTICLE_DESCR3.Contains("M12"))
            //{
            //    return "ConnectorLib_IEC61076/IEC61076-2/M12";
            //}
            //else
            //{
            //    return "AindaMIPInterfaceClassLib/ElectricInterface";
            //}

            return "AutomationMLComponentBaseICL/ElectricInterface";
        }

        //Checks if the template is a busnode
        private bool verifyTypeTemplateBUSNODE(string text)
        {
            if (text.Contains("busnode"))
            {
                return true;
            }
            else if (text.Contains("bus node"))
            {
                return true;
            }
            else if (text.Contains("BUSNODE"))
            {
                return true;
            }
            else if (text.Contains("BUS NODE"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Fill the template busnode with their respective values
        private void writeTemplateBUSNODE(string partnr, string manufacturer, string manwebsite, string devclass, string model, string prodcode,
            string product_permalink, string eplanpath, string componentname, string componentPicture, string file2dgm, string file3dgm)
        {
            // Create a new CAEX document
            _myDoc = CAEXDocument.New_CAEXDocument();

            string pathTemplateAML = _templateFileBUSNODE;

            _myDoc = CAEXDocument.LoadFromFile(pathTemplateAML);

            _internalElementType_BUSNODE_OR_MODULE = (InternalElementType)_myDoc.FindByID("1c5466db-5db2-423e-a3a4-c25b18bf9398");

            SystemUnitClassType systemUnitClass = (SystemUnitClassType)_myDoc.FindByID("46ad3004-57fe-435a-896b-e0b60dc72a76");
            systemUnitClass.Name = componentname;

            AttributeType attribute1 = (AttributeType)_myDoc.FindByID("1e87b904-56b6-4578-86e7-f8413b07b81e");
            attribute1.Value = manufacturer;
            attribute1.ID = null;

            AttributeType attribute2 = (AttributeType)_myDoc.FindByID("238900cc-3d05-44f8-b757-b0f46f2cb14d");
            attribute2.Value = manwebsite;
            attribute2.ID = null;

            AttributeType attribute3 = (AttributeType)_myDoc.FindByID("516e9423-a677-44b0-856d-8418e33d4db8");
            attribute3.Value = devclass;
            attribute3.ID = null;

            AttributeType attribute4 = (AttributeType)_myDoc.FindByID("6e757d7e-a483-44d5-97e1-e7203aa450d0");
            attribute4.Value = model;
            attribute4.ID = null;

            AttributeType attribute5 = (AttributeType)_myDoc.FindByID("c7422c53-d5f7-412e-82ad-d1a87481f51c");
            attribute5.Value = prodcode;
            attribute5.ID = null;

            AttributeType attribute6 = (AttributeType)_myDoc.FindByID("8beed8c3-d4a0-4203-89d7-86b8f39e2c1a");
            attribute6.Value = product_permalink;
            attribute6.ID = null;

            AttributeType attribute7 = (AttributeType)_myDoc.FindByID("e42cf0a9-8bee-40c6-b11c-3857cf3d9627");
            attribute7.Value = eplanpath;
            attribute7.ID = null;

            AttributeType attribute8 = (AttributeType)_myDoc.FindByID("46a028b2-1bb7-4897-b863-f05655568df0");
            attribute8.Value = componentPicture;
            attribute8.ID = null;

            AttributeType attribute9 = (AttributeType)_myDoc.FindByID("1247913d-2ab0-4c4c-beab-b824502e80e3");
            attribute9.Value = file2dgm;
            attribute9.ID = null;

            AttributeType attribute10 = (AttributeType)_myDoc.FindByID("7a22dce2-d949-46ca-99a1-0bd04d0ee669");
            attribute10.Value = file3dgm;
            attribute10.ID = null;



        }

        //Fill the data in the general template with their respective values
        private void writeTemplateDeviceGeneric(string partnr, string manufacturer, string manwebsite, string devclass, string model, string prodcode,
    string product_permalink, string eplanpath, string componentname, string componentPicture, string file2dgm, string file3dgm, string weight, string height,
    string width, string length, string descShort, string descLong, string address1, string address2, string zipcode, string city, string country,
    string contactmail, string contactphone, string voltage, string current, string ipcode, string material)
        {
            // Create a new CAEX document
            _myDoc = CAEXDocument.New_CAEXDocument();

            string pathTemplateAML = _templateFileGeneric;

            _myDoc = CAEXDocument.LoadFromFile(pathTemplateAML);

            _internalElementType_BUSNODE_OR_MODULE = (InternalElementType)_myDoc.FindByID("c6bafc65-22c7-4a42-981b-3ab5965d162e");

            SystemUnitClassType systemUnitClass = (SystemUnitClassType)_myDoc.FindByID("a3621b7f-553a-4d56-addf-fc8751648aaf");
            systemUnitClass.Name = partnr;

            //SystemUnitClassType systemUnitClass = (SystemUnitClassType)_myDoc.FindByID("a3621b7f-553a-4d56-addf-fc8751648aaf");
            //systemUnitClass.Name = componentname;

            AttributeType attribute1 = (AttributeType)_myDoc.FindByID("1e87b904-56b6-4578-86e7-f8413b07b81e");
            attribute1.Value = manufacturer;
            attribute1.DefaultValue = manufacturer;
            attribute1.ID = null;

            AttributeType attribute2 = (AttributeType)_myDoc.FindByID("238900cc-3d05-44f8-b757-b0f46f2cb14d");
            attribute2.Value = manwebsite;
            attribute2.DefaultValue = manwebsite;
            attribute2.ID = null;

            AttributeType attribute3 = (AttributeType)_myDoc.FindByID("516e9423-a677-44b0-856d-8418e33d4db8");
            attribute3.Value = devclass;
            attribute3.DefaultValue = devclass;
            attribute3.ID = null;

            AttributeType attribute4 = (AttributeType)_myDoc.FindByID("6e757d7e-a483-44d5-97e1-e7203aa450d0");
            attribute4.Value = model;
            attribute4.DefaultValue = model;
            attribute4.ID = null;

            AttributeType attribute5 = (AttributeType)_myDoc.FindByID("c7422c53-d5f7-412e-82ad-d1a87481f51c");
            attribute5.Value = prodcode;
            attribute5.DefaultValue = prodcode;
            attribute5.ID = null;

            AttributeType attribute6 = (AttributeType)_myDoc.FindByID("8beed8c3-d4a0-4203-89d7-86b8f39e2c1a");
            attribute6.Value = product_permalink;
            attribute6.DefaultValue = product_permalink;
            attribute6.ID = null;

            AttributeType attribute7 = (AttributeType)_myDoc.FindByID("446bb723-3ecb-4fb8-a44b-ec06f10ed51b");
            attribute7.Value = weight;
            attribute7.DefaultValue = weight;
            attribute7.ID = null;

            AttributeType attribute8 = (AttributeType)_myDoc.FindByID("38bfebd8-89bb-4b4b-96dc-fe5256f361a3");
            attribute8.Value = height;
            attribute8.DefaultValue = height;
            attribute8.ID = null;

            AttributeType attribute9 = (AttributeType)_myDoc.FindByID("ee666702-3474-4929-aacc-e084892c15d3");
            attribute9.Value = width;
            attribute9.DefaultValue = width;
            attribute9.ID = null;

            AttributeType attribute10 = (AttributeType)_myDoc.FindByID("4ffb342a-7d77-4960-ac22-6c15f834ae31");
            attribute10.Value = length;
            attribute10.DefaultValue = length;
            attribute10.ID = null;

            AttributeType attribute11 = (AttributeType)_myDoc.FindByID("5dd0a2b4-4457-4d89-aed9-4f79b4418559");
            attribute11.Value = descShort;
            attribute11.DefaultValue = descShort;
            attribute11.ID = null;

            AttributeType attribute12 = (AttributeType)_myDoc.FindByID("333be623-a837-445d-b9fb-bd8f558f553c");
            attribute12.Value = descLong;
            attribute12.DefaultValue = descLong;
            attribute12.ID = null;

            //===================================================================================================

            AttributeType attribute13 = (AttributeType)_myDoc.FindByID("720879e9-c3f7-4f47-8282-02bee9efd2bf");
            attribute13.Value = descShort;
            attribute13.DefaultValue = descShort;
            attribute13.ID = null;

            AttributeType attribute14 = (AttributeType)_myDoc.FindByID("4e1d2248-95e5-4b46-af7c-c6d573df90ad");
            attribute14.Value = descLong;
            attribute14.DefaultValue = descLong;
            attribute14.ID = null;

            AttributeType attribute15 = (AttributeType)_myDoc.FindByID("e4a733cb-0893-4b64-9937-e5ecc0ad9fcc");
            attribute15.Value = manufacturer;
            attribute15.DefaultValue = manufacturer;
            attribute15.ID = null;

            AttributeType attribute16 = (AttributeType)_myDoc.FindByID("6d1c0d18-0d37-4cf1-9c78-a3ddaec81fce");
            attribute16.Value = manwebsite;
            attribute16.DefaultValue = manwebsite;
            attribute16.ID = null;

            //--------------------------------------------------------

            AttributeType attribute17 = (AttributeType)_myDoc.FindByID("46a028b2-1bb7-4897-b863-f05655568df0");
            attribute17.Value = componentPicture;
            attribute17.DefaultValue = componentPicture;
            attribute17.ID = null;

            AttributeType attribute18 = (AttributeType)_myDoc.FindByID("1247913d-2ab0-4c4c-beab-b824502e80e3");
            attribute18.Value = file2dgm;
            //   attribute18.DefaultValue = file2dgm;    
            attribute18.ID = null;

            AttributeType attribute19 = (AttributeType)_myDoc.FindByID("7a22dce2-d949-46ca-99a1-0bd04d0ee669");
            attribute19.Value = file3dgm;
            attribute19.ID = null;

            AttributeType attribute20 = (AttributeType)_myDoc.FindByID("e42cf0a9-8bee-40c6-b11c-3857cf3d9627");
            attribute20.DefaultValue = eplanpath;
            attribute20.Value = eplanpath;
            attribute20.ID = null;

            AttributeType attribute21 = (AttributeType)_myDoc.FindByID("a9718945-4e2e-410d-8b38-9f1c14355851");
            attribute21.Value = contactphone;
            attribute21.DefaultValue = contactphone;
            attribute21.ID = null;

            AttributeType attribute22 = (AttributeType)_myDoc.FindByID("41bc1be5-b249-499d-b5a4-3db97d1912f9");
            attribute22.Value = contactmail;
            attribute22.DefaultValue = contactmail;
            attribute22.ID = null;

            AttributeType attribute23 = (AttributeType)_myDoc.FindByID("932070c1-3dce-4157-8596-724314521846");
            attribute23.Value = country;
            attribute23.DefaultValue = country;
            attribute23.ID = null;

            AttributeType attribute24 = (AttributeType)_myDoc.FindByID("afe7fb89-17eb-458d-9c23-f1d0e398a72f");
            attribute24.Value = city;
            attribute24.DefaultValue = city;
            attribute24.ID = null;

            AttributeType attribute25 = (AttributeType)_myDoc.FindByID("7d930989-9156-432e-b329-6876d1095ad7");
            attribute25.Value = zipcode;
            attribute25.DefaultValue = zipcode;
            attribute25.ID = null;

            AttributeType attribute26 = (AttributeType)_myDoc.FindByID("b87b2747-3f3a-4c12-9157-eb473b2143c1");
            attribute26.Value = address2;
            attribute26.DefaultValue = address2;
            attribute26.ID = null;

            AttributeType attribute27 = (AttributeType)_myDoc.FindByID("96cd5c22-a301-49e2-8d01-3bf0b6735152");
            attribute27.Value = address1;
            attribute27.DefaultValue = address1;
            attribute27.ID = null;

            AttributeType attribute28 = (AttributeType)_myDoc.FindByID("cbc9e0e2-b141-477c-9e2d-f6734c085afb");
            attribute28.Value = voltage;
            attribute28.DefaultValue = voltage;
            attribute28.ID = null;

            AttributeType attribute29 = (AttributeType)_myDoc.FindByID("81b3cfef-ae59-4f9c-9a5c-5edbfe01a50e");
            attribute29.Value = current;
            attribute29.DefaultValue = current;
            attribute29.ID = null;

            AttributeType attribute30 = (AttributeType)_myDoc.FindByID("fb3b8a92-ef6c-41b1-9e5d-389721f1bd6c");
            attribute30.Value = ipcode;
            attribute30.DefaultValue = ipcode;
            attribute30.ID = null;

            AttributeType attribute31 = (AttributeType)_myDoc.FindByID("a3b738c4-e213-4f75-aa8b-1f55dbdc025d");
            attribute31.Value = ipcode;
            attribute31.DefaultValue = ipcode;
            attribute31.ID = null;

            AttributeType attribute32 = (AttributeType)_myDoc.FindByID("aed70179-05f2-4993-b28c-3432e62d85bc");
            attribute32.Value = material;
            attribute32.DefaultValue = material;
            attribute32.ID = null;

        }

        //Fill the template module with their respective values
        private void writeTemplateMODULE(string partnr, string manufacturer, string manwebsite, string devclass, string model, string prodcode,
            string product_permalink, string eplanpath, string componentname, string componentPicture, string file2dgm, string file3dgm)
        {
            // Create a new CAEX document
            _myDoc = CAEXDocument.New_CAEXDocument();

            string pathTemplateAML = _templateFileIOMODULE;

            _myDoc = CAEXDocument.LoadFromFile(pathTemplateAML);

            _internalElementType_BUSNODE_OR_MODULE = (InternalElementType)_myDoc.FindByID("1c5466db-5db2-423e-a3a4-c25b18bf9398");

            SystemUnitClassType systemUnitClass = (SystemUnitClassType)_myDoc.FindByID("46ad3004-57fe-435a-896b-e0b60dc72a76");
            systemUnitClass.Name = componentname;

            AttributeType attribute1 = (AttributeType)_myDoc.FindByID("1e87b904-56b6-4578-86e7-f8413b07b81e");
            attribute1.Value = manufacturer;
            attribute1.ID = null;

            AttributeType attribute2 = (AttributeType)_myDoc.FindByID("238900cc-3d05-44f8-b757-b0f46f2cb14d");
            attribute2.Value = manwebsite;
            attribute2.ID = null;

            AttributeType attribute3 = (AttributeType)_myDoc.FindByID("516e9423-a677-44b0-856d-8418e33d4db8");
            attribute3.Value = devclass;
            attribute3.ID = null;

            AttributeType attribute4 = (AttributeType)_myDoc.FindByID("6e757d7e-a483-44d5-97e1-e7203aa450d0");
            attribute4.Value = model;
            attribute4.ID = null;

            AttributeType attribute5 = (AttributeType)_myDoc.FindByID("c7422c53-d5f7-412e-82ad-d1a87481f51c");
            attribute5.Value = prodcode;
            attribute5.ID = null;

            AttributeType attribute6 = (AttributeType)_myDoc.FindByID("8beed8c3-d4a0-4203-89d7-86b8f39e2c1a");
            attribute6.Value = product_permalink;
            attribute6.ID = null;

            AttributeType attribute7 = (AttributeType)_myDoc.FindByID("e42cf0a9-8bee-40c6-b11c-3857cf3d9627");
            attribute7.Value = eplanpath;
            attribute7.ID = null;

            AttributeType attribute8 = (AttributeType)_myDoc.FindByID("46a028b2-1bb7-4897-b863-f05655568df0");
            attribute8.Value = componentPicture;
            attribute8.ID = null;

            AttributeType attribute9 = (AttributeType)_myDoc.FindByID("1247913d-2ab0-4c4c-beab-b824502e80e3");
            attribute9.Value = file2dgm;
            attribute9.ID = null;

            AttributeType attribute10 = (AttributeType)_myDoc.FindByID("7a22dce2-d949-46ca-99a1-0bd04d0ee669");
            attribute10.Value = file3dgm;
            attribute10.ID = null;
        }

        //Return param of BusType to aml
        private string getBusType(string plcbussystem)
        {
            if (plcbussystem == "0" || plcbussystem == null)
            {
                return "Unknown";
            }
            else
            {
                for (int i = 0; i < _busTypeDataTable.Rows.Count; i++)
                {
                    string value = _busTypeDataTable.Rows[i][0].ToString();
                    if (value.Trim() == plcbussystem.Trim())
                    {
                        return _busTypeDataTable.Rows[i][1].ToString();
                    }
                }
            }

            return "Unknown";
        }

        //Return param of FunctionDefinition to aml
        private string getFunctionDefinition(string connectiondescription, string functiondefcategory, string functiondefgroup, string functiondefid, int paramToReturn)
        {
            string paramSearch = functiondefcategory.Trim() + "." + functiondefgroup.Trim() + "." + functiondefid.Trim();

            if (connectiondescription == null)
            {
                for (int i = 0; i < _functionsDefinitionsDataTable.Rows.Count; i++)
                {
                    string value = _functionsDefinitionsDataTable.Rows[i][0].ToString();
                    if (value.Trim() == paramSearch)
                    {
                        return _functionsDefinitionsDataTable.Rows[i][paramToReturn].ToString();
                    }
                }
            }
            else
            {
                return connectiondescription;
            }

            return "";
        }

        //Add the pins to the respective connector
        private void addPinInAML(DataForCreatePin dataForCreatePin)
        {

            ExternalInterfaceType externalInterfacePIN = (ExternalInterfaceType)_myDoc.FindByID(dataForCreatePin.externalInterface_id);

            var externalInterface = externalInterfacePIN.ExternalInterface.Append();
            externalInterface.Name = dataForCreatePin.connectionDesignation;
            externalInterface.ID = Guid.NewGuid().ToString();
            externalInterface.RefBaseClassPath = "AutomationMLComponentBaseICL/ElectricInterface";

            //attributes
            var attribute1 = externalInterface.Attribute.Append();
            attribute1.Name = "ContactMaterial";
            attribute1.AttributeDataType = "xs:string";
            attribute1.RefAttributeType = "";
            attribute1.Description = "Code of the material of the body of the contacts of a connector, relay or switch.";
            attribute1.New_RefSemantic("IRDI:0112/2///61360_4#AAE355#001");

            var attribute2 = externalInterface.Attribute.Append();
            attribute2.Name = "ContactFinish";
            attribute2.AttributeDataType = "xs:string";
            attribute2.RefAttributeType = "";
            attribute2.Description = "Code of the finish or surface material of the contacts of a connector, relay or switch.";
            attribute2.New_RefSemantic("IRDI:0112/2///61360_4#AAE350#001");

            var attribute3 = externalInterface.Attribute.Append();
            attribute3.Name = "ContactCurrentMax";
            attribute3.AttributeDataType = "xs:float";
            attribute3.Unit = "A";
            attribute3.RefAttributeType = "";
            attribute3.Description = "Maximum continuous rms current per contact of a connector, at specified ambient temperature";
            attribute3.New_RefSemantic("IRDI:0112/2///61360_4#AAE358#001");

            var attribute4 = externalInterface.Attribute.Append();
            attribute4.Name = "FunctionType";
            attribute4.AttributeDataType = "xs:string";
            attribute4.RefAttributeType = "";
            attribute4.Description = "Function description of the pin or contact.";
            attribute4.Value = dataForCreatePin.functionType;

        }

        //Add the new connector
        private void addConnectorInAML(string econnectorname, string connectorref, string busType, string connectortype, string voltage, string current, bool _isNewTerminalNr, string coding = "", string gender = "", string connectordesign = "", string angle = "", string polenr = "")
        {
            if (_isNewTerminalNr)
            {
                //_internalElementType_CONNECTOR = _internalElementType_BUSNODE_OR_MODULE.InternalElement.Append();
                //_internalElementType_CONNECTOR.Name = econnectorname;
                //_internalElementType_CONNECTOR.ID = Guid.NewGuid().ToString();

                _internalElementType_CONNECTOR = _internalElementType_BUSNODE_OR_MODULE.ExternalInterface.Append();
                _internalElementType_CONNECTOR.Name = econnectorname;
                _internalElementType_CONNECTOR.ID = Guid.NewGuid().ToString();
                _internalElementType_CONNECTOR.RefBaseClassPath = "AutomationMLComponentBaseICL/ElectricInterface";
            }

            //_externalInterfaceType_PIN = _internalElementType_CONNECTOR.ExternalInterface.Append();
            //_externalInterfaceType_PIN.Name = connectortype;
            //_externalInterfaceType_PIN.ID = Guid.NewGuid().ToString();
            //_externalInterfaceType_PIN.RefBaseClassPath = connectorref;

            //attributes
            var attribute1 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute1.Name = "Coding";
            attribute1.AttributeDataType = "xs:string";
            attribute1.Description = "Component device that prevents incorrect linking - it is, for example, required if two or several equal plug connectors are mounted to the same device and a commutability is not desired";
            attribute1.DefaultValue = coding;
            attribute1.Value = coding;
            attribute1.New_RefSemantic("IRDI:0173-1#02-AAC076#009");
            attribute1.New_RefSemantic("IRDI-PATH://0173-1---BASIC_1_1%2301-AAB572%23006 /0173-1%2301-AAB654%23009/0173-1%2301-AAB657%23010/0173-1%2301-AFR601%23007/0173-1%2302-AAC076%23009");

            var attribute2 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute2.Name = "Type of plug-in contact";
            attribute2.AttributeDataType = "xs:string";
            attribute2.Description = "connector on the side of the second head (sensor/actuator level) is designed (male/female) in such a way that an electrical connection is implemented either via the interior or the external surface";
            attribute2.DefaultValue = gender;
            attribute2.Value = gender;
            attribute2.New_RefSemantic("IRDI:0173-1#02-AAB379#008");
            attribute2.New_RefSemantic("IRDI-PATH://0173-1---BASIC_1_1%2301-AAB572%23006 /0173-1%2301-AAB654%23009/0173-1%2301-AAB657%23010/0173-1%2301-AFR601%23007/0173-1%2302-AAB379%23008");

            var attribute3 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute3.Name = "Design of the electrical connection";
            attribute3.AttributeDataType = "xs:string";
            attribute3.Description = "Physical design of the housing side to link an electrical operating unit to another in conformity with the intended use";
            attribute3.DefaultValue = connectordesign;
            attribute3.Value = connectordesign;
            attribute3.New_RefSemantic("IRDI:0173-1#02-AAB418#010");
            attribute3.New_RefSemantic("IRDI-PATH://0173-1---BASIC_1_1%2301-AAB572%23006 /0173-1%2301-AAB654%23009/0173-1%2301-AAB657%23010/0173-1%2301-AFR601%23007/0173-1%2302-AAB418%23010");

            var attribute4 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute4.Name = "arrangement of the cable lead-in";
            attribute4.AttributeDataType = "xs:string";
            attribute4.Description = "angle positions (head 2) that enable cable lead-inE";
            attribute4.DefaultValue = angle;
            attribute4.Value = angle;
            attribute4.New_RefSemantic("IRDI:0173-1#02-AAB225#009");
            attribute4.New_RefSemantic("IRDI-PATH://0173-1---BASIC_1_1%2301-AAB572%23006 /0173-1%2301-AAB654%23009/0173-1%2301-AAB657%23010/0173-1%2301-AFR601%23007/0173-1%2302-AAB225%23009");

            var attribute5 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute5.Name = "Pole number";
            attribute5.AttributeDataType = "xs:int";
            attribute5.Description = "Quantitative information on the number of current paths";
            attribute5.DefaultValue = polenr;
            attribute5.Value = polenr;
            attribute5.New_RefSemantic("IRDI:0173-1#02-AAT080#002");
            attribute5.New_RefSemantic("IRDI-PATH://0173-1---BASIC_1_1%2301-AAB572%23006 /0173-1%2301-AAB654%23009/0173-1%2301-AAB657%23010/0173-1%2301-AFR601%23007/0173-1%2302-AAT080%23002");

            var attribute6 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute6.Name = "ContactResistance";
            attribute6.AttributeDataType = "xs:float";
            attribute6.Unit = "Ω";
            attribute6.Description = "Maximum resistance of a mated set of contacts of a connector, relay or switch";
            attribute6.New_RefSemantic("IRDI:0112/2///61360_4#AAE920#001");
            attribute6.New_RefSemantic("IEC61076-2-111Ed1.0/6.4.5");

            var attribute7 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute7.Name = "InsulationResistance";
            attribute7.AttributeDataType = "xs:float";
            attribute7.Unit = "Ω";
            attribute7.Description = "minimum resistance between one terminal or several terminals connected together and the case or enclosure of a component at specified voltage";
            attribute7.New_RefSemantic("IRDI:0112/2///61360_4#AAE155#001");
            attribute7.New_RefSemantic("IEC61076-2-111Ed1.0/6.4.6");

            var attribute8 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute8.Name = "MechanicalOperations";
            attribute8.AttributeDataType = "xs:integer";
            attribute8.Description = "number of mechanical transactions of an item ranging from one defined status in a different one, induced by an external control information.";
            attribute8.New_RefSemantic("IRDI:0112/2///61360_4#AAE361#001");
            attribute8.New_RefSemantic("IRDI:0112/2///61360_4#ADA109#001");
            attribute8.New_RefSemantic("IEC61076-2-111Ed1.0/6.5.1");

            var attribute9 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute9.Name = "InsertionForce";
            attribute9.AttributeDataType = "xs:float";
            attribute9.Unit = "N";
            attribute9.Description = "maximum force required to engage a connector with its mating part, excluding the effect of a coupling, locking or similar device";
            attribute9.New_RefSemantic("IRDI:0112/2///61360_4#AAH067#002");
            attribute9.New_RefSemantic("IEC61076-2-111Ed1.0/6.5.3");

            var attribute10 = _internalElementType_CONNECTOR.Attribute.Append();
            attribute10.Name = "WithdrawalForce";
            attribute10.AttributeDataType = "xs:float";
            attribute10.Unit = "N";
            attribute10.Description = "minimum force required to separate a connector from its mating part, excluding the effect of a coupling, locking or similar device";
            attribute10.New_RefSemantic("IRDI:0112/2///61360_4#AAH068#002");
            attribute10.New_RefSemantic("IEC61076-2-111Ed1.0/6.4.3");


            //var roleRequeriments = _internalElementType_CONNECTOR.RoleRequirements.Append();
            //roleRequeriments.RefBaseRoleClassPath = "AutomationMLComponentStandardRCL/ElectricConnector";
            //var externalInterface2 = roleRequeriments.ExternalInterface.Append();
            //externalInterface2.Name = "ElectricInterface";
            //externalInterface2.ID = Guid.NewGuid().ToString();
            //externalInterface2.RefBaseClassPath = "AutomationMLComponentBaseICL/ElectricInterface";
        }

        //Import the csv's with the references to assemble the functiondefinition
        private void importFunctionsDefinitionsCSV()
        {
            string pathCSV = _appDataMURR + @"\Templates\function_definitions.csv";

            _functionsDefinitionsDataTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(pathCSV)), true))
            {
                _functionsDefinitionsDataTable.Load(csvReader);
            }
        }

        //Import the csv's with the references to assemble the bustype
        private void importBusTypeCSV()
        {
            string pathCSV = _appDataMURR + @"\Templates\bustype.csv";

            _busTypeDataTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(pathCSV)), true))
            {
                _busTypeDataTable.Load(csvReader);
            }

        }

        //Import the csv's with the references to assemble the manufacturers
        private void importManufacturersCSV()
        {

            string pathCSV = _appDataMURR + @"\Templates\manufacturers.csv";

            _manufacturers = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(pathCSV)), true))
            {
                _manufacturers.Load(csvReader);
            }
        }

        //Save an MLX file and add the respective files
        private void saveAMLX()
        {
            if (File.Exists(_pathAMLDestinationDirectory + "\\" + _AMLXFileName))
            {
                File.Delete(_pathAMLDestinationDirectory + "\\" + _AMLXFileName);
            }

            try
            {
                _myDoc.SaveToFile(_pathAMLDestinationDirectory + "\\" + _AMLFileName, true);
                AutomationMLContainer amlx = new AutomationMLContainer(_pathAMLDestinationDirectory + "\\" + _AMLXFileName, FileMode.OpenOrCreate);
                // create the PackageUri for the root amlx file
                Uri partUri = PackUriHelper.CreatePartUri(new Uri("/" + _AMLFileName, UriKind.Relative));
                amlx.AddRoot(_pathAMLDestinationDirectory + "\\" + _AMLFileName, partUri);

                if (_ComponentPicture != "" && _ComponentPicture != null)
                {
                    Uri partUriPicture = PackUriHelper.CreatePartUri(new Uri(_componentPictureRoot, UriKind.Relative));
                    amlx.AddRoot(_ComponentPicture, partUriPicture);
                }

                if (_file2dgmRoot != "" && _file2dgmRoot!=null)
                {
                    Uri partUriFile2dgm = PackUriHelper.CreatePartUri(new Uri(_file2dgmRoot, UriKind.Relative));
                    amlx.AddRoot(_file2dgm, partUriFile2dgm);
                }

                if (_file3dgmRoot != "" && _file3dgmRoot != null)
                {
                    Uri partUriFile3dgm = PackUriHelper.CreatePartUri(new Uri(_file3dgmRoot, UriKind.Relative));
                    amlx.AddRoot(_file3dgm, partUriFile3dgm);
                }

                Uri partUriEDZ = PackUriHelper.CreatePartUri(new Uri(_sugestFilename + ".edz", UriKind.Relative));
                amlx.AddRoot(_currentCompressedEDZFile, partUriEDZ);

                amlx.Save();
                amlx.Close();
                File.Delete(_pathAMLDestinationDirectory + "\\" + _AMLFileName);

            }
            catch (Exception ex)
            {
                File.Delete(_pathAMLDestinationDirectory + "\\" + _AMLFileName);
            }
        }

    }
}
