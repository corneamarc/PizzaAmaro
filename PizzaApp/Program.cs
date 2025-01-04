namespace PizzaApp;

class Program
{
    static void Main()
    {
        Ingrediente sunca = new Ingrediente("Sunca", 3m);
        Ingrediente porumb = new Ingrediente("Porumb", 1.6m);
        Ingrediente cascaval = new Ingrediente("Cascaval", 2m);
        
        Pizza standard = new PizzaStandard("Margherita", Dimensiune.Mica, 0);
        Console.WriteLine(standard);
        
        Pizza personalizata = new PizzaPersonalizata("Pizza Client", Dimensiune.Mare);
        personalizata.AdaugaIngrediente(sunca);
        personalizata.AdaugaIngrediente(porumb);
        personalizata.AdaugaIngrediente(cascaval);
        Console.WriteLine(personalizata);
    }
}