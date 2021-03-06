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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingInteligente.ventanas
{
    /// <summary>
    /// Lógica de interacción para ControlClientes.xaml
    /// </summary>
    public partial class ControlClientes : UserControl
    {
        private ControlClientesVM vm;

        public ControlClientes()
        {
            InitializeComponent();
            vm = new ControlClientesVM();
            this.DataContext = vm;
        }
    }
}
