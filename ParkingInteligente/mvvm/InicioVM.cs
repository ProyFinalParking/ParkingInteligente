using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ParkingInteligente.servicios;
using System;
using System.IO;

namespace ParkingInteligente.mvvm
{
    class InicioVM : ObservableObject
    {
        // Propiedad de Configuración de aplicaciones con el numero de plazas
        static readonly int plazasCoches = Properties.Settings.Default.PlazasCoches;
        static readonly int plazasMotos = Properties.Settings.Default.PlazasMotos;

        public RelayCommand GuardarNotas { get; }
        public RelayCommand ActualizarGraficos { get; }

        public InicioVM()
        {
            BlockNotas = Properties.Settings.Default.NotasGuardadas;
            FechaActual = DateTime.Now.ToLongDateString();

            GuardarNotas = new RelayCommand(GuardarBlockNotas);
            ActualizarGraficos = new RelayCommand(ActualizarDatosGraficos);

            ActualizarDatosGraficos();
        }

        private int plazasOcupadasMotos;
        public int PlazasOcupadasMotos
        {
            get { return plazasOcupadasMotos; }
            set { SetProperty(ref plazasOcupadasMotos, value); }
        }

        private int plazasOcupadasCoches;
        public int PlazasOcupadasCoches
        {
            get { return plazasOcupadasCoches; }
            set { SetProperty(ref plazasOcupadasCoches, value); }
        }

        private int plazasLibresMotos;
        public int PlazasLibresMotos
        {
            get { return plazasLibresMotos; }
            set { SetProperty(ref plazasLibresMotos, value); }
        }

        private int plazasLibresCoches;
        public int PlazasLibresCoches
        {
            get { return plazasLibresCoches; }
            set { SetProperty(ref plazasLibresCoches, value); }
        }

        private string fechaActual;
        public string FechaActual
        {
            get { return fechaActual; }
            set { SetProperty(ref fechaActual, value); }
        }

        private string blockNotas;
        public string BlockNotas
        {
            get { return blockNotas; }
            set { SetProperty(ref blockNotas, value); }
        }

        private void ActualizarDatosGraficos()
        {
            // Actualiza el numero de plazas ocupadas, consultando la BBDD
            PlazasOcupadasCoches = ServicioDB.GetNumberParkedCars();
            PlazasOcupadasMotos = ServicioDB.GetNumberParkedMotorcycles();

            PlazasLibresMotos = plazasMotos - PlazasOcupadasMotos;
            PlazasLibresCoches = plazasCoches - PlazasOcupadasCoches;
        }

        private void GuardarBlockNotas()
        {
            Properties.Settings.Default.NotasGuardadas = BlockNotas;
            Properties.Settings.Default.Save();
        }

    }
}
