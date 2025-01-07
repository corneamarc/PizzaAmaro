namespace PizzaAmaro;

public class Comanda
{
    public Client Client { get; set; }
    public List<Pizza> PizzeComandate { get; set; }
    public bool EsteLivrareLaDomiciliu { get; set; }
    public decimal CostTotal { get; private set; }

    public Comanda(Client client, List<Pizza> pizzeComandate, bool esteLivrareLaDomiciliu)
    {
        Client = client;
        PizzeComandate = pizzeComandate;
        EsteLivrareLaDomiciliu = esteLivrareLaDomiciliu;
        CalculeazaCostTotal();
    }

    private void CalculeazaCostTotal()
    {
        decimal cost = PizzeComandate.Sum(pizza => pizza.CalculeazaPret());
        if (EsteLivrareLaDomiciliu) cost += 10;
        if (Client.EsteFidel()) cost *= 0.9m;
        CostTotal = cost;
    }

    public override string ToString()
    {
        return $"Comanda pentru {Client.Nume}, Cost: {CostTotal} RON, Livrare: {(EsteLivrareLaDomiciliu ? "Da" : "Nu")}";
    }
}

  
