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

        public Page CargarInicio()
        {
            return new Inicio();
        }

        public Page CargarControlVehiculos()
        {
            return new ControlVehiculos();
        }

        public Page CargarControlClientes()
        {
            return new ControlClientes();
        }

        public Page CargarControlAparcamientos()
        {
            return new ControlAparcamientos();
        }
    }
}
