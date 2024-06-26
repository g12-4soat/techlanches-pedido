﻿using System.Net;
using System.Text.Json;
using TechLanches.Application.DTOs;
using TechLanches.Core;

namespace TechLanches.Adapter.API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionType = exception.GetType();

            var status = exceptionType switch
            {
                Type t when t == typeof(DomainException) => HttpStatusCode.BadRequest,
                Type t when t == typeof(NotImplementedException) => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError,
            };

            var message = (status == HttpStatusCode.InternalServerError) ? "Erro interno. Contate um administrador do sistema." : exception.Message;

            var exceptionResult = JsonSerializer.Serialize(new ErrorResponseDTO { MensagemErro = message, StatusCode = status });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}