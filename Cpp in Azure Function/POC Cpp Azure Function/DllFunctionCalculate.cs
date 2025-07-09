using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace POC_Cpp_Azure_Function
{
    public class DllFunctionCalculate
    {
        private readonly ILogger<DllFunctionCalculate> _logger;

        public DllFunctionCalculate(ILogger<DllFunctionCalculate> logger)
        {
            _logger = logger;
        }

        // Native function import
        [DllImport("PocDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Add(int a, int b);

        [Function("DllFunctionCalculate")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Calling native C++ function...");
            int result = Add(3, 4);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Result from C++ DLL: {result}");
            return response;
        }
    }
}
