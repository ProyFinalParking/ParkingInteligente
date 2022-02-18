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
    class EliminarVehiculoVM : ObservableObject
    {
        public RelayCommand EliminarVehiculoButton { get; }

        private Vehiculo vehiculoSeleccionado;

        public Vehiculo VehiculoSeleccionado
        {
            get { return vehiculoSeleccionado; }
            set { SetProperty(ref vehiculoSeleccionado, value); }
        }

        private string docVehiculoOriginal;

        public string DocVehiculoOriginal
        {
            get { return docVehiculoOriginal; }
            set { SetProperty(ref docVehiculoOriginal, value); }
        }

        public EliminarVehiculoVM()
        {
            VehiculoSeleccionado = new Vehiculo();
            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
            DocVehiculoOriginal = VehiculoSeleccionado.Matricula.ToString();
            EliminarVehiculoButton = new RelayCommand(EliminarVehiculo);
        }

        public void EliminarVehiculo()
        {
            if (ServicioDB.IsVehicleParked(docVehiculoOriginal))
            {
                ServicioDB.DeleteVehicle(docVehiculoOriginal);
                WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
            }
            else
            {
                ServicioDialogos.ErrorMensaje("El vehículo seleccionado tiene un estacionamiento sin finalizar.");
            }
        }
    }
}
