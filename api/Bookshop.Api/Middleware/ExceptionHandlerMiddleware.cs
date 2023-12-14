using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Bookshop.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {RequestType}", typeof(HttpContext).Name);
                await ConvertException(context, ex);
            }
        }

        private static Task ConvertException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var result = new ErrorResponse();

            switch (exception)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = validationException.CreateErrorResponse();
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = badRequestException.CreateErrorResponse();
                    break;
                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    result = notFoundException.CreateErrorResponse();
                    break;
                case Exception ex:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = new ErrorResponse(ex.Message);
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string json = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return context.Response.WriteAsync(json);
        }
    }
}
