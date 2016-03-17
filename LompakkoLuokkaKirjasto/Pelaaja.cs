using System;

namespace LompakkoOhjelma
{
    public class Pelaaja
    {
        //
        // Attribuutit
        //
        //private readonly Lompakko rahaa;
        private string nimimerkki;
        private int lompakkotunnus;
        private decimal saldo;
        private int pisteet;

        public Pelaaja(string Nimi, int Lompakko,decimal LompakonSaldo,int PelaajaPisteet)
        {
            saldo = LompakonSaldo;
            nimimerkki = Nimi;
            pisteet = PelaajaPisteet;
            Lompakkotunnus = Lompakko;
        }

        //
        // Propertyt 
        //

        public string Nimimerkki
        {
            get { return nimimerkki; }
            set { nimimerkki = value; }
        }

        public int Pisteet { get; set; }

        public int Lompakkotunnus
        {
            get { return lompakkotunnus; }
            set { lompakkotunnus = value; }
        }

        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }

        #region Oldstuff
        //public int haeLompakonnumero()
        //{
        //    return rahaa.Lompakkonumero;
        //}

        //
        // Muokkaa lompakon saldoa
        //

        //public decimal Muuta(decimal siirrä)
        //{
        //    decimal uusiSaldo;
        //    try
        //    {
        //        uusiSaldo = rahaa.laskutoimitus(siirrä);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return uusiSaldo;
        //}
        #endregion

        public override string ToString()
        {
            return  String.Format(
                    "Uusi Peli aloitettu " + Environment.NewLine + " Pelaaja: {0} " + Environment.NewLine +
                    " Pisteet: {1} " + Environment.NewLine + " Lompakon Numero: {2} " + Environment.NewLine +
                    " Saldo:  {3}", nimimerkki, pisteet, lompakkotunnus, saldo);
        }
    }
}