# Sample code for calling one from another Azure Function dependency injection
## Reference Documentation
   1) Dependency Injection: https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#use-injected-dependencies
   2) Azure Function with Private Endpoints and Virtual Network Integration: https://learn.microsoft.com/en-us/azure/azure-functions/functions-create-vnet
   3) Reference Architecture: https://learn.microsoft.com/en-us/samples/azure-samples/http-trigger-azure-function-premium-plan/http-trigger-azure-function-premium-plan/
   4) Tutorial: Connect to a web app using an Azure Private Endpoint: https://learn.microsoft.com/en-us/azure/private-link/tutorial-private-endpoint-webapp-portal?source=recommendations 

# Steps to use it via Public Functions
    Create 2 functions, deploy the Inner and Outer function, and execute the Console App 

# Steps to use it with Private Endpoints
### VNet and Subnets
    1) Create VNet with 2 subnets + Bastion:
        1) PrivateEndPoint
        2) FunctionsVNetIntegration
### Create Storage Account
    1) Create Storage Account Standard, and retrieve Connection String
    2) Add Private Endpoint to PrivateEndPoint subnet
    3) Restrict Access to the FunctionsVNetIntegration subnet
### Create Inner Function
    1) Create Function Inner: Code, ASPNET 6, Unix, Dedicated Plan, New Storate Account, App Insights
    2) Add the STG_CONNSTRING config variable with the Connection String of Storage Account
    3) Deploy the Code to the Function 
    4) Add Private Endpoint to PrivateEndPoint subnet
    5) Add VNet integration to FunctionsVNetIntegration vnet
    6) Test and get the API URL
### Create the Outer Function
    1) Create Function Inner: Code, ASPNET 6, Unix, Dedicated Plan, New Storate Account, App Insights
    2) Add the INNER_API_URL config variable with the Inner Function URL
    3) Deploy the Code and Test it - with Test function (not external calls)
    4) Add Private Endpoint to PrivateEndPoint subnet
    5) Add VNet integration to FunctionsVNetIntegration vnet
### Create a VM and validate the solution using the Bastion
    1) Create a Windows VM
    2) Connect to the VM using Bastion
    3) Run the following code on Powershell to validate the solution

```markdown
    $x=0
    while ($x -lt 30000)
    {
        "Call number $x"
        Invoke-RestMethod https://func-f2f-plink-outer-fra.azurewebsites.net/api/HttpTriggerOuterFunction
        $x++
    }
```
    
# Helpgful Kusto Queries to analyse the Duration of the Calls
### Check the percentiles of API calls duration

```markdown
    // Operations performance 
    // Calculate request count and duration by operations. 
    // To create an alert for this query, click '+ New alert rule'
    requests
    | summarize RequestsCount=sum(itemCount), AverageDuration=avg(duration), percentiles(duration, 50, 95, 99, 99.5,99.9, 99.99) by operation_Name // you can replace 'operation_Name' with another value to segment by a different property
    | order by RequestsCount desc // order from highest to lower (descending)
```

### Aggregate the duration by buckets

```markdown
    // Response time buckets 
    // Show how many requests are in each performance-bucket. 
    requests
    | summarize requestCount=sum(itemCount), avgDuration=avg(duration) by performanceBucket
    | order by avgDuration asc // sort by average request duration
```