using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }= string.Empty;
        public string Username { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        public Roles Role { get; set; }
        public bool isActive { get; set; } = true;

    }
}
