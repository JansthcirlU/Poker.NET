using Poker.NET.Worker;
using Poker.NET.Worker.Helpers;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IHandComparisonFileHelper, HandComparisonFileHelper>()
    .AddHostedService<Worker>();

IHost host = builder.Build();
host.Run();
