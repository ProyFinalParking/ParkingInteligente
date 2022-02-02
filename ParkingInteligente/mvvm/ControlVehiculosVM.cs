using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class ControlVehiculosVM : ObservableObject
    {
        private NavigationService servicio;

        public RelayCommand AñadirNuevoVehiculoCommand { get; }
        public RelayCommand EditarVehiculoCommand { get; }

        public ControlVehiculosVM()
        {
            servicio = new NavigationService();

            AñadirNuevoVehiculoCommand = new RelayCommand(AbrirDialogoNuevoVehiculo);
            EditarVehiculoCommand = new RelayCommand(AbrirDialogoEditarVehiculo);
        }

        private void AbrirDialogoNuevoVehiculo()
        {
            servicio.CargarDialogoAñadirVehiculo();
        }

        private void AbrirDialogoEditarVehiculo()
        {
            servicio.CargarDialogoEditarVehiculo();
        }

    }
}
