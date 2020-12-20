﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Directory.Entity
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

        public List<Communication> CommunicationList { get; set; }
    }
}
