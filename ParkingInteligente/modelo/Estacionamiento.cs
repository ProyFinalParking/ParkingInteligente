using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ParkingInteligente.modelo
{
    class Estacionamiento : ObservableObject
    {
        private int idEstacionamiento;
        public int IdEstacionamiento
        {
            get { return idEstacionamiento; }
            set { SetProperty(ref idEstacionamiento, value); }
        }

        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { SetProperty(ref idVehiculo, value); }
        }

        private string matricula;
        public string Matricula
        {
            get { return matricula; }
            set { SetProperty(ref matricula, value); }
        }

        private string entrada;
        public string Entrada
        {
            get { return entrada; }
            set { SetProperty(ref entrada, value); }
        }

        private string salida;
        public string Salida
        {
            get { return salida; }
            set { SetProperty(ref salida, value); }
        }

        private float importe;
        public float Importe
        {
            get { return importe; }
            set { SetProperty(ref importe, value); }
        }

        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { SetProperty(ref tipo, value); }
        }

    }
}
