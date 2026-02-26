using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Table
{
    public record CreateTableRequest(
     string TableNumber,
     int Capacity,
     string Location
 );
}
