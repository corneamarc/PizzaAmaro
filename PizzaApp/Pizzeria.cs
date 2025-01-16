using PizzaApp.Mancare;

namespace PizzaApp;

public class Pizzeria
{
    public string Nume { get; set; }
    public string Adresa { get; set; }
    public List<Pizza> Meniu { get; set; }

    public List<Ingredient> Ingrediente { get; set; }
    public List<Comanda> Comenzi { get; set; }
    
  

    

    public Pizzeria(string nume, string adresa, List<Pizza> meniu)
    {
        Nume = nume;
        Adresa = adresa;
        Meniu = new List<Pizza>();

    }

    public void AdaugaInMeniu(Pizza pizza)
    {
        Meniu.Add(pizza);
        Console.WriteLine($"Pizza {Nume} a fost adaugata in meniu");

    }

    public void StergePizza(string nume, DimensiunePizza dimensiune)
    {
        var pizza = Meniu.FirstOrDefault(p => p.Nume == nume && p.Dimensiune == dimensiune);
        if (pizza != null)
        {
            Meniu.Remove(pizza);
            Console.WriteLine($"Pizza {Nume} ({dimensiune}) a fost stearsa din meniu");
        }
        else
        {
            Console.WriteLine($"Pizza {Nume} ({dimensiune}) nu exista in  meniu");
        }
    }

    public void AfiseazaMeniul()
    {
        Console.WriteLine("\nMeniul este: ");
        foreach (var pizza in Meniu)
        {
            Console.WriteLine($"{pizza.Nume} ({pizza.Dimensiune}): {pizza.CalculeazaPret()} RON");
        }
    }

    public void ModificaPizza(string nume, DimensiunePizza dimensiune, List<Ingredient> ingredienteNoi = null)
    {
        var pizza = Meniu.FirstOrDefault(p => p.Nume == nume && p.Dimensiune == dimensiune);

        if (pizza == null)
        {
            Console.WriteLine($"Pizza {nume} ({dimensiune}) nu exista in meniu");
            return;
        }

        if (ingredienteNoi != null)
        {
            pizza.Ingrediente = ingredienteNoi;
            Console.WriteLine($"LIsta de ingrediente pentru pizza {nume} ({dimensiune}) a fost actualizata");
        }
    }

    public void AfiseazaLista()
    {
        Console.WriteLine("\nAfiseaza lista de ingrediente disponibila: ");
        if (Ingrediente.Count == 0)
        {
            Console.WriteLine("nu exista ingrediente disponibile");
        }

        foreach (var ingredient in Ingrediente)
        {
            Console.WriteLine($"{ingredient.Nume}: {ingredient.Cost} RON");
        }
    }

    public void ModificaPretIngredient(string numeIngredient, decimal pretNou)
    {
        var ingredient = Ingrediente.FirstOrDefault(i => i.Nume == numeIngredient);
        if (ingredient == null)
        {
            Console.WriteLine($"ingredientul {numeIngredient} nu exista in meniu");
            return;
        }

        ingredient.Cost = pretNou;
        Console.WriteLine($"ingredientul {numeIngredient} a fost actualizat cu preul nou {pretNou} RON");
    }

    public void StergeIngredient(string nume)
    {
        var ingredient = Ingrediente.FirstOrDefault(i => i.Nume == nume);

        if (ingredient == null)
        {
            Console.WriteLine($"ingredientul {nume} nu exista in meniu");
            return;
        }

        Ingrediente.Remove(ingredient);
        Console.WriteLine($"ingredienteul {nume} a fost sters din lista de ingrediente");
    }

    public void AdaugaComanda(Comanda comanda)
    {
        Comenzi.Add(comanda);
        Console.WriteLine("Comanda a fist adaugata in istoric");
    }

    public void AfiseazaIstoricComenzi()
    {
        Console.WriteLine("\nAfiseaza istoric comenzi: ");
        if (Comenzi.Count == 0)
        {
            Console.WriteLine("nu exista comenzi in istoric");
            return;
        }

        foreach (var comanda in Comenzi)
        {
            Console.WriteLine(comanda);
        }
    }

    public void AfiseazaComenziClient(string numeClient)
    {
        var comenziClient = Comenzi.Where(c => c.Client.Nume == numeClient).ToList();
        Console.WriteLine($"\nIstoricul comenzilor pt clientul {numeClient} este:");
        if (comenziClient.Count == 0)
        {
            Console.WriteLine($"nu exisa comenzi pentru clientul {numeClient}");
            return;
        }

        foreach (var comanda in comenziClient)
        {
            Console.WriteLine(comanda);
        }
    }
    
    
    public void PlaseazaComanda(Client client, bool livrareDomiciliu)
    {
        AfiseazaMeniul();
        Console.WriteLine("Introduceți numărul de sortimente dorite:");
        int numarSortimente = int.Parse(Console.ReadLine());

        var listaPizza = new List<Pizza>();
        for (int i = 0; i < numarSortimente; i++)
        {
            Console.Write("Introdu numele pizzei dorite: ");
            var numePizza = Console.ReadLine();
            Console.Write("Introdu dimensiunea pizzei (Mica, Medie, Mare): ");
            var dimensiunePizza = (DimensiunePizza)Enum.Parse(typeof(DimensiunePizza), Console.ReadLine());

            var pizza = Meniu.FirstOrDefault(p => p.Nume == numePizza && p.Dimensiune == dimensiunePizza);
            if (pizza != null)
            {
                listaPizza.Add(pizza);
            }
            else
            {
                Console.WriteLine($"Pizza {numePizza} ({dimensiunePizza}) nu este disponibilă în meniu.");
                i--; 
            }
        }

        var metodaLivrare = livrareDomiciliu ? "Livrare" : "Ridicare";
        var costTotal = listaPizza.Sum(p => p.CalculeazaPret());
        if (livrareDomiciliu)
        {
            costTotal += 10; 
        }

        if (Client.EsteFidel())
        {
            costTotal *= 0.9m;
        }

        var comanda = new Comanda(client, listaPizza, metodaLivrare, costTotal);
        AdaugaComanda(comanda);
    }
}