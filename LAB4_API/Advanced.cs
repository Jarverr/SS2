using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LAB4_API
{
    public class Advanced
    {
        [Key]
        public int Id { get; set; }
        public string Team { get; set; }
        public string Conference { get; set; }
        public bool excludeGarbageTime { get; set; }
        public int startWeek { get; set; }
        public int endWeek { get; set; }
        public int Year { get; set; }
    }
}
