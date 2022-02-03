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

        private readonly FaceService servicio;

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }

        public AñadirClienteVM()
        {
            NuevoCliente = new Cliente();
            servicio = new FaceService();
            AñadirClienteButton = new RelayCommand(AñadirCliente);
        }

        public void AñadirCliente()
        {
            NuevoCliente.Edad = servicio.ObtenerEdad(NuevoCliente.Foto);
            NuevoCliente.Genero = servicio.ObtenerGenero(NuevoCliente.Foto);
            ServicioDB.InsertClient(NuevoCliente);
            WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));
        }
    }
}
