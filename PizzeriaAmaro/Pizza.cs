using System;
using System.Collections.Generic;
using System.Linq;


namespace PizzeriaAmaro
{
    public enum DimensiunePizza{Mica, Medie, Mare}
    
    public abstract class Pizza
    {
        public string Nume{get; set;}
        public string Dimensiune {get; set;}
        
        public decimal Pret {get; set;}
        public List<Ingrediente> Ingredient { get; set; }

        public Pizza(string nume, string dimensiune, List<Ingrediente> ingredient)
        {
            Nume = nume;
            Dimensiune = dimensiune;
            Ingredient = ingredient;
        }

        public abstract decimal CalculeazaPret();

        public override string ToString()
        {
            return $"{Nume} {Dimensiune} ingrediente:{string.Join(",", Ingredient)}";
        }
    }
    
}

