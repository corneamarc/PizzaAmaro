using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PizzaApp.Mancare;
namespace PizzaApp;

public class Pizzeria
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Pizza> Menu { get; set; }
        public List<Order> Orders { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public Dictionary<string, Client> Clients { get; set; }

        public Pizzeria()
        {
            Menu = new List<Pizza>();
            Orders = new List<Order>();
            Ingredients = new List<Ingredient>();
            Clients = new Dictionary<string, Client>();
        }

        public Pizzeria(string name, string address) : this()
        {
            Name = name;
            Address = address;
        }

        public string GetName() => Name;
        public string GetAddress() => Address;
        public List<Pizza> GetMenu() => Menu;
        public List<Order> GetOrders() => Orders;
        public List<Ingredient> GetIngredients() => Ingredients;

        public void AddPizzaToMenu(Pizza pizza)
        {
            Menu.Add(pizza);
        }

        public void RemovePizzaFromMenu(string pizzaName)
        {
            Menu.RemoveAll(p => p.GetName() == pizzaName);
        }

        public void UpdatePizza(string pizzaName, PizzaSize size, decimal basePrice, List<Ingredient> ingredients)
        {
            var pizza = Menu.FirstOrDefault(p => p.GetName() == pizzaName);
            if (pizza != null)
            {
                pizza.SetSize(size);
                pizza.SetBasePrice(basePrice);
                pizza.SetIngredients(ingredients);
            }
        }

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(string ingredientName)
        {
            Ingredients.RemoveAll(i => i.GetName() == ingredientName);
        }

        public void UpdateIngredientPrice(string ingredientName, decimal newPrice)
        {
            var ingredient = Ingredients.FirstOrDefault(i => i.GetName() == ingredientName);
            if (ingredient != null)
            {
                ingredient.SetCost(newPrice);
            }
        }

        public void PlaceOrder(Order order)
        {
            Orders.Add(order);
            var client = Clients.GetValueOrDefault(order.GetClientId());
            if (client != null)
            {
                client.AddOrder(order);
            }
        }

        public List<Order> GetOrdersByDate(DateTime date)
        {
            return Orders.Where(o => o.GetOrderDate().Date == date.Date).ToList();
        }

        public List<Pizza> GetMostPopularPizzas()
        {
            return Orders.SelectMany(o => o.GetPizzas())
                         .GroupBy(p => p.GetName())
                         .OrderByDescending(g => g.Count())
                         .Select(g => g.First())
                         .ToList();
        }

        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            return Orders.Where(o => o.GetOrderDate() >= startDate && o.GetOrderDate() <= endDate)
                         .Sum(o => o.GetTotalCost());
        }

        public void SaveState(string filePath, IFileService fileService)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };
                string jsonString = JsonSerializer.Serialize(this, options);
                fileService.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving state: {ex.Message}");
            }
        }

        public static Pizzeria LoadState(string filePath, IFileService fileService)
        {
            try
            {
                if (fileService.Exists(filePath))
                {
                    string jsonString = fileService.ReadAllText(filePath);
                    var options = new JsonSerializerOptions 
                    { 
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    };
                    return JsonSerializer.Deserialize<Pizzeria>(jsonString, options);
                }
                else
                {
                    Console.WriteLine("State file not found. Creating a new pizzeria.");
                    return new Pizzeria("Pizza Palace", "123 Main St");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading state: {ex.Message}");
                return new Pizzeria("Pizza Palace", "123 Main St");
            }
        }

        public void AddClient(Client client)
        {
            if (!Clients.ContainsKey(client.GetName()))
            {
                Clients[client.GetName()] = client;
            }
        }

        public Client GetClient(string clientId)
        {
            return Clients.GetValueOrDefault(clientId);
        }
    }