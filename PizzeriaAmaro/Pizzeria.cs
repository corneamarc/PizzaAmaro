using System;
using System.Collections.Generic;
using System.Linq;


namespace PizzeriaAmaro
{
    public class Pizzeria
    {
        public string Nume { get; set; }
        public string Adresa { get; set; }
        public List<Pizza> Meniu { get; private set; }
        public List<Comanda> Comenzi { get; private set; }
        public List<Client> Clienti { get; private set; }

        public Pizzeria(string nume, string adresa)
        {
            Nume = nume;
            Adresa = adresa;
            Meniu = new List<Pizza>();
            Comenzi = new List<Comanda>();
            Clienti = new List<Client>();
        }

        public void AddPizza(Pizza pizza)
        {
            Meniu.Add(pizza);
        }

        public void AddClient(Client client)
        {
            Clienti.Add(client);
            Console.WriteLine();
        }

        public void AddOrder(Comanda comanda)
        {
            Comenzi.Add(comanda);
            Console.WriteLine($"pizza {Pizza.Nume} a fost adaugata in meniu");
        }

        public void AfisareMeniu()
        {
            Console.WriteLine("Meniu:");
            foreach (var pizza in Meniu)
            {
                Console.WriteLine(pizza);
          
            }
        }

        public void AfisareComanda()
        {
            Console.WriteLine("Comenzi inregisrate:");
            foreach (var comanda in Comenzi)
            {
                Console.WriteLine(comanda);
            }
        }

    }

