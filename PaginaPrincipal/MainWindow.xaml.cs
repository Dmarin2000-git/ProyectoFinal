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

        public SqlConnection EstablecerConexion()/*ABRIR LA CONEXION CON LA BASE DE DATOS*/
        { 
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
                    cmd.CommandText = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2 ";
                    cmd.Connection = conn; //creamos comando para para realizar el select
                    DataTable dt2 = new DataTable("Empleado");
                    sda.Fill(dt2); //selecciona la tabla demandada
                    dataGrid3.ItemsSource = dt2.DefaultView;
                    break;
                case 4:
                    cmd.CommandText = "select p.*,r.nombre as contacto, pp.apellido,pp.DNI, pp.direccion,pp.correo,pp.telefono from Proveedor p inner join Representante r on p.id = r.id_prov " +
                        "inner join Persona pp on pp.nombre = r.nombre";
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

        private void Button_Click_LoadArtcile (object sender, RoutedEventArgs e) /*FORZAR CARGA DE DATOS*/
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres forzar una carga de datos?", "FC", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try {

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

                        MessageBox.Show("ACTUALIZADO", "FC");

                    } catch (Exception ex) { MessageBox.Show("No hay información el fichero"); }
                    
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Operación cancelada", "FC");
                    break;

            }

        }

        private void backup() {

            SqlConnection conn = EstablecerConexion(); //establecemos la conexión con la base de datos
            conn.Open();
            SqlCommand sql_cmnd1 = new SqlCommand("app_backUpSystem", conn); //creamos un comando para ejecutar procedures
            sql_cmnd1.CommandType = CommandType.StoredProcedure;
            conn.Close();

            //abrirtablas(); //no hace falta ya que no mostramos las tablas del backup

            MessageBox.Show("COPIA DE SEGURIDAD REALIZADA", "BK");
        }

        private void Button_Click_BackUp(object sender, RoutedEventArgs e) /*LANZADOR DEL BACKUPS*/
        {
            MessageBoxResult result = MessageBox.Show("Va a realizar una copia de seguridad de todas las tablas.¿Desea continuar?", "BK", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    backup();

                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Operación cancelada", "BK");
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
                    MessageBox.Show("FILA ELIMINADA");
                    bindatagrid("Article", 1);//actualizamos la tabla
                    conn.Close();
                    DeleteID.Clear();//limpimaos el textbox
                }

            }
            catch (Exception ex) { MessageBox.Show("RELLENA EL RECUADRO CON EL ID DEL ARTICULO QUE QUIERES ELIMINAR"); }
            //en el caso de que el text box esté vacio saltara esta excepcion junto con el mensaje 
        }

        private void ButtonDelete_Click_W(object sender, RoutedEventArgs e) /*ELIMINAR FILA TABLA ALMACENES*/
        {
            SqlConnection conn = EstablecerConexion(); //establecemos conexion

            try
            {
                int delete_data = int.Parse(DeleteID1.Text); //convertimos el textbox en int
                if (delete_data == 0)
                {
                    MessageBox.Show("EL VALOR NO PUEDE SER 0"); //capturamos excepcion manualmente
                }
                else
                {
                    SqlCommand command = new SqlCommand("DELETE Almacenes where id =" + delete_data, conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("FILA ELIMINADA");
                    bindatagrid("Almacenes", 2);//actualizamos la tabla
                    conn.Close();
                    DeleteID1.Clear();//limpimaos el textbox
                }

            }
            catch (Exception ex) { MessageBox.Show("RELLENA EL RECUADRO CON EL ID DEL ARTICULO QUE QUIERES ELIMINAR"); }
            //en el caso de que el text box esté vacio saltara esta excepcion junto con el mensaje 
        }

        private void Button_UpdateWarehouse(object sender, RoutedEventArgs e) /*ACTUALIZAR STOCK Y EMPLEADOS DE LOS ALMACENES*/
        {
            SqlConnection conn = EstablecerConexion();
            conn.Open();
            SqlCommand sql_cmnd2 = new SqlCommand("app_CountWarehouse", conn);
            sql_cmnd2.CommandType = CommandType.StoredProcedure;
            sql_cmnd2.ExecuteNonQuery();
            MessageBox.Show("Stock y Empleados de cada almacen actualizados");
            conn.Close();
            bindatagrid("Almacenes", 2);
        }

        private void ButtonDelete_Click_E(object sender, RoutedEventArgs e) /*ELIMINAR FILA TABLA EMPLEADOS*/
        {
            SqlConnection conn = EstablecerConexion(); //establecemos conexion

            try
            {
                String delete_data = DeleteID2.Text; //convertimos el textbox en int
                if (String.IsNullOrEmpty(DeleteID2.Text))
                {
                    MessageBox.Show("Introduzca un nombre"); //capturamos excepcion manualmente
                }
                else
                {
                    SqlCommand command = new SqlCommand("DELETE Empleado where nombre =" + delete_data, conn);
                    SqlCommand command1 = new SqlCommand("DELETE Persona where nombre =" + delete_data, conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("FILA ELIMINADA");
                    bindatagrid("Empleado", 3);//actualizamos la tabla
                    conn.Close();
                    DeleteID2.Clear();//limpimaos el textbox
                }

            }
            catch (Exception ex) { MessageBox.Show("RELLENA EL RECUADRO CON EL ID DEL ARTICULO QUE QUIERES ELIMINAR"); }
            //en el caso de que el text box esté vacio saltara esta excepcion junto con el mensaje 
        }

        private void ButtonDelete_Click_P(object sender, RoutedEventArgs e) /*ELIMINAR FILA TABLA PROVEEDORES*/
        {
            SqlConnection conn = EstablecerConexion(); //establecemos conexion

            try
            {
                int delete_data = int.Parse(DeleteID3.Text);
                String delete_data1 = DeleteID4.Text; //convertimos el textbox en int
                if (String.IsNullOrEmpty(DeleteID3.Text) || String.IsNullOrEmpty(DeleteID4.Text))
                {
                    MessageBox.Show("Introduzca los valores"); //capturamos excepcion manualmente
                }
                else
                {
                    SqlCommand command = new SqlCommand("DELETE Proveedor where id =" + delete_data, conn);
                    SqlCommand command1 = new SqlCommand("DELETE Representante where nombre =" + delete_data1, conn);
                    SqlCommand command2 = new SqlCommand("DELETE Persona where nombre =" + delete_data1, conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("FILA ELIMINADA ");
                    bindatagrid("Proveedores", 4);//actualizamos la tabla
                    conn.Close();
                    DeleteID3.Clear();//limpimaos el textbox
                    DeleteID4.Clear();
                }

            }
            catch (Exception ex) { MessageBox.Show("RELLENA EL RECUADRO CON EL ID DEL ARTICULO QUE QUIERES ELIMINAR"); }
        }


        private void ButtonTruncate_Click(object sender, RoutedEventArgs e) /*VACIAR TABLA ENTERA ARTICLE*/
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

        private void ButtonTruncateW_Click(object sender, RoutedEventArgs e) /*VACIAR TABLA ENTERA WAREHOUSE*/
        {
            MessageBoxResult result = MessageBox.Show("Se vaciará la tabla por completo. ¿Estás seguro de que quieres continuar?", "TW", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SqlConnection conn = EstablecerConexion();
                    SqlCommand command = new SqlCommand("TRUNCATE TABLE Almacenes", conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("TABLA VACIADA", "TW");
                    bindatagrid("Almacenes", 2);
                    conn.Close();
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("OPERACIÓN CANCELADA, TW");
                    break;
            }
        }

        private void ButtonTruncateE_Click(object sender, RoutedEventArgs e) /*VACIAR TABLA ENTERA EMPLOYEES*/
        {
            MessageBoxResult result = MessageBox.Show("Se vaciará la tabla por completo. ¿Estás seguro de que quieres continuar?", "TE", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SqlConnection conn = EstablecerConexion();
                    SqlCommand command = new SqlCommand("delete persona where variante = 2", conn);//creamos comando
                    SqlCommand command1 = new SqlCommand("truncate table empleados ", conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    command1.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("TABLA VACIADA", "TE");
                    bindatagrid("Empleados", 2);
                    conn.Close();
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("OPERACIÓN CANCELADA, TE");
                    break;
            }
        }

        private void ButtonTruncateP_Click(object sender, RoutedEventArgs e) /*VACIAR TABLA PROVEEDORES*/
        {
            MessageBoxResult result = MessageBox.Show("Se vaciará la tabla por completo. ¿Estás seguro de que quieres continuar?", "TP", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SqlConnection conn = EstablecerConexion();
                    SqlCommand command = new SqlCommand("delete persona where variante = 1", conn);//creamos comando
                    SqlCommand command1 = new SqlCommand("truncate table representante ", conn);//creamos comando
                    SqlCommand command2 = new SqlCommand("truncate table proveedor ", conn);//creamos comando
                    command.Connection.Open();
                    command.ExecuteNonQuery();//ejecutamos comando
                    command1.ExecuteNonQuery();//ejecutamos comando
                    command2.ExecuteNonQuery();//ejecutamos comando
                    MessageBox.Show("TABLA VACIADA", "TP");
                    bindatagrid("Empleados", 2);
                    conn.Close();
                    break;

                case MessageBoxResult.No:
                    MessageBox.Show("OPERACIÓN CANCELADA, TP");
                    break;
            }
        }

        private void Button_Click_Load(object sender, RoutedEventArgs e) /*BOTON DE SEGURIDAD SI DESAPARECEN TABLAS VOLVER A CARGARLAS*/
        {
            abrirtablas(); //abrir las tablas de nuevo
        }

        
        private void BoxArticle_Selected(object sender, RoutedEventArgs e) /*ABRIR TABLA EN EL FILTRO DE ARTICLE*/
        {
            SqlConnection conn = EstablecerConexion();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            cmd.CommandText = "Select * from [Article]";
            cmd.Connection = conn; //creamos comando para para realizar el select
            DataTable dt = new DataTable("Article");
            sda.Fill(dt); //selecciona la tabla demandada
            DataGridFilter.ItemsSource = dt.DefaultView;
           
        }

        private void BoxProveedores_Selected(object sender, RoutedEventArgs e) /*ABRIR TABLA FILTRO PROVEEDORES */
        {
            SqlConnection conn = EstablecerConexion();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            cmd.CommandText = "select p.*,r.nombre as contacto, pp.apellido,pp.DNI, pp.direccion,pp.correo,pp.telefono from Proveedor p inner join Representante r on p.id = r.id_prov " +
                        "inner join Persona pp on pp.nombre = r.nombre"; 
            cmd.Connection = conn; //creamos comando para para realizar el select
            DataTable dt = new DataTable("Proveedores");
            sda.Fill(dt); //selecciona la tabla demandada
            DataGridFilter.ItemsSource = dt.DefaultView;
        }

        private void BoxEmpleados_Selected(object sender, RoutedEventArgs e) /*ABRIR TABLA FILTRO EMPLEADOS*/
        {
            SqlConnection conn = EstablecerConexion();
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            cmd.CommandText = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2";
            cmd.Connection = conn; //creamos comando para para realizar el select
            DataTable dt = new DataTable("Proveedores");
            sda.Fill(dt); //selecciona la tabla demandada
            DataGridFilter.ItemsSource = dt.DefaultView;
        }

        private void Button_Click_AFilter(object sender, RoutedEventArgs e) /*FILTRO ARTICLE*/
        {
            //definimos la condición del comando
            String condicion = ""; //este sera el select
            String and = " and "; // union 
            int avisador = 0; // indice para saber cuando no se ha seleccionado ningun filtro

            int idprov= 0;
            String idprString = null; //igualamos los valores de los text box en variables

            String art_code = null;

            int id_alm = 0;
            String id_almSTring = null;

            double cost;
            String costString;

            if (string.IsNullOrWhiteSpace(codefilter.Text)) 
            {
                //Si esta vacio el avisador suma
                avisador++;
            }
            else 
            {
                //si no esta vacio hace el select con la condicion del textbox
                art_code = codefilter.Text;
                condicion = "SELECT * FROM Article WHERE art_code ="+"'"+art_code+"'";
            }

            if (string.IsNullOrWhiteSpace(almfilter.Text))
            {
                //MessageBox.Show("Alm esta vacio");
                avisador++;
            }
            else {
                id_alm = int.Parse(almfilter.Text);
                id_almSTring = id_alm.ToString();

                if (string.IsNullOrWhiteSpace(codefilter.Text)) // si el filtro anterior a este esta vacio crea de nuevo el select con el filtro de este textbox
                {
                    condicion = "SELECT * FROM Article WHERE id_alm ="+id_alm;
                }
                else { condicion += and + "id_alm = " + id_alm; } //si no está vacio añade tu condicion a la cola
            }

            if (string.IsNullOrWhiteSpace(provfilter.Text))
            {
                //MessageBox.Show("Prov esta vacio");
                avisador++;
            }
            else {
                idprov = int.Parse(provfilter.Text);
                idprString = idprov.ToString();

                if (string.IsNullOrWhiteSpace(almfilter.Text) && string.IsNullOrWhiteSpace(codefilter.Text))
                {
                    condicion = "SELECT * FROM Article WHERE id_prov =" + idprov;
                }
                else { condicion += and + "id_prov = " + idprov; }

            }

            if (string.IsNullOrWhiteSpace(costfilter.Text))
            {
                //MessageBox.Show("Cost esta vacio");
                avisador++;
            }
            else 
            {
                cost = double.Parse(costfilter.Text);
                costString = cost.ToString();

                if (string.IsNullOrWhiteSpace(almfilter.Text) && string.IsNullOrWhiteSpace(codefilter.Text) && (string.IsNullOrWhiteSpace(almfilter.Text)))
                {
                    condicion = "SELECT * FROM Article WHERE precio =" + cost;
                }
                else { condicion = and + "precio = " + cost; }
                
            }


            //MessageBox.Show(condicion);
            SqlConnection conn = EstablecerConexion();
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            if (avisador == 4) // si el avisador es 4 no se ha seleccionado ningún filtro
            {
                MessageBox.Show("Introduzca algún filtro"); 
            }
            else {
                MessageBox.Show(condicion);
                cmd.CommandText =  condicion;
                cmd.Connection = conn; //creamos comando para para realizar el select
                DataTable dt = new DataTable("Article");
                sda.Fill(dt); //selecciona la tabla demandada
                DataGridFilter.ItemsSource = dt.DefaultView;
                conn.Close();
            }

        }

        private void Button_Click_WFilter(object sender, RoutedEventArgs e) /*FILTRO DE EMPLEADOS*/
        {
            String condicion = "";
            String and = " and ";
            int avisador = 0;

            String cargo = null;
            String nombre = null;
            String apellido = null;

            int id_alm = 0;
            String id_almSTring = null;

            if (String.IsNullOrEmpty(cargofilter.Text)) {
                 avisador ++;
            }
            else {
                cargo = cargofilter.Text;
                condicion = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2"+and+ "cargo = "+"'"+cargo+"'";
            }

            if (String.IsNullOrEmpty(almidfilter.Text))
            {
                avisador++;
            }
            else {
                id_alm = int.Parse(almidfilter.Text);
                id_almSTring = id_alm.ToString();

                if (string.IsNullOrWhiteSpace(cargofilter.Text))
                {
                    condicion = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2" + and + "e.almacenID= "+ id_almSTring;
                }
                else { condicion += and + "e.almacenID = " + id_almSTring; }
            }

            if (String.IsNullOrWhiteSpace(nombrefilter.Text))
            {
                avisador++;
            }
            else {
                nombre = nombrefilter.Text;

                if (String.IsNullOrEmpty(cargofilter.Text) && String.IsNullOrEmpty(almfilter.Text))
                {
                    condicion = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2" + and + "p.nombre = " + "'"+nombre+"'";
                }
                else {
                    condicion += and + "p.nombre = " + "'"+nombre+"'";
                }
            }

            if (String.IsNullOrEmpty(apellidofilter.Text))
            {
                avisador++;
            }
            else {
                apellido = apellidofilter.Text;
                if (String.IsNullOrEmpty(cargofilter.Text) && String.IsNullOrEmpty(almfilter.Text) && String.IsNullOrEmpty(nombrefilter.Text))
                {
                    condicion = "select e.*, p.apellido, p.DNI, p.direccion, p.telefono, p.correo from empleado e inner join persona p on e.nombre = p.nombre where p.variante = 2" + and + "p.apellido = " + "'"+apellido+"'";
                }
                else {
                        condicion += and + "p.apellido = " + "'"+apellido+"'";
                }
            }
            //MessageBox.Show(condicion);
            SqlConnection conn = EstablecerConexion();
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            if (avisador == 4)
            {
                MessageBox.Show("Introduzca algún filtro");
            }
            else
            {
                MessageBox.Show(condicion);
                cmd.CommandText = condicion;
                cmd.Connection = conn; //creamos comando para para realizar el select
                DataTable dt = new DataTable("Empleado");
                sda.Fill(dt); //selecciona la tabla demandada
                DataGridFilter.ItemsSource = dt.DefaultView;
                conn.Close();
            }

        }

        private void Button_Click_PFilter(object sender, RoutedEventArgs e) /*FILTRO PARA PROVEEDORES*/
        {
            String condicion = ""; 
            String and = " and ";
            int avisador = 0;

            String nom_prov = null;
            String nom_repre = null;
            String apellido_repre = null;

            if (String.IsNullOrEmpty(provnamefilter.Text))
            {
                avisador++;
            }
            else {
                nom_prov = provnamefilter.Text;
                condicion = "select p.*,r.nombre as contacto, pp.apellido,pp.DNI, pp.direccion,pp.correo,pp.telefono from Proveedor p inner join Representante r on p.id = r.id_prov " +
                        "inner join Persona pp on pp.nombre = r.nombre" + and + "p.prov_name = " + "'" + nom_prov + "'";
            }

            if (String.IsNullOrEmpty(reprenamefilter.Text))
            {
                avisador++;
            }
            else
            {
                nom_repre = reprenamefilter.Text;
                if (String.IsNullOrEmpty(provnamefilter.Text))
                {
                    condicion = "select p.*,r.nombre as contacto, pp.apellido,pp.DNI, pp.direccion,pp.correo,pp.telefono from Proveedor p inner join Representante r on p.id = r.id_prov " +
                        "inner join Persona pp on pp.nombre = r.nombre" + and + "pp.nombre = " + "'" + nom_repre + "'";
                }
                else
                {
                    condicion += and + "pp.nombre = " + "'" + nom_repre + "'";
                }
            }

                if (String.IsNullOrEmpty(represecondfilter.Text))
                {
                    avisador++;
                }
                else
                {
                    apellido_repre = represecondfilter.Text;
                    if (String.IsNullOrEmpty(cargofilter.Text) && String.IsNullOrEmpty(almfilter.Text))
                    {
                        condicion = "select p.*,r.nombre as contacto, pp.apellido,pp.DNI, pp.direccion,pp.correo,pp.telefono from Proveedor p inner join Representante r on p.id = r.id_prov " +
                        "inner join Persona pp on pp.nombre = r.nombre" + and + "pp.apellido = " + "'" + apellido_repre + "'";
                    }
                    else
                    {
                        condicion += and + "pp.apellido = " + "'" + apellido_repre + "'";
                    }

                }

            //MessageBox.Show(condicion);
            SqlConnection conn = EstablecerConexion();
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            if (avisador == 4)
            {
                MessageBox.Show("Introduzca algún filtro");
            }
            else
            {
                MessageBox.Show(condicion);
                cmd.CommandText = condicion;
                cmd.Connection = conn; //creamos comando para para realizar el select
                DataTable dt = new DataTable("Proveedor");
                sda.Fill(dt); //selecciona la tabla demandada
                DataGridFilter.ItemsSource = dt.DefaultView;
                conn.Close();
            }
        }

        private void Button_Click_Restore(object sender, RoutedEventArgs e) /*RESTAURAR LAS TABLAS DEL ULTIMO BACKUP*/
        {
            MessageBox.Show("Se restablecerán las tablas de la última copia de seguridad. ¿Desea continuar?");
            SqlConnection conn = EstablecerConexion(); //establecemos la conexión con la base de datos
            conn.Open();
            SqlCommand sql_cmnd1 = new SqlCommand("app_restoreTables", conn); //creamos un comando para ejecutar procedures
            sql_cmnd1.CommandType = CommandType.StoredProcedure;
            MessageBox.Show("Tablas restablecidas");
            conn.Close();
        }

        private void Button_Click_AM(object sender, RoutedEventArgs e) /*DESCARGA Y CARGA DE DATOS EN LA BASE DE DATOS*/
        {
            MessageBoxResult result = MessageBox.Show("Antes de cada actualización masiva se realiza una copia de seguridad. ¿Desea continuar?", "AM", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        backup(); //antes de nada realizamos una copia de seguridad

                        SqlConnection conn = EstablecerConexion(); //establecemos la conexión con la base de datos
                        conn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("app_OutStock", conn); //primero hacemos las salidas
                        sql_cmnd.CommandType = CommandType.StoredProcedure;
                        sql_cmnd.ExecuteNonQuery();
                        SqlCommand sql_cmnd1 = new SqlCommand("app_LoadArticles", conn); //por segundo entramos todos los nuevos
                        sql_cmnd1.CommandType = CommandType.StoredProcedure;
                        sql_cmnd1.ExecuteNonQuery(); //ejecutamos el procedure que carga los articulos 
                        SqlCommand sql_cmnd2 = new SqlCommand("app_CountWarehouse", conn);
                        sql_cmnd2.CommandType = CommandType.StoredProcedure;
                        sql_cmnd2.ExecuteNonQuery();//ejecutamos el procedure que actualiza el stock de los almacenes
                        conn.Close();

                        abrirtablas(); //reabrimos la tablas para que se vean actualizadas

                        MessageBox.Show("ACTUALIZADO", "AM");

                    }
                    catch (Exception ex) { MessageBox.Show("No hay información el fichero"); }

                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Operación cancelada", "AM");
                    break;

            }
        }

        private void Button_Click_OutArtcile(object sender, RoutedEventArgs e) /*SALIDA DE ARTICLES*/
        {
            MessageBoxResult result = MessageBox.Show("Se forzaran la salida de lso articulos ¿Desea continuar?", "EXIT", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        
                        SqlConnection conn = EstablecerConexion(); //establecemos la conexión con la base de datos
                        conn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("app_OutStock", conn); //hacemos las salidas
                        sql_cmnd.CommandType = CommandType.StoredProcedure;
                        sql_cmnd.ExecuteNonQuery();
                        SqlCommand sql_cmnd2 = new SqlCommand("app_CountWarehouse", conn);
                        sql_cmnd2.CommandType = CommandType.StoredProcedure;
                        sql_cmnd2.ExecuteNonQuery();//ejecutamos el procedure que actualiza el stock de los almacenes

                        conn.Close();

                        abrirtablas(); //reabrimos la tablas para que se vean actualizadas

                        MessageBox.Show("ACTUALIZADO", "EXIT");

                    }
                    catch (Exception ex) { MessageBox.Show("No hay información el fichero"); }

                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Operación cancelada", "EXIT");
                    break;

            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e) /*SALIR DE LA APLIACION*/
        {
            MessageBoxResult result = MessageBox.Show("¿Seguro que desea salir de la aplicación?", "EXIT", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close(); 
                break;
                case MessageBoxResult.No:
                    
                    break;
            }
        }

        private void Button_AddWarehouse(object sender, RoutedEventArgs e) /*AÑADIR ALMACENES*/
        {
            Window1 mostrar = new Window1();

            mostrar.Show();
        }

        private void Button_AddProvider(object sender, RoutedEventArgs e)
        {

            Window_Empleados mostrar = new Window_Empleados();
            mostrar.Show();
        }

    }
}
