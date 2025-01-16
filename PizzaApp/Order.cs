using PizzaApp.Mancare;
namespace PizzaApp;

public class Order
{
    public string ClientId { get; set; }
    public List<Pizza> Pizzas { get; set; }
    public bool IsDelivery { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime OrderDate { get; set; }

    public Order()
    {
        Pizzas = new List<Pizza>();
        OrderDate = DateTime.Now;
        TotalCost = 0;
    }

    public Order(Client client, List<Pizza> pizzas, bool isDelivery)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        ClientId = client.GetName();
        Pizzas = pizzas ?? new List<Pizza>();
        IsDelivery = isDelivery;
        OrderDate = DateTime.Now;
        TotalCost = CalculateTotalCost(client.IsLoyalCustomer());
    }

    private decimal CalculateTotalCost(bool isLoyalCustomer)
    {
        if (Pizzas == null) return 0;
            
        decimal total = Pizzas.Sum(p => p?.GetPrice() ?? 0);
        if (IsDelivery)
        {
            total += 10; // Delivery cost
        }
        if (isLoyalCustomer)
        {
            total *= 0.9m; // 10% discount for loyal customers
        }
        return total;
    }

    public string GetClientId() => ClientId;
    public List<Pizza> GetPizzas() => Pizzas ?? (Pizzas = new List<Pizza>());
    public bool GetIsDelivery() => IsDelivery;
    public decimal GetTotalCost() => TotalCost;
    public DateTime GetOrderDate() => OrderDate;
}