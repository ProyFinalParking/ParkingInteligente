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
    class AñadirClienteVM : ObservableObject
    {
        public RelayCommand AñadirClienteButton { get; }

        private readonly FaceService servicioFace;
        private readonly AzureBlobStorage servicioAlmacenamiento;

        private Cliente nuevoCliente;

        public Cliente NuevoCliente
        {
            get { return nuevoCliente; }
            set { SetProperty(ref nuevoCliente, value); }
        }

        public AñadirClienteVM()
        {
            servicioFace = new FaceService();
            servicioAlmacenamiento = new AzureBlobStorage();
            NuevoCliente = new Cliente();

            AñadirClienteButton = new RelayCommand(AñadirCliente);
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
            if(UrlImagenInterna != "")
            {
                NuevoCliente.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
                NuevoCliente.Genero = servicioFace.ObtenerGenero(NuevoCliente.Foto);
                NuevoCliente.Edad = servicioFace.ObtenerEdad(NuevoCliente.Foto);
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

        public void AñadirCliente()
        {
            if (!ServicioDB.IsExistsDocument(NuevoCliente.Documento) && nuevoCliente.Documento != "")
            {
                ServicioDB.InsertClient(NuevoCliente);
            }
            else
            {
                MessageBox.Show("Debes rellenar como minimo el DNI del usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));
        }
    }
}
