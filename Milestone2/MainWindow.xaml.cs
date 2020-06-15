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
        private object selectedCategoryToAdd = null;
        private object selectedCategoryToRemove = null;

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
            nameCol.Width = 200;
            businessGrid.Columns.Add(nameCol);

            DataGridTextColumn addressCol = new DataGridTextColumn();
            addressCol.Binding = new Binding("address");
            addressCol.Header = "Address";
            addressCol.Width = 200;
            businessGrid.Columns.Add(addressCol);

            DataGridTextColumn cityCol = new DataGridTextColumn();
            cityCol.Binding = new Binding("city");
            cityCol.Header = "City";
            cityCol.Width = 100;
            businessGrid.Columns.Add(cityCol);

            DataGridTextColumn stateCol = new DataGridTextColumn();
            stateCol.Binding = new Binding("businessState");
            stateCol.Header = "State";
            stateCol.Width = 60;
            businessGrid.Columns.Add(stateCol);
            
            DataGridTextColumn zipCol = new DataGridTextColumn();
            zipCol.Binding = new Binding("zip");
            zipCol.Header = "Postal Code";
            zipCol.Width = 85;
            businessGrid.Columns.Add(zipCol);

            DataGridTextColumn starCol = new DataGridTextColumn();
            starCol.Binding = new Binding("stars");
            starCol.Header = "Stars";
            starCol.Width = 60;
            businessGrid.Columns.Add(starCol);

            DataGridTextColumn reviewCountCol = new DataGridTextColumn();
            reviewCountCol.Binding = new Binding("reviewCount");
            reviewCountCol.Header = "# Reviews";
            reviewCountCol.Width = 85;
            businessGrid.Columns.Add(reviewCountCol);

            DataGridTextColumn numCheckinsCol = new DataGridTextColumn();
            numCheckinsCol.Binding = new Binding("numCheckins");
            numCheckinsCol.Header = "# Checkins";
            numCheckinsCol.Width = 85;
            businessGrid.Columns.Add(numCheckinsCol);

            DataGridTextColumn businessIDCol = new DataGridTextColumn();
            businessIDCol.Binding = new Binding("businessID");
            businessIDCol.Header = "businessID";
            businessIDCol.Width = 85;
            businessGrid.Columns.Add(businessIDCol);

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

        private void addZip(NpgsqlDataReader R)
        {
            zipList.Items.Add(R.GetInt32(0));
        }

        private void addCategory(NpgsqlDataReader R)
        {
            categoriesList.Items.Add(R.GetString(0));
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
            businessGrid.Items.Add(new Business() { businessName = R.GetString(0), address = R.GetString(1), city = R.GetString(2), businessState = R.GetString(3), zip = R.GetInt32(4), stars = R.GetFloat(5), reviewCount = R.GetInt32(6), numCheckins = R.GetInt32(7), businessID = R.GetString(8)});
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipList.Items.Clear();
            businessGrid.Items.Clear();
            
            if (cityList.SelectedIndex > -1)
            {
                // Populate zip codes in city in zipList.
                string sqlStr1 = "SELECT distinct zip FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY zip";
                executeQuery(sqlStr1, addZip);

                // Populate businesses in city in listbox.
                string sqlStr2 = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessid FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY businessName;";
                executeQuery(sqlStr2, addGridRow);
            }
        }

        // Toggle BusinessDetails window when business item is selected from listbox.
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

        private void ZipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriesList.Items.Clear();
            businessGrid.Items.Clear();

            if (zipList.SelectedIndex > -1)
            {
                // Populate business categories within selected zip.
                string sqlStr1 = "SELECT distinct name FROM Category, Business WHERE Business.businessID = Category.businessID AND zip = '" + zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY name;";
                executeQuery(sqlStr1, addCategory);

                // Populate businesses within zip in listbox.
                string sqlStr2 = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessid FROM Business WHERE zip = '" + zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY businessName;";
                executeQuery(sqlStr2, addGridRow);
            }
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((categoriesList.SelectedItem != null) && (categoriesList.SelectedIndex > -1))
            {
                selectedCategoryToAdd = categoriesList.SelectedItem;
            }
        }
        
        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedCategoryToAdd != null) && (categoriesList.SelectedIndex > -1))
            {
                AddToCategoryList(selectedCategoryToAdd);
            }
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedCategoryToRemove != null) && (selectedCategoriesList.SelectedIndex > -1))
            {
                RemoveFromCategoryList(selectedCategoryToRemove);
            }
        }

        private void AddToCategoryList(object item)
        {
            if (!selectedCategoriesList.Items.Contains(item))
            {
                selectedCategoriesList.Items.Add(item);
            }
        }

        private void RemoveFromCategoryList(object item)
        {
            if (selectedCategoriesList.Items.Contains(item))
            {
                selectedCategoriesList.Items.Remove(item);
            }
        }

        private void SearchBusinessButton_Click(object sender, RoutedEventArgs e)
        {
            // If categories are selected then add them into the query.
            if (selectedCategoriesList.Items.Count > 0)
            {
                string sqlStr = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessID FROM Business, " + 
                    GetCategoryItems() + " WHERE zip = '" +
                    zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND  Business.businessID = filteredCategories.query0ID";
                
                sqlStr += " ORDER BY businessName;";

                // Empty existing ListBox.
                businessGrid.Items.Clear();
                executeQuery(sqlStr, addGridRow);
            }
            
            else
            {
                string sqlStr2 = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessid FROM Business WHERE zip = '" + zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY businessName;";
                executeQuery(sqlStr2, addGridRow);
            }
            
        }

        private string GetCategoryItems()
        {
            string sqlStr = "(";

            for (int x = 0; x < selectedCategoriesList.Items.Count; x++)
            {
                if (x == 0)
                {
                    sqlStr += "SELECT query0ID FROM (SELECT businessID as query0ID FROM Category WHERE name = '" + selectedCategoriesList.Items[x].ToString() + "') as query0 ";
                }

                else
                {
                    if ((x+1) == selectedCategoriesList.Items.Count)
                    {
                        sqlStr += "INNER JOIN (SELECT businessID as query" + x.ToString() + "ID FROM Category WHERE name = '" + selectedCategoriesList.Items[x].ToString() + "') as query" +
                            x.ToString() + " ON query0.query0ID = query" + x.ToString() + ".query" + x.ToString() + "ID ";
                    }

                    else
                    {
                        sqlStr += "INNER JOIN (SELECT businessID as query" + x.ToString() + "ID FROM Category WHERE name = '" + selectedCategoriesList.Items[x].ToString() + "') as query" +
                            x.ToString() + " ON query0.query0ID = query" + x.ToString() + ".query" + x.ToString() + "ID ";
                    }
                }
            }
            sqlStr += ") as filteredCategories";
            return sqlStr;
        }

        private void SelectedCategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((selectedCategoriesList.SelectedItem != null) && (selectedCategoriesList.SelectedIndex > -1))
            {
                selectedCategoryToRemove = selectedCategoriesList.SelectedItem;
            }
        }
    }
}
