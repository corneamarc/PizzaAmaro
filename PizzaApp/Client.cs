using System.Text.RegularExpressions;
namespace PizzaAmaro;

public class Client
{
    public string Nume { get; set; }
    public string NumarTelefon { get; set; }
    public List<Comanda> IstoricComenzi { get; set; }

    public Client(string nume, string numarTelefon)
    {
        Nume = nume;
        NumarTelefon = numarTelefon;
        IstoricComenzi = new List<Comanda>();
    }

    public bool EsteFidel()
    {
        return IstoricComenzi.Count >= 5;
    }

    public static bool VerificaNumarTelefon(string numarTelefon)
    {
        var regex = new Regex(@"^(\+40|07)[0-9]{8}$");
        return regex.IsMatch(numarTelefon);
    }

    public override string ToString()
    {
        return $"{Nume}, Telefon: {NumarTelefon}";
    }
}