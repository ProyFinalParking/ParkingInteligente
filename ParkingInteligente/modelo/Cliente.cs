using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace ParkingInteligente.modelo
{
    class Cliente : ObservableObject
    {
        public Cliente()
        {
            // Se asigna una cadena vacía, para evitar Errores de Nulos
            nombre = "";
            foto = "";
            documento = "";
            genero = "";
            telefono = "";
        }

        public Cliente(int id, string nombre, string documento, string foto, int edad, string genero, string telefono)
        {
            Id = id;
            Nombre = nombre;
            Documento = documento;
            Foto = foto;
            Edad = edad;
            Genero = genero;
            Telefono = telefono;
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

        public override string ToString()
        {
            return "Cliente{" + "nombre=" + Nombre + ", documento=" + Documento + ", edad="
                + Edad + ", genero=" + Genero + ", telefono=" + Telefono + '}';
        }
    }
}
