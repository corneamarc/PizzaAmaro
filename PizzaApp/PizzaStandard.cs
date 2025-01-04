using System.Diagnostics.CodeAnalysis;

namespace PizzaApp;

public class PizzaStandard : Pizza
{


    public PizzaStandard(string nume, DimensiunePizza dimensiune, List<Ingredient> ingrediente, decimal pretbaza) : base(nume,
        dimensiune, ingrediente,pretbaza){}

    public override decimal CalculeazaPret()
    {
        switch (Dimensiune)
        {
            case DimensiunePizza.Mica:
                return 20m;
            case DimensiunePizza.Medie:
                return 30m;
            case DimensiunePizza.Mare:
                return 50m;
            default:
                throw new InvalidOperationException($"dimensiunea pizzei {Dimensiune} nu este valida");
        }
        
    }
}