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
        private readonly NavigationService servicio;

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
            get { return vehiculos; }
            set { SetProperty(ref vehiculos, value); }
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

                // Se resetea el vehiculo seleccionado para evitar errores
                VehiculoSeleccionado = new Vehiculo();
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
                ServicioDialogos.ErrorMensaje("No ha seleccionado ningún vehículo.");
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
                ServicioDialogos.ErrorMensaje("No ha seleccionado ningún vehículo.");
            }
        }

        private void AbrirDialogoEliminarVehiculo()
        {
            if (VehiculoSeleccionado.Matricula != "")
            {
                if (ServicioDB.IsVehicleParked(VehiculoSeleccionado.Matricula))
                {
                    servicio.CargarDialogoEliminarVehiculo();

                    // Resetea la selección
                    VehiculoSeleccionado = new Vehiculo();
                }
                else
                {
                    ServicioDialogos.ErrorMensaje("El vehicúculo seleccionado tiene un estaciónamiento sin finalizar.");
                }
            }
            else
            {
                ServicioDialogos.ErrorMensaje("No ha seleccionado ningún vehículo.");
            }
        }

    }
}
