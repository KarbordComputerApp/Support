namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Web_ErjDocK
    {
        public long SerialNumber { get; set; }

        public long? DocNo { get; set; }

        public string DocDate { get; set; }

        public string Spec { get; set; }

        public string Status { get; set; }

        public string CustCode { get; set; }

        public int? KhdtCode { get; set; }

        public string Eghdam { get; set; }

        public string Tanzim { get; set; }

        public string DocDesc { get; set; }

        public string FinalComm { get; set; }

        public string EghdamComm { get; set; }

        public string MhltDate { get; set; }

        public string AmalDate { get; set; }

        public string EndDate { get; set; }

        public string SpecialComm { get; set; }

        public short? Mahramaneh { get; set; }

        public string F01 { get; set; }

        public string F02 { get; set; }

        public string F03 { get; set; }

        public string F04 { get; set; }

        public string F05 { get; set; }

        public string F06 { get; set; }

        public string F07 { get; set; }

        public string F08 { get; set; }

        public string F09 { get; set; }

        public string F10 { get; set; }

        public string F11 { get; set; }

        public string F12 { get; set; }

        public string F13 { get; set; }

        public string F14 { get; set; }

        public string F15 { get; set; }

        public string F16 { get; set; }

        public string F17 { get; set; }

        public string F18 { get; set; }

        public string F19 { get; set; }

        public string F20 { get; set; }

        public string CustName { get; set; }

        public string KhdtName { get; set; }

        public int HasNewErja { get; set; }

        public string ToUserCode { get; set; }

        public string MahramanehName { get; set; }

        public double? RjTime { get; set; }

        public string RjTimeSt { get; set; }

        public string RelatedDocs { get; set; }

        public byte? SpecialCommTrs { get; set; }

    }
}