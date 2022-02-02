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
    /// Lógica de interacción para VerCliente.xaml
    /// </summary>
    public partial class VerCliente : Window
    {
        private VerClienteVM vm;
        public VerCliente()
        {
            InitializeComponent();
            vm = new VerClienteVM();
            this.DataContext = vm;
        }
    }
}
