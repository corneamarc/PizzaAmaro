using System;
using System.Collections.Generic;
using System.Linq;




namespace PizzaApp;
public enum Dimensiune{Mica,Medie,Mare}

public abstract class Pizza
{
    private string _nume;
    private Dimensiune _dimensiune;
    private decimal _pretDeBaza;
    private List<Ingrediente> _ingredientes;
    
    public Pizza(string nume, Dimensiune dimensiune, decimal pretDeBaza)
    {
        _nume = nume;
        _dimensiune = dimensiune;
        _pretDeBaza = pretDeBaza;
        _ingredientes =new List<Ingrediente>();
        
    }
    
    
    public string  Nume()=>_nume;
    public   Dimensiune Dimensiune() => _dimensiune;
    public abstract decimal CalculeazaPret();
    public List<Ingrediente> Ingredientes() => _ingredientes;
    


    
    
    //Adauga ingrediente la pizza
    public void AdaugaIngrediente(Ingrediente ingrediente)
    {
        _ingredientes.Add(ingrediente);
    }
    //Afisare
    public override string ToString()
    {
        

        return $"{_nume} ({_dimensiune}): Ingrediente: {Ingredientes()} - {CalculeazaPret()} RON";
    }


    


}