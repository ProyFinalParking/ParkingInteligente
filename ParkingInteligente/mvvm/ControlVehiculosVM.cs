using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.modelo;
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
        public RelayCommand VerVehiculoCommand { get; }


        private Vehiculo vehiculoSeleccionado;
        public Vehiculo VehiculoSeleccionado
        {
            get { return vehiculoSeleccionado; }
            set { SetProperty(ref vehiculoSeleccionado, value); }
        }

        private List<Vehiculo> vehiculos;
        public List<Vehiculo> Vehiculos
        {
            get
            {
                return vehiculos;
            }
            set
            {
                SetProperty(ref vehiculos, value);
            }
        }

        public ControlVehiculosVM()
        {
            Vehiculos = ServicioDB.GetListVehicles();
            servicio = new NavigationService();
            VehiculoSeleccionado = new Vehiculo();

            WeakReferenceMessenger.Default.Register<ControlVehiculosVM, VehiculoSeleccionadoRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.VehiculoSeleccionado);
            });

            WeakReferenceMessenger.Default.Register<ActualizarGridVehiculosMessage>(this, (r, m) =>
            {
                Vehiculos = m.Value;
            });

            AñadirNuevoVehiculoCommand = new RelayCommand(AbrirDialogoNuevoVehiculo);
            EditarVehiculoCommand = new RelayCommand(AbrirDialogoEditarVehiculo);
            VerVehiculoCommand = new RelayCommand(AbrirDialogoVerVehiculo);
        }

        private void AbrirDialogoNuevoVehiculo()
        {
            servicio.CargarDialogoAñadirVehiculo();
        }

        private void AbrirDialogoEditarVehiculo()
        {
            servicio.CargarDialogoEditarVehiculo();
        }

        private void AbrirDialogoVerVehiculo()
        {
            servicio.CargarDialogoVerVehiculo();
        }

    }
}
