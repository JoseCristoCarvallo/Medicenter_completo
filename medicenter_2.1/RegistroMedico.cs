using System;
using System.Collections.Generic;

namespace MEDICENTER
{
    // Clase RegistroMedico: representa una entrada en el historial médico de un paciente.
    // Es serializable para permitir su persistencia.
    [Serializable]
    public class RegistroMedico
    {
        public string IdRegistro { get; set; } // Identificador único del registro médico.
        public string IdPaciente { get; set; } // ID del paciente al que pertenece este registro.
        public string IdHospital { get; set; } // ID del hospital donde se realizó el registro.
        public DateTime Fecha { get; set; } // Fecha y hora en que se realizó el registro.
        public List<string> Sintomas { get; set; } // Lista de síntomas reportados en este registro.
        public string Diagnostico { get; set; } // Diagnóstico automático inicial (Naive Bayes).
        public string DiagnosticoConfirmado { get; set; } // Diagnóstico confirmado/corregido por el médico.
        public bool DiagnosticoCoincide { get; set; } // Indica si el diagnóstico automático coincidió con el del médico.
        public string Tratamiento { get; set; } // Tratamiento prescrito.
        public bool Confirmado { get; set; } // Indica si el diagnóstico ha sido confirmado por un médico.
        public string IdMedico { get; set; } // ID del médico que realizó o confirmó el registro.
        public string ObservacionDoctor { get; set; } // Observaciones adicionales del médico.

        // Constructor por defecto de RegistroMedico.
        // Inicializa la lista de síntomas, la fecha con la hora actual y el estado de confirmación.
        public RegistroMedico()
        {
            Sintomas = new List<string>(); // Inicializa la lista de síntomas.
            Fecha = DateTime.Now; // Establece la fecha actual.
            Confirmado = false; // Por defecto, el diagnóstico no está confirmado.
            DiagnosticoCoincide = false; // Por defecto, no se ha validado aún.
            DiagnosticoConfirmado = ""; // Inicializa vacío.
        }

        // Método para mostrar la información del registro médico de manera formateada en la consola.
        public void MostrarRegistro()
        {
            Console.WriteLine("\n────────────────────────────────────────────────────");
            Console.WriteLine($"  ID Registro: {IdRegistro}");
            Console.WriteLine($"  Fecha: {Fecha:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"  Hospital: {IdHospital}");
            Console.WriteLine($"  Sintomas: {string.Join(", ", Sintomas)}"); // Une los síntomas en una cadena.
            Console.WriteLine($"  Diagnostico: {Diagnostico}");
            // Muestra el diagnóstico confirmado si existe
            if (!string.IsNullOrEmpty(DiagnosticoConfirmado))
                Console.WriteLine($"  Diagnostico Confirmado: {DiagnosticoConfirmado}");
            // Muestra el tratamiento solo si no está vacío.
            if (!string.IsNullOrEmpty(Tratamiento))
                Console.WriteLine($"  Tratamiento: {Tratamiento}");
            // Muestra el estado de confirmación.
            Console.WriteLine($"  Estado: {(Confirmado ? "Confirmado" : "Pendiente")}");
            // Muestra si coincidió el diagnóstico
            if (Confirmado)
                Console.WriteLine($"  Diagnostico Coincide: {(DiagnosticoCoincide ? "Si" : "No")}");
            // Muestra el ID del médico y sus observaciones si están presentes.
            if (!string.IsNullOrEmpty(IdMedico))
                Console.WriteLine($"  Medico: {IdMedico}");
            if (!string.IsNullOrEmpty(ObservacionDoctor))
                Console.WriteLine($"  Observaciones: {ObservacionDoctor}");
            Console.WriteLine("────────────────────────────────────────────────────");
        }
    }
}
