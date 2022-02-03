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
    class EditarClienteVM : ObservableObject
    {
        public RelayCommand EditarClienteButton { get; }

        private readonly FaceService servicio;

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

        public EditarClienteVM()
        {
            servicio = new FaceService();
            ClienteSeleccionado = new Cliente();
            ClienteSeleccionado = WeakReferenceMessenger.Default.Send<ClienteSeleccionadoRequestMessage>();
            DocClienteOriginal = ClienteSeleccionado.ToString();
            EditarClienteButton = new RelayCommand(EditarCliente);
        }

        public void EditarCliente()
        {
            ClienteSeleccionado.Edad = servicio.ObtenerEdad(ClienteSeleccionado.Foto);
            ClienteSeleccionado.Genero = servicio.ObtenerGenero(ClienteSeleccionado.Foto);
            ServicioDB.UpdateClient(ClienteSeleccionado, DocClienteOriginal);
        }
    }
}
