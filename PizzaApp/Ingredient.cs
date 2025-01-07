namespace PizzaAmaro;

public class Ingredient
{
    public string Nume { get; set; }
    public decimal Pret { get; set; }

    public Ingredient(string nume, decimal pret)
    {
        Nume = nume;
        Pret = pret;
    }

    public override string ToString()
    {
        return $"{Nume}: {Pret} RON";
    }
}