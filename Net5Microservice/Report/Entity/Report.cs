using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Report.Entity
{
    public class Report
    {
        public Report()
        {
            ID = Guid.NewGuid();
            Date = DateTime.Now.Date;
            Status = (int)ReportStatus.Pending;
        }

        [Key]
        public Guid ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }


        [Column(TypeName = "varchar(10)")]
        public int Status { get; set; }
    }

    public enum ReportStatus
    {
        Pending = 0,
        Preparing = 1,
        Ready = 2
    }
}
