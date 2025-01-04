using System;
using System.Collections.Generic;
using PizzaApp.Mancare;
class Program
{
    static void Main()
    {
        Ingredient mozzarella = new Ingredient("Mozzarella", 1.5m);
        Ingredient salam = new Ingredient("Salam", 2m);
        Ingredient ciuperci = new Ingredient("Ciuperci", 1.5m);
        
        Pizza standard = new PizzaStandard("Margherita", Dimensiune.Mica, 0);
        Console.WriteLine(standard);
        
        Pizza personalizata = new PizzaPersonalizata("Pizza Client", Dimensiune.Mare);
        personalizata.AdaugaIngredient(mozzarella);
        personalizata.AdaugaIngredient(salam);
        personalizata.AdaugaIngredient(ciuperci);
        Console.WriteLine(personalizata);
    }
}