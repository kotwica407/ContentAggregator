namespace ContentAggregator.Models.Dtos
{
    public class LoginDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}