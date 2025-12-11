using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MEDICENTER
{
    // Clase encargada de la persistencia de datos del sistema mediante serialización binaria.
    // Guarda y carga objetos 'Usuario' (y sus tipos derivados) en archivos binarios individuales.
    public class PersistenciaBinaria
    {
        private readonly string _rutaEscritorio; // Ruta al directorio del Escritorio del usuario.
        private readonly string _rutaRaiz; // Ruta de la carpeta principal donde se guardarán todos los registros.

        // Constructor de la clase PersistenciaBinaria.
        public PersistenciaBinaria()
        {
            _rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Obtiene la ruta del Escritorio.
            _rutaRaiz = Path.Combine(_rutaEscritorio, "medicenter_registros"); // Define la ruta de la carpeta raíz de la aplicación.
        }

        // Asegura que la carpeta raíz 'medicenter_registros' exista en el Escritorio.
        public void AsegurarCarpetasDeDatos()
        {
            try
            {
                // Crea el directorio si no existe.
                Directory.CreateDirectory(_rutaRaiz);
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de que no se pueda crear el directorio.
                Console.WriteLine($"Error al crear el directorio raíz: {ex.Message}");
            }
        }

        // Guarda un objeto Usuario (o sus subclases como Paciente, PersonalHospitalario) en un archivo binario.
        public void GuardarRegistro(Usuario usuario)
        {
            // Valida que el usuario no sea nulo y tenga un ID.
            if (usuario == null || string.IsNullOrEmpty(usuario.Id)) return;

            // Construye la ruta de la carpeta específica para el usuario y el archivo de datos.
            string rutaUsuario = Path.Combine(_rutaRaiz, usuario.Id);
            string rutaArchivo = Path.Combine(rutaUsuario, "datos.bin");

            try
            {
                // Crea el directorio específico del usuario si no existe.
                Directory.CreateDirectory(rutaUsuario);
                // Abre un FileStream para escribir el objeto serializado.
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Create))
                {
                    // Deshabilita la advertencia sobre BinaryFormatter obsoleto.
#pragma warning disable SYSLIB0011
                    // Serializa el objeto 'usuario' al FileStream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, usuario);
#pragma warning restore SYSLIB0011
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de que falle la operación de guardado.
                Console.WriteLine($"Error al guardar el registro para {usuario.Id}: {ex.Message}");
            }
        }

        // Carga un objeto Usuario de un archivo binario dado su ID.
        public Usuario CargarRegistro(string idUsuario)
        {
            // Construye la ruta al archivo binario del usuario.
            string rutaArchivo = Path.Combine(_rutaRaiz, idUsuario, "datos.bin");

            // Si el archivo no existe, no hay registro que cargar.
            if (!File.Exists(rutaArchivo)) return null;

            try
            {
                // Abre un FileStream para leer el objeto serializado.
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Open))
                {
                    // Deshabilita la advertencia sobre BinaryFormatter obsoleto.
#pragma warning disable SYSLIB0011
                    // Deserializa el objeto del FileStream y lo retorna como Usuario.
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Usuario)formatter.Deserialize(fs);
#pragma warning restore SYSLIB0011
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de que falle la operación de carga.
                Console.WriteLine($"Error al cargar el registro para {idUsuario}: {ex.Message}");
                return null;
            }
        }

        // Carga todos los objetos Usuario (pacientes y personal) de todas las carpetas de registros.
        public List<Usuario> CargarTodosLosRegistros()
        {
            var listaUsuarios = new List<Usuario>();
            // Si la carpeta raíz no existe, no hay registros que cargar.
            if (!Directory.Exists(_rutaRaiz)) return listaUsuarios;

            try
            {
                // Obtiene todas las subcarpetas dentro de la carpeta raíz (cada una representa un usuario).
                string[] carpetasUsuarios = Directory.GetDirectories(_rutaRaiz);

                // Itera sobre cada carpeta de usuario.
                foreach (string carpeta in carpetasUsuarios)
                {
                    // Extrae el ID del usuario del nombre de la carpeta.
                    string idUsuario = new DirectoryInfo(carpeta).Name;
                    // Carga el registro de este usuario.
                    Usuario usuario = CargarRegistro(idUsuario);
                    if (usuario != null)
                    {
                        listaUsuarios.Add(usuario); // Añade el usuario cargado a la lista.
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores al cargar todos los registros.
                Console.WriteLine($"Error al cargar todos los registros: {ex.Message}");
            }

            return listaUsuarios;
        }

        // Elimina el registro de un usuario (su carpeta completa) dado su ID.
        public void EliminarRegistro(string idUsuario)
        {
            // Valida que el ID del usuario no sea nulo o vacío.
            if (string.IsNullOrEmpty(idUsuario)) return;

            // Construye la ruta de la carpeta del usuario a eliminar.
            string rutaUsuario = Path.Combine(_rutaRaiz, idUsuario);

            try
            {
                // Si la carpeta del usuario existe, la elimina recursivamente.
                if (Directory.Exists(rutaUsuario))
                {
                    Directory.Delete(rutaUsuario, true); // 'true' para eliminación recursiva.
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores al eliminar el registro.
                Console.WriteLine($"Error al eliminar el registro para {idUsuario}: {ex.Message}");
            }
        }
    }
}
