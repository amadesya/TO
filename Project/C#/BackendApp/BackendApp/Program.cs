using Microsoft.AspNetCore.Mvc.Formatters;
using static System.Console;
using BackendApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    WriteLine("Default output formatters:");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;
        if (mediaFormatter is null)
        {
            WriteLine($"  {formatter.GetType().Name}");
        }
        else
        {
            WriteLine(" {0}, Media types: {1}",
                arg0: mediaFormatter.GetType().Name,
                arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
        }
    }
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();
app.Run();


