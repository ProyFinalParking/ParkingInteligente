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
    class AñadirVehiculoVM : ObservableObject
    {
        public RelayCommand AñadirVehiculoButton { get; }

        private readonly CustomVisionService servicioCustomVision;
        private readonly ComputerVisionService servicioComputerVision;

        private Vehiculo nuevoVehiculo;

        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        public AñadirVehiculoVM()
        {
            servicioCustomVision = new CustomVisionService();
            servicioComputerVision = new ComputerVisionService();
            NuevoVehiculo = new Vehiculo();
            AñadirVehiculoButton = new RelayCommand(AñadirVehiculo);
        }

        public void AñadirVehiculo()
        {
            // TODO Añadir a Vehículo una Foto de este para poder detectar Tipo y Matrícula
            // NuevoVehiculo.Tipo = servicioCustomVision.ComprobarVehiculo(NuevoVehiculo.Foto);
            // NuevoVehiculo.Matricula = servicioComputerVision.LeerImagen(NuevoVehiculo.Foto);
            ServicioDB.InsertVehicle(NuevoVehiculo);
            WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
        }
    }
}
