using System.Windows.Forms;

namespace ParkingInteligente.servicios
{
    class ServicioDialogos
    {
        protected ServicioDialogos()
        {
        }

        // Dialogos Guardar Cliente Nuevo
        public static void ClienteGuardado()
        {
            _ = MessageBox.Show(
                "Se ha guardado el cliente en la base de datos.",
                "Cliente Guardado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ErrorGuardarCliente()
        {
            _ = MessageBox.Show(
                "No se ha guardado el cliente debido a un error.",
                "Error al guardar el cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Dialogos Editar Cliente
        public static void ClienteEditado()
        {
            _ = MessageBox.Show(
                "El Cliente se ha editado con exito.",
                "Cliente Editado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ErrorEditarCliente()
        {
            _ = MessageBox.Show(
                "No se ha editado el cliente debido a un error.",
                "Error al editar el cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Dialogos Eliminar Cliente
        public static void ClienteEliminado()
        {
            _ = MessageBox.Show(
                "El Cliente se ha eliminado con exito.",
                "Cliente Eliminado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ErrorEliminarCliente()
        {
            _ = MessageBox.Show(
                "No se ha eliminado el cliente debido a un error.",
                "Error al eliminar el cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
