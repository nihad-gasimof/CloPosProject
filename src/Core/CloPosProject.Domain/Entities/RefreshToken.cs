using CloPosProject.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
