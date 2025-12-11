using System;

namespace MEDICENTER
{
    // Enumeración que define los tipos de seguro médico disponibles.
    public enum TipoSeguro
    {
        SinSeguro,      // El paciente no tiene ningún tipo de seguro.
        SeguroBasico,   // El paciente tiene un seguro básico (posiblemente cubre solo hospitales públicos).
        SeguroCompleto  // El paciente tiene un seguro completo (cubre hospitales públicos y privados).
    }

    // Enumeración serializable que define los tipos de sangre.
    [Serializable]
    public enum TipoSangre
    {
        A_Positivo,
        A_Negativo,
        B_Positivo,
        B_Negativo,
        O_Positivo,
        O_Negativo,
        AB_Positivo,
        AB_Negativo
    }

    // Enumeración que define las opciones de género.
    public enum Genero
    {
        Masculino,
        Femenino,
        Otro
    }

    // Enumeración que define los niveles de acceso para el personal hospitalario.
    public enum NivelAcceso
    {
        MedicoGeneral,  // Personal con acceso de médico general.
        Administrador   // Personal con acceso de administrador.
    }

    // Clase base serializable para todos los usuarios del sistema (pacientes y personal).
    // Contiene propiedades comunes a todos los usuarios para autenticación y registro.
    [Serializable]
    public class Usuario
    {
        public string Id { get; set; } // Identificador único del usuario.
        public string Nombre { get; set; } // Nombre completo del usuario.
        public string Email { get; set; } // Correo electrónico del usuario (puede usarse como nombre de usuario).
        public string Password { get; set; } // Contraseña del usuario.
        public DateTime FechaRegistro { get; set; } // Fecha y hora en que se registró el usuario.

        // Constructor por defecto de Usuario. Inicializa la fecha de registro con la hora actual.
        public Usuario()
        {
            FechaRegistro = DateTime.Now;
        }

        // Constructor de Usuario con parámetros para inicializar todas las propiedades básicas.
        public Usuario(string id, string nombre, string email, string password)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
            Password = password;
            FechaRegistro = DateTime.Now; // Inicializa la fecha de registro con la hora actual.
        }
    }
}