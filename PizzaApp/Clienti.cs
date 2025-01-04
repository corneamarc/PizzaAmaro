using System.Text.RegularExpressions;

namespace PizzaApp;

public class Clienti
{
    private string _nume;
    private string _numarDeTelefon;
    private List<Comenzile> _istoriccomenzi;
    
    public string Nume=>_nume;
    public string NumarDeTelefon => _numarDeTelefon;
    public List<Comenzile> Comenzi => _istoriccomenzi;

    public Clienti(string nume, string numarDeTelefon, List<Comenzile> comenzi)
    {
        _nume = nume;
        _numarDeTelefon = numarDeTelefon;
        _istoriccomenzi = new List<Comenzile>(Comenzi);
    }

    public static bool ValidareTelefon(string telefon)
    {
        string numarDeTelefon = @"^07[0-9]{8}$";
        return Regex.IsMatch(telefon, numarDeTelefon);
    }

    

    public decimal CalculeazaReducere(decimal total)
    {
        if (EsteFidel())
        {
            Console.WriteLine($"{Nume} beneficiaza de o reducere de 10%.");
            return total * 0.9m;
        }
        return total;
    }

    public bool EsteFidel()
    {
        return _istoriccomenzi.Count >= 5;
    }

    public void AfiseazaDetalii()
    {
        Console.WriteLine($"Clienti:{Nume}");
        Console.WriteLine($"Numar de telefon:{NumarDeTelefon}");
        Console.WriteLine($"Numar Comenzi:{_istoriccomenzi.Count}");

        if (EsteFidel())
        {
            Console.WriteLine("Status:Client fidel(Reducere de 10%) ");
        }
        else
        {
            Console.WriteLine("Status:Client normal");
        }
    }
}