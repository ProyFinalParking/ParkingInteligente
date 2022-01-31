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
    }
}
