namespace PizzaAmaro;

public class PizzaPersonalizata : Pizza
{
    private const decimal CostPersonalizare = 30m;

    public PizzaPersonalizata(string nume, DimensiunePizza dimensiune, List<string> ingrediente)
        : base(nume, dimensiune, 0, ingrediente) { }

    public override decimal CalculeazaPret()
    {
        decimal pretIngredient = 0;
        foreach (var ingredient in Ingrediente)
        {
            pretIngredient += 5m; // Exemplu de pret fix per ingredient
        }
        return pretIngredient + CostPersonalizare;
    }

    public override string ToString()
    {
        return $"{Nume} (Personalizata, {Dimensiune}): {CalculeazaPret()} RON, Ingrediente: {string.Join(", ", Ingrediente)}";
    }
}

public enum DimensiunePizza { Mica, Medie, Mare }