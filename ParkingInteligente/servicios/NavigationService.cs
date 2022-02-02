using ParkingInteligente.dialogos;
using ParkingInteligente.ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEditarCliente()
        {
            EditarCliente dialogo = new EditarCliente();
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoAñadirVehiculo()
        {
            AñadirVehiculo dialogo = new AñadirVehiculo();
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoEditarVehiculo()
        {
            EditarVehiculo dialogo = new EditarVehiculo();
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoVerVehiculo()
        {
            VerVehiculo dialogo = new VerVehiculo();
            return dialogo.ShowDialog();
        }

        public bool? CargarDialogoVerCliente()
        {
            VerCliente dialogo = new VerCliente();
            return dialogo.ShowDialog();
        }
    }
}
