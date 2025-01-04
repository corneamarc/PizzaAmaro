namespace PizzaApp;

public class Ingrediente
{
    private string _nume;
    private decimal _cost;
    
    public string GetNume() => _nume;
    public decimal GetCost() => _cost;


    public Ingrediente(string nume, decimal cost)
    {
        _nume = nume;
        _cost = cost;
    }
}