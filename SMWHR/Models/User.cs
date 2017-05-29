namespace SMWHR.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public User() { }

        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }
}