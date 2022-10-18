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

namespace F2FSingleAppService.Function
{
    public class HttpTriggerInnerFunctionTest
    {
        public HttpTriggerInnerFunctionTest()
        {
        }

        [FunctionName("HttpTriggerInnerFunctionTest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var startTime = System.DateTime.Now;
            string guid = req.Query["guid"];
            log.LogInformation("Start execution of Inner for guid " + guid ?? "??guid??");
            StringBuilder sbLog = new StringBuilder();
            
            sbLog.Append("innertest;" + guid + ";"  + startTime.ToString("MM-dd-yyyTHH:mm:ss.fff") + ";");

            var endTime = System.DateTime.Now;
            sbLog.Append(endTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + (endTime - startTime).Milliseconds + ";");
            log.LogInformation("Finish execution of Inner for guid " + guid ?? "??guid??");

            return new OkObjectResult(sbLog.ToString());
        }
    }
}
