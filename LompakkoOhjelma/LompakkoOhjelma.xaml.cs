using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LompakkoOhjelma
{
    /// <summary>
    ///     Interaction logic for Valinta.xaml
    /// </summary>
    public partial class Valinta
    {
        private Pelaaja AloitaPeli;
        private Lompakko Muutos;
        private SqlConnection _yhteys;
        private SqlConnection _yhteys2;

        public Valinta()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region WindowActived does this better

            //_yhteys =
            //    new SqlConnection(
            //        "Server=tcp:ubr6dhaftm.database.windows.net,1433;Database=LompakkoDB;User Id=LompakkoAdmin@ubr6dhaftm;Password=Lompakko*;Trusted_Connection=False;Encrypt=True;");
            //try
            //{
            //    _yhteys.Open();
            //}
            //catch (Exception yhteysVirhe)
            //{
            //    MessageBox.Show(yhteysVirhe.Message);
            //    return;
            //}

            //var pelaajaHaku = new SqlCommand();
            //pelaajaHaku.CommandText =
            //    "SELECT nimi,pisteet FROM Pelaaja ";
            //pelaajaHaku.Connection = _yhteys;
            //SqlDataReader reader = pelaajaHaku.ExecuteReader();

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        var nimi = (string)reader.GetSqlString(0);
            //        string tiedot = nimi.Trim();
            //        lstPelaajat.Items.Add(tiedot);
            //    }
            //}
            //reader.Close();
            //_yhteys.Close();
            //lstPelaajat.SelectedIndex = 0;

            #endregion
        }


        public void lstPelaajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstLompakot.Items.Clear();
            if (lstPelaajat.Items.Count == 0)
            {
                return;
            }
            if (lstPelaajat.SelectedItem == null)
                return;

            try
            {
                _yhteys.Open();
            }
            catch (Exception yhteysVika)
            {
                MessageBox.Show(yhteysVika.Message);
                return;
            }


            var lompakkoHaku = new SqlCommand();
            var pelaajaPisteet = new SqlCommand();

            string valittuNimi = lstPelaajat.SelectedValue.ToString();

            string selectKomento =
                "SELECT nimi, lompakkotunnus, saldo  FROM Lompakko WHERE nimi=" + "'" + valittuNimi + "'";

            lompakkoHaku.CommandText = selectKomento;
            lompakkoHaku.Connection = _yhteys;

            SqlDataReader reader = lompakkoHaku.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    short lompakot = reader.GetInt16(1);

                    lstLompakot.Items.Add(lompakot);

                    lblNimi.Content = reader[0];
                    lblLompakko.Content = reader.GetInt16(1);
                    lblSaldo.Content = reader.GetDecimal(2);
                }
            }
            else
            {
                lstLompakot.Items.Add("Ei Lompakoita");
            }


            reader.Close();

            string selectPisteet = "SELECT nimi, pisteet FROM Pelaaja WHERE nimi=" + "'" + valittuNimi + "'";

            pelaajaPisteet.CommandText = selectPisteet;
            pelaajaPisteet.Connection = _yhteys;

            SqlDataReader pisteetReader = pelaajaPisteet.ExecuteReader();

            if (pisteetReader.HasRows)
            {
                while (pisteetReader.Read())
                {
                    lblPisteet.Content = pisteetReader.GetInt16(1);
                }
            }

            pisteetReader.Close();
            _yhteys.Close();
            lstLompakot.SelectedItem = 1;
        }


        private void lstLompakot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstLompakot.Items.Count == 0)
            {
                return;
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

            string valittuNimi = lstPelaajat.SelectedValue.ToString();
            string valittuLompakko = lstLompakot.SelectedValue.ToString();

            var lompakkoHaku = new SqlCommand();

            string selectKomento =
                "SELECT lompakkotunnus, saldo  FROM Lompakko WHERE lompakkotunnus=" + "'" + valittuLompakko + "'" +
                " AND nimi=" + "'" + valittuNimi + "'";

            lompakkoHaku.CommandText = selectKomento;
            lompakkoHaku.Connection = _yhteys2;

            SqlDataReader reader = lompakkoHaku.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lblLompakko.Content = reader.GetInt16(0);
                    lblSaldo.Content = reader.GetDecimal(1);
                }
            }
            else
            {
                //lstLompakot.Items.Add("Ei Lompakoita");
                btnAloitaPeli.IsEnabled = false;
                _yhteys2.Close();
            }

            if (lstLompakot.SelectedValue.ToString() == "Ei Lompakoita")
            {
                MessageBoxResult result =
                    MessageBox.Show("Pelaajalla " + valittuNimi + " ei ole lompakoita haluatko luoda uuden?",
                        "Ei lompakoita",
                        MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Ikkunat.ValittuPelaaja = valittuNimi;
                    _yhteys2.Close();
                    Ikkunat.UusiLompakko.Show();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }


            btnAloitaPeli.IsEnabled = true;

            reader.Close();
            _yhteys2.Close();
        }


        private void btnUusi_Click(object sender, RoutedEventArgs e)
        {
            _yhteys.Close();
            Ikkunat.Uusi.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Ikkunat.Paluu = this;
            Ikkunat.Uusi = new UusiPelaaja();
            Ikkunat.UusiLompakko = new UusiLompakko();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMuuta_Click(object sender, RoutedEventArgs e)
        {
            if (lstLompakot.SelectedItem == null)
            {
                MessageBox.Show("Valitse lompakko");
                return;
            }

            if (txtMuuta.Text == "Muuta Saldoa" || txtMuuta.Text == "")
            {
                MessageBox.Show("Anna ensin saldo");
                return;
            }

            try
            {
                _yhteys.Open();
            }
            catch (Exception yhteysVika)
            {
                MessageBox.Show(yhteysVika.Message);
                return;
            }

            string valittuNimi = lstPelaajat.SelectedValue.ToString();
            string valittuLompakko = lstLompakot.SelectedValue.ToString();


            decimal saldo = decimal.Parse(lblSaldo.Content.ToString());
            decimal muutos = decimal.Parse(txtMuuta.Text);

            if (saldo + muutos >= 0)
            {
                saldo = saldo + muutos;

                string updateKomento =
                    "update Lompakko SET saldo= @saldo Where nimi = @pelaaja AND lompakkotunnus= @lompakkoNumero";


                var päivitäSaldo = new SqlCommand(updateKomento, _yhteys);

                var saldoParametri = new SqlParameter("saldo", SqlDbType.Decimal);
                var pelaajaParametri = new SqlParameter("pelaaja", SqlDbType.Char, 50);
                var lompakkoParametri = new SqlParameter("lompakkonumero", SqlDbType.Int);

                saldoParametri.Value = saldo;
                pelaajaParametri.Value = valittuNimi;
                lompakkoParametri.Value = valittuLompakko;

                päivitäSaldo.Parameters.Add(saldoParametri);
                päivitäSaldo.Parameters.Add(pelaajaParametri);
                päivitäSaldo.Parameters.Add(lompakkoParametri);

                päivitäSaldo.ExecuteNonQuery();

                lblSaldo.Content = saldo;
                _yhteys.Close();

                int i = 0;
                txtMuuta.Text = i.ToString();
            }

            else
            {
                decimal summa = saldo + muutos;
                MessageBox.Show(" Saldo: " + saldo + Environment.NewLine + " Uusi saldo: " + summa +
                                Environment.NewLine + " Saldo ei saa mennä miinukselle");
            }
        }

        private void txtMuuta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            OnkoNumero(e);
        }

        public void OnkoNumero(TextCompositionEventArgs e)
        {
            decimal result;

            if (!(decimal.TryParse(e.Text, out result) || e.Text == "," || e.Text == "-"))
            {
                e.Handled = true;
            }
        }

        private void txtEtsi_TextChanged(object sender, TextChangedEventArgs e)
        {
            HaePelaajat();
        }

        private void HaePelaajat()
        {
            lstPelaajat.Items.Clear();

            try
            {
                _yhteys.Open();
            }
            catch (Exception yhteydenOttoVika)
            {
                MessageBox.Show(yhteydenOttoVika.Message);
                return;
            }

            var hakuPelaajat = new SqlCommand();
            string alku = txtEtsi.Text;

            if (String.IsNullOrEmpty(alku))
            {
                hakuPelaajat.CommandText = "SELECT nimi FROM Pelaaja ORDER BY nimi";
            }
            else
            {
                alku = alku.First().ToString().ToUpper() + alku.Substring(1);


                hakuPelaajat.CommandText = "SELECT nimi FROM Pelaaja " +
                                           "WHERE nimi LIKE '" + alku + "%' ORDER BY nimi  ";
            }
            hakuPelaajat.Connection = _yhteys;

            SqlDataReader reader = hakuPelaajat.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var nimi = (string) reader.GetSqlString(0);

                    string tiedot = nimi.Trim();

                    lstPelaajat.Items.Add(tiedot);
                }
            }
            else
            {
                lstPelaajat.Items.Add("Ei Pelaajia");
            }
            reader.Close();

            _yhteys.Close();
            lstPelaajat.SelectedIndex = 0;
        }

        private void btnUusiLompakko_Click(object sender, RoutedEventArgs e)
        {
            if (lstPelaajat.SelectedItem == null)
            {
                return;
            }
            int suurin = lstLompakot.Items.Count;
            lstLompakot.SelectedIndex = suurin;

            string vPelaaja = lstPelaajat.SelectedValue.ToString();
            string vLompakko = "" + suurin + "";
            Ikkunat.ValittuPelaaja = vPelaaja;
            Ikkunat.ValittuLompakko = vLompakko;

            Ikkunat.UusiLompakko.Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            lstPelaajat.Items.Clear();

            btnPoistaLompakko.IsEnabled = false;
            btnPoistaPelaaja.IsEnabled = false;
            btnPoisto.IsEnabled = true;
            btnPoistoPois.Visibility = Visibility.Hidden;
            lblPoisto.Visibility = Visibility.Hidden;

            btnAloitaPeli.IsEnabled = false;


            _yhteys =
                new SqlConnection(
                    "Server=tcp:ubr6dhaftm.database.windows.net,1433;Database=LompakkoDB;User Id=LompakkoAdmin@ubr6dhaftm;Password=Lompakko*;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=True;");

            _yhteys2 =
                new SqlConnection(
                    "Server=tcp:ubr6dhaftm.database.windows.net,1433;Database=LompakkoDB;User Id=LompakkoAdmin@ubr6dhaftm;Password=Lompakko*;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=True;");

            _yhteys.Open();


            var pelaajaHaku = new SqlCommand();
            pelaajaHaku.CommandText =
                "SELECT nimi,pisteet FROM Pelaaja ";
            pelaajaHaku.Connection = _yhteys;

            SqlDataReader reader = pelaajaHaku.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var nimi = (string) reader.GetSqlString(0);
                    string tiedot = nimi.Trim();
                    lstPelaajat.Items.Add(tiedot);
                }
            }
            reader.Close();

            _yhteys.Close();
        }

        private void lstPelaajat_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void btnAloitaPeli_Click(object sender, RoutedEventArgs e)
        {
            string valittuNimi = lstPelaajat.SelectedItem.ToString();
            int pisteet = int.Parse(lblPisteet.Content.ToString());
            decimal saldo = decimal.Parse(lblSaldo.Content.ToString());
            int lompakkoTunnus = int.Parse(lblLompakko.Content.ToString());

            #region Toinen tapa tehdä objektin sisältäö

            //var pelaajaHaku = new SqlCommand();
            //pelaajaHaku.CommandText =
            //    "SELECT nimi,pisteet FROM Pelaaja WHERE nimi=" + "'" + valittuNimi + "'"; 
            //pelaajaHaku.Connection = _yhteys;

            //SqlDataReader reader = pelaajaHaku.ExecuteReader();

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        pisteet = reader.GetInt16(1);
            //    }
            //}

            //reader.Close();

            //string valittuLompakko = lstLompakot.SelectedValue.ToString();

            //var lompakkoHaku = new SqlCommand();

            //string selectKomento =
            //    "SELECT lompakkotunnus, saldo  FROM Lompakko WHERE lompakkotunnus=" + "'" + valittuLompakko + "'" +
            //    " AND nimi=" + "'" + valittuNimi + "'";

            //lompakkoHaku.CommandText = selectKomento;
            //lompakkoHaku.Connection = _yhteys;

            //SqlDataReader lompakkoReader = lompakkoHaku.ExecuteReader();

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        lompakkoTunnus = reader.GetInt16(0);
            //        saldo = reader.GetDecimal(1);
            //    }
            //}
            //else
            //{
            //    lstLompakot.Items.Add("Virhe");
            //}


            //lompakkoReader.Close();
            //_yhteys.Close();

            #endregion

            AloitaPeli = new Pelaaja(valittuNimi, lompakkoTunnus, saldo, pisteet);

            MessageBox.Show(AloitaPeli.ToString());
        }

        private void txtMuuta_GotFocus(object sender, RoutedEventArgs e)
        {
            txtMuuta.Text = String.Empty;

        }

        private void btnPoistaPelaaja_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _yhteys.Open();
            }
            catch (Exception yhteydenOttoVika)
            {
                MessageBox.Show(yhteydenOttoVika.Message);
                return;
            }

            string nimi = lstPelaajat.SelectedItem.ToString();

            string poistoKomento =
                "exec Poistapelaaja @PoistettavaPelaaja";

            var poistaPelaaja = new SqlCommand(poistoKomento, _yhteys);

            var poistoParametri = new SqlParameter("PoistettavaPelaaja", SqlDbType.Char, 50);

            poistoParametri.Value = nimi;

            poistaPelaaja.Parameters.Add(poistoParametri);

            MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa pelaajan " + nimi + "",
    "Pelaajan poiso varmistus",
    MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                poistaPelaaja.ExecuteNonQuery();
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }



            MessageBox.Show("Poistettiin pelaaja " + nimi + " Ja kyseiselle pelaajalle kuuluvat lompakot ");

            _yhteys.Close();
        }

        private void btnPoistaLompakko_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _yhteys2.Open();
            }
            catch (Exception yhteydenOttoVika)
            {
                MessageBox.Show(yhteydenOttoVika.Message);
                return;
            }

            int poistettavaLompakko = int.Parse(lstLompakot.SelectedItem.ToString());
            string pelaaja = lstPelaajat.SelectedItem.ToString();
            decimal saldo = decimal.Parse(lblSaldo.Content.ToString());

            string poistoKomento =
                "exec PoistaLompakko @poistettavaLompakko, @pelaaja";

            var poistaLompakko = new SqlCommand(poistoKomento, _yhteys2);

            var lompakkoParametri = new SqlParameter("PoistettavaLompakko", SqlDbType.Int);
            var pelaajaParametri = new SqlParameter("pelaaja", SqlDbType.Char, 50);

            lompakkoParametri.Value = poistettavaLompakko;
            pelaajaParametri.Value = pelaaja;

            poistaLompakko.Parameters.Add(lompakkoParametri);
            poistaLompakko.Parameters.Add(pelaajaParametri);

            MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa pelaajan " + pelaaja + " Lompakon: " + poistettavaLompakko + "",
            "Pelaajan poiso varmistus",
            MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                poistaLompakko.ExecuteNonQuery();
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }

            MessageBox.Show("Poistettiin pelaajan " + pelaaja + " Lompakko " + poistettavaLompakko + " ");

            _yhteys2.Close();
        }
        
        private void btnPoisto_Click(object sender, RoutedEventArgs e)
        {
            btnPoistaLompakko.IsEnabled = true;
            btnPoistaPelaaja.IsEnabled = true;
            btnPoisto.IsEnabled = false;
            btnPoistoPois.Visibility = Visibility.Visible;
            lblPoisto.Visibility = Visibility.Visible;
        }

        private void btnPoistoPois_Click(object sender, RoutedEventArgs e)
        {
            btnPoistaLompakko.IsEnabled = false;
            btnPoistaPelaaja.IsEnabled = false;
            btnPoisto.IsEnabled = true;
            btnPoistoPois.Visibility = Visibility.Hidden;
            lblPoisto.Visibility = Visibility.Hidden;
        }
    }
}