using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "TANGO_CP_FORMAT")]
public class TangoCpFormat
{
    [XmlElement(ElementName = "HEADER")]
    public Header Header { get; set; }

    [XmlElement(ElementName = "LIMITS")]
    public Limits Limits { get; set; }

    [XmlElement(ElementName = "DIEDATA")]
    public DieData DieData { get; set; }
}

public class Header
{
    [XmlElement(ElementName = "VERSION")]
    public string Version { get; set; }

    [XmlElement(ElementName = "LOT_ID")]
    public string LotId { get; set; }

    [XmlElement(ElementName = "OP_NAME")]
    public string OpName { get; set; }

    [XmlElement(ElementName = "WAF_NO")]
    public int WafNo { get; set; }

    [XmlElement(ElementName = "WAFER_ID")]
    public string WaferId { get; set; }

    [XmlElement(ElementName = "PRODUCT_ID")]
    public string ProductId { get; set; }

    [XmlElement(ElementName = "GROSS_DIE")]
    public int GrossDie { get; set; }

    [XmlElement(ElementName = "TEST_DIE")]
    public int TestDie { get; set; }

    [XmlElement(ElementName = "PASS_CNT")]
    public int PassCount { get; set; }

    [XmlElement(ElementName = "EQP_ID")]
    public string EqpId { get; set; }

    [XmlElement(ElementName = "EQP_NAME")]
    public string EqpName { get; set; }

    [XmlElement(ElementName = "SUBSYS_ID")]
    public string SubsysId { get; set; }

    [XmlElement(ElementName = "OPERATOR_ID")]
    public string OperatorId { get; set; }

    [XmlElement(ElementName = "TEST_PG")]
    public string TestPg { get; set; }

    [XmlElement(ElementName = "ST_TIME")]
    public DateTime StartTime { get; set; }

    [XmlElement(ElementName = "END_TIME")]
    public DateTime EndTime { get; set; }

    [XmlElement(ElementName = "PROB_CARD_ID")]
    public string ProbCardId { get; set; }

    [XmlElement(ElementName = "LOAD_BOARD_ID")]
    public string LoadBoardId { get; set; }

    [XmlElement(ElementName = "TEMPERATURE")]
    public int Temperature { get; set; }

    [XmlElement(ElementName = "BIN_DEF_NAME")]
    public string BinDefName { get; set; }

    [XmlElement(ElementName = "VENDOR_ID")]
    public string VendorId { get; set; }

    [XmlElement(ElementName = "VENDORLOT_ID")]
    public string VendorLotId { get; set; }

    [XmlElement(ElementName = "FAB_LOT_ID")]
    public string FabLotId { get; set; }

    [XmlElement(ElementName = "PART_ID")]
    public string PartId { get; set; }

    [XmlElement(ElementName = "NOTCH")]
    public string Notch { get; set; }

    [XmlElement(ElementName = "XYDIR")]
    public int XyDir { get; set; }

    [XmlElement(ElementName = "TEST_VENDOR_ID")]
    public string TestVendorId { get; set; }

    [XmlElement(ElementName = "LOT_TYPE")]
    public string LotType { get; set; }

    [XmlElement(ElementName = "EXTEND_INFO")]
    public string ExtendInfo { get; set; }

    [XmlElement(ElementName = "RAWFILE")]
    public string RawFile { get; set; }
}

public class Limits
{
    [XmlElement(ElementName = "BIN")]
    public List<string> Bin { get; set; }
}

public class BinSum
{
    [XmlElement(ElementName = "BIN")]
    public List<string> Bin { get; set; }
}

public class BinMap
{
    [XmlText]
    public string Bin { get; set; }
}

public class Defect
{
    [XmlText]
    public string Text { get; set; }
}

public class RaData
{
    [XmlText]
    public string Text { get; set; }
}

public class RaSum
{
    [XmlText]
    public string Text { get; set; }
}

public class DataLog
{
    [XmlText]
    public string Text { get; set; }
}

public class FailureCount
{
    [XmlText]
    public string Text { get; set; }
}

public class DieData
{
    [XmlElement(ElementName = "BINSUM")]
    public BinSum BinSum { get; set; }

    [XmlElement(ElementName = "BINMAP")]
    public BinMap BinMap { get; set; }

    [XmlElement(ElementName = "DEFECT")]
    public Defect Defect { get; set; }

    [XmlElement(ElementName = "RADATA")]
    public RaData RaData { get; set; }

    [XmlElement(ElementName = "RASUM")]
    public RaSum RaSum { get; set; }

    [XmlElement(ElementName = "DATALOG")]
    public DataLog DataLog { get; set; }

    [XmlElement(ElementName = "FAILURE_CNT")]
    public FailureCount FailureCount { get; set; }
}
