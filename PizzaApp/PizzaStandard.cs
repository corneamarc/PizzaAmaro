using System;
using System.Collections.Generic;
namespace PizzaApp;

public class PizzaStandard : Pizza
{
    public PizzaStandard(string nume, Dimensiune dimensiune,decimal pretDeBaza) : base(nume, dimensiune, pretDeBaza) { }

    public override decimal CalculeazaPret()
    {
        return base.Dimensiune switch
        {
            PizzaApp.Dimensiune.Mica => 10m,
            PizzaApp.Dimensiune.Medie => 15m,
            PizzaApp.Dimensiune.Mare => 20m,
            _ => 0m,
        };
    }
}