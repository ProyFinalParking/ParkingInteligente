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
        public RelayCommand AddMarcaBtn { get; }
        public RelayCommand GuardarMarcaCommand { get; }

        private List<string> marcas;
        public List<string> Marcas
        {
            get { return marcas; }
            set { SetProperty(ref marcas, value); }
        }

        private bool activadoAddMarca;
        public bool ActivadoAddMarca
        {
            get { return activadoAddMarca; }
            set { SetProperty(ref activadoAddMarca, value); }
        }

        private string marca;
        public string Marca
        {
            get { return marca; }
            set { SetProperty(ref marca, value); }
        }

        private Vehiculo nuevoVehiculo;
        public Vehiculo NuevoVehiculo
        {
            get { return nuevoVehiculo; }
            set { SetProperty(ref nuevoVehiculo, value); }
        }

        private List<string> tipoVehiculos;
        public List<string> Tipos
        {
            get { return tipoVehiculos; }
            set { SetProperty(ref tipoVehiculos, value); }
        }

        private string documentoCliente;
        public string DocumentoCliente
        {
            get { return documentoCliente; }
            set
            {
                SetProperty(ref documentoCliente, value);
                DatosCliente = ConsultarDatosCliente();
            }
        }

        private string datosCliente;
        public string DatosCliente
        {
            get { return datosCliente; }
            set { SetProperty(ref datosCliente, value); }
        }

        public AñadirVehiculoVM()
        {
            ActivadoAddMarca = false;
            NuevoVehiculo = new Vehiculo();
            Marcas = CargarMarcas();
            DocumentoCliente = "";
            DatosCliente = "";
            Marca = "";

            Tipos = new List<string>();
            Tipos.Add("Coche");
            Tipos.Add("Moto");

            GuardarMarcaCommand = new RelayCommand(AddMarca);
            AddMarcaBtn = new RelayCommand(ActivarAddMarca);
        }

        public bool AddVehiculo()
        {
            bool anyadido = false;

            // Obtiene la ID del cliente segun su Documento de Identidad
            NuevoVehiculo.IdCliente = ServicioDB.GetIdClient(DocumentoCliente);
            if (nuevoVehiculo.Matricula != "")
            {
                if (!ServicioDB.IsExistsVehicle(NuevoVehiculo.Matricula))
                {
                    if (ServicioDB.IsExistsClientId(NuevoVehiculo.IdCliente) && NuevoVehiculo.IdCliente != 0)
                    {
                        ServicioDB.InsertVehicle(NuevoVehiculo);
                        WeakReferenceMessenger.Default.Send(new ActualizarGridVehiculosMessage(ServicioDB.GetListVehicles()));
                        anyadido = true;
                    }
                    else
                    {
                        ServicioDialogos.ErrorMensaje("No existe el usario con el documento indicado.");
                    }
                }
                else
                {
                    ServicioDialogos.ErrorMensaje("Ya existe un coche registrado con esta matrícula.");
                }
            }
            else
            {
                ServicioDialogos.ErrorMensaje("Debes introducir la matrícula del coche");
            }

            return anyadido;
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

        private string ConsultarDatosCliente()
        {
            string datos = "";

            if (DocumentoCliente != "")
            {
                Cliente c = ServicioDB.GetClientByDocument(DocumentoCliente);

                if (c.Nombre != "")
                {
                    datos = c.GetDataString();
                }
                else
                {
                    datos = "Cliente no encontrado";
                }
            }

            return datos;
        }

        public void AddMarca()
        {
            if (Marca != "")
            {
                if (ServicioDB.GetIdVehicleBrand(Marca) == 0)
                {
                    ServicioDB.InsertVehicleBrand(Marca);
                    Marcas = CargarMarcas();
                    ActivadoAddMarca = false;
                    Marca = "";
                }
                else
                {
                    ServicioDialogos.ErrorMensaje("La marca indicada ya está registrada.");
                }
            }
        }

        private void ActivarAddMarca()
        {
            ActivadoAddMarca = true;
        }
    }
}
