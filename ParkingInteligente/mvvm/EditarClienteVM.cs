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
    class EditarClienteVM : ObservableObject
    {
        public RelayCommand EditarClienteButton { get; }

        private readonly FaceService servicioFace;
        private readonly AzureBlobStorage servicioAlmacenamiento;

        private Cliente clienteSeleccionado;

        public Cliente ClienteSeleccionado
        {
            get { return clienteSeleccionado; }
            set { SetProperty(ref clienteSeleccionado, value); }
        }

        private string docClienteOriginal;

        public string DocClienteOriginal
        {
            get { return docClienteOriginal; }
            set { SetProperty(ref docClienteOriginal, value); }
        }

        public EditarClienteVM()
        {
            servicioFace = new FaceService();
            servicioAlmacenamiento = new AzureBlobStorage();
            ClienteSeleccionado = new Cliente();

            ClienteSeleccionado = WeakReferenceMessenger.Default.Send<ClienteSeleccionadoRequestMessage>();
            DocClienteOriginal = ClienteSeleccionado.Documento.ToString();

            EditarClienteButton = new RelayCommand(EditarCliente);
        }

        public BitmapImage imagenPorDefecto()
        {
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(ClienteSeleccionado.Foto, UriKind.Absolute);
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
                ClienteSeleccionado.Foto = servicioAlmacenamiento.SubirImagen(UrlImagenInterna);
                ClienteSeleccionado.Genero = servicioFace.ObtenerGenero(ClienteSeleccionado.Foto);
                ClienteSeleccionado.Edad = servicioFace.ObtenerEdad(ClienteSeleccionado.Foto);
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

        public void EditarCliente()
        {
            if (!ServicioDB.IsExistsDocument(ClienteSeleccionado.Documento) && ClienteSeleccionado.Documento != "")
            {
                ServicioDB.UpdateClient(ClienteSeleccionado, DocClienteOriginal);
                
            }
            else
            {
                MessageBox.Show("El campo DNI esta vacio o el DNI es igual que el de otro cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            WeakReferenceMessenger.Default.Send(new ActualizarGridClientesMessage(ServicioDB.GetListClients()));

        }
    }
}
