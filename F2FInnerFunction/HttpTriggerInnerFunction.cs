using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Text;

namespace TestASR.Function
{
    public static class HttpTriggerInnerFunction
    {
        [FunctionName("HttpTriggerInnerFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var startTime = System.DateTime.Now;
            string guid = req.Query["guid"];
            log.LogInformation("Start execution of Inner for guid " + guid ?? "??guid??");
            StringBuilder sbLog = new StringBuilder();
            
            sbLog.Append("inner;" + guid + ";"  + startTime.ToString("MM-dd-yyyTHH:mm:ss.fff") + ";");

            // var serviceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=storagetestasrblob;AccountKey=idnWk9PRy9xoNYhzIxxaWfNxeotpW1TVNRfkf70mPBGd7moETagb0rVYbCkycznQaf9bVgXx2juI+AStM9ZA+w==;EndpointSuffix=core.windows.net");
            //  var containers = serviceClient.GetBlobContainers();
            //  foreach(var container in containers){
            //     log.LogInformation("Container: " + container.Name);
            //  }

            var endTime = System.DateTime.Now;
            sbLog.Append(endTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + (endTime - startTime).Milliseconds + ";");
            log.LogInformation("Finish execution of Inner for guid " + guid ?? "??guid??");

            return new OkObjectResult(sbLog.ToString());
        }
    }
}
