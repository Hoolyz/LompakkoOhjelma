using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LompakkoOhjelma
{
    class Pelaaja
    {

        //
        // Attribuutit
        //
        private string nimimerkki;
        private int pisteet;

        //
        // Propertyt 
        //

        public string Nimimerkki
        {
            get { return nimimerkki; }
            set { nimimerkki = value; }
        }

        public int Pisteet
        {
            get { return pisteet; }
            set { pisteet = value; }
        }
        
        private Lompakko rahaa;



        //  
        // Pelaaja luokka konstruktori
        //

        public Pelaaja(string Nimimerkki,Lompakko lompakko)
        {
      
            rahaa = lompakko;
            nimimerkki = Nimimerkki;
            pisteet = 10;


        }

        public int haeLompakonnumero()
        {
            return rahaa.Lompakkonumero;
        }

        //
        // Muokkaa lompakon saldoa
        //

        public decimal Muuta (decimal siirrä)
        {
            decimal uusiSaldo;
            try
            {
            uusiSaldo = rahaa.laskutoimitus(siirrä);

            }
            catch (Exception)
            {
                
                throw;
            }

            return uusiSaldo;

        }

        

        public override string ToString()
        {
            return String.Format("{0} {1}", nimimerkki, rahaa);
        }
    }



}
