using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PizzaApp.Mancare;
using PizzaApp;
class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += (s, e) =>
                {
                    // Cleanup code here
                    Console.WriteLine("Application shutting down...");
                };

                var consoleService = new ConsoleService();
                var fileService = new FileService();
                
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string pizzeriaFilePath = Path.Combine(appPath, "pizzeria_state.json");
                string usersFilePath = Path.Combine(appPath, "users_state.json");

                var pizzeria = Pizzeria.LoadState(pizzeriaFilePath, fileService);
                var authService = new AuthenticationService();
                authService.LoadUsers(usersFilePath, fileService);

                // Adăugăm un admin implicit dacă nu există utilizatori
                if (!fileService.Exists(usersFilePath))
                {
                    authService.Register("admin", "admin", true);
                    consoleService.WriteLine("Created default admin user (username: admin, password: admin)");
                    authService.SaveUsers(usersFilePath, fileService);
                }

                // Add some default data if necessary
                if (!pizzeria.GetIngredients().Any())
                {
                    pizzeria.AddIngredient(new Ingredient("Cheese", 5));
                    pizzeria.AddIngredient(new Ingredient("Pepperoni", 7));
                    pizzeria.AddIngredient(new Ingredient("Mushrooms", 4));
                }
            
                if (!pizzeria.GetMenu().Any())
                {
                    pizzeria.AddPizzaToMenu(new Pizza("Margherita", PizzaSize.Medium, 20, new List<Ingredient> { pizzeria.GetIngredients()[0] }));
                    pizzeria.AddPizzaToMenu(new Pizza("Pepperoni", PizzaSize.Large, 25, new List<Ingredient> { pizzeria.GetIngredients()[0], pizzeria.GetIngredients()[1] }));
                }
            
                User loggedInUser = null;
                while (true)
                {
                    consoleService.WriteLine("Bine ati venit la Pizza Amaro!");
                    consoleService.WriteLine("Alege o varianta:");
                    consoleService.WriteLine("1. Log in");
                    consoleService.WriteLine("2. Register");
                    consoleService.WriteLine("3. Vizualizare Meniu (guest mode, nu poti plasa comenzi)");
                    consoleService.WriteLine("4. Exit");
            
                    string choice = consoleService.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            consoleService.WriteLine("Nume:");
                            string username = consoleService.ReadLine();
                            consoleService.WriteLine("Parola:");
                            string password = ((ConsoleService)consoleService).ReadPassword();
                            loggedInUser = authService.Login(username, password);
            
                            if (loggedInUser != null)
                            {
                                if (loggedInUser.GetIsAdmin())
                                {
                                    AdminMenu(pizzeria, consoleService, authService, fileService);
                                }
                                else
                                {
                                    UserMenu(pizzeria, loggedInUser, consoleService);
                                }
                            }
                            break;
            
                        case "2":
                            consoleService.WriteLine("Nume:");
                            string newUsername = consoleService.ReadLine();
                            consoleService.WriteLine("Parola:");
                            string newPassword = ((ConsoleService)consoleService).ReadPassword();
                            consoleService.WriteLine("Admin? (yes/no):");
                            bool isAdmin = consoleService.ReadLine().ToLower() == "yes";
                            
                            string phoneNumber = null;
                            if (!isAdmin)
                            {
                                consoleService.WriteLine("Numar de telefon:");
                                phoneNumber = consoleService.ReadLine();
                            }

                            authService.Register(newUsername, newPassword, isAdmin, phoneNumber);
                            break;
            
                        case "3":
                            ViewMenu(pizzeria, consoleService);
                            consoleService.WriteLine("You are in guest mode. Please log in or register to place orders.");
                            break;
            
                        case "4":
                            consoleService.WriteLine("Thank you for using the Pizzeria App. Goodbye!");
                            // Salvăm starea înainte de a ieși
                            pizzeria.SaveState(pizzeriaFilePath, fileService);
                            authService.SaveUsers(usersFilePath, fileService);
                            return;
            
                        default:
                            consoleService.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            
                // Salvăm starea la închiderea aplicației
                pizzeria.SaveState(pizzeriaFilePath, fileService);
                authService.SaveUsers(usersFilePath, fileService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                // Asigură-te că procesul se închide complet
                Environment.Exit(0);
            }
        }


        static void AdminMenu(Pizzeria pizzeria, IConsoleService consoleService, AuthenticationService authService, IFileService fileService)
        {
            while (true)
            {
                consoleService.WriteLine("\nAdmin Menu:");
                consoleService.WriteLine("1. Vizualizare Meniu");
                consoleService.WriteLine("2. Adauga Pizza");
                consoleService.WriteLine("3. Sterge Pizza");
                consoleService.WriteLine("4. Actualizeaza Pizza");
                consoleService.WriteLine("5. Vizualizare Ingrediente");
                consoleService.WriteLine("6. Adauga Ingredient");
                consoleService.WriteLine("7. Sterge Ingredient");
                consoleService.WriteLine("8. Actualizeaza pretul Ingredientului");
                consoleService.WriteLine("9. Vizualizeaza istoricul comenzilor pentru un client");
                consoleService.WriteLine("10. Inregistreaza noi clienti");
                consoleService.WriteLine("\nRapoarte si Statistici:");
                consoleService.WriteLine("11. Vizualizare comenzi pe zi");
                consoleService.WriteLine("12. Raport pizza populara");
                consoleService.WriteLine("13. Raport venituri pe perioada");
                consoleService.WriteLine("14. Exit");

                string choice = consoleService.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMenu(pizzeria, consoleService);
                        break;
                    case "2":
                        AddPizza(pizzeria, consoleService);
                        break;
                    case "3":
                        RemovePizza(pizzeria, consoleService);
                        break;
                    case "4":
                        UpdatePizza(pizzeria, consoleService);
                        break;
                    case "5":
                        ViewIngredients(pizzeria, consoleService);
                        break;
                    case "6":
                        AddIngredient(pizzeria, consoleService);
                        break;
                    case "7":
                        RemoveIngredient(pizzeria, consoleService);
                        break;
                    case "8":
                        UpdateIngredientPrice(pizzeria, consoleService);
                        break;
                    case "9":
                        ViewOrderHistoryForClient(pizzeria, consoleService);
                        break;
                    case "10":
                        RegisterNewUser(consoleService, authService, fileService);
                        break;
                    case "11":
                        ViewOrdersByDate(pizzeria, consoleService);
                        break;
                    case "12":
                        ViewPopularPizzas(pizzeria, consoleService);
                        break;
                    case "13":
                        ViewRevenueReport(pizzeria, consoleService);
                        break;
                    case "14":
                        return;
                    default:
                        consoleService.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void UserMenu(Pizzeria pizzeria, User loggedInUser, IConsoleService consoleService)
        {
            Client client = new Client(loggedInUser.GetUsername(), loggedInUser.GetPhoneNumber());

            while (true)
            {
                consoleService.WriteLine("User Meniu:");
                consoleService.WriteLine("1. Vizualizare Meniu");
                consoleService.WriteLine("2. Comanda");
                consoleService.WriteLine("3. Vizualizare istoric meniu");
                consoleService.WriteLine("4. Exit");

                string choice = consoleService.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMenu(pizzeria, consoleService);
                        break;
                    case "2":
                        PlaceOrder(pizzeria, client, consoleService);
                        break;
                    case "3":
                        ViewOrderHistoryForClient(pizzeria, consoleService, client);
                        break;
                    case "4":
                        return;
                    default:
                        consoleService.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RegisterNewUser(IConsoleService consoleService, AuthenticationService authService, IFileService fileService)
        {
            consoleService.WriteLine("Nume:");
            string username = consoleService.ReadLine();
            consoleService.WriteLine("Parola:");
            string password = ((ConsoleService)consoleService).ReadPassword();
            consoleService.WriteLine("Admin? (yes/no):");
            bool isAdmin = consoleService.ReadLine().ToLower() == "yes";

            string phoneNumber = null;
            if (!isAdmin)
            {
                consoleService.WriteLine("Introdu numarul de telefon:");
                phoneNumber = consoleService.ReadLine();
            }

            authService.Register(username, password, isAdmin, phoneNumber);
            
            string usersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users_state.json");
            authService.SaveUsers(usersFilePath, fileService);
        }

        static void PlaceOrder(Pizzeria pizzeria, Client client, IConsoleService consoleService)
        {
            if (client == null)
            {
                consoleService.WriteLine("Error: Informatii despre client gresite.");
                return;
            }

            // Adăugăm clientul în dicționarul pizzeriei
            pizzeria.AddClient(client);

            List<Pizza> pizzas = new List<Pizza>();
            ViewMenu(pizzeria, consoleService);
            while (true)
            {
                consoleService.WriteLine("Alege pizza pentru comanda (or 'done'):");
                string pizzaName = consoleService.ReadLine();
                if (pizzaName?.ToLower() == "done")
                {
                    break;
                }
                var pizza = pizzeria.GetMenu().FirstOrDefault(p => p.GetName() == pizzaName);
                if (pizza != null)
                {
                    // Creăm o copie a pizza pentru comandă
                    var orderPizza = new Pizza(pizza.GetName(), pizza.GetSize(), pizza.GetBasePrice(), pizza.GetIngredients().ToList());
                    pizzas.Add(orderPizza);
                    consoleService.WriteLine($"Adaugat {pizza.GetName()} la comanda.");
                }
                else
                {
                    consoleService.WriteLine("Nu exista acest tip de pizza.");
                }
            }

            if (!pizzas.Any())
            {
                consoleService.WriteLine("Order cannot be empty. Please add at least one pizza.");
                return;
            }

            consoleService.WriteLine("Doresti livrare? (yes/no):");
            bool isDelivery = consoleService.ReadLine()?.ToLower() == "yes";

            try
            {
                Order order = new Order(client, pizzas, isDelivery);
                pizzeria.PlaceOrder(order);
                consoleService.WriteLine($"Pretul total al comenzii: {order.GetTotalCost()} RON");
                
                // Salvăm starea după plasarea comenzii
                SavePizzeriaState(pizzeria, new FileService());
                consoleService.WriteLine("Comanda plasata!");
            }
            catch (Exception ex)
            {
                consoleService.WriteLine($"Error placing order: {ex.Message}");
            }
        }

        static void ViewMenu(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Meniu:");
            foreach (var pizza in pizzeria.GetMenu())
            {
                consoleService.WriteLine($"{pizza.GetName()} - {pizza.GetSize()} - {pizza.GetBasePrice()} RON");
                consoleService.WriteLine("Ingrediente:");
                foreach (var ingredient in pizza.GetIngredients())
                {
                    consoleService.WriteLine($"  {ingredient.GetName()} - {ingredient.GetCost()} RON");
                }
            }
        }

        static void AddPizza(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Adauga pizza:");
            string name = consoleService.ReadLine();
            consoleService.WriteLine("Introdu dimensiunea (Small, Medium, Large):");
            PizzaSize size = (PizzaSize)Enum.Parse(typeof(PizzaSize), consoleService.ReadLine(), true);
            consoleService.WriteLine("Introdu pretul de baza:");
            decimal basePrice = decimal.Parse(consoleService.ReadLine());

            List<Ingredient> ingredients = new List<Ingredient>();
            while (true)
            {
                consoleService.WriteLine("Introdu numele ingredientului (or 'done'):");
                string ingredientName = consoleService.ReadLine();
                if (ingredientName.ToLower() == "done")
                {
                    break;
                }
                var ingredient = pizzeria.GetIngredients().FirstOrDefault(i => i.GetName() == ingredientName);
                if (ingredient != null)
                {
                    ingredients.Add(ingredient);
                }
                else
                {
                    consoleService.WriteLine("Ingredientul nu a fost gasit, incearca din nou.");
                }
            }

            pizzeria.AddPizzaToMenu(new Pizza(name, size, basePrice, ingredients));
            consoleService.WriteLine("Pizza adaugata.");
            
            // Salvăm starea după adăugarea unei pizza noi
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void RemovePizza(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Introdu pizza de sters:");
            string name = consoleService.ReadLine();
            pizzeria.RemovePizzaFromMenu(name);
            consoleService.WriteLine("Pizza stearsa.");
            
            // Salvăm starea după ștergerea unei pizza
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void UpdatePizza(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Introdu pizza pentru actualizat:");
            string name = consoleService.ReadLine();
            var pizza = pizzeria.GetMenu().FirstOrDefault(p => p.GetName() == name);
            if (pizza == null)
            {
                consoleService.WriteLine("Pizza nu a fost gasita.");
                return;
            }

            consoleService.WriteLine("Introdu dimensiunea (Mica, Medie, Mare):");
            PizzaSize size = (PizzaSize)Enum.Parse(typeof(PizzaSize), consoleService.ReadLine(), true);
            consoleService.WriteLine("Introdu noul pret:");
            decimal basePrice = decimal.Parse(consoleService.ReadLine());

            List<Ingredient> ingredients = new List<Ingredient>();
            while (true)
            {
                consoleService.WriteLine("Introdu numele ingredientului (or 'done' to stop):");
                string ingredientName = consoleService.ReadLine();
                if (ingredientName.ToLower() == "done")
                {
                    break;
                }
                var ingredient = pizzeria.GetIngredients().FirstOrDefault(i => i.GetName() == ingredientName);
                if (ingredient != null)
                {
                    ingredients.Add(ingredient);
                }
                else
                {
                    consoleService.WriteLine("Ingredientul nu a fost gasit, incearca din nou.");
                }
            }

            pizzeria.UpdatePizza(name, size, basePrice, ingredients);
            consoleService.WriteLine("Pizza actualizata.");
            
            // Salvăm starea după actualizarea unei pizza
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void ViewIngredients(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Ingrediente:");
            foreach (var ingredient in pizzeria.GetIngredients())
            {
                consoleService.WriteLine($"{ingredient.GetName()} - {ingredient.GetCost()} RON");
            }
        }

        static void AddIngredient(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Introduceti numele ingredientului:");
            string name = consoleService.ReadLine();
            consoleService.WriteLine("Introduceti pretul ingredientului:");
            decimal cost = decimal.Parse(consoleService.ReadLine());
            pizzeria.AddIngredient(new Ingredient(name, cost));
            consoleService.WriteLine("Ingredient adaugat.");

            // Salvăm starea după adăugarea unui ingredient
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void RemoveIngredient(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Ingredientul pentru stergere:");
            string name = consoleService.ReadLine();
            pizzeria.RemoveIngredient(name);
            consoleService.WriteLine("Ingredient sters.");

            // Salvăm starea după ștergerea unui ingredient
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void UpdateIngredientPrice(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Introduceti numele ingredientului pentru actualizare:");
            string name = consoleService.ReadLine();
            consoleService.WriteLine("Introduceti un nou pret pentru ingredient:");
            decimal newPrice = decimal.Parse(consoleService.ReadLine());
            pizzeria.UpdateIngredientPrice(name, newPrice);
            consoleService.WriteLine("Ingredientele si pretul au fost actualizate.");

            // Salvăm starea după actualizarea prețului unui ingredient
            SavePizzeriaState(pizzeria, new FileService());
        }

        static void ViewOrderHistory(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("Istoric Comenzi:");
            foreach (var order in pizzeria.GetOrders())
            {
                var client = pizzeria.GetClient(order.GetClientId());
                consoleService.WriteLine($"Data comenzii: {order.GetOrderDate()}");
                consoleService.WriteLine($"Client: {client?.GetName() ?? "Unknown"}");
                consoleService.WriteLine($"Pretul total: {order.GetTotalCost()} RON");
                consoleService.WriteLine($"Livrare: {(order.GetIsDelivery() ? "Yes" : "No")}");
                consoleService.WriteLine("Pizza:");
                foreach (var pizza in order.GetPizzas())
                {
                    consoleService.WriteLine($"  {pizza.GetName()} - {pizza.GetSize()} - {pizza.GetPrice()} RON");
                }
                consoleService.WriteLine("-------------------");
            }
        }

        static void ViewOrderHistoryForClient(Pizzeria pizzeria, IConsoleService consoleService, Client specificClient = null)
        {
            string clientName;
            if (specificClient != null)
            {
                clientName = specificClient.GetName();
            }
            else
            {
                consoleService.WriteLine("Introduceti numele clientului:");
                clientName = consoleService.ReadLine();
            }

            var clientOrders = pizzeria.GetOrders().Where(o => o.GetClientId() == clientName).ToList();

            if (!clientOrders.Any())
            {
                consoleService.WriteLine("No orders found for this client.");
                return;
            }

            consoleService.WriteLine($"Istoricul comenzilor pentru {clientName}:");
            foreach (var order in clientOrders)
            {
                consoleService.WriteLine($"Order Date: {order.GetOrderDate()}");
                consoleService.WriteLine($"Total Cost: {order.GetTotalCost()} RON");
                consoleService.WriteLine("Pizzas:");
                foreach (var pizza in order.GetPizzas())
                {
                    consoleService.WriteLine($"  {pizza.GetName()} - {pizza.GetSize()} - {pizza.GetPrice()} RON");
                }
            }
        }

        static void SavePizzeriaState(Pizzeria pizzeria, IFileService fileService)
        {
            string pizzeriaFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzeria_state.json");
            pizzeria.SaveState(pizzeriaFilePath, fileService);
        }

        static void ViewOrdersByDate(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("\nIntroduceti data pentru care doriti sa vedeti comenzile (format: dd/MM/yyyy):");
            if (DateTime.TryParse(consoleService.ReadLine(), out DateTime date))
            {
                var orders = pizzeria.GetOrdersByDate(date);
                if (!orders.Any())
                {
                    consoleService.WriteLine($"Nu exista comenzi pentru data {date.ToShortDateString()}");
                    return;
                }

                consoleService.WriteLine($"\nComenzi pentru data {date.ToShortDateString()}:");
                foreach (var order in orders)
                {
                    var client = pizzeria.GetClient(order.GetClientId());
                    consoleService.WriteLine($"\nClient: {client?.GetName() ?? "Necunoscut"}");
                    consoleService.WriteLine($"Ora comenzii: {order.GetOrderDate().ToShortTimeString()}");
                    consoleService.WriteLine($"Total: {order.GetTotalCost()} RON");
                    consoleService.WriteLine($"Livrare: {(order.GetIsDelivery() ? "Da" : "Nu")}");
                    consoleService.WriteLine("Pizza comandata:");
                    foreach (var pizza in order.GetPizzas())
                    {
                        consoleService.WriteLine($"  - {pizza.GetName()} ({pizza.GetSize()}) - {pizza.GetPrice()} RON");
                    }
                    consoleService.WriteLine("-------------------");
                }
                consoleService.WriteLine($"\nTotal comenzi in aceasta zi: {orders.Count}");
                consoleService.WriteLine($"Valoare totală: {orders.Sum(o => o.GetTotalCost())} RON");
            }
            else
            {
                consoleService.WriteLine("Format data invalid. Va rugam sa folositi formatul dd/MM/yyyy");
            }
        }

        static void ViewPopularPizzas(Pizzeria pizzeria, IConsoleService consoleService)
        {
            var popularPizzas = pizzeria.GetMostPopularPizzas();
            if (!popularPizzas.Any())
            {
                consoleService.WriteLine("\nNu exista inca comenzi pentru a genera statistici.");
                return;
            }

            consoleService.WriteLine("\nTop cele mai populare pizza:");
            int rank = 1;
            foreach (var pizza in popularPizzas)
            {
                var orderCount = pizzeria.GetOrders()
                    .SelectMany(o => o.GetPizzas())
                    .Count(p => p.GetName() == pizza.GetName());

                consoleService.WriteLine($"{rank}. {pizza.GetName()}");
                consoleService.WriteLine($"   Comenzi: {orderCount}");
                consoleService.WriteLine($"   Preț: {pizza.GetPrice()} RON");
                consoleService.WriteLine($"   Ingrediente: {string.Join(", ", pizza.GetIngredients().Select(i => i.GetName()))}");
                consoleService.WriteLine("-------------------");
                rank++;
            }
        }

        static void ViewRevenueReport(Pizzeria pizzeria, IConsoleService consoleService)
        {
            consoleService.WriteLine("\nIntroduceti data de inceput (format: dd/MM/yyyy):");
            if (!DateTime.TryParse(consoleService.ReadLine(), out DateTime startDate))
            {
                consoleService.WriteLine("Format data invalid. Vă rugam sa folositi formatul dd/MM/yyyy");
                return;
            }

            consoleService.WriteLine("Introduceti data de sfarsit (format: dd/MM/yyyy):");
            if (!DateTime.TryParse(consoleService.ReadLine(), out DateTime endDate))
            {
                consoleService.WriteLine("Format data invalid. Vă rugam sa folositi formatul dd/MM/yyyy");
                return;
            }

            if (startDate > endDate)
            {
                consoleService.WriteLine("Data de inceput nu poate fi mai mare decat data de sfarsit!");
                return;
            }

            decimal totalRevenue = pizzeria.GetTotalRevenue(startDate, endDate);
            var ordersInPeriod = pizzeria.GetOrders()
                .Where(o => o.GetOrderDate().Date >= startDate.Date && o.GetOrderDate().Date <= endDate.Date)
                .ToList();

            consoleService.WriteLine($"\nRaport venituri pentru perioada {startDate.ToShortDateString()} - {endDate.ToShortDateString()}:");
            consoleService.WriteLine($"Numar total comenzi: {ordersInPeriod.Count}");
            consoleService.WriteLine($"Venit total: {totalRevenue} RON");
            
            if (ordersInPeriod.Any())
            {
                decimal averageOrderValue = totalRevenue / ordersInPeriod.Count;
                consoleService.WriteLine($"Valoare medie comanda: {averageOrderValue:F2} RON");
                
                var deliveryOrders = ordersInPeriod.Count(o => o.GetIsDelivery());
                consoleService.WriteLine($"Comenzi cu livrare: {deliveryOrders} ({(deliveryOrders * 100.0 / ordersInPeriod.Count):F1}%)");
                
                var mostExpensiveOrder = ordersInPeriod.MaxBy(o => o.GetTotalCost());
                consoleService.WriteLine($"Cea mai mare comandă: {mostExpensiveOrder.GetTotalCost()} RON");
            }
        }
    }