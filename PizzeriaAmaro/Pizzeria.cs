using System;
using System.Collections.Generic;
using System.Linq;


namespace PizzeriaAmaro
{
    public class Pizzeria
    {
        public string Nume { get; set; }
        public string Adresa { get; set; }
        public List<Pizza> Meniu { get; set; }
        public List<Comanda> Comenzi { get; set; }

        public Pizzeria(string nume, string adresa)
        {
            Nume = nume;
            Adresa = adresa;
            Meniu = new List<Pizza>();
            Comenzi = new List<Comanda>();
        }
        
        public void AdaugaComanda(Comanda comanda)
        {
            Comenzi.Add(comanda);
            Console.WriteLine("Comanda a fost plasată cu succes.");
        }
        

        private void SalveazaComanda(Comanda comanda)
        {
            string filePath = "orders.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(comanda.ToString());
            }
        }
        
    
    }
}

