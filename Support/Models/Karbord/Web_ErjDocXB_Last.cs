﻿namespace Support.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Web_ErjDocXB_Last
    {
        public long? SortRjStatus { get; set; }

        public long? SortRjDate { get; set; }

        public long? DocNo { get; set; }

        public string DocDate { get; set; }

        public string Spec { get; set; }

        public string Status { get; set; }

        public long SerialNumber { get; set; }

        public int? DocBStep { get; set; }

        public string RjRadif { get; set; }

        public int? BandNo { get; set; }

        public int? DocBMode { get; set; }

        public string RjDate { get; set; }

        public string RjStatus { get; set; }

        public string RjEndDate { get; set; }

        public string RjMhltDate { get; set; }

        public DateTime? RjUpdateDate { get; set; }

        public string RjUpdateUser { get; set; }

        public int? ErjaCount { get; set; }

        public double? RjTime { get; set; }

        public string RjTimeSt { get; set; }

        public string FromUserCode { get; set; }

        public string FromUserName { get; set; }

        public string ToUserCode { get; set; }

        public string ToUserName { get; set; }

        public string RjReadSt { get; set; }

        public byte DocAttachExists { get; set; }

        public byte? ChatMode { get; set; }

        public bool? ChatActive { get; set; }

        public bool? ChatFinish { get; set; }

        public string RjHour { get; set; }
    }
}