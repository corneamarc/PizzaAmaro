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

    public override string ToString()
    {
        return $"{Nume}, Telefon: {NumarTelefon}";
    }
}