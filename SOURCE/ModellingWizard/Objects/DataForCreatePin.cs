namespace ModellingWizard.Objects
{
    public class DataForCreatePin
    {
        public string connectionDesignation { get; set; }
        public string functionType { get; set; }
        public string externalInterface_id { get; set; }

        public DataForCreatePin(string _connectionDesignation, string _functionType, string _externalInterface_id = null)
        {
            connectionDesignation = _connectionDesignation;
            functionType = _functionType;
            externalInterface_id = _externalInterface_id;
        }
    }
}

