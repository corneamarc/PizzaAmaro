namespace PizzaApp;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public string PhoneNumber { get; set; }

    public User() { }

    public User(string username, string password, bool isAdmin, string phoneNumber = null)
    {
        Username = username;
        Password = password;
        IsAdmin = isAdmin;
        PhoneNumber = phoneNumber;
    }

    public string GetUsername() => Username;
    public string GetPassword() => Password;
    public bool GetIsAdmin() => IsAdmin;
    public string GetPhoneNumber() => PhoneNumber;
}