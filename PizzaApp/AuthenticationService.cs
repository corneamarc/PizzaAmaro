using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PizzaApp.Mancare;

namespace PizzaApp;
public class AuthenticationService
    {
        private List<User> _users;

        public AuthenticationService()
        {
            _users = new List<User>();
        }

        public void Register(string username, string password, bool isAdmin, string phoneNumber = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username and password are required.");
                return;
            }

            if (_users.Any(u => u.GetUsername() == username))
            {
                Console.WriteLine("Username already exists.");
                return;
            }

            if (!isAdmin && string.IsNullOrEmpty(phoneNumber))
            {
                Console.WriteLine("Phone number is required for non-admin users.");
                return;
            }

            _users.Add(new User(username, password, isAdmin, phoneNumber));
            Console.WriteLine("User registered successfully.");
        }

        public User Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.GetUsername() == username && u.GetPassword() == password);
            if (user == null)
            {
                Console.WriteLine("Invalid username or password.");
                return null;
            }

            Console.WriteLine("Login successful.");
            return user;
        }

        public void SaveUsers(string filePath, IFileService fileService)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };
                string jsonString = JsonSerializer.Serialize(_users, options);
                fileService.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        public void LoadUsers(string filePath, IFileService fileService)
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