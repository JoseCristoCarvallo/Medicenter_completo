using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // Necesario para FirstOrDefault y Where
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el personal médico revise y valide diagnósticos preliminares pendientes
    // generados por el sistema automático, o para atender directamente al paciente asociado.
    public partial class FormValidarDiagnosticos : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a los datos de registros, pacientes y personal.
        private PersonalHospitalario medico; // El objeto PersonalHospitalario (médico) logueado.
        private ListBox listBoxPendientes; // Control ListBox para mostrar los diagnósticos pendientes de validación.

        // Constructor del formulario.
        public FormValidarDiagnosticos(Sistema sistemaParam, PersonalHospitalario medicoParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            medico = medicoParam; // Asigna el objeto médico.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "Historial de Diagnósticos"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Historial de Diagnósticos";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(280, 20);
            lblTitulo.Size = new Size(340, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // ListBox para mostrar los diagnósticos pendientes.
            listBoxPendientes = new ListBox();
            listBoxPendientes.Location = new Point(50, 80);
            listBoxPendientes.Size = new Size(800, 550);
            listBoxPendientes.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
            listBoxPendientes.MouseDoubleClick += ListBoxPendientes_MouseDoubleClick; // Asigna el evento de doble clic.
            this.Controls.Add(listBoxPendientes);

            CargarPendientes(); // Llama al método para cargar los diagnósticos pendientes.

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(390, 640);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Carga y muestra TODOS los diagnósticos del hospital con su estado de revisión.
        private void CargarPendientes()
        {
            listBoxPendientes.Items.Clear(); // Limpia la lista antes de recargar.

            // Verifica si hay registros asociados al hospital del médico.
            if (!sistema.RegistrosPorHospital.ContainsKey(medico.IdHospital))
            {
                listBoxPendientes.Items.Add("No hay registros en este hospital");
                return;
            }

            // Obtiene TODOS los registros del hospital
            List<RegistroMedico> todosLosRegistros = sistema.RegistrosPorHospital[medico.IdHospital]
                .OrderByDescending(r => r.Fecha).ToList(); // Ordenados por fecha descendente (más recientes primero)

            // Verifica si hay registros.
            if (!todosLosRegistros.Any())
            {
                listBoxPendientes.Items.Add("No hay registros en este hospital");
                return;
            }

            // Cuenta registros por estado
            int pendientes = todosLosRegistros.Count(r => !r.Confirmado);
            int revisados = todosLosRegistros.Count(r => r.Confirmado);

            // Muestra estadísticas
            listBoxPendientes.Items.Add($"═══════════════════════════════════════════════");
            listBoxPendientes.Items.Add($"Total de registros: {todosLosRegistros.Count}");
            listBoxPendientes.Items.Add($"En revisión: {pendientes} | Revisados: {revisados}");
            listBoxPendientes.Items.Add("═══════════════════════════════════════════════");
            listBoxPendientes.Items.Add("");
            listBoxPendientes.Items.Add("Haga doble clic en un ID de registro para editarlo");
            listBoxPendientes.Items.Add("");

            // Itera sobre cada registro y lo muestra en el ListBox.
            foreach (var registro in todosLosRegistros)
            {
                // Busca el paciente asociado al registro para mostrar su nombre.
                Paciente paciente = sistema.Pacientes.FirstOrDefault(p => p.Id == registro.IdPaciente);
                string nombrePaciente = paciente?.Nombre ?? registro.IdPaciente; // Muestra el nombre o el ID.

                // Determina el estado del registro
                string estado = registro.Confirmado ? "✓ REVISADO" : "⏳ EN REVISIÓN";
                string colorEstado = registro.Confirmado ? "[COMPLETADO]" : "[PENDIENTE]";

                // Añade la información formateada del registro al ListBox.
                listBoxPendientes.Items.Add("───────────────────────────────────────────────");
                listBoxPendientes.Items.Add($"ID Registro: {registro.IdRegistro}  |  Estado: {estado}");
                listBoxPendientes.Items.Add($"Paciente: {nombrePaciente} (ID: {registro.IdPaciente})");
                listBoxPendientes.Items.Add($"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm}");

                // Muestra el diagnóstico (confirmado o preliminar)
                if (!string.IsNullOrEmpty(registro.DiagnosticoConfirmado))
                {
                    listBoxPendientes.Items.Add($"Diagnóstico confirmado: {registro.DiagnosticoConfirmado}");
                }
                else
                {
                    listBoxPendientes.Items.Add($"Diagnóstico preliminar: {registro.Diagnostico}");
                }

                listBoxPendientes.Items.Add(""); // Línea en blanco para separar entradas.
            }
        }

        // Manejador de eventos para el doble clic del ratón en un elemento del ListBox.
        // Abre el formulario de edición del registro médico seleccionado.
        private void ListBoxPendientes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Ignora clics en las líneas de cabecera.
            if (listBoxPendientes.SelectedItem == null || listBoxPendientes.SelectedIndex < 8)
            {
                return;
            }

            string lineaSeleccionada = listBoxPendientes.SelectedItem.ToString();

            // Verifica si la línea contiene un ID de registro
            if (!lineaSeleccionada.Contains("ID Registro:"))
            {
                return;
            }

            string idRegistro = "";

            try
            {
                // Extrae el ID del registro de la línea seleccionada.
                int startIndex = lineaSeleccionada.IndexOf("ID Registro: ");
                if (startIndex != -1)
                {
                    string idPart = lineaSeleccionada.Substring(startIndex + "ID Registro: ".Length);
                    // Extrae solo el ID antes del separador "|"
                    idRegistro = idPart.Split(new[] { '|', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                }
                else
                {
                    return; // No es una línea de registro válida.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al extraer ID de registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(idRegistro))
            {
                return; // No se pudo extraer un ID válido.
            }

            // Busca el objeto RegistroMedico correspondiente al ID.
            RegistroMedico registroSeleccionado = sistema.RegistrosPorHospital[medico.IdHospital]
                                                        .FirstOrDefault(r => r.IdRegistro == idRegistro);

            if (registroSeleccionado == null)
            {
                MessageBox.Show("Registro no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Busca el paciente asociado al registro
            Paciente paciente = sistema.Pacientes.FirstOrDefault(p => p.Id == registroSeleccionado.IdPaciente);
            if (paciente == null)
            {
                MessageBox.Show("Paciente asociado al registro no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Abre el formulario de edición del registro médico
            FormEditarRegistroMedico formEditar = new FormEditarRegistroMedico(sistema, paciente, registroSeleccionado);
            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                // Recarga la lista después de editar
                CargarPendientes();
            }
        }

        // Abre el formulario FormAtenderPaciente para que el médico atienda al paciente asociado al registro.
        private void AtenderPaciente(RegistroMedico registro)
        {
            // Busca el paciente asociado al registro.
            Paciente paciente = sistema.Pacientes.FirstOrDefault(p => p.Id == registro.IdPaciente);
            if (paciente == null)
            {
                MessageBox.Show("Paciente asociado al registro no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Abre el formulario de atención al paciente, pasando el sistema, médico, paciente y registro.
            FormAtenderPaciente formAtender = new FormAtenderPaciente(sistema, medico, paciente, registro);
            formAtender.ShowDialog();
            CargarPendientes(); // Recarga la lista de pendientes después de atender al paciente.
        }

        // Abre un diálogo para que el médico valide el registro, agregando observaciones y tratamiento.
        private void ValidarRegistro(RegistroMedico registro)
        {
            // Crea un diálogo para obtener las observaciones y el tratamiento del médico.
            FormDialogoValidacion formDialogo = new FormDialogoValidacion(registro);
            if (formDialogo.ShowDialog() == DialogResult.OK) // Si el médico acepta en el diálogo.
            {
                registro.Confirmado = true; // Marca el registro como confirmado.
                registro.IdMedico = medico.Id; // Asigna el ID del médico validador.
                registro.ObservacionDoctor = formDialogo.Observaciones; // Asigna las observaciones.
                registro.Tratamiento = formDialogo.Tratamiento; // Asigna el tratamiento.

                // Persiste los cambios del registro en el historial del paciente.
                Paciente paciente = sistema.Pacientes.FirstOrDefault(p => p.Id == registro.IdPaciente);
                if (paciente != null)
                {
                    // Encuentra el registro específico en el historial del paciente y lo actualiza.
                    var regEnHistorial = paciente.Historial.FirstOrDefault(r => r.IdRegistro == registro.IdRegistro);
                    if (regEnHistorial != null)
                    {
                        regEnHistorial.Confirmado = registro.Confirmado;
                        regEnHistorial.IdMedico = registro.IdMedico;
                        regEnHistorial.ObservacionDoctor = registro.ObservacionDoctor;
                        regEnHistorial.Tratamiento = registro.Tratamiento;
                    }
                    sistema.GuardarUsuario(paciente); // Guarda el paciente actualizado.

                    // También se guarda el objeto médico (aunque en este flujo no se modifica, es buena práctica).
                    sistema.GuardarUsuario(medico);

                    MessageBox.Show("Diagnóstico validado y registro actualizado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarPendientes(); // Recarga la lista de pendientes después de la validación.
                }
                else
                {
                    MessageBox.Show("Paciente asociado al registro no encontrado, no se pudo guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}