using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Table
{
    public class TableSummaryResponse
    {
        public Guid Id { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
        public string Location { get; set; } = string.Empty;    
        public bool IsActive { get; set; }
    }
}
