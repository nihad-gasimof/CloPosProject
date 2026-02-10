using Azure;
using CloPosProject.Application.Abstract.Exception;
using CloPosProject.Application.BaseResponseModel;

namespace CloPosProject.WebApi.Middleware.GlobalExceptionHandling
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var Response = new SimpleResponse<string>
                {
                    Message = ex.Message,
                    StatusCode = 500,
                    Success = false,
                };

                if (ex is IBaseException baseException)
                {
                    Response.StatusCode = baseException.StatusCode;
                    Response.Message = ex.Message;
                }
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = Response.StatusCode;
                await context.Response.WriteAsJsonAsync(Response);
            }
        }

    }
}