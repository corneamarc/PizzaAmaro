namespace PizzaApp;

public class Ingredient
{
    public string Nume { get; set; }
    public decimal Cost { get; set; }

    public Ingredient(string nume, decimal cost)
    {
        Nume = nume;
        Cost = cost;
    }
}