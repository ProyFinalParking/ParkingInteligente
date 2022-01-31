using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ParkingInteligente.modelo
{
    class MarcaVehiculo : ObservableObject
    {
        public MarcaVehiculo(int id, string marca)
        {
            Id = id;
            Marca = marca;
        }

        public MarcaVehiculo()
        {
        }

        // Al insertar una Marca en la BBDD, no se tiene en cuenta la ID del objeto
        // La ID es Autoincremental
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

        public override string ToString()
        {
            return "Marca{" + "id=" + Id + ", marca=" + Marca + '}';
        }
    }
}
