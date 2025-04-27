namespace ECommerce.Infrastructure.Repositories.Service
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string From { get; set; }
        public string Username { get; set; }   
        public string Password { get; set; }
        public string Smtp { get; set; }
    }
}