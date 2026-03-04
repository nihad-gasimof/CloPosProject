using CloPosProject.Application.BaseResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Ai
{
    public interface IAdminAIService
    {
        Task<SimpleResponse<string>> ProcessAdminRequestAsync(string adminQuery);
    }
}
