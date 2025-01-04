using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class PizzaStandard : Pizza
    {
        public PizzaStandard(string nume, Dimensiune dimensiune, decimal pretDeBaza, List<Ingredient> ingrediente)
            : base(nume, dimensiune, pretDeBaza, ingrediente) { }

        public override decimal CalculeazaPret()
        {
            return PretDeBaza;
        }
    }
}