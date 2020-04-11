using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Configuration;

namespace PaginaPrincipal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            bindatagrid();

        }

        private void bindatagrid()
        {
            //throw new NotImplementedException();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionddbb"].ConnectionString;
            conn.Open();
            MessageBox.Show("connected");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from [Almacenes]";
            cmd.Connection = conn;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Almacenes");
            sda.Fill(dt);

            dataGrid1.ItemsSource = dt.DefaultView;
        }

        /*public void showDDBB() {
            try {
                SqlConnection conn = new SqlConnection("Server=(local);Database=WA317; Integrated Security = true;");
                conn.Open();
                MessageBox.Show("connected");

                string sql = "SELECT * FROM Almacenes";

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("emp");
                sda.Fill(dt);

                dataGrid1.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }*/

        
    }
}
