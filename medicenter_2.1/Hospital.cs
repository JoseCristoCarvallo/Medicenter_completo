using System;
using System.Collections.Generic;

namespace MEDICENTER
{
    // Clase Hospital: representa un hospital en el sistema MediCenter.
    // Almacena información relevante sobre el hospital, su personal, pacientes y características.
    public class Hospital
    {
        public string Id { get; set; } // Identificador único del hospital.
        public string Nombre { get; set; } // Nombre del hospital.
        public bool EsPublico { get; set; } // Indica si el hospital es público (true) o privado (false).
        public decimal CostoConsulta { get; set; } // Costo de una consulta en el hospital (relevante para hospitales privados).
        public List<string> PersonalIds { get; set; } // Lista de IDs del personal asignado a este hospital.
        public List<string> PacientesAtendidos { get; set; } // Lista de IDs de pacientes que han sido atendidos o están en este hospital.
        public int PrecisionDiagnostico { get; set; } // Nivel de precisión diagnóstica del hospital (valor ficticio).
        public int TiempoPromedioMin { get; set; } // Tiempo promedio de espera en minutos (valor ficticio).

        // Constructor por defecto de Hospital.
        // Inicializa las listas de IDs de personal y pacientes atendidos.
        public Hospital()
        {
            PersonalIds = new List<string>(); // Inicializa la lista de IDs del personal.
            PacientesAtendidos = new List<string>(); // Inicializa la lista de IDs de pacientes atendidos.
        }

        // Método para mostrar información formateada del hospital en la consola.
        public void MostrarInformacion()
        {
            Console.WriteLine($"\n  [{Id}] {Nombre}"); // Muestra el ID y el nombre del hospital.
            // Indica si es público o privado.
            Console.WriteLine($"       Tipo: {(EsPublico ? "Hospital Publico" : "Hospital Privado")}");
            // Muestra el costo de consulta solo si es un hospital privado.
            if (!EsPublico)
                Console.WriteLine($"       Costo Consulta: ${CostoConsulta:F2}");
            // Muestra la precisión diagnóstica y el tiempo promedio.
            Console.WriteLine($"       Precision: {PrecisionDiagnostico}% | Tiempo: {TiempoPromedioMin} min");
            // Muestra la cantidad de personal y pacientes asociados.
            Console.WriteLine($"       Personal: {PersonalIds.Count} | Pacientes: {PacientesAtendidos.Count}");
        }

        // Sobrescribe el método ToString para proporcionar una representación de cadena legible del hospital.
        public override string ToString()
        {
            return $"[{Id}] {Nombre}"; // Retorna el ID y el nombre del hospital.
        }
    }
}
