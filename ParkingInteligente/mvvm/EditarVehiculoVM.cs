using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Win32;
using ParkingInteligente.modelo;
using ParkingInteligente.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ParkingInteligente.mvvm
{
    class EditarVehiculoVM : ObservableObject
    {
        public RelayCommand EditarVehiculoButton { get; }

        private Vehiculo vehiculoSeleccionado;
        public Vehiculo VehiculoSeleccionado
        {
            get { return vehiculoSeleccionado; }
            set { SetProperty(ref vehiculoSeleccionado, value); }
        }

        private string docVehiculoOriginal;
        public string MatVehiculoOriginal
        {
            get { return docVehiculoOriginal; }
            set { SetProperty(ref docVehiculoOriginal, value); }
        }

        private List<string> marcas;
        public List<string> Marcas
        {
            get { return marcas; }
            set { SetProperty(ref marcas, value); }
        }

        public EditarVehiculoVM()
        {
            Marcas = CargarMarcas();

            VehiculoSeleccionado = new Vehiculo();

            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
            MatVehiculoOriginal = VehiculoSeleccionado.Matricula.ToString();

            EditarVehiculoButton = new RelayCommand(EditarVehiculo);
        }

        public string AbrirDialogo()
        {
            string nombreArchivo = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                nombreArchivo = openFileDialog.FileName;
            }

            return nombreArchivo;
        }

        public void EditarVehiculo()
        {
            if (VehiculoSeleccionado.Matricula != "" && !ServicioDB.IsExistsVehicle(MatVehiculoOriginal))
            {
                ServicioDB.UpdateVehicle(VehiculoSeleccionado, MatVehiculoOriginal);
                WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
            }
            else
            {
                ServicioDialogos.ErrorMensaje("La matricula está vacía o no existe ningún vehículo con la matricula: " + MatVehiculoOriginal);
            }
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
