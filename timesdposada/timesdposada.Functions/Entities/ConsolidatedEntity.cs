using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace timesdposada.Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public int MinutesCompleted { get; set; }
    }
}
