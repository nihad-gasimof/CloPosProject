using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.DTOs.Cloudinary
{
    public class CloudinaryOptionsDto
    {
        public string CloudName { get; set; }=string.Empty;
        public string ApiKey { get; set; }=string.Empty;
        public string ApiSecret { get; set; }=string.Empty;
    }
}
