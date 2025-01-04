using System.Text.RegularExpressions;

namespace PizzaApp;

public class Client
{
    public string Nume { get; set; }
    public string NrTelefon { get; set; }
    public List<Comanda> IstoricComenzi { get; set; }
    
    public bool EsteAutentificat{ get; set; }
    
    public bool EsteFidel => IstoricComenzi.Count >= 5;

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
}