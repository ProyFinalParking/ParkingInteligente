using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ParkingInteligente.modelo
{
    class MarcaVehiculo : ObservableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string marca;
        public string Marca
        {
            get { return marca; }
            set { SetProperty(ref marca, value); }
        }

    }
}
