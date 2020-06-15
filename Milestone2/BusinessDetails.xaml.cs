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
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        private string businessID = "";
        public BusinessDetails(string businessID)
        {
            InitializeComponent();
            this.businessID = String.Copy(businessID);
            addColumns2Grid();
            loadBusinessDetails();
            loadBusinessNumbers();
            loadReviews();
        }

        public class Review
        {
            public string reviewID { get; set; }
            public string userID { get; set; }
            public string userName { get; set; }
            public string businessID { get; set; }
            public float stars { get; set; }
            public string content { get; set; }
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn userIDCol = new DataGridTextColumn();
            userIDCol.Binding = new Binding("userName");
            userIDCol.Header = "User Name";
            userIDCol.Width = 75;
            reviewDataGrid.Columns.Add(userIDCol);

            DataGridTextColumn starsCol = new DataGridTextColumn();
            starsCol.Binding = new Binding("stars");
            starsCol.Header = "Stars";
            starsCol.Width = 75;
            reviewDataGrid.Columns.Add(starsCol);

            DataGridTextColumn contentCol = new DataGridTextColumn();
            contentCol.Binding = new Binding("content");
            contentCol.Header = "Text";
            contentCol.Width = 500;
            reviewDataGrid.Columns.Add(contentCol);
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password=mustafa";
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
                        reader.Read();
                        myf(reader);
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

        private void setBusinessDetails(NpgsqlDataReader R)
        {
            bname.Text = R.GetString(0);
            state.Text = R.GetString(1);
            city.Text = R.GetString(2);
            
        }

        void setNumInState(NpgsqlDataReader R)
        {
            numInState.Content = R.GetInt16(0).ToString();
        }

        void setNumInCity(NpgsqlDataReader R)
        {
            numInCity.Content = R.GetInt16(0).ToString();
        }

        private void loadBusinessNumbers ()
        {
            string sqlStr1 = "SELECT count(*) FROM Business WHERE businessState = (SELECT businessState FROM Business WHERE businessID = '" + this.businessID + "');";
            executeQuery(sqlStr1, setNumInState);
            string sqlStr2 = "SELECT count(*) FROM Business WHERE city = (SELECT city FROM Business WHERE businessID = '" + this.businessID + "');";
            executeQuery(sqlStr2, setNumInCity);
              
        }

        private void loadBusinessDetails()
        {
            string sqlStr = "SELECT businessName, businessState, city FROM Business WHERE businessID = '" + this.businessID + "';";
            executeQuery(sqlStr, setBusinessDetails);
        }

        private void loadReviews()
        {
            string sqlStr = "SELECT name, Review.stars, content FROM UserTable, Review, Business WHERE Business.businessID = '" + this.businessID + "' AND Review.businessID = '" + this.businessID +
                "' AND UserTable.userID = Review.userID;";
            executeQuery(sqlStr, addGridRow);
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            reviewDataGrid.Items.Add(new Review() { userName = R.GetString(0), stars = R.GetFloat(1), content = R.GetString(2) });
        }
    }

    
}
