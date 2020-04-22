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

        public MainWindow() /*MAIN*/
        {
            InitializeComponent();

            abrirtablas(); //llamaos este metodo para la abrir en la pagina principal todas las tablas en sus respectivos datagrids

        }

        public SqlConnection EstablecerConexion()
        { /*ABRIR LA CONEXION CON LA BASE DE DATOS*/
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionddbb"].ConnectionString;
            return conn;
        }

        private void bindatagrid(String buscar, int indicador) /*HACER UN SELECT DE LAS TABLAS DE LA VENTANA PRINCIPAL*/
        {
            //throw new NotImplementedException();
            String search = buscar; //indica la tabla que quieres visualizar
            int indicador_grid = indicador; //indicador para diferenciar entre datagrids

            SqlConnection conn = EstablecerConexion();

            conn.Open(); //codigo para abrir la conexión con la base de datos
                         //hace referencia al App.conig donde tenemos las credenciales
                         //MessageBox.Show("connected"); 

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            //sda.Fill(dt); 

            switch (indicador_grid)
            { //utilizamos identificados, cada case es un datagrid 
                case 1:

                    cmd.CommandText = "Select * from [Article]";
                    cmd.Connection = conn; //creamos comando para para realizar el select
                    DataTable dt = new DataTable("Article");
                    sda.Fill(dt); //selecciona la tabla demandada
                    dataGrid1.ItemsSource = dt.DefaultView;
                    break;

                case 2:
                    cmd.CommandText = "Select * from [Almacenes]";
                    cmd.Connection = conn; //creamos comando para para realizar el select
                    DataTable dt1 = new DataTable("Almacenes");
                    sda.Fill(dt1); //selecciona la tabla demandada
                    dataGrid2.ItemsSource = dt1.DefaultView;
                    break;

                case 3:
                    cmd.CommandText = "Select * from Persona p inner join Empleado e on p.id = e.id ";
                    cmd.Connection = conn; //creamos comando para para realizar el select
                    DataTable dt2 = new DataTable("Persona");
                    sda.Fill(dt2); //selecciona la tabla demandada
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    break;
                case 4:
                    cmd.CommandText = "Select * from Persona p inner join Representante e on p.id = e.id ";
                    cmd.Connection = conn; //creamos comando para para realizar el select
                    DataTable dt3 = new DataTable("Persona");
                    sda.Fill(dt3); //selecciona la tabla demandada
                    dataGrid4.ItemsSource = dt3.DefaultView;
                    break;
            }
            //dataGrid2.ItemsSource = dt.DefaultView;
        }

        public void abrirtablas() /*METOD AL QUE LLAMA EL MAIN PARA QUE ABRA TODAS LAS TABLAS*/
        {
            //abrimos las tablas llamando al metodo bindatagrid
            bindatagrid("Article", 1);
            bindatagrid("Almacenes", 2);
            bindatagrid("Empleado", 3);
            bindatagrid("Proveedor", 4);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) /*LANZADOR DEL AM*/
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres lanzar una actualización masiva?", "AM", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    SqlConnection conn = EstablecerConexion(); //establecemos la conexión con la base de datos
                    conn.Open();
                    SqlCommand sql_cmnd1 = new SqlCommand("app_LoadArticles", conn); //creamos un comando para ejecutar procedures
                    sql_cmnd1.CommandType = CommandType.StoredProcedure;
                    sql_cmnd1.ExecuteNonQuery(); //ejecutamos el procedure que carga los articulos 
                    SqlCommand sql_cmnd2 = new SqlCommand("app_CountWarehouse", conn);
                    sql_cmnd2.CommandType = CommandType.StoredProcedure;
                    sql_cmnd2.ExecuteNonQuery();//ejecutamos el procedure que actualiza el stock de los almacenes
                    conn.Close();

                    abrirtablas(); //reabrimos la tablas para que se vean actualizadas

                    MessageBox.Show("ACTUALIZADO", "AM");

                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Operación cancelada", "AM");
                    break;

            }

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e) /*ELIMINAR FILA DE LA TABLA ARTICLE*/
        {
            SqlConnection conn = EstablecerConexion(); //establecemos conexion
            

            try
            {
                int delete_data = int.Parse(DeleteID.Text); //convertimos el textbox en int
                if (delete_data == 0)
                {
                    MessageBox.Show("EL VALOR NO PUEDE SER 0"); //capturamos excepcion manualmente
                }
                else
                {
                    SqlCommand command = new SqlCommand("DELETE Article where id =" + delete_data, conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("FILA ELIMINADA CON EL VALOR: " + delete_data);
                    bindatagrid("Article", 1);//actualizamos la tabla
                    conn.Close();
                    DeleteID.Clear();//limpimaos el textbox
                }

            }
            catch (Exception ex) { MessageBox.Show("RELLENA EL RECUADRO CON EL ID DEL ARTICULO QUE QUIERES ELIMINAR"); }
            //en el caso de que el text box esté vacio saltara esta excepcion junto con el mensaje 
        }

        private void ButtonTruncate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Se vaciará la tabla por completo. ¿Estás seguro de que quieres continuar?", "TR", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SqlConnection conn = EstablecerConexion();
                    SqlCommand command = new SqlCommand("TRUNCATE TABLE Article",conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("TABLA VACIADA", "TR");
                    bindatagrid("Article", 1);
                    conn.Close();
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("OPERACIÓN CANCELADA");
                    break;
            }
        }
    }
}
