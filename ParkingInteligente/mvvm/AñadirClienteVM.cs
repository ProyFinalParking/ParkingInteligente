using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class AñadirClienteVM : ObservableObject
    {
        private ServicioSqliteDB servicioDB;
        public RelayCommand AñadirClienteButton { get; }

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }

        public AñadirClienteVM()
        {
            NuevoCliente = new Cliente();
            servicioDB = new ServicioSqliteDB();
            AñadirClienteButton = new RelayCommand(AñadirCliente);
        }

        public void AñadirCliente()
        {
            NuevoCliente.Edad = 22;
            NuevoCliente.Genero = "Hombre";
            servicioDB.InsertClient(NuevoCliente);
        }
    }
}
