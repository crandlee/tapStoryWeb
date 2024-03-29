﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tapStoryWebData.EF.Models
{
    public class Audit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public AuditTable TableName { get; set; }

        [Required]
        public AuditRecordType RecordType { get; set; }

        [Required, Column(TypeName="datetime2")]
        public DateTime AuditTime { get; set; }

        [Required]
        public int AuditUser { get; set; }

        [MaxLength(200)]
        public string AuditTableId { get; set; }

        [MaxLength(500)]
        public string AdditionalInformation { get; set; }
    }
}
