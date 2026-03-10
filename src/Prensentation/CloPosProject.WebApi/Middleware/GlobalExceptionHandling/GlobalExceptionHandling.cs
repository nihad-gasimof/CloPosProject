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
                var errors = new List<string>();
                var currentException = ex;

                while (currentException != null)
                {
                    errors.Add(currentException.Message);
                    currentException = currentException.InnerException;
                }

                var response = new SimpleResponse<string>
                {
                    Message = ex.Message,
                    Errors = errors,
                    StatusCode = 500,
                    Success = false
                };

                if (ex is IBaseException baseException)
                {
                    response.StatusCode = baseException.StatusCode;
                }

                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = response.StatusCode;

                await context.Response.WriteAsJsonAsync(response);
            }
        }

    }
}