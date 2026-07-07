using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessCore.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Ocurrió un error no controlado: {Message}", exception.Message);

            var response = new ApiResponseDto<object>
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Ocurrió un error interno en el servidor"
            };

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = notFoundEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { notFoundEx.Message };
                    break;

                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Error de validación";
                    response.Errors = new System.Collections.Generic.List<string>();
                    foreach (var error in validationEx.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                    break;

                case BusinessException businessEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = businessEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { businessEx.Message };
                    break;

                case UnauthorizedException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = unauthorizedEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { unauthorizedEx.Message };
                    break;

                case ForbiddenException forbiddenEx:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Message = forbiddenEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { forbiddenEx.Message };
                    break;

                case ConflictException conflictEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = conflictEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { conflictEx.Message };
                    break;

                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = badRequestEx.Message;
                    response.Errors = new System.Collections.Generic.List<string> { badRequestEx.Message };
                    break;

                case InfrastructureException infraEx:
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Message = "Error en la infraestructura del sistema";
                    response.Errors = new System.Collections.Generic.List<string> { infraEx.Message };
                    break;

                case DatabaseException dbEx:
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Message = "Error al acceder a la base de datos";
                    response.Errors = new System.Collections.Generic.List<string> { dbEx.Message };
                    break;

                case FileException fileEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Error al procesar el archivo";
                    response.Errors = new System.Collections.Generic.List<string> { fileEx.Message };
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Ocurrió un error interno en el servidor";
                    response.Errors = new System.Collections.Generic.List<string> { exception.Message };
                    break;
            }

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}