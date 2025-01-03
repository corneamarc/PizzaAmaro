using System;
using System.Collections.Generic;

namespace PizzaApp.Mancare;
public enum Dimensiune { Mica, Medie, Mare }

public abstract class Pizza
{
    private string _nume;
    private decimal _pretDeBaza;
    //public enum Dimensiune { Mica, Medie, Mare }
    private Dimensiune _dimensiune;
    private List<Ingredient> _ingrediente;

    public Pizza(string nume, Dimensiune dimensiune, decimal pretDeBaza)
    {
        _nume = nume;
        _dimensiune = dimensiune;
        _pretDeBaza = pretDeBaza;
        _ingrediente = new List<Ingredient>();
    }
    
    public string Nume => _nume;
    public Dimensiune Dimensiune => _dimensiune;
    public List<Ingredient> Ingrediente => _ingrediente;
    public abstract decimal CalculeazaPret();

    public void AdaugaIngredient(Ingredient ingredient)
    {
        _ingrediente.Add(ingredient);
    }
    
    //Afisare
    public override string ToString()
    {
        string ingredienteStr = _ingrediente.Count > 0
            ? string.Join(",", _ingrediente.ConvertAll(i => i.Nume))
            : "Fara ingrediente";

        return $"{_nume} ({_dimensiune}): Ingrediente: {ingredienteStr} - {CalculeazaPret()} RON";
    }

}