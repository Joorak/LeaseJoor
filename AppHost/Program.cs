using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var configuration = builder.Configuration;
//var apiService = builder.AddProject<Projects.WebApi>("webapi").WithHttpEndpoint(port: configuration.GetValue<int>("WebApiURL:Port", 5000), name: "https");

builder.Build().Run();