using System;
using System.Collections.Generic;

namespace PizzaAmaro
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bun venit la Pizzeria App!");

            // Exemplu de creare a unei pizza standard
            var pizzaMargherita = new PizzaStandard("Margherita", DimensiunePizza.Medie, 25.5m, new List<string> { "Mozzarella", "Rosii", "Busuioc" });

            // Exemplu de creare a unei pizza personalizate
            var pizzaPersonalizata = new PizzaPersonalizata("Pizza Custom", DimensiunePizza.Mare, new List<string> { "Mozzarella", "Sunca", "Ciuperci" });

            var client1 = new Client("Ion Popescu", "+40712345678");
            var comanda1 = new Comanda(client1, new List<Pizza> { pizzaMargherita, pizzaPersonalizata }, true);

            Console.WriteLine(pizzaMargherita);
            Console.WriteLine(pizzaPersonalizata);
            Console.WriteLine(client1);
            Console.WriteLine(comanda1);
        }
    }
}