using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LAB4_API
{
    public class Teams
    {
        [Key]
        public int Id { get; set; }
        public string School { get; set; }
        public string Abbreviation { get; set; }
        //visual płacze jak nie ma forejn keja
        [ForeignKey("Conference")]
        public string Conference { get; set; }
        public string Team { get; set; }
        public string Divison { get; set; }
        public string Color { get; set; }
        public string Alt_Color { get; set; }

    }
}
