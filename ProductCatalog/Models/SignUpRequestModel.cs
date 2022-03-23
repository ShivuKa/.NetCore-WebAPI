namespace ProductCatalog.Models
{
    public class SignUpRequestModel
    {
        internal string client_id;
        public string Client_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
        public string Connection { get; set; }

    }
}
