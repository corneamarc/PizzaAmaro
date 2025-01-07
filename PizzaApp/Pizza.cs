namespace PizzaAmaro;

public abstract class Pizza
{
    public string Nume { get; set; }
    public DimensiunePizza Dimensiune { get; set; }
    public decimal PretDeBaza { get; set; }
    public List<string> Ingrediente { get; set; }

    protected Pizza(string nume, DimensiunePizza dimensiune, decimal pretDeBaza, List<string> ingrediente)
    {
        Nume = nume;
        Dimensiune = dimensiune;
        PretDeBaza = pretDeBaza;
        Ingrediente = ingrediente;
    }

    public abstract decimal CalculeazaPret();

    public override string ToString()
    {
        return $"{Nume} ({Dimensiune}): {PretDeBaza} RON, Ingrediente: {string.Join(", ", Ingrediente)}";
    }
}
