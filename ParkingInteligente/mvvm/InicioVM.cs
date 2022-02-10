using Microsoft.Toolkit.Mvvm.ComponentModel;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class InicioVM : ObservableObject
    {
        // Propiedad de Configuración de aplicaciones con el numero de plazas
        static readonly int plazasCoches = Properties.Settings.Default.PlazasCoches;
        static readonly int plazasMotos = Properties.Settings.Default.PlazasMotos;

        public InicioVM()
        {
            ActualizarGraficos();
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

        private void ActualizarGraficos()
        {
            // Bucle infinito que actualiza el numero de plazas ocupadas, cada segundo
            PlazasOcupadasCoches = ServicioDB.GetNumberParkedCars();
            PlazasOcupadasMotos = ServicioDB.GetNumberParkedMotorcycles();

            PlazasLibresMotos = plazasMotos - PlazasOcupadasMotos;
            PlazasLibresCoches = plazasCoches - PlazasOcupadasCoches;
        }

    }
}
