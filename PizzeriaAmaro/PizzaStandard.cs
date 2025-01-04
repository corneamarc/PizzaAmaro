using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class StandardPizza : Pizza
    {
        private decimal PretFix { get; set; }

        public StandardPizza(string nume, string dimensiune, decimal pretfix, List<Ingrediente> ingredient) : base(nume,
            dimensiune, ingredient)
        {
            PretFix = pretfix;
        }

        public decimal CalculeazaPretBaza()
        {
            return PretFix;
        }

        public override string ToString()
        {
            return $"PretBaza: {PretFix} RON";
        }
    }
            
    
}
