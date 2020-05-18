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

namespace PaginaPrincipal
{
    /// <summary>
    /// Lógica de interacción para loginAd.xaml
    /// </summary>
    public partial class loginAd : Window
    {
        private String contraseña = "dani";
        



        public loginAd()
        {
            InitializeComponent();
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String intento = password.Password;


            if (intento == contraseña)
            {
                Administrador.admin = true;

                this.Hide();
            }
            else
            {
                Administrador.admin = false;
            }
        }

        private void Button_Click_OUT(object sender, RoutedEventArgs e)
        {
            Administrador.admin = false;
            this.Hide();
        }
    }
}
