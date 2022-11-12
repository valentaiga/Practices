using Microsoft.AspNetCore.Server.Kestrel.Core;
using Practices.gRPC.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
const int http1Port = 5131;
const int http2Port = 7249;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(
        http1Port, o => o.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(
        http2Port, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BooksService>();

await app.RunAsync(); 