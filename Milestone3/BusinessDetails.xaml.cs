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
            addColumns2ReviewGrid();
            addColumns2OpenTimesGrid();
            loadBusinessDetails();
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

        public class OpenTimes
        {
            public string day { get; set; }
            public string openTime { get; set; }
            public string closeTime { get; set; }
        }

        private void addColumns2ReviewGrid()
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
            contentCol.Width = 800;
            reviewDataGrid.Columns.Add(contentCol);
        }

        private void addColumns2OpenTimesGrid()
        {
            DataGridTextColumn dayCol = new DataGridTextColumn();
            dayCol.Binding = new Binding("day");
            dayCol.Header = "Day";
            dayCol.Width = 120;
            openTimesDataGrid.Columns.Add(dayCol);

            DataGridTextColumn openTimeCol = new DataGridTextColumn();
            openTimeCol.Binding = new Binding("openTime");
            openTimeCol.Header = "Opens at";
            openTimeCol.Width = 120;
            openTimesDataGrid.Columns.Add(openTimeCol);

            DataGridTextColumn closeTimeCol = new DataGridTextColumn();
            closeTimeCol.Binding = new Binding("closeTime");
            closeTimeCol.Header = "Closes at";
            closeTimeCol.Width = 120;
            openTimesDataGrid.Columns.Add(closeTimeCol);
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
                        while (reader.Read())
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
            address.Text = R.GetString(1);
            city.Text = R.GetString(2);
            state.Text = R.GetString(3);
        }
        
        private void loadBusinessDetails()
        {
            string sqlStr = "SELECT businessName, address, city, businessState FROM Business WHERE businessID = '" + this.businessID + "';";
            executeQuery(sqlStr, setBusinessDetails);
        }

        private void loadOpenTimes()
        {
            string sqlStr = "SELECT day, openTime, closeTime FROM Business, OpenTimes WHERE Business.businessID = '" + this.businessID + "' AND OpenTimes.businessID = '" + this.businessID + "';";
            executeQuery(sqlStr, addOpenTimesGridRow);
        }

        private void loadReviews()
        {
            string sqlStr = "SELECT name, Review.stars, content FROM UserTable, Review, Business WHERE Business.businessID = '" + this.businessID + "' AND Review.businessID = '" + this.businessID +
                "' AND UserTable.userID = Review.userID;";
            executeQuery(sqlStr, addReviewGridRow);
        }

        private void addReviewGridRow(NpgsqlDataReader R)
        {
            reviewDataGrid.Items.Add(new Review() { userName = R.GetString(0), stars = R.GetFloat(1), content = R.GetString(2) });
        }

        private void addOpenTimesGridRow(NpgsqlDataReader R)
        {
            openTimesDataGrid.Items.Add(new OpenTimes() { day = R.GetString(0), openTime = R.GetString(1), closeTime = R.GetString(2) });
        }
    }
}
