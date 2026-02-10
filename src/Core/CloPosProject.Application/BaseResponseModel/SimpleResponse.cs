using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.BaseResponseModel
{
    public class SimpleResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; }

        public SimpleResponse()
        {
            Success = true;
            Message = "Successfully";
            Data = default;
            Errors = new List<string>();
            StatusCode = 200;
        }

        public SimpleResponse(T data)
        {
            Success = true;
            Message = string.Empty;
            Data = data;
            Errors = new List<string>();
            StatusCode = 200;
        }

        public SimpleResponse(string message, T data)
        {
            Success = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
            StatusCode = 200;
        }

        public SimpleResponse(string message, List<string> errors, int statusCode = 400)
        {
            Success = false;
            Message = message;
            Data = default;
            Errors = errors ?? new List<string>();
            StatusCode = statusCode;
        }

        public SimpleResponse( string message, T? data, List<string>? errors, int statusCode)
        {
            Success = false;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
            StatusCode = statusCode;

        }
    }
}


