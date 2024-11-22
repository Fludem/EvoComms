namespace EvoComms.Devices.HanvonVF.Messages;

public class PARAM
{
    public string alg_edition { get; set; }
    public string id { get; set; }
    public int identifyType { get; set; }
    public bool isUserUpdate { get; set; }
    public string name { get; set; }
    public int pass { get; set; }
    public int recogPermission { get; set; }
    public int recogType { get; set; }
    public string score { get; set; }
    public string sn { get; set; }
    public int state { get; set; }
    public int tablePermission { get; set; }
    public string threshold { get; set; }
    public string time { get; set; }
    public string type { get; set; }
    public int wearMask { get; set; }
    public int workCode { get; set; }
    public int workStatus { get; set; }
}

public class HanvonMessage
{
    public string COMMAND { get; set; }
    public PARAM PARAM { get; set; }
}