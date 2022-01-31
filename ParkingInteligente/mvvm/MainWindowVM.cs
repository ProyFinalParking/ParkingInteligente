using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkingInteligente.mvvm
{
    class MainWindowVM : ObservableObject
    {
        private NavigationService servicio;

        private Page contenido;

        public Page Contenido
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
        }


        public void AbrirInicio()
        {
            contenido = servicio.CargarInicio();
        }

        public void AbrirControlClientes()
        {
            contenido = servicio.CargarControlClientes();
        }

        public void AbrirControlVehiculos()
        {
            contenido = servicio.CargarControlVehiculos();
        }

        public void AbrirControlAparcamientos()
        {
            contenido = servicio.CargarControlAparcamientos();
        }

    }
}
