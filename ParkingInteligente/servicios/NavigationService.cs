using ParkingInteligente.dialogos;
using ParkingInteligente.ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ParkingInteligente.servicios
{
    class NavigationService
    {
        public NavigationService()
        {

        }

        public UserControl CargarInicio()
        {
            return new Inicio();
        }

        public UserControl CargarControlVehiculos()
        {
            return new ControlVehiculos();
        }

        public UserControl CargarControlClientes()
        {
            return new ControlClientes();
        }

        public UserControl CargarControlAparcamientos()
        {
            return new ControlAparcamientos();
        }

        public bool? CargarDialogoAñadirCliente()
        {
            AñadirCliente dialogo = new AñadirCliente();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEditarCliente()
        {
            EditarCliente dialogo = new EditarCliente();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoAñadirVehiculo()
        {
            AñadirVehiculo dialogo = new AñadirVehiculo();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEditarVehiculo()
        {
            EditarVehiculo dialogo = new EditarVehiculo();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoVerVehiculo()
        {
            VerVehiculo dialogo = new VerVehiculo();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoVerCliente()
        {
            VerCliente dialogo = new VerCliente();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoPagarFinalizarParking()
        {
            PagarEstacionamiento dialogo = new PagarEstacionamiento();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEliminarCliente()
        {
            EliminarCliente dialogo = new EliminarCliente();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEliminarVehiculo()
        {
            EliminarVehiculo dialogo = new EliminarVehiculo();
            dialogo.Owner = Application.Current.MainWindow;
            return dialogo.ShowDialog();
        }
    }
}
