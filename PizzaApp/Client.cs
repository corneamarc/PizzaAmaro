using System.Text.RegularExpressions;

namespace PizzaApp;

public class Client
{
    public string Nume { get; set; }
    public string NrTelefon { get; set; }
    public List<Comanda> IstoricComenzi { get; set; }
    
    public bool EsteAutentificat{ get; set; }
    
    public bool EsteFidel()
    {
        return IstoricComenzi.Count >= 5;
    }

    public void Autentificare(string telefon)
    {
        if (NrTelefon == telefon)
        {
            EsteAutentificat = true;
            Console.WriteLine($"Clientul {Nume} a fos autentificat cu succes");
        }
        else
        {
            Console.WriteLine("Numarul de telefon nu este corect. Autentificarea a esuat");
        }
    }

    public void AfisareMeniu(Pizzeria pizzeria)
    {
        if (!EsteAutentificat)
        {
            Console.WriteLine("Trebuie sa fiti autentificai pt a plasa o comanda, totusi puteti vizualiza meniul");
        }
        pizzeria.AfiseazaMeniul();
    }

    private bool ValidareTelefon(string telefon)
    {
        telefon = telefon.Trim();
        return telefon.Length == 10 && (telefon.StartsWith("07") || telefon.StartsWith("02") || telefon.StartsWith("03") && telefon.All(char.IsDigit));
        
    }

    public void AdaugaLaIstoric(Comanda comanda)
    {
        IstoricComenzi.Add(comanda);
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
    
    
    public void AfiseazaDetalii()
    {
        Console.WriteLine($"Clienti:{Nume}");
        Console.WriteLine($"Numar de telefon:{NrTelefon}");
        Console.WriteLine($"Numar Comenzi:{IstoricComenzi.Count}");

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