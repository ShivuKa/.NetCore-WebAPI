namespace ProductCatalog.Models
{
    public class GetAccessTokenRequestModel
    {
        public string Grant_type { get; set; }       
        public string Client_id { get; set; }        
        public string Audience { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Client_secret { get; set; }
    }
}
