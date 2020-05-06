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
using System.Windows.Shapes;

namespace PaginaPrincipal
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Se añadirá un nuevo Almacén en la base de datos. ¿Desea continuar?", "EXIT", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    MainWindow conexion = new MainWindow();

                    SqlConnection conn = conexion.EstablecerConexion();
                    conn.Open();

                    if (String.IsNullOrEmpty(idAlmacen.Text) && String.IsNullOrEmpty(nombreAlmacen.Text) && String.IsNullOrEmpty(stockAlmacen.Text) &&
                        String.IsNullOrEmpty(direccionAlmacen.Text) && String.IsNullOrEmpty(empleadosAlmacen.Text))
                    {
                        MessageBox.Show("Para poder insertar un almacén, es necesario rellenar todos los campos.");
                    }
                    else {

                        try
                        {

                            SqlCommand cmd = new SqlCommand();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);

                            String id = idAlmacen.Text;
                            String nombre = nombreAlmacen.Text;
                            String direccion = direccionAlmacen.Text;
                            String stock = stockAlmacen.Text;
                            String empleados = empleadosAlmacen.Text;
                            String coma = ",";
                            String comilla = "'";

                            cmd.CommandText = "insert into Almacenes values(" + id + coma + comilla + nombre + comilla + coma + stock + coma + comilla + direccion + comilla + coma + empleados + ")";
                            //String prueba = "insert into Almacenes values(" + id + coma + comilla + nombre + comilla + coma + stock + coma + comilla + direccion + comilla + coma + empleados+")";
                            //MessageBox.Show(prueba);

                            cmd.Connection = conn; //creamos comando para para realizar el select
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Tabla Almacenes actualizada");

                            idAlmacen.Clear();
                            nombreAlmacen.Clear();
                            direccionAlmacen.Clear();
                            stockAlmacen.Clear();
                            empleadosAlmacen.Clear();

                        }
                        catch (SqlException ex) { MessageBox.Show(ex.ToString()); }
                    }

                    break;
                case MessageBoxResult.No:

                    break;
            }
        }
    }
}
