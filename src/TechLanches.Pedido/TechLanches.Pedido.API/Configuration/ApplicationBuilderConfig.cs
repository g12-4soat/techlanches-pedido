﻿
using TechLanches.Adapter.API.Middlewares;
using TechLanchesPedido.API.Middlewares;

namespace TechLanches.Adapter.API.Configuration
{
    public static class ApplicationBuilderConfig
    {
        public static IApplicationBuilder AddCustomMiddlewares(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
            applicationBuilder.UseMiddleware<JwtTokenMiddleware>();

            return applicationBuilder;
        }
    }
}