using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace LompakkoOhjelma
{
    /// <summary>
    ///     Interaction logic for UusiPelaaja.xaml
    /// </summary>
    public partial class UusiPelaaja 
    {
        private SqlConnection _yhteys;

        public UusiPelaaja()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _yhteys =
                new SqlConnection(
                    "Server=tcp:ubr6dhaftm.database.windows.net,1433;Database=LompakkoDB;User Id=LompakkoAdmin@ubr6dhaftm;Password=Lompakko*;Trusted_Connection=False;Encrypt=True;");
            try
            {
                _yhteys.Open();
            }
            catch (Exception yhteysVirhe)
            {
                MessageBox.Show(yhteysVirhe.Message);
                return;
            }
            _yhteys.Close();
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Ikkunat.Uusi = this;
            Ikkunat.Paluu = new Valinta();
        }

        private void btnLuo_Click(object sender, RoutedEventArgs e)
        {

            if (txtNimi.Text == "Nimi" || txtNimi.Text == "")
            {
                MessageBox.Show("Anna jokin nimi");
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


            

            string uusiPelaaja = txtNimi.Text;
            uusiPelaaja= uusiPelaaja.First().ToString().ToUpper() + uusiPelaaja.Substring(1);

            var pelaaja = new SqlCommand();

            string selectPelaaja = "SELECT nimi FROM Pelaaja WHERE nimi=" + "'" + uusiPelaaja + "'";

            pelaaja.CommandText = selectPelaaja;
            pelaaja.Connection = _yhteys;

            SqlDataReader pelaajaReader = pelaaja.ExecuteReader();

            if (pelaajaReader.HasRows)
            {
                while (pelaajaReader.Read())
                {
                    MessageBox.Show("Nimi " + uusiPelaaja + " on varattu. Kokeile jotain toista nimeä");
                    _yhteys.Close();
                    return;
                }
            }

            pelaajaReader.Close();

            string insertKomento =
             "insert into Pelaaja (nimi,pisteet) values(@uusiPelaaja,'1')";

            SqlCommand lisääPelaaja = new SqlCommand(insertKomento,_yhteys);

            SqlParameter pelaajaParametri = new SqlParameter("uusiPelaaja", SqlDbType.Char, 50);

            pelaajaParametri.Value = uusiPelaaja;

            lisääPelaaja.Parameters.Add(pelaajaParametri);

            lisääPelaaja.ExecuteNonQuery();

            MessageBox.Show("Luotiin uusi pelaaja " + uusiPelaaja + " ");

            _yhteys.Close();


            Hide();
        }


        private void btnPaluu_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtNimi_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNimi.Text = String.Empty;
        }
    }
}