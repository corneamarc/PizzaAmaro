namespace PizzaApp;

public class PizzaPersonalizata : Pizza
{
    public PizzaPersonalizata(string nume, Dimensiune dimensiune) : base(nume, dimensiune, 30m)
    {
    }

    public override decimal CalculeazaPret()
    {
        decimal costIngrediente = 0;
        foreach (var ingrediente in Ingredientes())
        {
            costIngrediente += ingrediente.GetCost();
        }

        return 30m + costIngrediente;

    }

}