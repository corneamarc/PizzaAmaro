


using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var pizzeria = new Pizzeria("Pizzeria Mea", "Strada Exemplu 123", new List<Pizza>());
            pizzeria.Ingrediente = new List<Ingredient>
            {
                new Ingredient("Mozzarella", 5),
                new Ingredient("Pepperoni", 7),
                new Ingredient("Ciuperci", 4),
                new Ingredient("Ardei", 3),
                new Ingredient("Ceapă", 2)
            };

        
            var client = new Client
                { Nume = "Ion Popescu", NrTelefon = "0712345678", IstoricComenzi = new List<Comanda>() };

        
            client.Autentificare("0712345678");

           
            while (true)
            {
                Console.WriteLine("\nMeniu Pizzerie:");
                Console.WriteLine("1. Adaugă pizza în meniu");
                Console.WriteLine("2. Afișează meniul");
                Console.WriteLine("3. Plasează o comandă");
                Console.WriteLine("4. Afișează istoric comenzi");
                Console.WriteLine("5. Adaugă ingredient");
                Console.WriteLine("6. Modifică ingredient");
                Console.WriteLine("7. Șterge ingredient");
                Console.WriteLine("0. Ieși");
                Console.Write("Alege o opțiune: ");

                var optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        AdaugaPizzaInMeniu(pizzeria);
                        break;
                    case "2":
                        pizzeria.AfiseazaMeniul();
                        break;
                    case "3":
                        pizzeria.PlaseazaComanda(client, true);
                        break;
                    case "4":
                        pizzeria.AfiseazaIstoricComenzi();
                        break;
                    case "5":
                        AdaugaIngredient(pizzeria);
                        break;
                    case "6":
                        ModificaIngredient(pizzeria);
                        break;
                    case "7":
                        StergeIngredient(pizzeria);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opțiune invalidă. Te rog să alegi din nou.");
                        break;
                }
            }
        }

        private static void AdaugaPizzaInMeniu(Pizzeria pizzeria)
        {
            Console.Write("Introdu numele pizzei: ");
            var nume = Console.ReadLine();
            Console.Write("Introdu dimensiunea pizzei (Mica, Medie, Mare): ");
            var dimensiune = (DimensiunePizza)Enum.Parse(typeof(DimensiunePizza), Console.ReadLine());
            Console.Write("Introdu prețul de bază: ");
            var pretBaza = decimal.Parse(Console.ReadLine());

            var ingrediente = new List<Ingredient>();
            Console.WriteLine("Introdu ingredientele (scrie 'gata' pentru a termina):");
            while (true)
            {
                var ingredientNume = Console.ReadLine();
                if (ingredientNume.ToLower() == "gata") break;

                var ingredient = pizzeria.Ingrediente.FirstOrDefault(i => i.Nume == ingredientNume);
                if (ingredient != null)
                {
                    ingrediente.Add(ingredient);
                }
                else
                {
                    Console.WriteLine("Ingredientul nu există în lista de ingrediente.");
                }
            }

            var pizza = new PizzaStandard(nume, dimensiune, ingrediente, pretBaza);
            pizzeria.AdaugaInMeniu(pizza);
        }

        private static void AdaugaIngredient(Pizzeria pizzeria)
        {
            Console.Write("Introdu numele ingredientului: ");
            var nume = Console.ReadLine();
            Console.Write("Introdu costul ingredientului: ");
            var cost = decimal.Parse(Console.ReadLine());

            var ingredient = new Ingredient(nume, cost);
            pizzeria.Ingrediente.Add(ingredient);
            Console.WriteLine($"Ingredientul {nume} a fost adăugat.");
        }

        private static void ModificaIngredient(Pizzeria pizzeria)
        {
            Console.Write("Introdu numele ingredientului pe care vrei să-l modifici: ");
            var nume = Console.ReadLine();
            Console.Write("Introdu noul cost: ");
            var costNou = decimal.Parse(Console.ReadLine());
        }

        private static void StergeIngredient(Pizzeria pizzeria)
        {
            Console.Write("Introdu numele ingredientului pe care vrei să-l ștergi: ");
            var nume = Console.ReadLine();
            pizzeria.StergeIngredient(nume);
        }
    }
}
