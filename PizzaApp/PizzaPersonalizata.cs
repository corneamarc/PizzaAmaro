using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace PizzaAmaro;

public class PizzaPersonalizata : Pizza
{
    private const decimal CostPersonalizare = 30m;

    public PizzaPersonalizata(string nume, DimensiunePizza dimensiune, List<string> ingrediente)
        : base(nume, dimensiune, 0, ingrediente) { }

    public override decimal CalculeazaPret()
    {
        decimal pretIngredient = Ingrediente
            .Sum(ingredient => Program.Ingrediente.FirstOrDefault(i => i.Nume == ingredient)?.Pret ?? 0);
        return pretIngredient + CostPersonalizare;
    }

    public override string ToString()
    {
        return $"{Nume} (Personalizata, {Dimensiune}): {CalculeazaPret()} RON, Ingrediente: {string.Join(", ", Ingrediente)}";
    }
}

// Enumerare pentru dimensiuni pizza
public enum DimensiunePizza { Mica, Medie, Mare }