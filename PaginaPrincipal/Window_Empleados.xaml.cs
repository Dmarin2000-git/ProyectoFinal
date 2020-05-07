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
using System.Data.SqlClient;

namespace PaginaPrincipal
{
    /// <summary>
    /// Lógica de interacción para Window_Empleados.xaml
    /// </summary>
    public partial class Window_Empleados : Window
    {
        public Window_Empleados()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Se añadirá un nuevo Empleado a la base de datos. ¿Desea continuar?", "EXIT", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    MainWindow conexion = new MainWindow();

                    SqlConnection conn = conexion.EstablecerConexion();
                    conn.Open();

                    if (String.IsNullOrEmpty(idEmpleado.Text) && String.IsNullOrEmpty(cargoAlmacen.Text) && String.IsNullOrEmpty(idAlmacen.Text) &&
                        String.IsNullOrEmpty(nombreEmpleado.Text) && String.IsNullOrEmpty(apellidoEmpleado.Text) && String.IsNullOrEmpty(DNIEmpleado.Text)
                        && String.IsNullOrEmpty(correoEmpleado.Text) && String.IsNullOrEmpty(direccionEmpleado.Text) && String.IsNullOrEmpty(telefonoEmpleado.Text))
                    {
                        MessageBox.Show("Para poder insertar un almacén es necesario rellenar todos los campos");
                    }
                    else
                    {
                        try
                        {

                            SqlCommand cmd = new SqlCommand();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            SqlCommand cmd1 = new SqlCommand();
                            SqlDataAdapter sda1 = new SqlDataAdapter(cmd);

                            String id = idEmpleado.Text;
                            String cargo = "'" + cargoAlmacen.Text + "'";
                            String numeroAlmacen = idAlmacen.Text;
                            String nombre = "'" + nombreEmpleado.Text + "'";
                            String apellido = "'" + apellidoEmpleado.Text + "'";
                            String DNI = "'" + DNIEmpleado.Text + "'";
                            String correo = "'" + correoEmpleado.Text + "'";
                            String direccion = "'" + direccionEmpleado.Text + "'";
                            String tel = telefonoEmpleado.Text;
                            String valor = "2";

                            String coma = ",";
                            /* String comilla = "'";*/

                            cmd.CommandText = "insert into Empleado values(" + id + coma + cargo + coma + numeroAlmacen + coma + nombre + ")";
                            /*String prueba = "insert into Empleado values(" + id + coma + cargo + coma + numeroAlmacen + coma + nombre + ")";
                            MessageBox.Show(prueba);*/

                            cmd1.CommandText = "insert into Persona values(" + DNI + coma + nombre + coma + apellido + coma + correo + coma + direccion + coma + tel + coma + valor + ")";
                            /*String prueba2 = "insert into Persona values(" + DNI + coma + nombre + coma + apellido + coma + correo + coma + direccion + coma + tel + coma + valor + ")";
                            MessageBox.Show(prueba2);*/

                            cmd1.Connection = conn;// primero ejecutamos esta tabla porque tiene menos probabilidad de fallar y así no se ensucia la bbdd
                            cmd1.ExecuteNonQuery();
                            cmd.Connection = conn; //creamos comando para para realizar el select
                            cmd.ExecuteNonQuery();
                           
                            MessageBox.Show("Tabla Empelado actualizada");

                            idEmpleado.Clear();
                            cargoAlmacen.Clear();
                            idAlmacen.Clear();
                            nombreEmpleado.Clear();
                            apellidoEmpleado.Clear();
                            DNIEmpleado.Clear();
                            correoEmpleado.Clear();
                            direccionEmpleado.Clear();
                            telefonoEmpleado.Clear();

                            conn.Close();

                        }
                        catch (SqlException ex) { MessageBox.Show(ex.ToString()); }
                    }

                    break;
                case MessageBoxResult.No:

                    break;
            }
        }

        private void Button_ClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

   
}
