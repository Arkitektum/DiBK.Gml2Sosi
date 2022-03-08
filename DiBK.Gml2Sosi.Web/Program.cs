using DiBK.Gml2Sosi.Application.Config;
using DiBK.Gml2Sosi.Application.Services.MultipartRequest;
using DiBK.Gml2Sosi.Reguleringsplanforslag.Config;
using DiBK.Gml2Sosi.Web.Configuration;
using DiBK.Gml2Sosi.Web.Middleware;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using OSGeo.OGR;
using Serilog;
using System.Globalization;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = new[] { "text/vnd.sosi" };
    options.Providers.Add<GzipCompressionProvider>();
});

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DiBK GML to SOSI Converter", Version = "v1" });
    options.OperationFilter<MultipartOperationFilter>();
});

services.AddHttpContextAccessor();
services.AddTransient<IMultipartRequestService, MultipartRequestService>();

services.AddApplicationServices();
services.AddApplicationServicesForReguleringsplanforslag();
services.ConfigureDatasets(configuration);

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger, true);

Ogr.RegisterAll();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

app.UseResponseCompression();

app.UseMiddleware<SerilogMiddleware>();

app.Use(async (context, next) => {
    context.Request.EnableBuffering();
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
