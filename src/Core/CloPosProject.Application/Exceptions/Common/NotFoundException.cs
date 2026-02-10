using CloPosProject.Application.Abstract.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Exceptions.Common
{
    public class NotFoundException : Exception, IBaseException
    {
        public NotFoundException(string message="Not Found"):base(message)
        {
            
        }
        
        public int StatusCode { get; set; } = 404;
    }
}
