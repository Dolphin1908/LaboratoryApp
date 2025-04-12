﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Models.English
{
    public class PosModel
    {
        [Column("Id")]
        public long Id { get; set; }
        [Column("WordId")]
        public long WordId { get; set; }
        [Column("Pos")]
        public string Pos { get; set; }
    }
}
