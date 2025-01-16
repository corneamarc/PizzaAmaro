using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace PizzaApp.Mancare; 

public class Client
    {
        [JsonIgnore]
        public List<Order> OrderHistory { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public Client()
        {
            OrderHistory = new List<Order>();
        }

        public Client(string name, string phoneNumber) : this()
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public string GetName() => Name;
        public string GetPhoneNumber() => PhoneNumber;
        public List<Order> GetOrderHistory() => OrderHistory ?? (OrderHistory = new List<Order>());
        public void AddOrder(Order order)
        {
            if (OrderHistory == null)
                OrderHistory = new List<Order>();
            OrderHistory.Add(order);
        }
        public bool IsLoyalCustomer() => GetOrderHistory().Count >= 5;
    }
