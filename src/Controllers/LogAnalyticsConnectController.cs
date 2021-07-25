using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.OperationalInsights;
using Microsoft.Extensions.Logging;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Text.Json;

namespace LogAnalyticsSDKOnKubernetes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogAnalyticsConnectController : ControllerBase
    {
        private readonly ILogger<LogAnalyticsConnectController> _logger;

        public LogAnalyticsConnectController(ILogger<LogAnalyticsConnectController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetLogAnalyticsLogEntries")]
        public string GetLogAnalyticsLogEntries(string query)
        {
            var workspaceId = "";//"<your workspace ID>"; // Retrieve from Environment variable
            var clientId = "";//"<your client ID>"; // Retrieve from Environment variable
            var clientSecret = "";//"<your client secret>"; // Retrieve from Environment variable
            var json = "";
            var domain = "microsoft.onmicrosoft.com";//"<your AAD domain>";
            var authEndpoint = "https://login.microsoftonline.com";
            var tokenAudience = "https://api.loganalytics.io/";
            var adSettings = new ActiveDirectoryServiceSettings
            {
                AuthenticationEndpoint = new Uri(authEndpoint),
                TokenAudience = new Uri(tokenAudience),
                ValidateAuthority = true
            };
            var creds = ApplicationTokenProvider.LoginSilentAsync(domain, clientId, clientSecret, adSettings).GetAwaiter().GetResult();

            var client = new OperationalInsightsDataClient(creds);
            client.WorkspaceId = workspaceId;
            if (query != null)
            {
                var results = client.Query(query);
                json = JsonSerializer.Serialize(results);
            }
            else
            {
                json = "Possibly innecurate Query.";
            }
            return json;
        }

        [HttpGet("GetLPowerBIStorageEntries")]
        public void GetLPowerBIStorageEntries(string serverAddress, string database, string XMLAQuery)
        {

        }

    }
}
