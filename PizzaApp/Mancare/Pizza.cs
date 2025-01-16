using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace PizzaApp.Mancare; 

public enum PizzaSize { Small, Medium, Large }

public class Pizza
{
    public string Name { get; set; }
    public PizzaSize Size { get; set; }
    public decimal BasePrice { get; set; }
    public List<Ingredient> Ingredients { get; set; }

    public Pizza() 
    {
        Ingredients = new List<Ingredient>();
    }

    public Pizza(string name, PizzaSize size, decimal basePrice, List<Ingredient> ingredients)
    {
        Name = name;
        Size = size;
        BasePrice = basePrice;
        Ingredients = ingredients ?? new List<Ingredient>();
    }

    public string GetName() => Name;
    public PizzaSize GetSize() => Size;
    public decimal GetBasePrice() => BasePrice;
    public List<Ingredient> GetIngredients() => Ingredients;
    public decimal GetPrice() => BasePrice + Ingredients.Sum(i => i.Cost);
    public void SetSize(PizzaSize size) => Size = size;
    public void SetBasePrice(decimal basePrice) => BasePrice = basePrice;
    public void SetIngredients(List<Ingredient> ingredients) => Ingredients = ingredients;
}