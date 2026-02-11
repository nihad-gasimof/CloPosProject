using CloPosProject.Application.Abstract.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.Exceptions.InvalidToken
{
    public class InvalidTokenException : Exception,IBaseException
    {
       

        public InvalidTokenException(string? message="Invalid refresh Token") : base(message)
        {
        }

        public int StatusCode { get; set; } = 401;
    }
}
