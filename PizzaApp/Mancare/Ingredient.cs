using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace PizzaApp.Mancare; 
public class Ingredient
{
    public string Name { get; set; }
    public decimal Cost { get; set; }

    public Ingredient() { }

    public Ingredient(string name, decimal cost)
    {
        Name = name;
        Cost = cost;
    }

    public string GetName() => Name;
    public decimal GetCost() => Cost;
    public void SetCost(decimal cost) => Cost = cost;
}