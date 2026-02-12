using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Abstract.Authentication
{
    public interface IJwtGenerator
    {
        AuthResponseDto GenerateToken(List<Claim> claims);
    }
}
