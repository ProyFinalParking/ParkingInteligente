using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.mvvm
{
    class VerVehiculoVM : ObservableObject
    {
        private Vehiculo vehiculoSeleccionado;

        public Vehiculo VehiculoSeleccionado
        {
            get { return vehiculoSeleccionado; }
            set { SetProperty(ref vehiculoSeleccionado, value); }
        }

        private List<string> marcas;
        public List<string> Marcas
        {
            get { return marcas; }
            set { SetProperty(ref marcas, value); }
        }

        public VerVehiculoVM()
        {
            Marcas = CargarMarcas();
            VehiculoSeleccionado = new Vehiculo();
            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
        }

        private List<string> CargarMarcas()
        {
            List<MarcaVehiculo> listaObjetosMarca = ServicioDB.GetListVehicleBrands();
            List<string> listaMarcas = new List<string>();

            // La primera entrada esta en blanco, que es la opción por defecto
            listaMarcas.Add("");

            foreach (MarcaVehiculo m in listaObjetosMarca)
            {
                listaMarcas.Add(m.Marca);
            }

            return listaMarcas;
        }
    }
}
