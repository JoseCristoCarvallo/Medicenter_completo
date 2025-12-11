using System;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Clase principal del programa. Contiene el punto de entrada de la aplicación.
    internal static class Program
    {
        // Atributo STAThread indica que el modelo de subprocesos para la aplicación es Single-Threaded Apartment.
        [STAThread]
        static void Main()
        {
            // Habilita los estilos visuales para la aplicación, dándole una apariencia moderna.
            Application.EnableVisualStyles();
            // Establece el valor predeterminado para cómo las aplicaciones controlan la representación de texto.
            Application.SetCompatibleTextRenderingDefault(false);
            // Ejecuta la aplicación, iniciando con el formulario principal 'FormPrincipal'.
            Application.Run(new FormPrincipal());
        }
    }
}