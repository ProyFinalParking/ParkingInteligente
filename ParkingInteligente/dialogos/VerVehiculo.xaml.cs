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
    /// Lógica de interacción para VerVehiculo.xaml
    /// </summary>
    public partial class VerVehiculo : Window
    {
        private VerVehiculoVM vm;
        public VerVehiculo()
        {
            InitializeComponent();
            vm = new VerVehiculoVM();
            this.DataContext = vm;
        }
    }
}
