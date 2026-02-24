

using EmiSchedular.Application.Common;
using EmiSchedularService.Application.Common.Exceptions;
using System.Net;

namespace CryptoHub.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            this._logger = logger;
            this._next = next;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);


            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();
                //Log the Exception in File That We Create to maintain Log
                _logger.LogError(ex, "{ErrorId} : {Message}", errorId, ex.Message);
                //Return Custome Exception

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";

                ApiResponse<string> response;

                switch (ex)
                {
                    case NotFoundException:
                        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                        response = ApiResponse<string>.FailureResponse(
                            ex.Message,
                            "NOT_FOUND",
                            null
                        );
                        break;

                    default:
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        if (_webHostEnvironment.IsDevelopment())
                        {
                            response = ApiResponse<string>.FailureResponse(
                                ex.Message,
                                ex.GetType().Name,
                                ex.StackTrace
                            );
                        }
                        else
                        {
                            response = ApiResponse<string>.FailureResponse(
                                "An unexpected error occurred",
                                "INTERNAL_SERVER_ERROR",
                                $"ErrorId: {errorId}"
                            );
                        }
                        break;
                }

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
