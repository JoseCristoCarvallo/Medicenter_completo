using System;
using System.Collections.Generic;

namespace MEDICENTER
{
    // Clase PersonalHospitalario: representa al personal de un hospital (médicos, administradores, etc.).
    // Hereda de la clase base 'Usuario' para incluir funcionalidades de autenticación.
    // NOTA: Aunque la clase se llama PersonalHospitalario, está definida en el archivo Doctor.cs.
    [Serializable]
    public class PersonalHospitalario : Usuario
    {
        public string IdHospital { get; set; } // ID del hospital al que el personal está asignado.
        public NivelAcceso NivelAcceso { get; set; } // Nivel de acceso del personal (ej. MédicoGeneral, Administrador).
        public string Especialidad { get; set; } // Especialidad del personal, si aplica (ej. "Cardiología" para médicos).
        public List<string> PacientesAsignados { get; set; } // Lista de IDs de pacientes asignados a este personal.
        public bool CambioPassword { get; set; } // Indica si el personal debe cambiar su contraseña al iniciar sesión.

        // Constructor por defecto de PersonalHospitalario.
        // Llama al constructor base de Usuario e inicializa las listas y flags.
        public PersonalHospitalario() : base()
        {
            PacientesAsignados = new List<string>(); // Inicializa la lista de pacientes asignados.
            CambioPassword = false; // Por defecto, no se requiere cambio de contraseña.
        }

        // Método para obtener la información formateada del personal.
        // Utilizado para mostrar los detalles del personal de una manera legible.
        public string ObtenerInformacionFormateada()
        {
            string info = "\n╔════════════════════════════════════════════════════╗\n";
            info += "║         INFORMACIÓN DEL PERSONAL                   ║\n";
            info += "╚════════════════════════════════════════════════════╝\n";
            info += $"  ID: {Id}\n";
            info += $"  Nombre: {Nombre}\n";
            info += $"  Email: {Email}\n";
            info += $"  Hospital: {IdHospital}\n";
            info += $"  Nivel de Acceso: {NivelAcceso}\n";
            // Solo añade la especialidad si no está vacía.
            if (!string.IsNullOrEmpty(Especialidad))
                info += $"  Especialidad: {Especialidad}\n";
            info += $"  Pacientes Asignados: {PacientesAsignados.Count}\n";
            info += "════════════════════════════════════════════════════";
            return info;
        }
    }
}