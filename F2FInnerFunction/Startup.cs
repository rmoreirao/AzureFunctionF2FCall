using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using Microsoft.Win32;

[assembly: FunctionsStartup(typeof(TestInner.Function.Startup))]

namespace TestInner.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddAzureClients(clientBuilder =>
                {
                    
                    // Add a storage account client
                    clientBuilder.AddBlobServiceClient("put the connection string to blob here");
                    // clientBuilder.AddBlobServiceClient(GetEnvironmentVariable("STORAGE_ACCOUNT"));
                });
        }

        private static string GetEnvironmentVariable(string name)
        {
            return name + ": " +
                System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}