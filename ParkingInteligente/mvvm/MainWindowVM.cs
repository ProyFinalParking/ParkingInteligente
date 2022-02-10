using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkingInteligente.mvvm
{
    class MainWindowVM : ObservableObject
    {
        private readonly NavigationService servicio;

        private UserControl contenido;

        public UserControl Contenido
        {
            get { return contenido; }
            set { SetProperty(ref contenido, value); }
        }

        public RelayCommand Inicio { get; }
        public RelayCommand ControlVehiculos { get; }
        public RelayCommand ControlClientes { get; }
        public RelayCommand ControlAparcamientos { get; }


        public MainWindowVM()
        {
            servicio = new NavigationService();
            Inicio = new RelayCommand(AbrirInicio);
            ControlVehiculos = new RelayCommand(AbrirControlVehiculos);
            ControlClientes = new RelayCommand(AbrirControlClientes);
            ControlAparcamientos = new RelayCommand(AbrirControlAparcamientos);
            AbrirInicio();
        }


        public void AbrirInicio()
        {
            Contenido = servicio.CargarInicio();
        }

        public void AbrirControlClientes()
        {
            Contenido = servicio.CargarControlClientes();
        }

        public void AbrirControlVehiculos()
        {
            Contenido = servicio.CargarControlVehiculos();
        }

        public void AbrirControlAparcamientos()
        {
            Contenido = servicio.CargarControlAparcamientos();
        }

    }
}
