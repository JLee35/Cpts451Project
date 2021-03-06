﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private Business selectedFavoriteToRemove = null;

        private string selectedOrder = "businessName";
        private string selectedUserLat = "0";
        private string selectedUserLong = "0";
        public string SelectedUserID = null;

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
            public float distance { get; set; }

            public int numCheckins { get; set; }
            public int reviewCount { get; set; }
            public int openStatus { get; set; }
            public int zip { get; set; }
        }

        public class User
        {
            public string userID { get; set; }
            public string name { get; set; }
            
            public float avgStars { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }

            public int numFans { get; set; }
            public int votes { get; set; }

            public DateTime yelpingSince { get; set; }
        }

        public class Review
        {
            public string userName { get; set; }
            public string businessName { get; set; }
            public string city { get; set; }
            public string text { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addStates();

            // Add data grids.
            addColumns2BusinessGrid();
            addColumns2UserFavoriteBusinessesDataGrid();
            addColumns2UserFriendsDataGrid();
            addColumns2UserFriendsReviewDataGrid();

            addSortResultOptions();
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

        private void addSortResultOptions()
        {
            sortResultsByList.Items.Add("Name (default)");
            sortResultsByList.Items.Add("Highest rating (stars)");
            sortResultsByList.Items.Add("Most reviewed");
            sortResultsByList.Items.Add("Best review rating (highest avg review rating)");
            sortResultsByList.Items.Add("Most check-ins");
            sortResultsByList.Items.Add("Nearest");

            // Set default selection.
            sortResultsByList.SelectedIndex = 0;
        }

        private void addColumns2BusinessGrid()
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

            DataGridTextColumn distanceCol = new DataGridTextColumn();
            businessIDCol.Binding = new Binding("distance");
            businessIDCol.Header = "distance";
            businessIDCol.Width = 85;
            businessGrid.Columns.Add(distanceCol);
            
        }

        private void addColumns2UserFavoriteBusinessesDataGrid()
        {
            DataGridTextColumn nameCol = new DataGridTextColumn();
            nameCol.Binding = new Binding("businessName");
            nameCol.Header = "Business Name";
            nameCol.Width = 150;
            userFavoriteBusinessesDataGrid.Columns.Add(nameCol);

            DataGridTextColumn starsCol = new DataGridTextColumn();
            starsCol.Binding = new Binding("stars");
            starsCol.Header = "Stars";
            starsCol.Width = 75;
            userFavoriteBusinessesDataGrid.Columns.Add(starsCol);

            DataGridTextColumn cityCol = new DataGridTextColumn();
            cityCol.Binding = new Binding("city");
            cityCol.Header = "City";
            cityCol.Width = 100;
            userFavoriteBusinessesDataGrid.Columns.Add(cityCol);

            DataGridTextColumn zipCol = new DataGridTextColumn();
            zipCol.Binding = new Binding("zip");
            zipCol.Header = "Zipcode";
            zipCol.Width = 75;
            userFavoriteBusinessesDataGrid.Columns.Add(zipCol);

            DataGridTextColumn addressCol = new DataGridTextColumn();
            addressCol.Binding = new Binding("address");
            addressCol.Header = "Address";
            addressCol.Width = 300;
            userFavoriteBusinessesDataGrid.Columns.Add(addressCol);

            DataGridTextColumn businessIDCol = new DataGridTextColumn();
            businessIDCol.Binding = new Binding("businessID");
            businessIDCol.Header = "businessID";
            businessIDCol.Width = 50;
            userFavoriteBusinessesDataGrid.Columns.Add(businessIDCol);
        }

        private void addColumns2UserFriendsDataGrid()
        {
            DataGridTextColumn nameCol = new DataGridTextColumn();
            nameCol.Binding = new Binding("name");
            nameCol.Header = "Name";
            nameCol.Width = 100;
            userFriendsDataGrid.Columns.Add(nameCol);

            DataGridTextColumn starsCol = new DataGridTextColumn();
            starsCol.Binding = new Binding("avgStars");
            starsCol.Header = "Avg Stars";
            starsCol.Width = 75;
            userFriendsDataGrid.Columns.Add(starsCol);

            DataGridTextColumn yelpingSinceCol = new DataGridTextColumn();
            yelpingSinceCol.Binding = new Binding("yelpingSince");
            yelpingSinceCol.Header = "Yelping Since";
            yelpingSinceCol.Width = 150;
            userFriendsDataGrid.Columns.Add(yelpingSinceCol);
        }

        private void addColumns2UserFriendsReviewDataGrid()
        {
            DataGridTextColumn nameCol = new DataGridTextColumn();
            nameCol.Binding = new Binding("userName");
            nameCol.Header = "User Name";
            nameCol.Width = 100;
            userFriendsReviewDataGrid.Columns.Add(nameCol);

            DataGridTextColumn businessNameCol = new DataGridTextColumn();
            businessNameCol.Binding = new Binding("businessName");
            businessNameCol.Header = "Business";
            businessNameCol.Width = 100;
            userFriendsReviewDataGrid.Columns.Add(businessNameCol);

            DataGridTextColumn cityCol = new DataGridTextColumn();
            cityCol.Binding = new Binding("city");
            cityCol.Header = "City";
            cityCol.Width = 100;
            userFriendsReviewDataGrid.Columns.Add(cityCol);

            DataGridTextColumn textCol = new DataGridTextColumn();
            textCol.Binding = new Binding("text");
            textCol.Header = "Text";
            textCol.Width = 400;
            userFriendsReviewDataGrid.Columns.Add(textCol);
        }

        private void executeQuery(string sqlstr, string order, Action<NpgsqlDataReader> myf)
        {
            if (order != null && sqlstr.Length > 1)
            {
                if (sqlstr[sqlstr.Length - 1] == ';')
                        sqlstr = sqlstr.Substring(0, sqlstr.Length - 2) + " ORDER BY " + order + ';';
                    else
                        sqlstr = sqlstr + " ORDER BY " + order + ';';
            }
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
                string sqlStr = "SELECT distinct city FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "';";
                executeQuery(sqlStr, null, addCity);

                // Refresh business list.
                SearchBusinessButton_Click(null, null);
            }
        }

        private void addBusinessGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { businessName = R.GetString(0), address = R.GetString(1), city = R.GetString(2), businessState = R.GetString(3), zip = R.GetInt32(4), stars = R.GetFloat(5), reviewCount = R.GetInt32(6), numCheckins = R.GetInt32(7), businessID = R.GetString(8), distance = R.GetFloat(9)});
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipList.Items.Clear();
            businessGrid.Items.Clear();
            
            if (cityList.SelectedIndex > -1)
            {
                // Populate zip codes in city in zipList.
                string sqlStr1 = "SELECT distinct zip FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "';";
                executeQuery(sqlStr1, null, addZip);

                // Populate businesses in city in listbox.
                string sqlStr2 = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessid, latitude, longitude FROM Business WHERE businessState = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'";
                sqlStr2 = "SELECT *, myDistance(" + selectedUserLat + "," + selectedUserLong +
                        ",query.latitude, query.longitude) as distance FROM (" + sqlStr2 + ") as query";
                executeQuery(sqlStr2, selectedOrder, addBusinessGridRow);
            }
        }

        // Toggle BusinessDetails window when business item is selected from listbox.
        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1 && SelectedUserID != null)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.businessID != null) && (B.businessID.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businessWindow = new BusinessDetails(B.businessID.ToString(), SelectedUserID, this);
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
                string sqlStr1 = "SELECT distinct name FROM Category, Business WHERE Business.businessID = Category.businessID AND zip = '" + zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "';";
                executeQuery(sqlStr1, null, addCategory);

                // Populate businesses within zip in listbox.
                string sqlStr2 = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessid, latitude, longitude FROM Business WHERE zip = '" + zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'";
                sqlStr2 = "SELECT *, myDistance(" + selectedUserLat + "," + selectedUserLong +
                        ",query.latitude, query.longitude) as distance FROM (" + sqlStr2 + ") as query";
                executeQuery(sqlStr2, selectedOrder, addBusinessGridRow);
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
            // Empty existing ListBox.
            businessGrid.Items.Clear();

            string sqlStr = "";

            bool stateIsSelected = stateList.SelectedItem != null;
            bool cityIsSelected = cityList.SelectedItem != null;
            bool zipIsSelected = zipList.SelectedItem != null;
            bool categoriesAreSelected = selectedCategoriesList.Items.Count > 0;

            sqlStr = "SELECT businessName, address, city, businessState, zip, stars, reviewCount, numCheckins, businessID, latitude, longitude FROM Business ";
            
            // If categories are selected, then the user has selected a state, city, and zip.
            if (categoriesAreSelected)
            {
                sqlStr += ", " + GetCategoryItems() + " WHERE zip = '" +
                    zipList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND  Business.businessID = filteredCategories.query0ID";
            }
            
            // Build query based on what the user has provided without categories.
            else
            {
                // If state is specified, add to query.
                if (stateIsSelected)
                {
                    sqlStr += "WHERE businessState = '" + stateList.SelectedItem.ToString() + "'";
                }

                // If city is specified then state must have also been selected.
                if (cityIsSelected)
                {
                    sqlStr += " AND city = '" + cityList.SelectedItem.ToString() + "'";
                }

                // If zip is specified then city must have also been selected.
                if (zipIsSelected)
                {
                    sqlStr += " AND zip = '" + zipList.SelectedItem.ToString() + "'";
                }
            }
            sqlStr = "SELECT *, myDistance(" + selectedUserLat + "," + selectedUserLong +
                        ",query.latitude, query.longitude) as distance FROM (" + sqlStr + ") as query";
            executeQuery(sqlStr, selectedOrder, addBusinessGridRow);
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

        private void SortResultsByList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortResultsByList.SelectedItem.ToString() == "Name (default)")
                selectedOrder = "businessName";
            else if (sortResultsByList.SelectedItem.ToString() == "Highest rating (stars)")
                selectedOrder = "stars DESC";
            else if (sortResultsByList.SelectedItem.ToString() == "Most reviewed")
                selectedOrder = "reviewCount DESC";
            else if (sortResultsByList.SelectedItem.ToString() == "Best review rating (highest avg review rating)")
                selectedOrder = "reviewRating DESC";
            else if (sortResultsByList.SelectedItem.ToString() == "Most check-ins")
                selectedOrder = "numCheckins DESC";
            else if (sortResultsByList.SelectedItem.ToString() == "Nearest")
                selectedOrder = "distance";
            
            SearchBusinessButton_Click(null, null);
        }

        private void SetCurrentUserTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PopulateUserIDListBoxWithSearchResults(setCurrentUserTextBox.Text);
        }

        private void PopulateUserIDListBoxWithSearchResults(string nameToSearchFor)
        {
            if (userIDListBox != null && userIDListBox.Items.Count > 0)
            {
                userIDListBox.Items.Clear();
            } 

            string sqlStr = "SELECT userID FROM UserTable WHERE name LIKE '" + nameToSearchFor + "%';";
            executeQuery(sqlStr, null, AddToUserNameList);
        }
        
        private void AddToUserNameList(NpgsqlDataReader R)
        {
            userIDListBox.Items.Add(R.GetString(0));
        }

        private void UserIDListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userIDListBox.SelectedIndex > -1 && userIDListBox.SelectedItem != null)
            {
                SelectedUserID = userIDListBox.SelectedItem.ToString();
                ClearCurrentUserInfoFields();
                UpdateCurrentUserInfoFields(userIDListBox.SelectedItem.ToString());
            }
        }

        private void UpdateCurrentUserInfoFields(string userID)
        {
            // Update general user information.
            string sqlStr1 = "SELECT name, avgStars, yelpingSince, latitude, longitude, numFans, votes FROM UserTable WHERE userID = '" + userID + "';";
            executeQuery(sqlStr1, null, AddCurrentUserInformation);

            UpdateUserFavoriteBusinesses(userID);

            // Update friends list.
            string sqlStr3 = "SELECT name, avgStars, yelpingSince FROM UserTable, UserFriend WHERE UserFriend.friendUserID = UserTable.userID AND UserFriend.userID = '" + userID + "';";
            executeQuery(sqlStr3, null, addCurrentUserFriends);

            // Update friends reviews list.
            string sqlStr4 = "SELECT UserTable.name, businessName, city, content " +
                            "FROM (" +
                            "SELECT UserFriend.friendUserID, MAX(reviewID) as recentReview " +
                            "FROM UserTable, UserFriend, Review " +
                            "WHERE UserFriend.friendUserID = UserTable.userID AND Review.userID = UserTable.userID AND UserFriend.userID = '" + userID + "' " +
                            "GROUP BY UserFriend.friendUserID) as mostRecent, UserTable, Business, Review " +
                            "WHERE mostRecent.friendUserID = UserTable.userID AND Review.reviewID = mostRecent.recentReview AND Review.businessID = Business.businessID;";
            executeQuery(sqlStr4, null, addCurrentUserFriendsReviews);
        }

        internal void UpdateUserFavoriteBusinesses(string userID)
        {
            userFavoriteBusinessesDataGrid.Items.Clear();
            
            string sqlStr2 = "SELECT Business.businessName, Business.stars, Business.city, Business.zip, Business.address, Business.businessID FROM Business, UserFavorite, UserTable WHERE UserTable.userID = '" + userID + "' AND " +
                "UserFavorite.userID = '" + userID + "' AND Business.businessID = UserFavorite.businessID;";
            executeQuery(sqlStr2, null, AddCurrentUserFavoriteBusinesses);
        }

        private void ClearCurrentUserInfoFields()
        {
            userNameTextBlock.Text = "";
            userStarsTextBlock.Text = "";
            userFansTextBlock.Text = "";
            userYelpingSinceTextBlock.Text = "";
            userLatTextBox.Text = "";
            userLongTextBox.Text = "";
            userFavoriteBusinessesDataGrid.Items.Clear();
            userFriendsDataGrid.Items.Clear();
            userFriendsReviewDataGrid.Items.Clear();
        }

        private void AddCurrentUserInformation(NpgsqlDataReader R)
        {
            userNameTextBlock.Text = R.GetString(0);
            userStarsTextBlock.Text = R.GetFloat(1).ToString();
            userYelpingSinceTextBlock.Text = R.GetDate(2).ToString();
            userLatTextBox.Text = R.GetFloat(3).ToString();
            userLongTextBox.Text = R.GetFloat(4).ToString();
            selectedUserLat = R.GetFloat(3).ToString();
            selectedUserLong = R.GetFloat(4).ToString();
            userFansTextBlock.Text = R.GetInt32(5).ToString();
        }

        private void AddCurrentUserFavoriteBusinesses(NpgsqlDataReader R)
        {
            userFavoriteBusinessesDataGrid.Items.Add(new Business() { businessName = R.GetString(0), stars = R.GetFloat(1), city = R.GetString(2), zip = R.GetInt32(3), address = R.GetString(4), businessID = R.GetString(5) });
        }

        private void addCurrentUserFriends(NpgsqlDataReader R)
        {
            userFriendsDataGrid.Items.Add(new User() { name = R.GetString(0), avgStars = R.GetFloat(1), yelpingSince = R.GetDateTime(2) });
        }

        private void addCurrentUserFriendsReviews(NpgsqlDataReader R)
        {
            userFriendsReviewDataGrid.Items.Add(new Review() { userName = R.GetString(0), businessName = R.GetString(1), city = R.GetString(2), text = R.GetString(3) });
        }

        private void UserFavoriteBusinessesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((userFavoriteBusinessesDataGrid.SelectedItem != null) && (userFavoriteBusinessesDataGrid.SelectedIndex > -1))
            {
                selectedFavoriteToRemove = userFavoriteBusinessesDataGrid.SelectedItem as Business;
            }
        }

        private void RemoveSelectedFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlStr = "DELETE FROM UserFavorite WHERE userID = '" + SelectedUserID + "' AND businessID = '" + selectedFavoriteToRemove.businessID + "';";
            executeQuery(sqlStr, null, null);
            UpdateUserFavoriteBusinesses(SelectedUserID);
        }

        private void UpdateLocationButton_Click(object sender, RoutedEventArgs e)
        {
            // Test to see if lat/long input is a number and can be parsed.
            int.TryParse(userLatTextBox.Text, out int latResult);
            int.TryParse(userLongTextBox.Text, out int longResult);
            
            // Input is not a number, prompt user to correct.
            if (latResult == 0 || longResult == 0)
            {
                Console.WriteLine("Error - User latitude or longitude input is not a number.");
                System.Windows.MessageBox.Show("Invalid Input: Please provide a floating point value for Latitude and Longitude");
            }

            // Input is a number, so parse to float.
            else
            {
                // Even though we provide the SQL query with a string, we need to put these values in decimal format.
                float latitude = float.Parse(userLatTextBox.Text);
                float longitude = float.Parse(userLongTextBox.Text);

                string sqlStr = "UPDATE UserTable SET latitude = '" + latitude.ToString() + "', " +
                        "longitude = '" + longitude.ToString() + "' WHERE userID = '" + SelectedUserID + "';";

                executeQuery(sqlStr, null, null);
            }
        }
    }
}
