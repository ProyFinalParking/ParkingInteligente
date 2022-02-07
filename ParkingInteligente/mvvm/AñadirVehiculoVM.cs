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
            NuevoVehiculo = new Vehiculo();
            NuevoVehiculo.Foto = "https://motor.elpais.com/wp-content/uploads/2018/11/CUPRA-Ateca057H.jpg";

            // TODO Añadir a Vehículo una Foto de este para poder detectar Tipo y Matrícula
            servicioCustomVision = new CustomVisionService();
            servicioComputerVision = new ComputerVisionService();
            NuevoVehiculo.Tipo = servicioCustomVision.ComprobarVehiculo(NuevoVehiculo.Foto);
            NuevoVehiculo.Matricula = servicioComputerVision.GetMatricula(NuevoVehiculo.Foto);

            AñadirVehiculoButton = new RelayCommand(AñadirVehiculo);
        }

        public void AñadirVehiculo()
        {
            if (!ServicioDB.IsExistsVehicle(NuevoVehiculo.Matricula))
            {
                ServicioDB.InsertVehicle(NuevoVehiculo);
            }
            else
            {
                // TODO: Avisar que ya existe un Vehiculo con la Matricuula indicada
            }

            WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
        }
    }
}
