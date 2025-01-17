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
        /// <summary>
        /// Metoda principală a aplicației care gestionează întregul flux al programului
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                // Handler pentru închiderea aplicației - se asigură că totul este salvat corect
                AppDomain.CurrentDomain.ProcessExit += (s, e) =>
                {
                    Console.WriteLine("Application shutting down...");
                };

                // Inițializăm serviciile necesare
                var consoleService = new ConsoleService();      // Pentru interacțiunea cu consola
                var fileService = new FileService();            // Pentru operații cu fișiere
                
                // Definim căile către fișierele de date
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string pizzeriaFilePath = Path.Combine(appPath, "pizzeria_state.json");
                string usersFilePath = Path.Combine(appPath, "users_state.json");

                // Încărcăm starea pizzeriei și a utilizatorilor din fișiere
                var pizzeria = Pizzeria.LoadState(pizzeriaFilePath, fileService);
                var authService = new AuthenticationService();
                authService.LoadUsers(usersFilePath, fileService);

                // Creăm un cont de admin implicit dacă nu există utilizatori
                if (!fileService.Exists(usersFilePath))
                {
                    authService.Register("admin", "admin", true);
                    consoleService.WriteLine("Created default admin user (username: admin, password: admin)");
                    authService.SaveUsers(usersFilePath, fileService);
                }

                // Adăugăm date implicite dacă meniul este gol
                if (!pizzeria.GetIngredients().Any())
                {
                    // Ingrediente de bază
                    pizzeria.AddIngredient(new Ingredient("Cheese", 5));
                    pizzeria.AddIngredient(new Ingredient("Pepperoni", 7));
                    pizzeria.AddIngredient(new Ingredient("Mushrooms", 4));
                }
            
                if (!pizzeria.GetMenu().Any())
                {
                    // Pizza-uri standard în meniu
                    pizzeria.AddPizzaToMenu(new Pizza("Margherita", PizzaSize.Medium, 20, 
                        new List<Ingredient> { pizzeria.GetIngredients()[0] }));
                    pizzeria.AddPizzaToMenu(new Pizza("Pepperoni", PizzaSize.Large, 25, 
                        new List<Ingredient> { pizzeria.GetIngredients()[0], pizzeria.GetIngredients()[1] }));
                }
            
                User loggedInUser = null;

                // Bucla principală a aplicației
                while (true)
                {
                    // Meniul principal
                    consoleService.WriteLine("Bine ati venit la Pizza Amaro!");
                    consoleService.WriteLine("Alege o varianta:");
                    consoleService.WriteLine("1. Log in");
                    consoleService.WriteLine("2. Register");
                    consoleService.WriteLine("3. Vizualizare Meniu (guest mode, nu poti plasa comenzi)");
                    consoleService.WriteLine("4. Exit");
            
                    string choice = consoleService.ReadLine();
                    switch (choice)
                    {
                        case "1": // Autentificare utilizator
                            consoleService.WriteLine("Nume:");
                            string username = consoleService.ReadLine();
                            consoleService.WriteLine("Parola:");
                            string password = ((ConsoleService)consoleService).ReadPassword();
                            loggedInUser = authService.Login(username, password);
            
                            if (loggedInUser != null)
                            {
                                // Direcționăm utilizatorul către meniul corespunzător (admin sau user)
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
            
                        case "2": // Înregistrare utilizator nou
                            consoleService.WriteLine("Nume:");
                            string newUsername = consoleService.ReadLine();
                            consoleService.WriteLine("Parola:");
                            string newPassword = ((ConsoleService)consoleService).ReadPassword();
                            consoleService.WriteLine("Admin? (yes/no):");
                            bool isAdmin = consoleService.ReadLine().ToLower() == "yes";
                            
                            // Pentru utilizatori non-admin, cerem numărul de telefon
                            string phoneNumber = null;
                            if (!isAdmin)
                            {
                                consoleService.WriteLine("Numar de telefon:");
                                phoneNumber = consoleService.ReadLine();
                            }

                            authService.Register(newUsername, newPassword, isAdmin, phoneNumber);
                            break;
            
                        case "3": // Vizualizare meniu în mod guest
                            ViewMenu(pizzeria, consoleService);
                            consoleService.WriteLine("You are in guest mode. Please log in or register to place orders.");
                            break;
            
                        case "4": // Ieșire din aplicație
                            consoleService.WriteLine("Thank you for using the Pizzeria App. Goodbye!");
                            // Salvăm starea înainte de ieșire
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
                // Gestionarea erorilor neașteptate
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                // Ne asigurăm că procesul se închide complet
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Meniul administratorului - oferă acces la toate funcționalitățile de management
        /// </summary>
        static void AdminMenu(Pizzeria pizzeria, IConsoleService consoleService, AuthenticationService authService, IFileService fileService)
        {
            while (true)
            {
                // Afișăm opțiunile disponibile pentru administrator
                consoleService.WriteLine("\nAdmin Menu:");
                // Opțiuni pentru gestionarea meniului
                consoleService.WriteLine("1. Vizualizare Meniu");
                consoleService.WriteLine("2. Adauga Pizza");
                consoleService.WriteLine("3. Sterge Pizza");
                consoleService.WriteLine("4. Actualizeaza Pizza");
                // Opțiuni pentru gestionarea ingredientelor
                consoleService.WriteLine("5. Vizualizare Ingrediente");
                consoleService.WriteLine("6. Adauga Ingredient");
                consoleService.WriteLine("7. Sterge Ingredient");
                consoleService.WriteLine("8. Actualizeaza pretul Ingredientului");
                // Opțiuni pentru gestionarea clienților și comenzilor
                consoleService.WriteLine("9. Vizualizeaza istoricul comenzilor pentru un client");
                consoleService.WriteLine("10. Inregistreaza noi clienti");
                // Rapoarte și statistici
                consoleService.WriteLine("\nRapoarte si Statistici:");
                consoleService.WriteLine("11. Vizualizare comenzi pe zi");
                consoleService.WriteLine("12. Raport pizza populara");
                consoleService.WriteLine("13. Raport venituri pe perioada");
                consoleService.WriteLine("14. Exit");

                string choice = consoleService.ReadLine();
                switch (choice)
                {
                    // Gestionare meniu
                    case "1": ViewMenu(pizzeria, consoleService); break;
                    case "2": AddPizza(pizzeria, consoleService); break;
                    case "3": RemovePizza(pizzeria, consoleService); break;
                    case "4": UpdatePizza(pizzeria, consoleService); break;
                    
                    // Gestionare ingrediente
                    case "5": ViewIngredients(pizzeria, consoleService); break;
                    case "6": AddIngredient(pizzeria, consoleService); break;
                    case "7": RemoveIngredient(pizzeria, consoleService); break;
                    case "8": UpdateIngredientPrice(pizzeria, consoleService); break;
                    
                    // Gestionare clienți și comenzi
                    case "9": ViewOrderHistoryForClient(pizzeria, consoleService); break;
                    case "10": RegisterNewUser(consoleService, authService, fileService); break;
                    
                    // Rapoarte și statistici
                    case "11": ViewOrdersByDate(pizzeria, consoleService); break;
                    case "12": ViewPopularPizzas(pizzeria, consoleService); break;
                    case "13": ViewRevenueReport(pizzeria, consoleService); break;
                    
                    case "14": return; // Ieșire din meniul admin
                    
                    default:
                        consoleService.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Meniul utilizatorului normal - permite vizualizarea meniului și plasarea comenzilor
        /// </summary>
        static void UserMenu(Pizzeria pizzeria, User loggedInUser, IConsoleService consoleService)
        {
            // Creăm un obiect Client pentru utilizatorul autentificat
            // Acest obiect va fi folosit pentru a gestiona comenzile și istoricul
            Client client = new Client(loggedInUser.GetUsername(), loggedInUser.GetPhoneNumber());

            while (true)
            {
                // Afișăm opțiunile disponibile pentru client
                consoleService.WriteLine("User Meniu:");
                consoleService.WriteLine("1. Vizualizare Meniu");           // Vizualizare produse disponibile
                consoleService.WriteLine("2. Comanda");                     // Plasare comandă nouă
                consoleService.WriteLine("3. Vizualizare istoric meniu");   // Istoric comenzi personale
                consoleService.WriteLine("4. Exit");                        // Ieșire din meniu

                string choice = consoleService.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMenu(pizzeria, consoleService);                 // Afișează meniul complet
                        break;
                    case "2":
                        PlaceOrder(pizzeria, client, consoleService);       // Procesează o nouă comandă
                        break;
                    case "3":
                        // Afișează istoricul comenzilor pentru clientul curent
                        ViewOrderHistoryForClient(pizzeria, consoleService, client);
                        break;
                    case "4":
                        return;                                             // Ieșire din meniul utilizatorului
                    default:
                        consoleService.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RegisterNewUser(IConsoleService consoleService, AuthenticationService authService, IFileService fileService)
        {
            // Colectăm informațiile necesare pentru înregistrare
            consoleService.WriteLine("Nume:");
            string username = consoleService.ReadLine();
            
            // Citim parola în mod securizat (caractere ascunse)
            consoleService.WriteLine("Parola:");
            string password = ((ConsoleService)consoleService).ReadPassword();
            
            // Determinăm dacă utilizatorul va fi admin
            consoleService.WriteLine("Admin? (yes/no):");
            bool isAdmin = consoleService.ReadLine().ToLower() == "yes";

            // Pentru utilizatorii non-admin, cerem și numărul de telefon
            string phoneNumber = null;
            if (!isAdmin)
            {
                consoleService.WriteLine("Introdu numarul de telefon:");
                phoneNumber = consoleService.ReadLine();
            }

            // Înregistrăm utilizatorul în sistem
            authService.Register(username, password, isAdmin, phoneNumber);
            
            // Salvăm modificările în fișierul de utilizatori
            string usersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users_state.json");
            authService.SaveUsers(usersFilePath, fileService);
        }

        /// <summary>
        /// Metodă pentru plasarea unei comenzi noi
        /// Permite alegerea între pizza standard și personalizată
        /// </summary>
        static void PlaceOrder(Pizzeria pizzeria, Client client, IConsoleService consoleService)
        {
            // Validăm clientul
            if (client == null)
            {
                consoleService.WriteLine("Error: Informatii despre client gresite.");
                return;
            }

            // Înregistrăm/actualizăm clientul în sistemul pizzeriei
            pizzeria.AddClient(client);

            // Lista pentru stocarea pizzelor comandate
            List<Pizza> pizzas = new List<Pizza>();
            bool finishOrder = false;

            // Bucla pentru adăugarea pizzelor în comandă
            while (!finishOrder)
            {
                // Meniul de selecție a tipului de pizza
                consoleService.WriteLine("\nAlege tipul de pizza:");
                consoleService.WriteLine("1. Pizza Standard (pret fix in functie de dimensiune)");
                consoleService.WriteLine("2. Pizza Personalizata (pret bazat pe ingrediente + 30 RON)");
                consoleService.WriteLine("3. Finalizare comanda");

                string choice = consoleService.ReadLine();
                switch (choice)
                {
                    case "1": 
                        AddStandardPizza(pizzeria, pizzas, consoleService);    // Adăugare pizza din meniu
                        break;
                    case "2": 
                        AddCustomPizza(pizzeria, pizzas, consoleService);      // Creare pizza personalizată
                        break;
                    case "3": 
                        finishOrder = true;                                    // Finalizare comandă
                        break;
                    default:
                        consoleService.WriteLine("Optiune invalida. Te rog incearca din nou.");
                        break;
                }
            }

            // Verificăm dacă comanda conține cel puțin o pizza
            if (!pizzas.Any())
            {
                consoleService.WriteLine("Comanda nu poate fi goala. Te rog adauga cel putin o pizza.");
                return;
            }

            // Afișăm rezumatul comenzii pentru confirmare
            consoleService.WriteLine("\nResumat comanda:");
            foreach (var pizza in pizzas)
            {
                consoleService.WriteLine($"- {pizza.GetName()} ({pizza.GetSize()}) - {pizza.GetPrice()} RON");
                consoleService.WriteLine("  Ingrediente: " + string.Join(", ", pizza.GetIngredients().Select(i => i.GetName())));
            }

            // Opțiunea de livrare (adaugă 10 RON la cost)
            consoleService.WriteLine("\nDoresti livrare? (yes/no):");
            bool isDelivery = consoleService.ReadLine()?.ToLower() == "yes";

            try
            {
                // Creăm și procesăm comanda
                Order order = new Order(client, pizzas, isDelivery);
                pizzeria.PlaceOrder(order);
                
                // Calculăm și afișăm costul total
                decimal totalCost = order.GetTotalCost();
                consoleService.WriteLine($"\nPretul total al comenzii: {totalCost} RON");
                
                // Afișăm detalii despre taxe și reduceri aplicate
                if (isDelivery)
                {
                    consoleService.WriteLine("(Include taxa de livrare: 10 RON)");
                }
                if (client.IsLoyalCustomer())
                {
                    consoleService.WriteLine("Client fidel! Ai primit reducere de 10%");
                }
                
                // Salvăm starea actualizată a pizzeriei
                SavePizzeriaState(pizzeria, new FileService());
                consoleService.WriteLine("\nComanda plasata cu succes!");
            }
            catch (Exception ex)
            {
                consoleService.WriteLine($"Eroare la plasarea comenzii: {ex.Message}");
            }
        }

        /// <summary>
        /// Metodă pentru adăugarea unei pizza standard din meniu în comandă
        /// </summary>
        static void AddStandardPizza(Pizzeria pizzeria, List<Pizza> pizzas, IConsoleService consoleService)
        {
            // Afișăm meniul disponibil
            ViewMenu(pizzeria, consoleService);
            
            // Selectăm pizza din meniu după nume
            consoleService.WriteLine("\nAlege numele pizzei din meniu:");
            string pizzaName = consoleService.ReadLine();
            var pizza = pizzeria.GetMenu().FirstOrDefault(p => p.GetName() == pizzaName);
            
            if (pizza != null)
            {
                // Creăm o copie nouă a pizzei pentru comandă
                // Acest lucru previne modificarea accidentală a pizzei din meniu
                var orderPizza = new Pizza(
                    pizza.GetName(), 
                    pizza.GetSize(), 
                    pizza.GetBasePrice(), 
                    pizza.GetIngredients().ToList()
                );
                pizzas.Add(orderPizza);
                consoleService.WriteLine($"Adaugat {pizza.GetName()} la comanda.");
            }
            else
            {
                consoleService.WriteLine("Pizza nu a fost gasita in meniu.");
            }
        }

        /// <summary>
        /// Metodă pentru crearea și adăugarea unei pizza personalizate în comandă
        /// Preț de bază: 30 RON + costul ingredientelor
        /// </summary>
        static void AddCustomPizza(Pizzeria pizzeria, List<Pizza> pizzas, IConsoleService consoleService)
        {
            consoleService.WriteLine("\nCreaza pizza personalizata:");
            
            // Pasul 1: Alegerea dimensiunii
            consoleService.WriteLine("Alege dimensiunea (Small/Medium/Large):");
            if (!Enum.TryParse<PizzaSize>(consoleService.ReadLine(), true, out PizzaSize size))
            {
                consoleService.WriteLine("Dimensiune invalida. Pizza nu a fost adaugata.");
                return;
            }

            // Pasul 2: Afișarea ingredientelor disponibile cu prețuri
            consoleService.WriteLine("\nIngrediente disponibile:");
            foreach (var ingredient in pizzeria.GetIngredients())
            {
                consoleService.WriteLine($"{ingredient.GetName()} - {ingredient.GetCost()} RON");
            }

            // Pasul 3: Selectarea ingredientelor
            List<Ingredient> selectedIngredients = new List<Ingredient>();
            while (true)
            {
                consoleService.WriteLine("\nIntrodu numele ingredientului (sau 'done' pentru a termina):");
                string ingredientName = consoleService.ReadLine();
                
                if (ingredientName?.ToLower() == "done")
                    break;

                // Căutăm și adăugăm ingredientul selectat
                var ingredient = pizzeria.GetIngredients().FirstOrDefault(i => i.GetName() == ingredientName);
                if (ingredient != null)
                {
                    selectedIngredients.Add(ingredient);
                    consoleService.WriteLine($"Adaugat {ingredient.GetName()} la pizza.");
                }
                else
                {
                    consoleService.WriteLine("Ingredient negasit. Te rog incearca din nou.");
                }
            }

            // Verificăm dacă au fost selectate ingrediente
            if (!selectedIngredients.Any())
            {
                consoleService.WriteLine("Trebuie sa alegi cel putin un ingredient.");
                return;
            }

            // Pasul 4: Crearea pizzei personalizate
            decimal basePrice = 30; // Preț de bază fix pentru pizza personalizată
            var customPizza = new Pizza("Pizza Personalizata", size, basePrice, selectedIngredients);
            pizzas.Add(customPizza);

            // Afișăm detaliile și defalcarea prețului
            decimal totalPrice = customPizza.GetPrice();
            decimal ingredientsCost = totalPrice - basePrice;
            consoleService.WriteLine($"\nPizza personalizata adaugata la comanda!");
            consoleService.WriteLine($"Pret total: {totalPrice} RON");
            consoleService.WriteLine($"  - Pret baza: {basePrice} RON");
            consoleService.WriteLine($"  - Cost ingrediente: {ingredientsCost} RON");
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
                consoleService.WriteLine("Introdu numele ingredientului (sau 'done' pentru a termina):");
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
            // Solicităm data pentru care dorim să vedem comenzile
            consoleService.WriteLine("\nIntroduceti data pentru care doriti sa vedeti comenzile (format: dd/MM/yyyy):");
            if (DateTime.TryParse(consoleService.ReadLine(), out DateTime date))
            {
                // Obținem toate comenzile din ziua respectivă
                var orders = pizzeria.GetOrdersByDate(date);
                if (!orders.Any())
                {
                    consoleService.WriteLine($"Nu exista comenzi pentru data {date.ToShortDateString()}");
                    return;
                }

                // Afișăm detaliile fiecărei comenzi
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

                // Afișăm sumarul zilei
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
            // Obținem lista de pizza ordonată după popularitate
            var popularPizzas = pizzeria.GetMostPopularPizzas();
            if (!popularPizzas.Any())
            {
                consoleService.WriteLine("\nNu exista inca comenzi pentru a genera statistici.");
                return;
            }

            // Afișăm clasamentul pizzelor
            consoleService.WriteLine("\nTop cele mai populare pizza:");
            int rank = 1;
            foreach (var pizza in popularPizzas)
            {
                // Calculăm numărul total de comenzi pentru această pizza
                var orderCount = pizzeria.GetOrders()
                    .SelectMany(o => o.GetPizzas())
                    .Count(p => p.GetName() == pizza.GetName());

                // Afișăm detaliile pentru fiecare pizza
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
            // Solicităm perioada pentru raport
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

            // Validăm perioada selectată
            if (startDate > endDate)
            {
                consoleService.WriteLine("Data de inceput nu poate fi mai mare decat data de sfarsit!");
                return;
            }

            // Calculăm statisticile pentru perioada selectată
            decimal totalRevenue = pizzeria.GetTotalRevenue(startDate, endDate);
            var ordersInPeriod = pizzeria.GetOrders()
                .Where(o => o.GetOrderDate().Date >= startDate.Date && o.GetOrderDate().Date <= endDate.Date)
                .ToList();

            // Afișăm raportul detaliat
            consoleService.WriteLine($"\nRaport venituri pentru perioada {startDate.ToShortDateString()} - {endDate.ToShortDateString()}:");
            consoleService.WriteLine($"Numar total comenzi: {ordersInPeriod.Count}");
            consoleService.WriteLine($"Venit total: {totalRevenue} RON");
            
            if (ordersInPeriod.Any())
            {
                // Calculăm valoarea medie per comandă
                decimal averageOrderValue = totalRevenue / ordersInPeriod.Count;
                consoleService.WriteLine($"Valoare medie comanda: {averageOrderValue:F2} RON");
                
                // Calculăm procentul comenzilor cu livrare
                var deliveryOrders = ordersInPeriod.Count(o => o.GetIsDelivery());
                double deliveryPercentage = (deliveryOrders * 100.0 / ordersInPeriod.Count);
                consoleService.WriteLine($"Comenzi cu livrare: {deliveryOrders} ({deliveryPercentage:F1}%)");
                
                var mostExpensiveOrder = ordersInPeriod.MaxBy(o => o.GetTotalCost());
                consoleService.WriteLine($"Cea mai mare comandă: {mostExpensiveOrder.GetTotalCost()} RON");
            }
        }
    }