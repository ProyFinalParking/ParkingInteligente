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
    class EditarVehiculoVM : ObservableObject
    {
        public RelayCommand EditarVehiculoButton { get; }

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

        public EditarVehiculoVM()
        {
            VehiculoSeleccionado = new Vehiculo();
            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
            DocVehiculoOriginal = VehiculoSeleccionado.ToString();
            EditarVehiculoButton = new RelayCommand(EditarVehiculo);
        }

        public void EditarVehiculo()
        {
            ServicioDB.UpdateVehicle(VehiculoSeleccionado, DocVehiculoOriginal);
        }
    }
}
