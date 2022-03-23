namespace ProductCatalog.Models
{
    public class GetAccessTokenResponseModel
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
    }
}
