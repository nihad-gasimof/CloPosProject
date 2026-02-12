using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.Concurate.Encrytping
{
    internal static class SigninCredentialHelper
    {

        public static SigningCredentials CreateSigninCredentials(SecurityKey securityKey)
        {
            return new(securityKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
