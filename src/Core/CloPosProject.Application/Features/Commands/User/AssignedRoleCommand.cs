using CloPosProject.Application.BaseResponseModel;
using CloPosProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Features.Commands.User
{
    public record AssignedRoleCommand(Guid Id,Roles role):IRequest<SimpleResponse<string>>
    {
    }
   
}
