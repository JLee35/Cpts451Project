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
        private string selectedUserID = "";

        public BusinessDetails(string businessID, string selectedUserID)
        {
            InitializeComponent();
            this.businessID = String.Copy(businessID);
            this.selectedUserID = String.Copy(selectedUserID);
            addColumns2ReviewGrid();
            addColumns2OpenTimesGrid();
            loadBusinessDetails();
            loadReviews();
            loadOpenTimes();
            addReviewScoreOptions();
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
            public TimeSpan openTime { get; set; }
            public TimeSpan closeTime { get; set; }
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

        private bool executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
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
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return true;
                }
            }
        }

        private void setBusinessDetails(NpgsqlDataReader R)
        {
            bname.Text = R.GetString(0);
            address.Text = R.GetString(1);
            city.Text = R.GetString(2);
            state.Text = R.GetString(3);

            if (R.GetInt16(4) == 0)
            {
                status.Text = "Closed";
            }

            else
            {
                status.Text = "Open";
            }
        }

        private void loadBusinessDetails()
        {
            string sqlStr = "SELECT businessName, address, city, businessState, openStatus FROM Business WHERE businessID = '" + this.businessID + "';";
            executeQuery(sqlStr, setBusinessDetails);
        }

        private void loadOpenTimes()
        {
            string sqlStr = "SELECT day, openTime, closeTime FROM OpenTimes WHERE OpenTimes.businessID = '" + this.businessID + "';";
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
            openTimesDataGrid.Items.Add(new OpenTimes() { day = R.GetString(0), openTime = R.GetTimeSpan(1), closeTime = R.GetTimeSpan(2) });
        }

        private void AddToFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.businessID != null && this.businessID != "" && this.selectedUserID != null && this.selectedUserID != "")
            {
                Debug.WriteLine("Adding businessID: " + this.businessID + " and userID: " + this.selectedUserID);

                string sqlStr = "INSERT INTO UserFavorite (userID, businessID) VALUES ('" + this.selectedUserID + "', '" + this.businessID + "');";
                executeQuery(sqlStr, null);
            }
        }

        private void addReviewScoreOptions()
        {
            ReviewScoreList.Items.Add("5");
            ReviewScoreList.Items.Add("4");
            ReviewScoreList.Items.Add("3");
            ReviewScoreList.Items.Add("2");
            ReviewScoreList.Items.Add("1");

            // Set default selection.
            ReviewScoreList.SelectedIndex = 0;
        }

        //idk how to change the button name
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            string sqlStr;
            string reviewID;
            if (ReviewContentBox.Text != null && this.businessID != null && this.businessID != "" && this.selectedUserID != null && this.selectedUserID != "")
            {
                Debug.WriteLine("Adding businessID: " + this.businessID + " and userID: " + this.selectedUserID);
                reviewID = this.selectedUserID + this.businessID;
                while (!success)
                {
                    sqlStr = "INSERT INTO Review (reviewID, userID, businessID, stars, content) VALUES ('" + reviewID + "', '" +
                        this.selectedUserID + "', '" + this.businessID + "', '" + Int32.Parse(ReviewScoreList.SelectedItem.ToString()) + "', '" + ReviewContentBox.Text + "');";
                    success = executeQuery(sqlStr, null);
                    if (!success)
                        reviewID = reviewID + 'A'; //super makeshift solution, i know
                }
                reviewDataGrid.Items.Clear();
                loadReviews();
            }
        }

        //don't delete this function, i added it like a retard and now idk how to get rid of it
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
