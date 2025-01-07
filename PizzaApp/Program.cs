using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PizzaAmaro
{
    class Program
    {
        private static List<Utilizator> utilizatori = new List<Utilizator>();
        private static List<Client> clienti = new List<Client>();
        private static List<Pizza> meniu = new List<Pizza>();
        public static List<Ingredient> Ingrediente = new List<Ingredient>();
        private static List<Comanda> comenzi = new List<Comanda>();

        static void Main(string[] args)
        {
            Console.WriteLine("Bun venit la Pizzeria App!");
            utilizatori.Add(new Utilizator("admin", "admin", true));
            Program.Ingrediente.Add(new Ingredient("Mozzarella", 5));
            Program.Ingrediente.Add(new Ingredient("Sunca", 7));
            Program.Ingrediente.Add(new Ingredient("Ciuperci", 6));

            while (true)
            {
                Console.WriteLine("1. Autentificare\n2. Inregistrare\n3. Iesire");
                var optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "1": Autentificare(); break;
                    case "2": Inregistrare(); break;
                    case "3": return;
                    default: Console.WriteLine("Optiune invalida."); break;
                }
            }
        }

        static void Autentificare()
        {
            Console.Write("Nume utilizator: "); var nume = Console.ReadLine();
            Console.Write("Parola: "); var parola = Console.ReadLine();
            var utilizator = utilizatori.Find(u => u.Nume == nume && u.Parola == parola);
            if (utilizator != null)
            {
                Console.WriteLine("Autentificare reusita.");
                if (utilizator.EsteAdministrator) MeniuAdministrator(); else MeniuClient(nume);
            }
            else Console.WriteLine("Autentificare esuata.");
        }

        static void Inregistrare()
        {
            Console.Write("Nume: "); var nume = Console.ReadLine();
            Console.Write("Numar telefon: "); var telefon = Console.ReadLine();
            if (!Client.VerificaNumarTelefon(telefon))
            {
                Console.WriteLine("Numar de telefon invalid.");
                return;
            }
            Console.Write("Parola: "); var parola = Console.ReadLine();
            utilizatori.Add(new Utilizator(nume, parola, false));
            clienti.Add(new Client(nume, telefon));
            Console.WriteLine("Inregistrare reusita.");
        }

        static void MeniuAdministrator()
        {
            while (true)
            {
                Console.WriteLine("1. Vizualizare meniu\n2. Adaugare pizza\n3. Stergere pizza\n4. Vizualizare comenzi\n5. Raport statistici\n6. Gestionare ingrediente\n7. Iesire");
                var optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "1": VizualizareMeniu(); break;
                    case "2": AdaugarePizza(); break;
                    case "3": StergerePizza(); break;
                    case "4": VizualizareComenzi(); break;
                    case "5": GenerareRaport(); break;
                    case "6": GestionareIngrediente(); break;
                    case "7": return;
                    default: Console.WriteLine("Optiune invalida."); break;
                }
            }
        }

        static void GestionareIngrediente()
        {
            while (true)
            {
                Console.WriteLine("1. Vizualizare ingrediente\n2. Adaugare ingredient\n3. Stergere ingredient\n4. Modificare pret ingredient\n5. Iesire");
                var optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "1": VizualizareIngrediente(); break;
                    case "2": AdaugareIngredient(); break;
                    case "3": StergereIngredient(); break;
                    case "4": ModificarePretIngredient(); break;
                    case "5": return;
                    default: Console.WriteLine("Optiune invalida."); break;
                }
            }
        }

        static void VizualizareIngrediente()
        {
            foreach (var ingredient in Program.Ingrediente) 
                Console.WriteLine(ingredient);
        }

        static void AdaugareIngredient()
        {
            Console.Write("Nume ingredient: "); var nume = Console.ReadLine();
            Console.Write("Pret: "); var pret = decimal.Parse(Console.ReadLine());
            Program.Ingrediente.Add(new Ingredient(nume, pret));
            Console.WriteLine("Ingredient adaugat cu succes.");
        }

        static void StergereIngredient()
        {
            Console.Write("Nume ingredient: "); var nume = Console.ReadLine();
            Program.Ingrediente.RemoveAll(i => i.Nume == nume);
            Console.WriteLine("Ingredient sters.");
        }

        static void ModificarePretIngredient()
        {
            Console.Write("Nume ingredient: "); var nume = Console.ReadLine();
            var ingredient = Program.Ingrediente.FirstOrDefault(i => i.Nume == nume);
            if (ingredient != null)
            {
                Console.Write("Pret nou: ");
                ingredient.Pret = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Pret modificat cu succes.");
            }
            else Console.WriteLine("Ingredientul nu a fost gasit.");
        }

        static void VizualizareMeniu()
        {
            foreach (var pizza in meniu)
                Console.WriteLine(pizza);
        }

        static void AdaugarePizza()
        {
            Console.Write("Nume pizza: "); var nume = Console.ReadLine();
            Console.Write("Dimensiune (Mica/Medie/Mare): ");
            var dimensiune = (DimensiunePizza)Enum.Parse(typeof(DimensiunePizza), Console.ReadLine(), true);
            Console.Write("Pret de baza: "); var pretDeBaza = decimal.Parse(Console.ReadLine());
            Console.Write("Ingrediente (separate prin virgula): ");
            var ingredienteList = Console.ReadLine().Split(',').Select(i => i.Trim()).ToList();
            meniu.Add(new PizzaStandard(nume, dimensiune, pretDeBaza, ingredienteList));
            Console.WriteLine("Pizza adaugata cu succes.");
        }

        static void StergerePizza()
        {
            Console.Write("Nume pizza: "); var nume = Console.ReadLine();
            meniu.RemoveAll(p => p.Nume == nume);
            Console.WriteLine("Pizza stearsa din meniu.");
        }

        static void VizualizareComenzi()
        {
            foreach (var comanda in comenzi)
                Console.WriteLine(comanda);
        }

        static void GenerareRaport()
        {
            var raport = comenzi.GroupBy(c => c.PizzeComandate.First().Nume)
                .Select(g => new { Pizza = g.Key, NumarComenzi = g.Count() })
                .OrderByDescending(r => r.NumarComenzi);

            Console.WriteLine("Raport comenzi:");
            foreach (var item in raport)
                Console.WriteLine($"{item.Pizza}: {item.NumarComenzi} comenzi");
        }

        static void MeniuClient(string numeClient)
        {
            var client = clienti.Find(c => c.Nume == numeClient);
            if (client == null)
            {
                Console.WriteLine("Client inexistent.");
                return;
            }

            while (true)
            {
                Console.WriteLine(
                    "1. Vizualizare meniu\n2. Plasare comanda\n3. Vizualizare istoric comenzi\n4. Iesire");
                var optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "1": VizualizareMeniu(); break;
                    case "2": PlasareComanda(client); break;
                    case "3": VizualizareIstoric(client); break;
                    case "4": return;
                    default: Console.WriteLine("Optiune invalida."); break;
                }
            }
        }
        static void PlasareComanda(Client client)
        {
            var pizzeComandate = new List<Pizza>();

            while (true)
            {
                Console.WriteLine("Selecteaza pizza din meniu (sau scrie 'stop' pentru a finaliza comanda):");
                VizualizareMeniu();
                var numePizza = Console.ReadLine();
                if (numePizza.ToLower() == "stop") break;

                var pizzaGasita = meniu.FirstOrDefault(p => p.Nume.Equals(numePizza, StringComparison.OrdinalIgnoreCase));
                if (pizzaGasita != null)
                {
                    pizzeComandate.Add(pizzaGasita);
                    Console.WriteLine($"Pizza {pizzaGasita.Nume} adaugata in comanda.");
                }
                else
                {
                    Console.WriteLine("Pizza nu a fost gasita in meniu.");
                }
            }

            Console.Write("Doriti livrare la domiciliu? (da/nu): ");
            var livrare = Console.ReadLine().ToLower() == "da";

            var comandaNoua = new Comanda(client, pizzeComandate, livrare);
            comenzi.Add(comandaNoua);
            client.IstoricComenzi.Add(comandaNoua);

            Console.WriteLine($"Comanda plasata cu succes! Cost total: {comandaNoua.CostTotal} RON.");
        }

        static void VizualizareIstoric(Client client)
        {
            Console.WriteLine($"Istoric comenzi pentru {client.Nume}:");
            foreach (var comanda in client.IstoricComenzi)
                Console.WriteLine(comanda);
        }
    }
}