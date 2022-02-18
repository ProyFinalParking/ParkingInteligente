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
    class AñadirVehiculoVM : ObservableObject
    {
        //TODO Existe un error al eliminar el vehiculo que añades nuevo

        public RelayCommand AñadirVehiculoButton { get; }

        private readonly CustomVisionService servicioCustomVision;
        private readonly ComputerVisionService servicioComputerVision;
        private readonly AzureBlobStorage servicioAlmacenamiento;

        private List<MarcaVehiculo> marcas;
        public List<MarcaVehiculo> Marcas
        {
            get { return marcas; }
            set { SetProperty(ref marcas, value); }
        }

        private Vehiculo nuevoVehiculo;

        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        public AñadirVehiculoVM()
        {
            NuevoVehiculo = new Vehiculo();
            Marcas = ServicioDB.GetListVehicleBrands();

            servicioCustomVision = new CustomVisionService();
            servicioComputerVision = new ComputerVisionService();
            servicioAlmacenamiento = new AzureBlobStorage();

            AñadirVehiculoButton = new RelayCommand(AñadirVehiculo);
        }

        public BitmapImage imagenPorDefecto()
        {
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("/assets/IconoEmpresa.jpg", UriKind.Relative);
            bi.EndInit();

            return bi;
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

        public BitmapImage CargarImagen()
        {
            string UrlImagenInterna;

            UrlImagenInterna = AbrirDialogo();
            if (UrlImagenInterna != "")
            {
                NuevoVehiculo.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
                NuevoVehiculo.Matricula = servicioComputerVision.GetMatricula(NuevoVehiculo.Foto);
                NuevoVehiculo.Tipo = servicioCustomVision.ComprobarVehiculo(NuevoVehiculo.Foto);

                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.UriSource = new Uri(UrlImagenInterna, UriKind.Absolute);
                bi.EndInit();

                return bi;
            }
            else
            {
                return imagenPorDefecto();
            }
        }

        public void AñadirVehiculo()
        {
            if (!ServicioDB.IsExistsVehicle(NuevoVehiculo.Matricula) && nuevoVehiculo.Matricula != "")
            {
                ServicioDB.InsertVehicle(NuevoVehiculo);
            }
            else
            {
                ServicioDialogos.ErrorMensaje("Debes insertar una foto para poder añadir un vehiculo");
            }

            WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
        }
    }
}
