using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace timesdposada.Functions.Entities
{
    public class TimeEntity : TableEntity
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public bool IsConsolidated { get; set; }  
    } 
}
