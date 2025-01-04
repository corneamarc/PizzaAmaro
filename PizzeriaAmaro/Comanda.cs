using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class Comanda
    {
        public string NumeClient{get;set;}
        public List<Pizza> ListaPizza{get; private set;}
        public TipComanda Livrare { get; set; }
        public decimal CostTotal { get; set; }
        
        private Dictionary<string, decimal> Meniu;
        
        private const decimal CostLivrare = 10m;


        public Comanda(string numeClient, List<Pizza> listaPizza, TipComanda livrare, decimal costTotal, Dictionary<string, decimal> meniu)
        {
            NumeClient = numeClient;
            ListaPizza = listaPizza;
            Livrare = livrare;
            CostTotal = costTotal;
            Meniu = new Dictionary<string, decimal>();
        }

        public bool ValideazaComanda(List<Pizza> meniu)
        {
            foreach (var pizza in ListaPizza)
            {
                bool pizzaExistenta = meniu.Exists(p => p.Nume == pizza.Nume && p.DimensiunePizza == pizza.DimensiunePizza);
                if (!pizzaExistenta)
                {
                    Console.WriteLine($"Pizza {pizza.Nume} de dimensiune {pizza.DimensiunePizza} nu există în meniu.");
                    return false;
                }
            }
            return true; 
        }
        
        private bool ValideazaComanda()
        {
            foreach (var pizza in ListaPizza)
            {
                if (!Meniu.ContainsKey(pizza))
                {
                    return false;
                }
            }
            return true;

        }

        private void CalculeazaPretTotal()
        {
            foreach (var pizza in ListaPizza)
            {
                CostTotal += Meniu[pizza];
            }

            if (Livrare == TipComanda.LivrareDomiciliu)
            {
                CostTotal += CostLivrare;
            }
        }
        
        public override string ToString()
        {
            return $"Client: {NumeClient}" +
                   $"Lista Pizza: {string.Join(", ", ListaPizza)}" +
                   $"Metoda Livrare: {Livrare}" +
                   $"Cost Total: {CostTotal} RON";
        }
        
      
    }
}