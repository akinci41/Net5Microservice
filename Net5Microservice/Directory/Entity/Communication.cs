using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Directory.Entity
{
    public class Communication
    {
        public Communication(Guid contactID, string type, string content)
        {
            ID = Guid.NewGuid();
            ContactID = contactID;
            Type = type;
            Content = content;
        }

        [Key]

        public Guid ID { get; set; }
        public Guid ContactID { get; set; }

        [Column(TypeName = "char(1)")]
        public string Type { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string Content { get; set; }
    }
}
