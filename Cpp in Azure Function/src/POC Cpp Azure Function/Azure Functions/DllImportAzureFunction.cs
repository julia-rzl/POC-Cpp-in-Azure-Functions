using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using POC_Cpp_Azure_Function.Calculators;

namespace POC_Cpp_Azure_Function.Azure_Functions
{
    public class DllImportAzureFunction
    {
        private readonly ILogger<DllImportAzureFunction> _logger;

        private DllImportAzureFunction(ILogger<DllImportAzureFunction> logger)
        {
            _logger = logger;
        }

        [Function("CalculateByDllImport")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Calling native C++ function...");

            int result = CalculateByDllImport.Add(3, 5);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Result from C++ DllImport Azure Function: {result}");
            return response;
        }
    }
}
