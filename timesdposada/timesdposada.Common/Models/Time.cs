using System;

namespace timesdposada.Common.Models
{
    public class Time
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public bool Consolidate { get; set; }
    }
}
