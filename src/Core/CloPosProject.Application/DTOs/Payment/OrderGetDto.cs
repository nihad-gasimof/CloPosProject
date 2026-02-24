using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Payment
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class OrderGetDto
    {
        public int Id { get; set; }
        public string HppUrl { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Cvv2AuthStatus { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }

    }
