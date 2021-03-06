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
        private readonly NavigationService servicio;

        private Cliente clienteSeleccionado;
        public Cliente ClienteSeleccionado
        {
            get { return clienteSeleccionado; }
            set
            {
                SetProperty(ref clienteSeleccionado, value);

                if (ClienteSeleccionado != null)
                {
                    NumVehiculosRegistrados = ServicioDB.GetNumClientVehicles(ClienteSeleccionado.Id);
                    NumEstacionamientosActivos = ServicioDB.GetNumClientParkedVehicles(ClienteSeleccionado.Id);

                    HaySeleccion = true;
                }
            }
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
            get { return clientes; }
            set { SetProperty(ref clientes, value); }
        }

        private bool haySeleccion;
        public bool HaySeleccion
        {
            get { return haySeleccion; }
            set { SetProperty(ref haySeleccion, value); }
        }

        private int numVehiculosRegistrados;
        public int NumVehiculosRegistrados
        {
            get { return numVehiculosRegistrados; }
            set { SetProperty(ref numVehiculosRegistrados, value); }
        }

        private int numEstacionamientosActivos;
        public int NumEstacionamientosActivos
        {
            get { return numEstacionamientosActivos; }
            set { SetProperty(ref numEstacionamientosActivos, value); }
        }

        public RelayCommand AñadirNuevoClienteCommand { get; }
        public RelayCommand EditarClienteCommand { get; }
        public RelayCommand VerClienteCommand { get; }
        public RelayCommand EliminarClienteCommand { get; }

        public ControlClientesVM()
        {
            Clientes = ServicioDB.GetListClients();
            servicio = new NavigationService();
            ClienteSeleccionado = new Cliente();
            HaySeleccion = false;

            WeakReferenceMessenger.Default.Register<ControlClientesVM, ClienteSeleccionadoRequestMessage>(this, (r, m) =>
            {
                if (!m.HasReceivedResponse)
                {
                    m.Reply(r.ClienteSeleccionado);
                }
            });

            WeakReferenceMessenger.Default.Register<ActualizarGridClientesMessage>(this, (r, m) =>
            {
                Clientes = m.Value;
            });

            AñadirNuevoClienteCommand = new RelayCommand(AbrirDialogoNuevoCliente);
            EditarClienteCommand = new RelayCommand(AbrirDialogoEditarCliente);
            VerClienteCommand = new RelayCommand(AbrirVerCliente);
            EliminarClienteCommand = new RelayCommand(AbrirDialogoEliminarCliente);
        }

        private void AbrirDialogoNuevoCliente()
        {
            servicio.CargarDialogoAñadirCliente();
        }

        private void AbrirDialogoEditarCliente()
        {
            // Comprueba que haya un cliente seleccionado
            if (ClienteSeleccionado.Documento != "")
            {
                servicio.CargarDialogoEditarCliente();
            }
            else
            {
                ServicioDialogos.ErrorMensaje("No hay ningún cliente seleccionado.");
            }
        }

        private void AbrirVerCliente()
        {
            // Comprueba que haya un cliente seleccionado
            if (ClienteSeleccionado.Documento != "")
            {
                servicio.CargarDialogoVerCliente();
            }
            else
            {
                ServicioDialogos.ErrorMensaje("No hay ningún cliente seleccionado.");
            }
        }

        private void AbrirDialogoEliminarCliente()
        {
            // Comprueba que haya un cliente seleccionado
            if (ClienteSeleccionado.Documento != "")
            {
                // Comprueba que no tenga vehiculos estacionados actualmente
                if (!ServicioDB.IsParked(ClienteSeleccionado.Documento))
                {
                    servicio.CargarDialogoEliminarCliente();
                    ClienteSeleccionado = new Cliente();
                    HaySeleccion = false;
                }
                else
                {
                    ServicioDialogos.ErrorMensaje("No se puede eliminar el Cliente.\nTiene estacionamientos activos.");
                }
            }
            else
            {
                ServicioDialogos.ErrorMensaje("No hay ningún cliente seleccionado.");
            }
        }
    }
}
