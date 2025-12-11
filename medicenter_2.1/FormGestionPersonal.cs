using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // Necesario para .FirstOrDefault()
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para la gestión y visualización del personal hospitalario por parte de un administrador.
    // Muestra una lista de todo el personal y permite editar o eliminar registros al hacer doble clic.
    public partial class FormGestionPersonal : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a los datos de personal y hospitales.
        private PersonalHospitalario admin; // El objeto PersonalHospitalario que representa al administrador logueado.
        private ListBox listBoxPersonal; // Control ListBox para mostrar la lista del personal.

        // Constructor del formulario.
        public FormGestionPersonal(Sistema sistemaParam, PersonalHospitalario adminParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            admin = adminParam; // Asigna el objeto administrador.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "Gestión de Personal Hospitalario"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Gestión de Personal Hospitalario";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(900, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            int yPos = 80; // Posición vertical inicial para los controles.

            // ListBox para mostrar el personal.
            listBoxPersonal = new ListBox();
            listBoxPersonal.Location = new Point(50, yPos);
            listBoxPersonal.Size = new Size(800, 500);
            listBoxPersonal.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
            listBoxPersonal.MouseDoubleClick += ListBoxPersonal_MouseDoubleClick; // Asigna el evento de doble clic.
            this.Controls.Add(listBoxPersonal);

            CargarPersonal(); // Llama al método para cargar y mostrar la lista del personal.

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(390, 630);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Método para cargar y mostrar la información de todo el personal hospitalario en el ListBox.
        private void CargarPersonal()
        {
            listBoxPersonal.Items.Clear(); // Limpia los elementos existentes antes de recargar.

            if (sistema.Personal.Count == 0)
            {
                listBoxPersonal.Items.Add("No hay personal registrado en el sistema.");
                return;
            }

            // Añade cabeceras y separadores a la lista para una mejor presentación.
            listBoxPersonal.Items.Add("═══════════════════════════════════════════════════════════════");
            listBoxPersonal.Items.Add("                  PERSONAL HOSPITALARIO (TODOS)");
            listBoxPersonal.Items.Add("═══════════════════════════════════════════════════════════════");
            listBoxPersonal.Items.Add("");

            int contador = 1; // Contador para enumerar las entradas.
            // Itera sobre cada miembro del personal en el sistema.
            foreach (var p in sistema.Personal)
            {
                // Busca el hospital al que está asignado el personal para mostrar su nombre.
                Hospital hospitalAsignado = sistema.Hospitales.FirstOrDefault(h => h.Id == p.IdHospital);
                string nombreHospital = hospitalAsignado?.Nombre ?? "N/A"; // Obtiene el nombre o "N/A" si no se encuentra.

                // Añade la información formateada del personal al ListBox.
                listBoxPersonal.Items.Add($"{contador}. Rol: {p.NivelAcceso} | ID: {p.Id} | Nombre: {p.Nombre}");
                listBoxPersonal.Items.Add($"   Email: {p.Email}");
                listBoxPersonal.Items.Add($"   Contraseña: {new string('*', p.Password.Length)}"); // Muestra la contraseña enmascarada.
                listBoxPersonal.Items.Add($"   Hospital: {nombreHospital}");
                // Muestra la especialidad solo si no está vacía.
                if (!string.IsNullOrEmpty(p.Especialidad))
                    listBoxPersonal.Items.Add($"   Especialidad: {p.Especialidad}");
                listBoxPersonal.Items.Add(""); // Línea en blanco para separar entradas.
                contador++;
            }
        }

        // Manejador de eventos para el doble clic del ratón en un elemento del ListBox.
        // Permite abrir el formulario de edición para el personal seleccionado.
        private void ListBoxPersonal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Evita clics en elementos no válidos (ej. cabeceras o líneas vacías).
            if (listBoxPersonal.SelectedItem == null || listBoxPersonal.SelectedIndex < 3) 
            {
                return;
            }

            string lineaSeleccionada = listBoxPersonal.SelectedItem.ToString();
            string idPersonal = "";

            try
            {
                // Intenta extraer el ID del personal de la línea seleccionada.
                int startIndex = lineaSeleccionada.IndexOf("ID: ");
                if (startIndex != -1)
                {
                    string idPart = lineaSeleccionada.Substring(startIndex + "ID: ".Length);
                    idPersonal = idPart.Split(' ')[0].Trim(); // El ID es la primera palabra después de "ID: ".
                }
                else
                {
                    // Si la línea no contiene "ID: ", no es una línea de usuario válida para editar.
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al extraer el ID del personal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(idPersonal))
            {
                return; // No se pudo extraer un ID válido.
            }

            // Excepción: Evita que el propio administrador (ADMIN001) se edite o elimine a sí mismo desde aquí.
            if (idPersonal == admin.Id && idPersonal == "ADMIN001")
            {
                MessageBox.Show("No puede editar o eliminar la cuenta de 'ADMIN001' desde esta ventana. Use 'Cambiar mis datos' en el menú principal.", "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pide confirmación al administrador antes de abrir el formulario de edición.
            DialogResult confirmacion = MessageBox.Show($"¿Desea hacer cambios al usuario {idPersonal}?", "Confirmar Edición", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                // Busca el objeto PersonalHospitalario correspondiente al ID extraído.
                PersonalHospitalario personalAEditar = sistema.Personal.FirstOrDefault(p => p.Id == idPersonal);

                if (personalAEditar != null)
                {
                    // Abre el formulario FormEditarPersonal con el objeto de personal a editar.
                    FormEditarPersonal formEditarPersonal = new FormEditarPersonal(sistema, personalAEditar);
                    if (formEditarPersonal.ShowDialog() == DialogResult.OK)
                    {
                        CargarPersonal(); // Recarga la lista si se hicieron cambios o se eliminó un usuario.
                    }
                }
                else
                {
                    MessageBox.Show("Personal no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
