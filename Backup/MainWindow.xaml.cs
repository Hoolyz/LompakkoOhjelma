using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LompakkoOhjelma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //
        // Olio
        //
        Pelaaja UusiPelaaja;

        public MainWindow()
        {
            InitializeComponent();


        }

        //
        // Luo uuden Pelaajan ja lompakon 
        //

        private void btnNappi_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int LompakkoNumero = rnd.Next(1,20000);

            string nimimerkki = txtNimi.Text;

            UusiPelaaja = new Pelaaja(nimimerkki,new Lompakko(0,LompakkoNumero));

            lblNimi.Content = UusiPelaaja.Nimimerkki;

            lblLompakko.Content = UusiPelaaja.haeLompakonnumero().ToString();
            lblPisteet.Content = UusiPelaaja.Pisteet;
            btnNappi.IsEnabled = false;
            txtNimi.IsEnabled = false;


        }

        //
        // Muokkaa lompakon saldoa
        //

        private void btnMuutaSaldo_Click(object sender, RoutedEventArgs e)
        {
           
            decimal temp;
            if (decimal.TryParse(txtUusiSaldo.Text, out temp))
                try
                {
                    decimal saldo = decimal.Parse(txtUusiSaldo.Text);
                    lblSaldo.Content = UusiPelaaja.Muuta(saldo);

                }
                catch (Exception saldoylitys)
                {

                    MessageBox.Show(saldoylitys.Message);
                }
        }


    }
}
