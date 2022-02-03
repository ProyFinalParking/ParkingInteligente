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
    class EliminarClienteVM : ObservableObject
    {
        public RelayCommand EliminarClienteButton { get; }

        private Cliente clienteSeleccionado;

        public Cliente ClienteSeleccionado
        {
            get { return clienteSeleccionado; }
            set { SetProperty(ref clienteSeleccionado, value); }
        }

        private string docClienteOriginal;

        public string DocClienteOriginal
        {
            get { return docClienteOriginal; }
            set { SetProperty(ref docClienteOriginal, value); }
        }

        public EliminarClienteVM()
        {
            ClienteSeleccionado = new Cliente();
            ClienteSeleccionado = WeakReferenceMessenger.Default.Send<ClienteSeleccionadoRequestMessage>();
            DocClienteOriginal = ClienteSeleccionado.Documento.ToString();
            EliminarClienteButton = new RelayCommand(EliminarCliente);
        }

        public void EliminarCliente()
        {
            ServicioDB.DeleteClient(docClienteOriginal);
            WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));
        }
    }
}
