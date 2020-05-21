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
            public string name { get; set; }

            public string state { get; set; }

            public string city { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns2Grid();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milstone1b; password=marines182";
        }

        private void addStates()
        {
            // A DB is required to continue.   
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            businessGrid.Columns.Add(col3);

            businessGrid.Items.Add(new Business() { name = "business-1", state = "WA", city = "Pullman" });
            businessGrid.Items.Add(new Business() { name = "business-2", state = "CA", city = "Pasadena" });
            businessGrid.Items.Add(new Business() { name = "business-3", state = "NV", city = "Las Vegas" });
        }
    }
}
