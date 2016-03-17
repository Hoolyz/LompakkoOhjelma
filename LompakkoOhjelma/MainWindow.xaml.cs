//using System;
//using System.Windows;

//namespace LompakkoOhjelma
//{
//    /// <summary>
//    ///     Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindow : Window
//    {
//        //
//        // Olio
//        //
//        private Pelaaja _uusiPelaaja;

//        public MainWindow()
//        {
//            InitializeComponent();
//        }

//        //
//        // Luo uuden Pelaajan ja lompakon 
//        //

//        private void btnNappi_Click(object sender, RoutedEventArgs e)
//        {
//            var rnd = new Random();
//            int lompakkoNumero = rnd.Next(1, 20000);

//            string nimimerkki = txtNimi.Text;

//            //_uusiPelaaja = new Pelaaja(nimimerkki, new Lompakko(0, lompakkoNumero));

//            lblNimi.Content = _uusiPelaaja.Nimimerkki;

//            lblLompakko.Content = _uusiPelaaja.haeLompakonnumero().ToString();
//            lblPisteet.Content = _uusiPelaaja.Pisteet;
//            btnNappi.IsEnabled = false;
//            txtNimi.IsEnabled = false;
//        }

//        //
//        // Muokkaa lompakon saldoa
//        //

//        private void btnMuutaSaldo_Click(object sender, RoutedEventArgs e)
//        {
//            decimal temp;
//            if (decimal.TryParse(txtUusiSaldo.Text, out temp))
//                try
//                {
//                    decimal saldo = decimal.Parse(txtUusiSaldo.Text);
//                    lblSaldo.Content = _uusiPelaaja.Muuta(saldo);
//                }
//                catch (Exception saldoylitys)
//                {
//                    MessageBox.Show(saldoylitys.Message);
//                }
//        }
//    }
//}