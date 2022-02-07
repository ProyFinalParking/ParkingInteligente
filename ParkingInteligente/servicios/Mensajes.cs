using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using ParkingInteligente.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingInteligente.servicios
{
    class ClienteSeleccionadoRequestMessage : RequestMessage<Cliente> { }
    class VehiculoSeleccionadoRequestMessage : RequestMessage<Vehiculo> { }
    class EstacionamientoSeleccionadoRequestMessage : RequestMessage<Estacionamiento> { };
    class ActualizarGridClientesMessage : ValueChangedMessage<List<Cliente>>
    {
        public ActualizarGridClientesMessage(List<Cliente> valor) : base(valor) { }
    }

    class ActualizarGridVehiculosMessage : ValueChangedMessage<List<Vehiculo>>
    {
        public ActualizarGridVehiculosMessage(List<Vehiculo> valor) : base(valor) { }
    }

    // Actualiza la lista de Estacionamientos por suscripcion al finalizar un estacionamiento
    class ActualizarGridEstacionamientosMessage : ValueChangedMessage<List<Estacionamiento>>
    {
        public ActualizarGridEstacionamientosMessage(List<Estacionamiento> valor) : base(valor) { }
    }
}
