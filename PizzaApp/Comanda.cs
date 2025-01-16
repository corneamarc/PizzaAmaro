namespace PizzaApp;

public class Comanda
{
    public Client Client { get; set; }
    public List<Pizza> ListaPizza { get; set; } 
    public string MetodaLivrare { get; set; }
    public decimal Cost { get; set; }

    public Comanda(Client client, List<Pizza> listaPizza, string metodaLivrare, decimal cost)
    {
        Client = client;
        ListaPizza = listaPizza;
        MetodaLivrare = metodaLivrare;
        Cost = cost;
    }

    private decimal CalculeazaCostTotal()
    {
        decimal cost = ListaPizza.Sum(p => p.CalculeazaPret());
        if (MetodaLivrare == "Livrare")
        {
            cost += 10;
        }

        if (Client.EsteFidel())
        {
            cost *= 0.9m;
        }
        return cost;
    }

    public override string ToString()
    {
        return $"Client: {Client.Nume}, Livrare: {MetodaLivrare}, Cost: {Cost} ron, Pizza: ";
    }
}