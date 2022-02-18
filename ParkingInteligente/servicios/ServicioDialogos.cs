using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingInteligente.servicios
{
    class ServicioDialogos
    {
        protected ServicioDialogos()
        {
        }

        public static void ErrorMensaje(string mensaje)
        {
            _ = MessageBox.Show(
                mensaje,
                "¡Error!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
