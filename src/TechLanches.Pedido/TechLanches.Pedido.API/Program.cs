using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;
using TechLanches.Adapter.API.Configuration;
using TechLanches.Adapter.API.Options;
using TechLanches.Adapter.AWS.SecretsManager;
using TechLanches.Adapter.RabbitMq.Options;
using TechLanches.Adapter.SqlServer;
using TechLanches.Application;
using TechLanches.Application.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
    .AddEnvironmentVariables();

//AWS Secrets Manager
builder.WebHost.ConfigureAppConfiguration(((_, configurationBuilder) =>
{
    configurationBuilder.AddAmazonSecretsManager("us-east-1", "database-credentials");
    configurationBuilder.AddAmazonSecretsManager("us-east-1", "lambda-auth-credentials");
}));

builder.Services.Configure<TechLanchesDatabaseSecrets>(builder.Configuration);
builder.Services.Configure<TechLanchesCognitoSecrets>(builder.Configuration);


AppSettings.Configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add cognito auth
builder.Services.Configure<AuthenticationCognitoOptions>(builder.Configuration.GetSection("Authentication"));
builder.Services.AddAuthenticationConfig();

//Setting Swagger
builder.Services.AddSwaggerConfiguration();

//DI Abstraction
builder.Services.AddDependencyInjectionConfiguration();

//Setting DBContext
builder.Services.AddDatabaseConfiguration();

//Setting mapster
builder.Services.RegisterMaps();

builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("ApiMercadoPago"));

//Setting healthcheck
builder.Services.AddHealthCheckConfig(builder.Configuration);

//Criar uma politica de retry (tente 3x, com timeout de 3 segundos)
var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

//Registrar httpclient
builder.Services.AddHttpClient("MercadoPago", httpClient =>
{
    httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration.GetSection($"ApiMercadoPago:AccessToken").Value);
    httpClient.BaseAddress = new Uri(builder.Configuration.GetSection($"ApiMercadoPago:BaseUrl").Value);
}).AddPolicyHandler(retryPolicy);

var app = builder.Build();

app.AddCustomMiddlewares();

app.UseDatabaseConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseSwaggerConfiguration();

app.AddHealthCheckEndpoint();

app.UseMapEndpointsConfiguration();

app.UseStaticFiles();

app.Run();