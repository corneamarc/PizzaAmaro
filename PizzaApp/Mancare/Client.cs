/*namespace PizzaApp.Mancare;

public class Client
{
    public string Nume { get; }
    public string Telefon { get; }

    public Client(string nume, string telefon)
    {
        if (string.IsNullOrWhiteSpace(nume))
        {
            throw new ArgumentException("Numele clientului nu poate fi gol.");
        }

        if (!IsValidPhoneNumber(telefon))
        {
            throw new ArgumentException("Numărul de telefon nu este valid.");
        }

        Nume = nume;
        Telefon = telefon;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.StartsWith("+407") && phoneNumber.Length == 13;
    }
}*/
