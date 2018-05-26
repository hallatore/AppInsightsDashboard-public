using System;

namespace AppInsightsDashboard.Web.Business.Dashboard.Models
{
    public class ApiToken
    {
        public ApiToken(string applicationId, string apiKey)
        {
            ApplicationId = new Guid(applicationId);
            ApiKey = apiKey;
        }

        public Guid ApplicationId { get; }
        public string ApiKey { get; }
    }
}