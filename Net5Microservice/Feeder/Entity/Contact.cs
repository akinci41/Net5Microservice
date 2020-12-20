using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feeder.Entity
{
    public class Contact
    {
        [Key]
        public Guid ID { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Surname { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string FirmName { get; set; }
    }
}
