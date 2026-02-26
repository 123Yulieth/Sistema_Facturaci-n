using System;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread] 
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia la aplicación mostrando el formulario Login
            Application.Run(new frmLogin());
        }
    }
}
