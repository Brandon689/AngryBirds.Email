namespace AngryBirds.Email.Providers.ElasticEmail
{
    public class ElasticEmailConfig
    {
        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; } = "https://api.elasticemail.com/v2/";
    }
}