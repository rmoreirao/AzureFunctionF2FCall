using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;


[assembly: FunctionsStartup(typeof(TestOuter.Function.Startup))]

namespace TestOuter.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddAzureClients(clientBuilder =>
                {
                    // Add this property to the Function App with the Conn String of the Storage Account!
                    clientBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("STG_CONNSTRING"));
                });
        }
    }
}