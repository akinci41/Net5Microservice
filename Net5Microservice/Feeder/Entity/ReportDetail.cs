using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feeder.Entity
{
    public class ReportDetail
    {
        [Key]
        public int ID { get; set; }
        public Guid ReportID { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string Location { get; set; }
        public int ContactCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
