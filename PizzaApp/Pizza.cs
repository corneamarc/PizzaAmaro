using System.Text.Json.Serialization.Metadata;

namespace PizzaApp;

public abstract class Pizza
{
    public string Nume { get; set; }
    public DimensiunePizza Dimensiune { get; set; }
    public List<Ingredient> Ingrediente { get; set; }
    
    public decimal PretBaza { get; set; }

    public Pizza(string nume, DimensiunePizza dimensiune, List<Ingredient> ingrediente, decimal pretBaza)
    {
       Nume = nume;
       Dimensiune = dimensiune;
       Ingrediente = ingrediente;
       PretBaza = pretBaza;
    }

    public abstract decimal CalculeazaPret();

    public void AdaugaIngrediente(Ingredient ingrediente)
    {
        Ingrediente.Add(ingrediente);
    }

    public override string ToString()
    {
        return $"nume {Nume}, dimensiune: {Dimensiune}, ingrediente {Ingrediente}, pret: {CalculeazaPret()} RON";
    }
}