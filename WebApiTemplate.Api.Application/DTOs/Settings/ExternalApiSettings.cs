namespace WebApiTemplate.Api.Application.DTOs.Settings
{
    public class ExternalApiSettings
    {
        public string Name { get; set; }

        public string BaseUrl { get; set; }

        public string GetUrl { get; set; }

        public string PostUrl { get; set; }

        public string FailureMessage { get; set; }
    }
}
