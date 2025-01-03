using System;
using System.Collections.Generic;

namespace PizzaApp.Mancare;

public class Ingredient
{
    private string _nume;
    private decimal _pret;

    public Ingredient(string nume, decimal pret)
    {
        _nume = nume;
        _pret = pret;
    }
    
    public string Nume => _nume;
    public decimal Pret => _pret;
}