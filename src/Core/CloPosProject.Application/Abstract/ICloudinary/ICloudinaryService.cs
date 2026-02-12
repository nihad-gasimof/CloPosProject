using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.ICloudinary
{
    public interface ICloudinaryService
    {
        Task<string> FileCreateAsync(IFormFile file);
        Task<bool> FileDeleteAsync(string Filepath);
    }
}
