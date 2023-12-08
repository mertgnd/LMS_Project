using LMS_Project.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System;
using LMS_Project.Common;

namespace LMS_Project.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger logger)
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
            catch (BadRequestException badRequestException)
            {
                LogError("BadRequestException: ", badRequestException);
                await ErrorResponseBuilder(context, HttpStatusCode.BadRequest, badRequestException.Message);
            }
            catch (AppException appException)
            {
                LogError("AppException: ", appException);
                await ErrorResponseBuilder(context, HttpStatusCode.InternalServerError, appException.Message);
            }
            catch (NotFoundException notFoundException)
            {
                LogError("NotFoundException: ", notFoundException);
                await ErrorResponseBuilder(context, HttpStatusCode.NotFound, notFoundException.Message);
            }
            catch (ConflictException conflictException)
            {
                LogError("ConflictException: ", conflictException);
                await ErrorResponseBuilder(context, HttpStatusCode.Conflict, conflictException.Message);
            }
            catch (Exception ex)
            {
                LogError("Unhandled Exception: ", ex);
                await ErrorResponseBuilder(context, HttpStatusCode.InternalServerError, $"Internal Server Error - {ex.Message}");
            }
        }

        private void LogError(string Message, Exception ex)
        {
            _logger.LogError(Message, ex);
        }

        private Task ErrorResponseBuilder(HttpContext context, HttpStatusCode statusCode, string message)
        {
            var response = new ErrorResponse { Message = message, StatusCode = (int)statusCode };

            context.Response.StatusCode = (int)statusCode;

            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
