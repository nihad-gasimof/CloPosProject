using CloPosProject.Application.Abstract.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Exceptions.Common
{
    public class NotMinusException : Exception,IBaseException
    {
        public int StatusCode { get; set; } = 400;
        public NotMinusException()
        {
        }

        public NotMinusException(string? message="Deyer sifirdan boyuk olmalidir") : base(message)
        {
        }
    }
}
