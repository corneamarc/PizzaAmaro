using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PizzaApp.Mancare;

namespace PizzaApp;
/// <summary>
    /// Serviciu pentru autentificare și gestionarea utilizatorilor
    /// Gestionează toate operațiile legate de utilizatori:
    /// - Înregistrare
    /// - Autentificare
    /// - Salvare/Încărcare utilizatori din fișier
    /// </summary>
    public class AuthenticationService
    {
        // Lista utilizatorilor înregistrați
        private List<User> _users;

        /// <summary>
        /// Constructor - inițializează lista de utilizatori
        /// </summary>
        public AuthenticationService()
        {
            _users = new List<User>();
        }

        /// <summary>
        /// Înregistrează un utilizator nou în sistem
        /// Validează datele și verifică duplicatele
        /// </summary>
        /// <param name="username">Numele de utilizator</param>
        /// <param name="password">Parola utilizatorului</param>
        /// <param name="isAdmin">Indicator pentru drepturi de administrator</param>
        /// <param name="phoneNumber">Număr de telefon (obligatoriu pentru non-admin)</param>
        public void Register(string username, string password, bool isAdmin, string phoneNumber = null)
        {
            // Validări
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username and password are required.");
                return;
            }

            // Verifică dacă username-ul există deja
            if (_users.Any(u => u.GetUsername() == username))
            {
                Console.WriteLine("Username already exists.");
                return;
            }

            // Verifică numărul de telefon pentru utilizatorii non-admin
            if (!isAdmin && string.IsNullOrEmpty(phoneNumber))
            {
                Console.WriteLine("Phone number is required for non-admin users.");
                return;
            }

            // Creează și adaugă utilizatorul nou
            _users.Add(new User(username, password, isAdmin, phoneNumber));
            Console.WriteLine("User registered successfully.");
        }

        /// <summary>
        /// Autentifică un utilizator în sistem
        /// Verifică credențialele și returnează obiectul User dacă sunt valide
        /// </summary>
        public User Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => 
                u.GetUsername() == username && 
                u.GetPassword() == password);

            if (user == null)
            {
                Console.WriteLine("Invalid username or password.");
                return null;
            }

            Console.WriteLine("Login successful.");
            return user;
        }

        /// <summary>
        /// Salvează lista de utilizatori în format JSON
        /// Folosește ReferenceHandler.Preserve pentru a gestiona referințele circulare
        /// </summary>
        public void SaveUsers(string filePath, IFileService fileService)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string jsonString = JsonSerializer.Serialize(_users, options);
                fileService.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        /// <summary>
        /// Încarcă lista de utilizatori din fișierul JSON
        /// Creează o listă nouă dacă fișierul nu există
        /// </summary>
        public void LoadUsers(string filePath, IFileService fileService)
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
                    _users = JsonSerializer.Deserialize<List<User>>(jsonString, options);
                }
                else
                {
                    Console.WriteLine("Users file not found. Starting with an empty user list.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                _users = new List<User>();
            }
        }
    }