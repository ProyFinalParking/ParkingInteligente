using ParkingInteligente.mvvm;
using ParkingInteligente.servicios;
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

namespace ParkingInteligente.dialogos
{
    /// <summary>
    /// Lógica de interacción para AñadirCliente.xaml
    /// </summary>
    public partial class AñadirCliente : Window
    {
        private AñadirClienteVM vm;

        public AñadirCliente()
        {
            InitializeComponent();
            vm = new AñadirClienteVM();
            this.DataContext = vm;

            FotoClienteImage.Source = vm.imagenPorDefecto();
        }

        private void FotoCliente_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FotoClienteImage.Source = vm.CargarImagen();
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            //Aqui llamamos al método para grabarlo en BD
            DialogResult = true;
        }
    }
}
