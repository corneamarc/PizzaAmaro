namespace PizzaApp;

public class PizzaPersonalizata:Pizza
{
    public PizzaPersonalizata(string nume, DimensiunePizza dimensiune, List<Ingredient> ingrediente, decimal pretbaza) : base(nume,
        dimensiune, ingrediente,pretbaza){}

    public override decimal CalculeazaPret()
    {
        return Ingrediente.Sum(i => i.Cost) + 30;
    }
}