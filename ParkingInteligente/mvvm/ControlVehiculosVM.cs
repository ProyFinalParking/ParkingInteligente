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
        public RelayCommand EliminarVehiculoCommand { get; }


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
                if (!m.HasReceivedResponse)
                {
                    m.Reply(r.VehiculoSeleccionado);
                }
            });

            WeakReferenceMessenger.Default.Register<ActualizarGridVehiculosMessage>(this, (r, m) =>
            {
                Vehiculos = m.Value;
            });

            AñadirNuevoVehiculoCommand = new RelayCommand(AbrirDialogoNuevoVehiculo);
            EditarVehiculoCommand = new RelayCommand(AbrirDialogoEditarVehiculo);
            VerVehiculoCommand = new RelayCommand(AbrirDialogoVerVehiculo);
            EliminarVehiculoCommand = new RelayCommand(AbrirDialogoEliminarVehiculo);
        }

        private void AbrirDialogoNuevoVehiculo()
        {
            servicio.CargarDialogoAñadirVehiculo();
        }

        private void AbrirDialogoEditarVehiculo()
        {
            if (VehiculoSeleccionado.Matricula != "")
            {
                servicio.CargarDialogoEditarVehiculo();
            }
            else
            {
                // TODO: Avisar de que hay que seleccionar un vehiculo
            }
        }

        private void AbrirDialogoVerVehiculo()
        {
            if (VehiculoSeleccionado.Matricula != "")
            {
                servicio.CargarDialogoVerVehiculo();
            }
            else
            {
                // TODO: Avisar de que hay que seleccionar un vehiculo
            }
        }

        private void AbrirDialogoEliminarVehiculo()
        {
            if (VehiculoSeleccionado.Matricula != "")
            {
                if (ServicioDB.IsVehicleParked(VehiculoSeleccionado.Matricula))
                {
                    servicio.CargarDialogoEliminarVehiculo();
                    VehiculoSeleccionado = new Vehiculo();
                }
                else
                {
                    // TODO: Avisar de que el cliente tiene estacionamiento activo sin finalizar
                }
            }
            else
            {
                // TODO: Avisar de que hay que seleccionar un vehiculo
            }
        }

    }
}
