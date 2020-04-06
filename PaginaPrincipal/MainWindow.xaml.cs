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

namespace PaginaPrincipal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            
        }

        public void showDDBB() {
            try {
                SqlConnection conn = new SqlConnection("Server=(local);Database=WA317;Uid=miau;Password=miau;");
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
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            showDDBB();
        }

        private void DataGrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
