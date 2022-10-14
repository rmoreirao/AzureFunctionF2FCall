using System.Net.Http;
using System;
using System.Text;

var logfileName = "log_" + DateTime.Now.ToString("MMddyyyTHHmmss") + ".txt"; // 08-04-2021T23:58:30.999
File.WriteAllText(logfileName, "count;guid;starttime;finishtime;totalmiliseconds;exception;response;");
var count = 0;
HttpClient newClient = new HttpClient();
while(true){
    count++;
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("");
    string? exceptionText = null;
    string responseBody = null;
    try{
        var guid = Guid.NewGuid().ToString();
        var startTime = System.DateTime.Now;
        sb.Append(count + ";");
        sb.Append(guid + ";"  + startTime.ToString("MM-dd-yyy HH:mm:ss.fff") + ";");
        
        HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, String.Format("https://func-outer-dedicated-we.azurewebsites.net/api/HttpTriggerOuterFunction?guid={0}",guid));
        //Read Server Response
        HttpResponseMessage httpResponse = await newClient.SendAsync(newRequest);
        
        var endTime = System.DateTime.Now;
        sb.Append(System.DateTime.Now.ToString("MM-dd-yyy HH:mm:ss.fff") + ";");
        sb.Append((endTime - startTime).TotalMilliseconds + ";");

        httpResponse.EnsureSuccessStatusCode();
        responseBody = httpResponse.Content.ReadAsStringAsync().Result;
        
    }catch(Exception ex){
        Console.WriteLine(ex);
        exceptionText = ex.ToString().Replace(";","");
    }

    if (String.IsNullOrEmpty(exceptionText)){
        sb.Append(";");
    }else{
        sb.Append(exceptionText + ";");
    }
    sb.Append(responseBody + ";");
    // flush every 20 seconds as you do it
    File.AppendAllText(logfileName, sb.ToString());
    Console.WriteLine(sb.ToString());
    sb.Clear();
}
