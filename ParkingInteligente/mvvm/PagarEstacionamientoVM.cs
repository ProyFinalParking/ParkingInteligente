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
    class PagarEstacionamientoVM : ObservableObject
    {
        public RelayCommand PagarFinalizarButton { get; }

        private Estacionamiento estacionamientoSeleccionado;

        public Estacionamiento EstacionamientoSeleccionado
        {
            get { return estacionamientoSeleccionado; }
            set { SetProperty(ref estacionamientoSeleccionado, value); }
        }

        public PagarEstacionamientoVM()
        {
            EstacionamientoSeleccionado = new Estacionamiento();
            EstacionamientoSeleccionado = WeakReferenceMessenger.Default.Send<EstacionamientoSeleccionadoRequestMessage>();
            PagarFinalizarButton = new RelayCommand(PagarFinalizar);
        }

        public void PagarFinalizar()
        {
            // Se calcula el importe
            int costeParking = 10;

            // Se añade la hora de salida
            string horaSalida = DateTime.Now.ToString();
            EstacionamientoSeleccionado.Salida = horaSalida;
            EstacionamientoSeleccionado.Importe = costeParking;

            // Se actualiza el estacionamiento
            ServicioDB.UpdateParkedVehicle(EstacionamientoSeleccionado);

            EstacionamientoSeleccionado = new Estacionamiento();
            //WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));
        }
    }
}
