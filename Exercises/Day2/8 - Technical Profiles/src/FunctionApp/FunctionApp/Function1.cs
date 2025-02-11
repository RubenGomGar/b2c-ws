using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] FunctionRequest request, HttpRequest req,
            ILogger log)
        {
            log.LogInformation("UserValidation");

            // parse Basic Auth username and password
            var header = req.Headers["Authorization"].ToString(); // get the header
            log.LogInformation(header);
            var headerValue = header.Split(' ')[1];
            var base64EncodedBytes = System.Convert.FromBase64String(headerValue);
            var auth = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var parts = auth.Split(':');
            var username = parts[0];
            var password = parts[1];

            if (
                username != Environment.GetEnvironmentVariable("BASIC_AUTH_USERNAME") ||
                password != Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD")
            )
            {
                return GetB2cApiConnectorResponse("ShowBlockPage", "UserValidation-Failed", "Error authenticating call", 200, false); 
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation(requestBody);


            log.LogInformation("Validate UserId: "+request.UserId);

            var isValidUser = int.TryParse(request.UserId[^1..], out int value);

            return GetB2cApiConnectorResponse("Continue", "UserValidation-Succeeded", "User validated", 200, isValidUser);
        }

        private static IActionResult GetB2cApiConnectorResponse(string action, string code, string userMessage, int statusCode, bool isValidUser)
        {
            var responseProperties = new Dictionary<string, object>
            {
                { "version", "1.0.0" },
                { "action", action },
                { "userMessage", userMessage },
                { "isValidUser", isValidUser }
            };
            return new JsonResult(responseProperties) { StatusCode = statusCode };
        }
    }

    public class FunctionRequest
    {
        public string UserId { get; set; }
    }
}
