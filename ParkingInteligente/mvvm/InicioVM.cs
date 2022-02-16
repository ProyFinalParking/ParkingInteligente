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
        static readonly string notasGuardadas = Properties.Settings.Default.NotasGuardadas;

        public InicioVM()
        {
            FechaActual = DateTime.Now.ToLongDateString();
            BlockNotas = notasGuardadas;
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

        private void ActualizarGraficos()
        {
            // Actualiza el numero de plazas ocupadas, consultando la BBDD
            PlazasOcupadasCoches = ServicioDB.GetNumberParkedCars();
            PlazasOcupadasMotos = ServicioDB.GetNumberParkedMotorcycles();

            PlazasLibresMotos = plazasMotos - PlazasOcupadasMotos;
            PlazasLibresCoches = plazasCoches - PlazasOcupadasCoches;
        }

    }
}
