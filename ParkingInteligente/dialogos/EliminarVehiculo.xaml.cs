using ParkingInteligente.mvvm;
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
    /// Lógica de interacción para EliminarVehiculo.xaml
    /// </summary>
    public partial class EliminarVehiculo : Window
    {
        private EliminarVehiculoVM vm;

        public EliminarVehiculo()
        {
            InitializeComponent();
            vm = new EliminarVehiculoVM();
            this.DataContext = vm;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            //Aqui llamamos al método para grabarlo en BD
            DialogResult = true;
        }
    }
}
