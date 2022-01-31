﻿using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ParkingInteligente.modelo
{
    class Vehiculo : ObservableObject
    {
        private int idVehiculo;
        public int IdVehiculo
        {
            get { return idVehiculo; }
            set { SetProperty(ref idVehiculo, value); }
        }

        private int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set { SetProperty(ref idCliente, value); }
        }

        private string matricula;
        public string Matricula
        {
            get { return matricula; }
            set { SetProperty(ref matricula, value); }
        }

        private int idMarca;
        public int IdMarca
        {
            get { return idMarca; }
            set { SetProperty(ref idMarca, value); }
        }

        private string modelo;
        public string Modelo
        {
            get { return modelo; }
            set { SetProperty(ref modelo, value); }
        }

        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { SetProperty(ref tipo, value); }
        }
    }
}