using CloPosProject.Application.BaseResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.User
{
    public record ConfirmEmailCommand(string userId,string token):IRequest<Response<string>>
    {
    }
}
