using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using POC_Cpp_Azure_Function.Calculators;

namespace POC_Cpp_Azure_Function.Azure_Functions
{
    internal class CLIAzureFunction
    {
        private readonly ILogger<CLIAzureFunction> _logger;

        public CLIAzureFunction(ILogger<CLIAzureFunction> logger)
        {
            _logger = logger;
        }

        [Function("CalculateByCLI")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            double a = double.Parse(query["a"] ?? "0");
            double b = double.Parse(query["b"] ?? "0");

            _logger.LogInformation("Calling native C++ function...");
            CalculateByCLI calculateByCLI = new();
            var result = calculateByCLI.Add(a, b);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Result from C++ DLL: {result}");
            return response;
        }
    }
}

