using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Authentication
{
    public record AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiredAt { get; set; }
    }
}
