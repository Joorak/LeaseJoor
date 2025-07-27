using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls(builder.Configuration["HostURL"]!.ToString());

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(builder.Configuration));
// تنظیم Serilog برای لاگ‌گیری
//builder.Host.UseSerilog((context, services, configuration) =>
//{
//    configuration
//        .ReadFrom.Configuration(context.Configuration)
//        .ReadFrom.Services(services)
//        .Enrich.FromLogContext()
//        .WriteTo.Console();
//});

// افزودن سرویس‌های Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// افزودن کنترلرها
builder.Services.AddControllers();

// تنظیم OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LeaseJoor API", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter JWT with Bearer into field",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] { }
//        }
//    });
//});

// تنظیم CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    
    //using (var scope = app.Services.CreateScope())
    //{
    //    DbContext db = scope.ServiceProvider.GetRequiredService<IdentityDb>();
    //    await db.Database.MigrateAsync();
    //    db.Database.EnsureCreated();
    //}
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// تنظیم مدیریت خطا
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        var response = RequestResponse.Failure($"An error occurred: {exception?.Message}");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    });
});

// اجرای Seed داده‌ها
if (builder.Configuration.GetValue<bool>("RunSeedingOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await DataSeeder.SeedDataAsync(services);
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
/* use /scalar/v1 at the end of address  */
app.MapScalarApiReference();

//app.Run();
await app.StartAsync();
Console.WriteLine($"Application has started at : {string.Join(", ", app.Urls)}");
await app.WaitForShutdownAsync();