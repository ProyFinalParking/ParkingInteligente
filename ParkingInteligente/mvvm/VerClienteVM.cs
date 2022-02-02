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
    class VerClienteVM : ObservableObject
    {
        private Cliente clienteSeleccionado;

        public Cliente ClienteSeleccionado
        {
            get { return clienteSeleccionado; }
            set { SetProperty(ref clienteSeleccionado, value); }
        }

        public VerClienteVM()
        {
            ClienteSeleccionado = new Cliente();
            ClienteSeleccionado = WeakReferenceMessenger.Default.Send<ClienteSeleccionadoRequestMessage>();
        }
    }
}
