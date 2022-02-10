using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;

namespace ParkingInteligente.mvvm
{
    class ControlEstacionamientosVM : ObservableObject
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

        public ControlEstacionamientosVM()
        {
            ParkingList = ServicioDB.GetListActiveParkedVehicles();
            servicio = new NavigationService();
            ParkingSelect = new Estacionamiento();
            CobrarFinalizarCommand = new RelayCommand(AbrirDialogoCobrarParking);

            WeakReferenceMessenger.Default.Register<ControlEstacionamientosVM, EstacionamientoSeleccionadoRequestMessage>(this, (r, m) =>
            {
                if (!m.HasReceivedResponse)
                {
                    m.Reply(r.ParkingSelect);
                }
            });

            WeakReferenceMessenger.Default.Register<ActualizarGridEstacionamientosMessage>(this, (r, m) =>
            {
                ParkingList = m.Value;
            });
        }

        private void AbrirDialogoCobrarParking()
        {
            if (ParkingSelect.IdEstacionamiento > 0)
            {
                servicio.CargarDialogoPagarFinalizarParking();
            }
            else
            {
                // TODO: Avisar de que hay que seleccionar una Estacionamiento
            }
        }

    }
}
