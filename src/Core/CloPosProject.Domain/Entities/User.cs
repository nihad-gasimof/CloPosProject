using CloPosProject.Domain.Entities.Base;
using CloPosProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Domain.Entities
{
    public class User :IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
