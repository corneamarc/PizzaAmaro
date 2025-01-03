using System;
using System.Collections.Generic;

namespace PizzaApp.Mancare;

public class PizzaPersonalizata : Pizza
{
    public PizzaPersonalizata(string nume, Dimensiune dimensiune) : base(nume, dimensiune, 30m) { }

    public override decimal CalculeazaPret()
    {
        decimal costIngrediente = 0;
        foreach (var ingrediente in Ingrediente)
        {
            costIngrediente += ingrediente.Pret;
        }
        return 30m + costIngrediente;
    }
}