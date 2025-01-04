using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class Ingredient
    {
        public string Nume { get; set; }
        public decimal Cost { get; set; }

        public Ingredient(string nume, decimal cost)
        {
            Nume = nume;
            Cost = cost;
        }
    }
}