namespace Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Salt { get; set; }
    }
}
