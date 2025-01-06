using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class PersonalizataPizza:Pizza
    {
        private const decimal PretPersonalizat = 30;

        public PersonalizataPizza(string name, string dimensiune, List<Ingrediente> ingredient): base(name, dimensiune, ingredient)
        {
        
        }

        public decimal CalculeazaPretPersonalizat()
        {
            decimal ingredientePret = 0;
            foreach (var ingrediente in Ingredient)
            {
                ingredientePret += ingrediente.Pret;
            }
            return ingredientePret + PretPersonalizat;
        }

        public override string ToString()
        {
            return base.ToString() + $", PretPersonalizat: {CalculeazaPretPersonalizat()}";
        }
    }
}