using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class PizzaPersonalizata : Pizza
    { 
        private const decimal CostPersonalizat = 30m;

        public PizzaPersonalizata(string nume, Dimensiune dimensiune, List<Ingredient> ingrediente)
            : base(nume, dimensiune, 0, ingrediente) { }

        public override decimal CalculeazaPret()
        {
            decimal costTotal = CostPersonalizat;
            foreach (var ingredient in Ingrediente)
            {
                costTotal += ingredient.Cost;
            }
            return costTotal;
        }

        public void AdaugaIngredient(Ingredient ingredient)
        {
            Ingrediente.Add(ingredient);
        }
    }
}