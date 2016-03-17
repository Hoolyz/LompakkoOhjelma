using System;

namespace LompakkoOhjelma
{
    public class Lompakko
    {
        //
        // Attribuutit
        //

        private int lompakkonumero;
        private decimal saldo;

        public Lompakko(decimal saldo, int Arvottunumero)
        {
            this.saldo = Saldo;
            Lompakkonumero = Arvottunumero;
        }

        //
        // Propertyt 
        //
        // Antaa lompakolle numeron väliltä 0-20000 
        //ja tarkastaa ettei lompakon numero ole tätä suurempi tai pienempi
        //

        public int Lompakkonumero
        {
            get { return lompakkonumero; }
            set
            {
                if (value >= 0 && value <= 20000)
                {
                    lompakkonumero = value;
                }

                else
                {
                    throw new ArgumentException("Tilinumero ei ole 0-20000");
                }
            }
        }

        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }

        //
        // Lompakko kosntruktori
        //


        //
        // Muokkaa lompakon saldoa ja tarkastaa ettei lompakon saldo ole negatiivinen
        //

        public decimal laskutoimitus(decimal summa)
        {
            if (summa + saldo >= 0)
            {
                saldo = saldo + summa;
            }
            else
            {
                throw new ArgumentException(" Saldo: " + saldo + Environment.NewLine + " Annettu arvo: " + summa +
                                            Environment.NewLine + " Saldo ei saa mennä miinukselle");
            }
            return saldo;
        }


        public override string ToString()
        {
            return String.Format("{0}", Lompakkonumero);
        }
    }
}