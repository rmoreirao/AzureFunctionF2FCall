using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Polly.Timeout;
using Polly;

namespace F2FSingleAppService.Function
{


    public class HttpTriggerOuterFunctionTest
    {
        public HttpTriggerOuterFunctionTest()
        {
        }

        [FunctionName("HttpTriggerOuterFunctionTest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var startTime = System.DateTime.Now;
            string guid = req.Query["guid"];
            log.LogInformation("Start execution of Outer for guid " + guid ?? "??guil??");
            StringBuilder sbLog = new StringBuilder();
            
            sbLog.Append("outer;" + guid + ";"  + startTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + context.InvocationId + ";" + Activity.Current.RootId + ";");

       
            var endTime = System.DateTime.Now;
            sbLog.Append(endTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + (endTime - startTime).TotalMilliseconds + ";");
            
            log.LogInformation("Finish execution of Outer for guid " + guid ?? "??guid??");

            return new OkObjectResult(sbLog.ToString());
        }
    }
}
