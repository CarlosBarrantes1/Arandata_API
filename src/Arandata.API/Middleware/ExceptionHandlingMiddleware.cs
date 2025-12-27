using Arandata.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arandata.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, Microsoft.Extensions.Logging.ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;

            // Mostrar el mensaje real del error para depuración
            string message = exception.Message;
            if (exception.InnerException != null)
            {
                message += " | Inner: " + exception.InnerException.Message;
            }

            switch (exception)
            {
                case NotFoundException nf:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = nf.Message ?? "Recurso no encontrado";
                    break;
                case BusinessRuleException br:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = br.Message ?? "Regla de negocio violada";
                    break;
                case DuplicateEntityException de:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = de.Message ?? "Entidad duplicada";
                    break;
                case DomainException d:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = d.Message ?? "Error del dominio";
                    break;
                case DbUpdateException dbu:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Operación no válida en la base de datos";
                    // log details server-side
                    _logger.LogError(dbu, "DbUpdateException en pipeline");
                    break;
                default:
                    // For unexpected exceptions, log details and return generic message
                    _logger.LogError(exception, "Excepción no controlada en pipeline");
                    break;
            }

            context.Response.StatusCode = statusCode;
            var payload = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(payload);
        }
    }
}
