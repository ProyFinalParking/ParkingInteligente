using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class ControlClientesVM : ObservableObject
    {
        private NavigationService servicio;
        private ServicioSqliteDB servicioDB;

        private Cliente clienteSeleccionado;
        public Cliente ClienteSeleccionado
        {
            get { return clienteSeleccionado; }
            set { SetProperty(ref clienteSeleccionado, value); }
        }

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }


        private List<Cliente> clientes;
        public List<Cliente> Clientes
        {
            get
            {
                return clientes;
            }
            set
            {
                SetProperty(ref clientes, value); 
            }
        }

        public RelayCommand AñadirNuevoClienteCommand { get; }
        public RelayCommand EditarClienteCommand { get; }

        public ControlClientesVM()
        {
            servicioDB = new ServicioSqliteDB();
            Clientes = servicioDB.GetListClients();
            servicio = new NavigationService();
            ClienteSeleccionado = new Cliente();

            WeakReferenceMessenger.Default.Register<ControlClientesVM, ClienteSeleccionadoRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.ClienteSeleccionado);
            });

            AñadirNuevoClienteCommand = new RelayCommand(AbrirDialogoNuevoCliente);
            EditarClienteCommand = new RelayCommand(AbrirDialogoEditarCliente);
        }

        private void AbrirDialogoNuevoCliente()
        {
            servicio.CargarDialogoAñadirCliente();
        }

        private void AbrirDialogoEditarCliente()
        {
            servicio.CargarDialogoEditarCliente();
        }

    }
}
