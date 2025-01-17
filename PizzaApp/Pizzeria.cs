using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PizzaApp.Mancare;
namespace PizzaApp;

/// <summary>
    /// Clasa principală care gestionează întreaga logică a pizzeriei
    /// Responsabilă pentru gestionarea meniului, comenzilor, clienților și ingredientelor
    /// </summary>
    public class Pizzeria
    {
        /// <summary>
        /// Numele pizzeriei
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adresa fizică a pizzeriei
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Lista de pizza disponibile în meniu
        /// </summary>
        public List<Pizza> Menu { get; set; }

        /// <summary>
        /// Istoricul tuturor comenzilor plasate
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// Lista ingredientelor disponibile pentru prepararea pizzelor
        /// </summary>
        public List<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Dicționar care mapează numele clienților la obiectele Client
        /// Folosit pentru acces rapid la informațiile clienților
        /// </summary>
        public Dictionary<string, Client> Clients { get; set; }

        /// <summary>
        /// Constructor implicit - inițializează toate colecțiile necesare
        /// </summary>
        public Pizzeria()
        {
            Menu = new List<Pizza>();
            Orders = new List<Order>();
            Ingredients = new List<Ingredient>();
            Clients = new Dictionary<string, Client>();
        }

        /// <summary>
        /// Constructor cu parametri - setează numele și adresa pizzeriei
        /// </summary>
        /// <param name="name">Numele pizzeriei</param>
        /// <param name="address">Adresa fizică a pizzeriei</param>
        public Pizzeria(string name, string address) : this()
        {
            Name = name;
            Address = address;
        }

        // Metode de acces pentru proprietățile de bază
        public string GetName() => Name;
        public string GetAddress() => Address;
        public List<Pizza> GetMenu() => Menu;
        public List<Order> GetOrders() => Orders;
        public List<Ingredient> GetIngredients() => Ingredients;

        /// <summary>
        /// Adaugă o nouă pizza în meniul pizzeriei
        /// </summary>
        /// <param name="pizza">Obiectul Pizza de adăugat în meniu</param>
        public void AddPizzaToMenu(Pizza pizza)
        {
            Menu.Add(pizza);
        }

        /// <summary>
        /// Elimină o pizza din meniu după nume
        /// </summary>
        /// <param name="pizzaName">Numele pizzei de eliminat</param>
        public void RemovePizzaFromMenu(string pizzaName)
        {
            Menu.RemoveAll(p => p.GetName() == pizzaName);
        }

        /// <summary>
        /// Actualizează detaliile unei pizza existente în meniu
        /// </summary>
        /// <param name="pizzaName">Numele pizzei de actualizat</param>
        /// <param name="size">Noua dimensiune</param>
        /// <param name="basePrice">Noul preț de bază</param>
        /// <param name="ingredients">Noua listă de ingrediente</param>
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

        /// <summary>
        /// Adaugă un nou ingredient în lista de ingrediente disponibile
        /// </summary>
        /// <param name="ingredient">Ingredientul de adăugat</param>
        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        /// <summary>
        /// Elimină un ingredient din lista de ingrediente disponibile
        /// </summary>
        /// <param name="ingredientName">Numele ingredientului de eliminat</param>
        public void RemoveIngredient(string ingredientName)
        {
            Ingredients.RemoveAll(i => i.GetName() == ingredientName);
        }

        /// <summary>
        /// Actualizează prețul unui ingredient existent
        /// </summary>
        /// <param name="ingredientName">Numele ingredientului</param>
        /// <param name="newPrice">Noul preț al ingredientului</param>
        public void UpdateIngredientPrice(string ingredientName, decimal newPrice)
        {
            var ingredient = Ingredients.FirstOrDefault(i => i.GetName() == ingredientName);
            if (ingredient != null)
            {
                ingredient.SetCost(newPrice);
            }
        }

        /// <summary>
        /// Înregistrează o nouă comandă în sistem și o adaugă la istoricul clientului
        /// </summary>
        /// <param name="order">Comanda de înregistrat</param>
        public void PlaceOrder(Order order)
        {
            Orders.Add(order);
            var client = Clients.GetValueOrDefault(order.GetClientId());
            if (client != null)
            {
                client.AddOrder(order);
            }
        }

        /// <summary>
        /// Adaugă sau actualizează un client în dicționarul de clienți
        /// </summary>
        /// <param name="client">Clientul de adăugat sau actualizat</param>
        public void AddClient(Client client)
        {
            if (!Clients.ContainsKey(client.GetName()))
            {
                Clients[client.GetName()] = client;
            }
        }

        /// <summary>
        /// Obține un client după ID (nume)
        /// </summary>
        /// <param name="clientId">ID-ul (numele) clientului</param>
        /// <returns>Obiectul Client sau null dacă nu este găsit</returns>
        public Client GetClient(string clientId)
        {
            return Clients.GetValueOrDefault(clientId);
        }

        /// <summary>
        /// Obține toate comenzile plasate la o anumită dată
        /// </summary>
        /// <param name="date">Data pentru care se doresc comenzile</param>
        /// <returns>Lista de comenzi din ziua respectivă</returns>
        public List<Order> GetOrdersByDate(DateTime date)
        {
            return Orders.Where(o => o.GetOrderDate().Date == date.Date).ToList();
        }

        /// <summary>
        /// Obține lista de pizza ordonată după popularitate (numărul de comenzi)
        /// Folosit pentru rapoarte și statistici
        /// </summary>
        /// <returns>Lista de pizza ordonată descrescător după numărul de comenzi</returns>
        public List<Pizza> GetMostPopularPizzas()
        {
            return Orders.SelectMany(o => o.GetPizzas())
                         .GroupBy(p => p.GetName())
                         .OrderByDescending(g => g.Count())
                         .Select(g => g.First())
                         .ToList();
        }

        /// <summary>
        /// Calculează venitul total pentru o perioadă specificată
        /// </summary>
        /// <param name="startDate">Data de început a perioadei</param>
        /// <param name="endDate">Data de sfârșit a perioadei</param>
        /// <returns>Suma totală a veniturilor din perioada specificată</returns>
        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            return Orders.Where(o => o.GetOrderDate() >= startDate && o.GetOrderDate() <= endDate)
                         .Sum(o => o.GetTotalCost());
        }

        /// <summary>
        /// Salvează starea curentă a pizzeriei în format JSON
        /// Include meniul, comenzile, ingredientele și clienții
        /// </summary>
        /// <param name="filePath">Calea către fișierul în care se salvează starea</param>
        /// <param name="fileService">Serviciul pentru operații cu fișiere</param>
        public void SaveState(string filePath, IFileService fileService)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string jsonString = JsonSerializer.Serialize(this, options);
                fileService.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving state: {ex.Message}");
            }
        }

        /// <summary>
        /// Încarcă starea pizzeriei din fișierul JSON
        /// Dacă fișierul nu există, creează o nouă instanță cu date implicite
        /// </summary>
        /// <param name="filePath">Calea către fișierul din care se încarcă starea</param>
        /// <param name="fileService">Serviciul pentru operații cu fișiere</param>
        /// <returns>Instanța Pizzeria încărcată sau una nouă dacă fișierul nu există</returns>
        public static Pizzeria LoadState(string filePath, IFileService fileService)
        {
            try
            {
                if (fileService.Exists(filePath))
                {
                    string jsonString = fileService.ReadAllText(filePath);
                    var options = new JsonSerializerOptions 
                    { 
                        ReferenceHandler = ReferenceHandler.Preserve
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
    }