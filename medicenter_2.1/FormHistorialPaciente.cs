using System.Drawing;
using System.Linq; // Necesario para OrderBy y FirstOrDefault
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para mostrar el historial médico de un paciente específico.
    // Muestra una lista detallada de todos los registros médicos del paciente.
    public partial class FormHistorialPaciente : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a datos de hospitales y personal.
        private Paciente paciente; // El objeto Paciente cuyo historial se va a mostrar.
        private ListBox listBoxHistorial; // Control ListBox para mostrar los registros del historial.

        // Constructor del formulario.
        public FormHistorialPaciente(Sistema sistemaParam, Paciente pacienteParam) // Constructor modificado
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            paciente = pacienteParam; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(1100, 800); // Tamaño del formulario ampliado.
            this.Text = "Historial Médico"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.Sizable; // Permite redimensionar la ventana.
            this.MaximizeBox = true; // Habilita el botón maximizar.
            this.AutoScroll = true; // Habilita scroll automático.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Historial Médico";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.Location = new Point(420, 20);
            lblTitulo.Size = new Size(260, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Verifica si el paciente tiene registros médicos.
            if (paciente.Historial.Count == 0)
            {
                // Muestra un mensaje si no hay registros.
                Label lblSinRegistros = new Label();
                lblSinRegistros.Text = "No hay registros médicos";
                lblSinRegistros.Font = new Font("Segoe UI", 14);
                lblSinRegistros.Location = new Point(300, 300);
                lblSinRegistros.Size = new Size(300, 40);
                lblSinRegistros.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(lblSinRegistros);
            }
            else
            {
                // ListBox para mostrar el historial.
                listBoxHistorial = new ListBox();
                listBoxHistorial.Location = new Point(50, 80);
                listBoxHistorial.Size = new Size(1000, 600);
                listBoxHistorial.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
                listBoxHistorial.SelectionMode = SelectionMode.One; // Permite seleccionar un solo elemento.
                listBoxHistorial.MouseDoubleClick += ListBoxHistorial_MouseDoubleClick; // Evento para editar registro.
                listBoxHistorial.HorizontalScrollbar = true; // Habilita scroll horizontal.

                // Ordena el historial por fecha de manera descendente (los más recientes primero).
                var historialOrdenado = paciente.Historial.OrderByDescending(r => r.Fecha).ToList();

                // Itera sobre cada registro en el historial ordenado.
                foreach (var registro in historialOrdenado) 
                {
                    string nombreMedico = "Desconocido"; // Valor por defecto si no se encuentra el médico.
                    if (!string.IsNullOrEmpty(registro.IdMedico))
                    {
                        // Busca el objeto PersonalHospitalario (médico) para mostrar su nombre.
                        var medicoAtendio = sistema.Personal.FirstOrDefault(p => p.Id == registro.IdMedico);
                        nombreMedico = medicoAtendio?.Nombre ?? registro.IdMedico; // Muestra el nombre o el ID.
                    }

                    // Añade la información formateada del registro médico al ListBox.
                    listBoxHistorial.Items.Add("──────────────────────────────────────────────");
                    listBoxHistorial.Items.Add($"ID Registro: {registro.IdRegistro}");
                    listBoxHistorial.Items.Add($"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm}");
                    // Muestra el nombre del hospital o su ID si no se encuentra.
                    listBoxHistorial.Items.Add($"Hospital: {sistema.Hospitales.FirstOrDefault(h => h.Id == registro.IdHospital)?.Nombre ?? registro.IdHospital}");
                    listBoxHistorial.Items.Add($"Síntomas: {string.Join(", ", registro.Sintomas)}"); // Une los síntomas en una cadena.
                    listBoxHistorial.Items.Add($"Diagnóstico: {registro.Diagnostico}");
                    // Muestra el tratamiento solo si no está vacío.
                    if (!string.IsNullOrEmpty(registro.Tratamiento))
                        listBoxHistorial.Items.Add($"Tratamiento: {registro.Tratamiento}");
                    // Muestra el estado de confirmación del registro.
                    listBoxHistorial.Items.Add(
                        $"Estado: {(registro.Confirmado ? "Confirmado" : "Pendiente")}");
                    listBoxHistorial.Items.Add($"Médico: {nombreMedico}"); // Muestra el nombre del médico.
                    // Muestra las observaciones del doctor solo si no están vacías.
                    if (!string.IsNullOrEmpty(registro.ObservacionDoctor))
                        listBoxHistorial.Items.Add($"Observaciones: {registro.ObservacionDoctor}");
                    listBoxHistorial.Items.Add(""); // Línea en blanco para separar registros.
                }

                this.Controls.Add(listBoxHistorial);
            }

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(490, 710);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Manejador de eventos para el doble clic en el ListBox de historial.
        // Permite ver/editar un registro médico seleccionado.
        private void ListBoxHistorial_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxHistorial == null || listBoxHistorial.SelectedItem == null)
                return;

            string lineaSeleccionada = listBoxHistorial.SelectedItem.ToString();

            // Verifica si la línea contiene un ID de registro
            if (!lineaSeleccionada.Contains("ID Registro:"))
                return;

            try
            {
                // Extrae el ID del registro de la línea
                int startIndex = lineaSeleccionada.IndexOf("ID Registro: ");
                if (startIndex == -1)
                    return;

                string idPart = lineaSeleccionada.Substring(startIndex + "ID Registro: ".Length);
                string idRegistro = idPart.Split(new[] { '\n', ' ' }, System.StringSplitOptions.RemoveEmptyEntries)[0].Trim();

                // Busca el registro en el historial del paciente
                RegistroMedico registroSeleccionado = paciente.Historial.FirstOrDefault(r => r.IdRegistro == idRegistro);

                if (registroSeleccionado == null)
                {
                    MessageBox.Show("Registro no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Abre un formulario de diálogo para editar el registro
                FormEditarRegistroMedico formEditar = new FormEditarRegistroMedico(sistema, paciente, registroSeleccionado);
                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    // Recarga el formulario para mostrar los cambios
                    this.Controls.Clear();
                    InitializeComponent();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error al procesar el registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}