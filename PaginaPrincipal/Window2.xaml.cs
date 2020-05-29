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
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void AddProvider_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Se añadirá un nuevo Empleado a la base de datos. ¿Desea continuar?", "EXIT", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    MainWindow conexion = new MainWindow();

                    SqlConnection conn = conexion.EstablecerConexion();
                    conn.Open();

                    if (String.IsNullOrEmpty(idProvider.Text) && String.IsNullOrEmpty(direccionProvider.Text) && String.IsNullOrEmpty(nameProvider.Text) &&
                        String.IsNullOrEmpty(nameContact.Text) && String.IsNullOrEmpty(secondContact.Text) && String.IsNullOrEmpty(DNIContact.Text)
                        && String.IsNullOrEmpty(correoContact.Text) && String.IsNullOrEmpty(direccionContact.Text) && String.IsNullOrEmpty(telefonoContact.Text))
                    {
                        MessageBox.Show("Para poder insertar un almacén es necesario rellenar todos los campos");
                    }
                    else
                    {
                        try
                        {

                            SqlCommand cmd = new SqlCommand();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            cmd.Connection = conn;// primero ejecutamos esta tabla porque tiene menos probabilidad de fallar y así no se ensucia la bbdd

                            String id = idProvider.Text;
                            String direcProv = "'" + direccionProvider.Text + "'";
                            String nombreProveedor = "'" + nameProvider.Text + "'";
                            String nombre = "'" + nameContact.Text + "'";
                            String apellido = "'" + secondContact.Text + "'";
                            String DNI = "'" + DNIContact.Text + "'";
                            String correo = "'" + correoContact.Text + "'";
                            String direccion = "'" + direccionContact.Text + "'";
                            String tel = telefonoContact.Text;
                            String valor = "1";

                            String coma = ",";
                            /* String comilla = "'";*/

                            cmd.CommandText = "insert into Proveedor values(" + id + coma + direcProv + coma + nombreProveedor+ ")";
                            String prueba = "insert into Proveedor values(" + id + coma + direcProv + coma + nombreProveedor + ")";
                            //MessageBox.Show(prueba);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "insert into Representante values(" + id + coma + nombre+ ")";
                            String prueba3 = "insert into Representantes values(" + id + coma + nombre + ")";
                            //MessageBox.Show(prueba3);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "insert into Persona values(" + DNI + coma + nombre + coma + apellido + coma + correo + coma + direccion + coma + tel + coma + valor + ")";
                            String prueba2 = "insert into Persona values(" + DNI + coma + nombre + coma + apellido + coma + correo + coma + direccion + coma + tel + coma + valor + ")";
                            //MessageBox.Show(prueba2);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Tabla Proveedores actualizada");
                            conexion.abrirtablas();
                            conn.Close();

                            idProvider.Clear();
                            direccionProvider.Clear();
                            nameProvider.Clear();
                            secondContact.Clear();
                            nameContact.Clear();
                            DNIContact.Clear();
                            correoContact.Clear();
                            direccionContact.Clear();
                            telefonoContact.Clear();
                            this.Close();

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
