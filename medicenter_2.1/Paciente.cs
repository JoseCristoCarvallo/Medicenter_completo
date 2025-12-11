using System;
using System.Collections.Generic;

namespace MEDICENTER
{
    // Clase Paciente: representa a un paciente en el sistema MediCenter.
    // Hereda de la clase base 'Usuario' para incluir funcionalidades de autenticación.
    [Serializable]
    public class Paciente : Usuario
    {
        public int Edad { get; set; } // Edad del paciente.
        public string Telefono { get; set; } // Número de teléfono del paciente.
        public Genero Genero { get; set; } // Género del paciente (usando la enumeración Genero).
        public TipoSangre TipoSangre { get; set; } // Tipo de sangre del paciente (usando la enumeración TipoSangre).
        public TipoSeguro TipoSeguro { get; set; } // Tipo de seguro médico del paciente (usando la enumeración TipoSeguro).
        public string NumeroSeguro { get; set; } // Número de identificación del seguro médico.
        public string ContactoEmergencia { get; set; } // Información de contacto en caso de emergencia.
        public List<RegistroMedico> Historial { get; set; } // Lista del historial médico del paciente.
        public byte[] ImagenSeguroFrontal { get; set; } // Imagen frontal del seguro médico (almacenada como bytes).
        public byte[] ImagenSeguroTrasera { get; set; } // Imagen trasera del seguro médico (almacenada como bytes).

        // Constructor por defecto de Paciente.
        // Llama al constructor base de Usuario e inicializa la lista de historial médico.
        public Paciente() : base()
        {
            Historial = new List<RegistroMedico>(); // Inicializa la lista para el historial médico.
        }

        // Método para mostrar la información detallada del paciente en la consola.
        public void MostrarInformacion()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║         INFORMACION DEL PACIENTE                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine($"  ID: {Id}");
            Console.WriteLine($"  Nombre: {Nombre}");
            Console.WriteLine($"  Email: {Email}");
            Console.WriteLine($"  Edad: {Edad} años");
            Console.WriteLine($"  Telefono: {Telefono}");
            Console.WriteLine($"  Genero: {Genero}");
            Console.WriteLine($"  Tipo de Sangre: {FormatearTipoSangre(TipoSangre)}"); // Formatea el tipo de sangre para mostrar.
            Console.WriteLine($"  Seguro Medico: {FormatearSeguro(TipoSeguro)}"); // Formatea el tipo de seguro para mostrar.
            // Muestra el número de seguro solo si no está vacío.
            if (!string.IsNullOrEmpty(NumeroSeguro))
                Console.WriteLine($"  Numero de Seguro: {NumeroSeguro}");
            Console.WriteLine($"  Contacto Emergencia: {ContactoEmergencia}");
            Console.WriteLine($"  Registros Medicos: {Historial.Count}"); // Muestra la cantidad de registros en el historial.
            Console.WriteLine("════════════════════════════════════════════════════");
        }

        // Método auxiliar privado para formatear la enumeración TipoSangre a una cadena más legible.
        private string FormatearTipoSangre(TipoSangre tipo)
        {
            return tipo.ToString().Replace("_Positivo", "+").Replace("_Negativo", "-");
        }

        // Método auxiliar privado para formatear la enumeración TipoSeguro a una cadena más legible.
        private string FormatearSeguro(TipoSeguro tipo)
        {
            switch (tipo)
            {
                case TipoSeguro.SinSeguro: return "Sin Seguro";
                case TipoSeguro.SeguroBasico: return "Seguro Basico";
                case TipoSeguro.SeguroCompleto: return "Seguro Completo";
                default: return "No especificado"; // En caso de un valor de enumeración inesperado.
            }
        }
    }
}