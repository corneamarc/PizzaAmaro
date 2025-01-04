using System;
using System.Collections.Generic;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public abstract class Pizza
    {
        public string Nume { get; set; }
        public Dimensiune DimensiunePizza { get; set; }
        public decimal PretDeBaza { get; set; }
        public List<Ingredient> Ingrediente { get; set; }

        public Pizza(string nume, Dimensiune dimensiune, decimal pretDeBaza, List<Ingredient> ingrediente)
        {
            Nume = nume;
            DimensiunePizza = dimensiune;
            PretDeBaza = pretDeBaza;
            Ingrediente = new List<Ingredient>();
        }


        public abstract decimal CalculeazaPret();

        public override string ToString()
        {
            return $"{Nume} {DimensiunePizza} ingrediente:{string.Join(",", Ingrediente)}";
        }
    }
}