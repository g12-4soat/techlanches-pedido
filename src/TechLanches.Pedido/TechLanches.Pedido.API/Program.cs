using Polly;
using Polly.Extensions.Http;
using TechLanches.Adapter.API.Configuration;
using TechLanches.Adapter.API.Options;
using TechLanches.Adapter.AWS.SecretsManager;
using TechLanches.Adapter.RabbitMq.Options;
using TechLanches.Adapter.SqlServer;
using TechLanches.Application.Constantes;
using TechLanches.Pedido.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
    .AddEnvironmentVariables();

//AWS Secrets Manager
builder.Configuration
    .AddAmazonSecretsManager("us-east-1", "database-credentials")
    .AddAmazonSecretsManager("us-east-1", "lambda-auth-credentials");

builder.Services.Configure<TechLanchesPedidoDatabaseSecrets>(builder.Configuration);
builder.Services.Configure<TechLanchesCognitoSecrets>(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Add cognito auth
builder.Services.Configure<AuthenticationCognitoOptions>(builder.Configuration.GetSection("Authentication"));
builder.Services.AddAuthenticationConfig();

//Setting Swagger
builder.Services.AddSwaggerConfiguration();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(60);
    });
}



builder.Services.AddMemoryCache();

//DI Abstraction
builder.Services.AddDependencyInjectionConfiguration();

//Setting DBContext
builder.Services.AddDatabaseConfiguration();

//Setting mapster
builder.Services.RegisterMaps();

builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddHostedService<PedidoConsumerHostedService>();

//Setting healthcheck
builder.Services.AddHealthCheckConfig(builder.Configuration);

//Criar uma politica de retry (tente 3x, com timeout de 3 segundos)
var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));

//Registrar httpclient
builder.Services.AddHttpClient(Constants.NOME_API_PAGAMENTOS, httpClient =>
{
    var url = Environment.GetEnvironmentVariable("PAGAMENTO_SERVICE")!;
    httpClient.BaseAddress = new Uri("http://" + url + ":5055");

}).AddPolicyHandler(retryPolicy);

var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    await next();
});
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.AddCustomMiddlewares();

app.UseDatabaseConfiguration();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerConfiguration();

app.AddHealthCheckEndpoint();

app.UseMapEndpointsConfiguration();

app.UseStaticFiles();

app.Run();