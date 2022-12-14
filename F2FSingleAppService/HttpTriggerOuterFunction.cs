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


    public class HttpTriggerOuterFunction
    {
        private readonly HttpClient _client;
        private readonly string _innerApiUrl;
        public HttpTriggerOuterFunction(IHttpClientFactory httpClientFactory)
        {
            this._client = httpClientFactory.CreateClient();
            // Set this as a property of your function
            // Sample: https://func-f2f-private-link-inner.azurewebsites.net/api/HttpTriggerInnerFunction
            _innerApiUrl = Environment.GetEnvironmentVariable("INNER_API_URL");
        }

        [FunctionName("HttpTriggerOuterFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var startTime = System.DateTime.Now;
            string guid = req.Query["guid"];
            log.LogInformation("Start execution of Outer for guid " + guid ?? "??guil??");
            StringBuilder sbLog = new StringBuilder();
            
            sbLog.Append("outer;" + guid + ";"  + startTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + context.InvocationId + ";" + Activity.Current.RootId + ";");

       
            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, _innerApiUrl + "?guid=" + guid);

            //Read Server Response
            HttpResponseMessage httpResponse = await _client.SendAsync(newRequest);
            var endTime = System.DateTime.Now;
            sbLog.Append(endTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";" + (endTime - startTime).TotalMilliseconds + ";");

            httpResponse.EnsureSuccessStatusCode();
            var responseBody = httpResponse.Content.ReadAsStringAsync().Result;
            
            
            
            log.LogInformation("Finish execution of Outer for guid " + guid ?? "??guid??");

            return new OkObjectResult(sbLog.ToString() + responseBody);
        }
    }
}
