using CloPosProject.Application.Abstract.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Exceptions.Common
{
    public class NullException : Exception, IBaseException
    {
        public NullException()
        {
        }

        public NullException(string? message="Null Ola bilmez") : base(message)
        {
        }

        public int StatusCode { get; set; }=400;
    }
}
