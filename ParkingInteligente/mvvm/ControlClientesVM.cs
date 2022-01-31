using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }


        private ObservableCollection<Cliente> clientes;
        public ObservableCollection<Cliente> Clientes
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
            Clientes = new ObservableCollection<Cliente>();
            this.GenerateOrders();
            servicio = new NavigationService();
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

        private void GenerateOrders()
        {
            clientes.Add(new Cliente(1001, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1002, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1003, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1004, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1005, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1006, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1007, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1008, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1009, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1010, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1011, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1012, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1013, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
            clientes.Add(new Cliente(1014, "Maria", "4523532X", "foto", 24, "Mujer", "633243123"));
        }
    }
}
