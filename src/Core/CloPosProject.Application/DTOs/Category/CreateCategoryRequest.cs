using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Category
{
    public record CreateCategoryRequest(string Name,string Description,int DisplayOrder);
}
