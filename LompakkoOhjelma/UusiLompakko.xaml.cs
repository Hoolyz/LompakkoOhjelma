using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace LompakkoOhjelma
{
    /// <summary>
    ///     Interaction logic for UusiLompakko.xaml
    /// </summary>
    public partial class UusiLompakko : Window
    {
        private SqlConnection _yhteys2;

        public UusiLompakko()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _yhteys2 =
                new SqlConnection(
                    "Server=tcp:ubr6dhaftm.database.windows.net,1433;Database=LompakkoDB;User Id=LompakkoAdmin@ubr6dhaftm;Password=Lompakko*;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=True;");
            try
            {
                if (_yhteys2.State == ConnectionState.Closed)
                _yhteys2.Open();
            }
            catch (Exception yhteysVirhe)
            {
                MessageBox.Show(yhteysVirhe.Message);
                return;
            }
            _yhteys2.Close();
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Ikkunat.UusiLompakko = this;
            Ikkunat.Paluu = new Valinta();
        }

        private void btnLuo_Click(object sender, RoutedEventArgs e)
        {

            if (txtSaldo.Text == "Saldo" || txtSaldo.Text == "")
            {
                MessageBox.Show("Anna jokin saldo");
                return;

            }

            decimal saldo = decimal.Parse(txtSaldo.Text);

            if (saldo == 0)
            {
                MessageBoxResult result = MessageBox.Show("Haluatko luoda uuden lompakon 0 saldolla",
                    "Nolla arvonen saldo",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    saldo = saldo;
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }


            try
            {
                _yhteys2.Open();
            }
            catch (Exception yhteysVika)
            {
                MessageBox.Show(yhteysVika.Message);
                return;
            }



            int LompakkoNumero = 0;
            string pelaaja = Ikkunat.ValittuPelaaja;

            var lompakkoHaku = new SqlCommand();

            string valittuNimi = pelaaja;

            string selectKomento =
                "SELECT nimi, lompakkotunnus, saldo  FROM Lompakko WHERE nimi=" + "'" + valittuNimi + "'";

            lompakkoHaku.CommandText = selectKomento;
            lompakkoHaku.Connection = _yhteys2;

            SqlDataReader reader = lompakkoHaku.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    LompakkoNumero = reader.GetInt16(1);
                    LompakkoNumero++;
                }
            }
            else
            {
                LompakkoNumero = 1;
            }
            reader.Close();
 


            string insertKomento =
                "insert into Lompakko (nimi,saldo,lompakkotunnus) values(@pelaaja,@saldo,@lompakkoNumero)";

            SqlCommand lisääLompakko = new SqlCommand(insertKomento, _yhteys2);

            SqlParameter saldoParametri = new SqlParameter("saldo", SqlDbType.Decimal);
            SqlParameter pelaajaParametri = new SqlParameter("pelaaja",SqlDbType.Char,50);
            SqlParameter lompakkoParametri = new SqlParameter("lompakkonumero",SqlDbType.Int);


            saldoParametri.Value = saldo;
            pelaajaParametri.Value = pelaaja;
            lompakkoParametri.Value = LompakkoNumero;

            lisääLompakko.Parameters.Add(saldoParametri);
            lisääLompakko.Parameters.Add(pelaajaParametri);
            lisääLompakko.Parameters.Add(lompakkoParametri);

            try
            {
                lisääLompakko.ExecuteNonQuery();
            }
            catch (Exception Vika)
            {

                MessageBox.Show(Vika.Message);
            }

             _yhteys2.Close();
            MessageBox.Show("Luotiin uusi Lompakko Pelaajalle " + pelaaja + " Saldoksi annettiin " + saldo + " ");


            int i = 0;
            txtSaldo.Text = i.ToString();
            Hide();
         
            
        }


        private void btnPaluu_Click(object sender, RoutedEventArgs e)
        {
            _yhteys2.Close();
            Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            lblNimi.Content = Ikkunat.ValittuPelaaja;
        }

        private void txtSaldo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSaldo.Text = String.Empty;
        }

        private void txtSaldo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnkoNumero(e);
        }

        public void OnkoNumero(TextCompositionEventArgs e)
        {
            decimal result;

            if (!(decimal.TryParse(e.Text, out result) || e.Text == ","))
            {
                e.Handled = true;
            }
        }
    }
}