namespace IDC.Shared.Configurations
{
    public class ApiConfiguration
    {
        public string ApiTitle { get; set; } = default!;
        public string ApiName { get; set; } = default!;
        public string ApiVersion { get; set; } = default!;
        public string ApiBaseUrl { get; set; } = default!;
        public string IdentityServerBaseUrl { get; set; } = default!;
        public string IssuerUri { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string CorsAllowOrigin { get; set; } = default!;
    }
}
