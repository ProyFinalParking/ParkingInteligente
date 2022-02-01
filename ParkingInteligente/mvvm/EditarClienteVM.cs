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
        private ServicioSqliteDB servicioDB;
        public RelayCommand EditarClienteButton { get; }

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
            ClienteSeleccionado = new Cliente();
            ClienteSeleccionado = WeakReferenceMessenger.Default.Send<ClienteSeleccionadoRequestMessage>();
            DocClienteOriginal = ClienteSeleccionado.ToString();
            servicioDB = new ServicioSqliteDB();
            EditarClienteButton = new RelayCommand(EditarCliente);
        }

        public void EditarCliente()
        {
            ClienteSeleccionado.Edad = 22;
            ClienteSeleccionado.Genero = "Hombre";
            servicioDB.UpdateClient(ClienteSeleccionado, DocClienteOriginal);
        }
    }
}
