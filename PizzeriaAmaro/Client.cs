using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaAmaro
{
    public class Client
    {
        public string Nume { get; set; }
        public string NrTelefon { get; private set; }
        public List<Comanda> IstoricComenzi { get; set; }

        public Client(string nume, string nrTelefon, List<Comanda> istoricComenzi)
        {
            Nume = nume;
            if (validareNrTelefon(nrTelefon))
            {
                NrTelefon = nrTelefon;
            }
            else
            {
                throw new ArgumentException("Numărul de telefon nu este valid.");
            }
           
            
            IstoricComenzi = istoricComenzi;
        }

        public bool validareNrTelefon(string nrTelefon)
        {
            if ((nrTelefon.StartsWith("+40") && nrTelefon.Length == 12) ||
                (nrTelefon.StartsWith("0") && nrTelefon.Length == 10))
            {
                for (int i = nrTelefon.StartsWith("+40") ? 3 : 1; i < nrTelefon.Length; i++)
                {
                    if (!char.IsDigit(nrTelefon[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        
        public bool EsteFidel()
        {
            return IstoricComenzi.Count >= 5;
        }

        public decimal CalculeazaReducere(decimal sumaComanda)
        {
            if (EsteFidel())
            {
                return sumaComanda * 0.9m; 
            }
            return sumaComanda;
        }
    }
}