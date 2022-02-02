using Microsoft.Toolkit.Mvvm.ComponentModel;
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
    class VerVehiculoVM : ObservableObject
    {
        private Vehiculo vehiculoSeleccionado;

        public Vehiculo VehiculoSeleccionado
        {
            get { return vehiculoSeleccionado; }
            set { SetProperty(ref vehiculoSeleccionado, value); }
        }

        public VerVehiculoVM()
        {
            VehiculoSeleccionado = new Vehiculo();
            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
        }
    }
}
