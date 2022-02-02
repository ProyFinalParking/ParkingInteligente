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
    class AñadirClienteVM : ObservableObject
    {
        public RelayCommand AñadirClienteButton { get; }

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }

        public AñadirClienteVM()
        {
            NuevoCliente = new Cliente();
            AñadirClienteButton = new RelayCommand(AñadirCliente);
        }

        public void AñadirCliente()
        {
            NuevoCliente.Edad = 22;
            NuevoCliente.Genero = "Hombre";
            ServicioDB.InsertClient(NuevoCliente);
            WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));
        }
    }
}
