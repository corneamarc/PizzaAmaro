namespace PizzaAmaro;

public class PizzaStandard : Pizza
{
    public PizzaStandard(string nume, DimensiunePizza dimensiune, decimal pretDeBaza, List<string> ingrediente)
        : base(nume, dimensiune, pretDeBaza, ingrediente) { }

    public override decimal CalculeazaPret()
    {
        return PretDeBaza;
    }
}