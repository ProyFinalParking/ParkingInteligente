using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class AñadirVehiculoVM : ObservableObject
    {
        public RelayCommand AñadirVehiculoButton { get; }

        private Vehiculo nuevoVehiculo;

        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        public AñadirVehiculoVM()
        {
            NuevoVehiculo = new Vehiculo();
            AñadirVehiculoButton = new RelayCommand(AñadirVehiculo);
        }

        public void AñadirVehiculo()
        {
            ServicioDB.InsertVehicle(NuevoVehiculo);
        }
    }
}
