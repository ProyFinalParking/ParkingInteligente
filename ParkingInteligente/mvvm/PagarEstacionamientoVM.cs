using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;

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

            // Calcula el importe a pagar
            EstacionamientoSeleccionado.Importe = CalcularImporteEstacionamiento();

            PagarFinalizarButton = new RelayCommand(PagarFinalizar);
        }

        public void PagarFinalizar()
        {
            // Se Edita el Estacionamiento, para finalizarlo
            EstacionamientoSeleccionado.Salida = DateTime.Now.ToString();

            // Se actualiza el estacionamiento
            ServicioDB.UpdateParkedVehicle(EstacionamientoSeleccionado);

            // Se resetea la propiedad
            EstacionamientoSeleccionado = new Estacionamiento();

            // Actualiza el listado al finalizar un estacionamiento
            WeakReferenceMessenger.Default.Send(new ActualizarGridEstacionamientosMessage(ServicioDB.GetListActivesParkedVehicles()));
        }

        private double CalcularImporteEstacionamiento()
        {
            // Se añade la hora de salida
            string horaSalida = DateTime.Now.ToString();

            // Calcula los minutos del estacionamiento
            DateTime entrada = Convert.ToDateTime(EstacionamientoSeleccionado.Entrada);
            DateTime salida = Convert.ToDateTime(horaSalida);
            TimeSpan diferenciaTiempo = salida - entrada;
            double minutos = diferenciaTiempo.TotalMinutes;

            // Obtiene el coste por minuto
            double precioMinuto = Properties.Settings.Default.PrecioMinuto;

            // Calcula el importe a pagar redondeando a 2 decimales
            double importe = Math.Round(minutos * precioMinuto, 2);

            // Si el vehiculo está registrado, se le hace el descuento del 10%
            if (EstacionamientoSeleccionado.IdVehiculo > 1)
            {
                importe *= 0.9;
            }

            return importe;
        }
    }
}
