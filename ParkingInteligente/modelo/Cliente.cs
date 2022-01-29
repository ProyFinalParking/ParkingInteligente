using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ParkingInteligente.modelo
{
    class Cliente : ObservableObject
    {
        public Cliente(int id, string nombre, string documento, string foto, int edad, string genero, string telefono)
        {
            this.id = id;
            this.nombre = nombre;
            this.documento = documento;
            this.foto = foto;
            this.edad = edad;
            this.genero = genero;
            this.telefono = telefono;
        }

        public Cliente()
        {
        }

        // Al insertar un Cliente en la BBDD, no se tiene en cuenta la ID del objeto
        // La ID es Autoincremental
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { SetProperty(ref nombre, value); }
        }

        private string documento;
        public string Documento
        {
            get { return documento; }
            set { SetProperty(ref documento, value); }
        }

        private string foto;
        public string Foto
        {
            get { return foto; }
            set { SetProperty(ref foto, value); }
        }

        private int edad;
        public int Edad
        {
            get { return edad; }
            set { SetProperty(ref edad, value); }
        }

        private string genero;
        public string Genero
        {
            get { return genero; }
            set { SetProperty(ref genero, value); }
        }

        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { SetProperty(ref telefono, value); }
        }

    }
}
