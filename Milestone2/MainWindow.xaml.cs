using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class Business
        {
            public string businessID { get; set; }
            public string businessName { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string businessState { get; set; }
            
            public float avgScore { get; set; }
            public float reviewRating { get; set; }
            public float stars { get; set; }

            public int numCheckins { get; set; }
            public int reviewCount { get; set; }
            public int openStatus { get; set; }
            public int zip { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns2Grid();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password=mustafa";
        }

        private void addStates()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct businessState FROM Business ORDER BY businessState";

                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn nameCol = new DataGridTextColumn();
            nameCol.Binding = new Binding("businessName");
            nameCol.Header = "BusinessName";
            nameCol.Width = 255;
            businessGrid.Columns.Add(nameCol);

            DataGridTextColumn stateCol = new DataGridTextColumn();
            stateCol.Binding = new Binding("businessState");
            stateCol.Header = "State";
            stateCol.Width = 60;
            businessGrid.Columns.Add(stateCol);

            DataGridTextColumn cityCol = new DataGridTextColumn();
            cityCol.Binding = new Binding("city");
            cityCol.Header = "City";
            cityCol.Width = 150;
            businessGrid.Columns.Add(cityCol);

            DataGridTextColumn zipCol = new DataGridTextColumn();
            zipCol.Binding = new Binding("zip");
            zipCol.Header = "Postal Code";
            zipCol.Width = 60;
            businessGrid.Columns.Add(zipCol);

            //DataGridTextColumn idCol = new DataGridTextColumn();
            //idCol.Binding = new Binding("businessID");
            //idCol.Header = "";
            //idCol.Width = 0;
            //businessGrid.Columns.Add(idCol);
            
        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            myf(reader);

                    } catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addCity(NpgsqlDataReader R)
        {
            cityList.Items.Add(R.GetString(0));
        }

        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityList.Items.Clear();
            if ( stateList.SelectedIndex > -1 )
            {
                string sqlStr = "SELECT distinct city FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlStr, addCity);
            }
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { businessName = R.GetString(0), businessState = R.GetString(1), city = R.GetString(2), zip = R.GetInt32(3) });
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT businessName, businessState, city, zip FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY businessName;";
                executeQuery(sqlStr, addGridRow);
            }
        }

        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.businessID != null) && (B.businessID.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businessWindow = new BusinessDetails(B.businessID.ToString());
                    businessWindow.Show();
                }
            }
        }
    }
}
