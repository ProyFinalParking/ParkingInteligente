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
        //TODO Necesitamos que se guarden las URL de las imagenes en la DB, ya que si no la uri de la imagen no es correcta porque no existe.

        public RelayCommand EditarVehiculoButton { get; }

        private readonly CustomVisionService servicioCustomVision;
        private readonly ComputerVisionService servicioComputerVision;
        private readonly AzureBlobStorage servicioAlmacenamiento;

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

        public EditarVehiculoVM()
        {
            servicioCustomVision = new CustomVisionService();
            servicioComputerVision = new ComputerVisionService();
            servicioAlmacenamiento = new AzureBlobStorage();
            VehiculoSeleccionado = new Vehiculo();

            VehiculoSeleccionado = WeakReferenceMessenger.Default.Send<VehiculoSeleccionadoRequestMessage>();
            MatVehiculoOriginal = VehiculoSeleccionado.Matricula.ToString();

            EditarVehiculoButton = new RelayCommand(EditarVehiculo);
        }

        public BitmapImage imagenPorDefecto()
        {
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            //TODO Descomentar cuando la BD tenga el campo foto y borrar la que acaba en relative.
            //bi.UriSource = new Uri(VehiculoSeleccionado.Foto, UriKind.Absolute); 
            bi.UriSource = new Uri(VehiculoSeleccionado.Foto, UriKind.Relative);
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
                VehiculoSeleccionado.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
                VehiculoSeleccionado.Matricula = servicioComputerVision.GetMatricula(VehiculoSeleccionado.Foto);
                VehiculoSeleccionado.Tipo = servicioCustomVision.ComprobarVehiculo(VehiculoSeleccionado.Foto);
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

        public void EditarVehiculo()
        {
            if(!ServicioDB.IsExistsVehicle(VehiculoSeleccionado.Matricula) && VehiculoSeleccionado.Matricula != "")
            {
                ServicioDB.UpdateVehicle(VehiculoSeleccionado, MatVehiculoOriginal);
            }
            else
            {
                if(vehiculoSeleccionado.Matricula == MatVehiculoOriginal)
                {
                    ServicioDB.UpdateVehicle(VehiculoSeleccionado, MatVehiculoOriginal);
                }
                else
                {
                    MessageBox.Show("El campo matricula esta vacio o la matricula es igual a la de otro coche", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
        }
    }
}
