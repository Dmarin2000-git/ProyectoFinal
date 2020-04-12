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

            abrirtablas(); //llamaos este metodo para la abrir en la pagina principal todas las tablas en sus respectivos datagrids

        }

        private void bindatagrid(String buscar, int indicador)
        {
            //throw new NotImplementedException();
            String search = buscar; //indica la tabla que quieres visualizar
            int indicador_grid = indicador; //indicador para diferenciar entre datagrids

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionddbb"].ConnectionString;
            conn.Open(); //codigo para abrir la conexión con la base de datos
            //hace referencia al App.conig donde tenemos las credenciales
            //MessageBox.Show("connected");

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            
            //sda.Fill(dt); 

            switch (indicador_grid) { //utilizamos identificados, cada case es un datagrid 
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

        public void abrirtablas() {
            //abrimos las tablas llamando al metodo bindatagrid
            bindatagrid("Article", 1);
            bindatagrid("Almacenes", 2);
            bindatagrid("Empleado", 3);
            bindatagrid("Proveedor", 4);
        }

        
    }
}
