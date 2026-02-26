using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Category
{
    public record UpdateCategoryRequest(string Name, string Description, int DisplayOrder);
}
