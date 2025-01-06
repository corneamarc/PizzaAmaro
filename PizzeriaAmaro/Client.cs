using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class Client
    {
        public string NumeClient { get; set; }
        public string NrTelefon { get; set; }
        public List<Comanda> IstoricComenzi { get; private set; }

        public Client(string numeClient, string nrTelefon)
        {
            NumeClient = numeClient;
            NrTelefon = nrTelefon;
            IstoricComenzi = new List<Comanda>();
        }


    }
}