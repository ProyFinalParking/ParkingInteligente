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
    class ControlAparcamientosVM : ObservableObject
    {
        private readonly NavigationService servicio;

        private Estacionamiento parkingSelect;
        public Estacionamiento ParkingSelect
        {
            get { return parkingSelect; }
            set { SetProperty(ref parkingSelect, value); }
        }

        private List<Estacionamiento> parkingList;
        public List<Estacionamiento> ParkingList
        {
            get { return parkingList; }
            set { SetProperty(ref parkingList, value); }
        }

        public RelayCommand CobrarFinalizarCommand { get; }

        public ControlAparcamientosVM()
        {
            ParkingList = ServicioDB.GetListActivesParkedVehicles();
            servicio = new NavigationService();
            ParkingSelect = new Estacionamiento();

            WeakReferenceMessenger.Default.Register<ControlAparcamientosVM, EstacionamientoSeleccionadoRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.ParkingSelect);
            });

            CobrarFinalizarCommand = new RelayCommand(AbrirDialogoCobrarParking);
        }

        private void AbrirDialogoCobrarParking()
        {
            servicio.CargarDialogoPagarFinalizarParking();
        }

    }
}
