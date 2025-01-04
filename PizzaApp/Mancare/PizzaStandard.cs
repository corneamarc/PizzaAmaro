using System;
using System.Collections.Generic;
namespace PizzaApp.Mancare;

public class PizzaStandard : Pizza
{
    public PizzaStandard(string nume, Dimensiune dimensiune,decimal pretDeBaza) : base(nume, dimensiune, pretDeBaza) { }

    public override decimal CalculeazaPret()
    {
        return base.Dimensiune switch
        {
            Dimensiune.Mica => 10m,
            Dimensiune.Medie => 15m,
            Dimensiune.Mare => 20m,
            _ => 0m,
        };
    }
}