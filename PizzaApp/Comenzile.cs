namespace PizzaApp;

public class Comenzile
{
    public Clienti Clienti { get; set; }
    public List<Pizza>PizzaComandata { get; set; }
    public string MetodaLivrare{get;set;}
    public decimal CostTotal { get;private set; }

    public bool EsteLivrareLaDomiciliu;

    public Comenzile(Clienti clienti, string metodaLivrare)
    {
        Clienti = clienti;
        MetodaLivrare = metodaLivrare;
        PizzaComandata = new List<Pizza>();
    }

    public void AfisarePizza(Pizza pizza)
    {
        PizzaComandata.Add(pizza);
    }

    public void CalculeazaCostTotal()
    {
        decimal totalCost = 0;
        foreach (var pizza in PizzaComandata)
        {
            CostTotal += pizza.CalculeazaPret();
        }

        if (EsteLivrareLaDomiciliu)
        {
            CostTotal += 10; 
        }

        if (Clienti.EsteFidel())
        {
            CostTotal *= 0.9m; 
        }

        CostTotal = CostTotal;
    }
    
    public override string ToString()
    {
        return $"Comanda pentru {Clienti.Nume}, Cost: {CostTotal} RON, Livrare: {(EsteLivrareLaDomiciliu ? "Da" : "Nu")}";
    }
}