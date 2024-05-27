namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Web_ErjDocXK
    {
        /*  [Key]
          public long SerialNumber { get; set; }

          public short? ModeCode { get; set; }

          public long? DocNo { get; set; }

          public string DocDate { get; set; }

          public string Spec { get; set; }

          public string Status { get; set; }

          public string CustName { get; set; }

          public string Eghdam { get; set; }

          public string Tanzim { get; set; }

          public byte DocRead { get; set; }

          public string ResultSt { get; set; }

          public bool? ResultRead { get; set; }

          public byte ChatMode { get; set; }

          public bool? ChatActive { get; set; }

          public bool? ChatFinish { get; set; }

          public bool? ChatDownload { get; set; }

          public string LinkSt { get; set; }

          public string LockNo { get; set; }

          public string Text { get; set; }

          public string Motaghazi { get; set; }

          public byte DocAttachExists { get; set; }


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


          public string EghdamName { get; set; }

          public string TanzimName { get; set; }*/

        [Key]
        public byte DocAttachExists { get; set; }

        public string LockNo { get; set; }

        public string Text { get; set; }

        public string Motaghazi { get; set; }

        [Key]

        public long SerialNumber { get; set; }

        public short? ModeCode { get; set; }

        public long? DocNo { get; set; }

        public string CustName { get; set; }

        public string DocDate { get; set; }


        public string Spec { get; set; }

        
        public string Status { get; set; }

        public byte? ChatMode { get; set; }

        public bool? ChatActive { get; set; }

        public bool? ChatFinish { get; set; }

        public bool? ChatDownload { get; set; }

        
        public string LinkSt { get; set; }

        public bool? DocRead { get; set; }

        public bool? ResultRead { get; set; }

        
        public string ResultSt { get; set; }

        
        public string Eghdam { get; set; }

        
        public string Tanzim { get; set; }

        
        public string DocDesc { get; set; }

        
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

        [Key]
        [Column(Order = 2)]
        
        public string EghdamName { get; set; }

        [Key]
        [Column(Order = 3)]
        
        public string TanzimName { get; set; }

    }
}
