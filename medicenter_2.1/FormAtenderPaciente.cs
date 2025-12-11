using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el personal médico (doctor) atienda a un paciente.
    // Permite al médico revisar el registro inicial, establecer un diagnóstico final, tratamiento y observaciones,
    // y luego confirmar y guardar el registro. También permite devolver al paciente a la cola.
    public partial class FormAtenderPaciente : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y datos.
        private PersonalHospitalario medico; // Objeto PersonalHospitalario que representa al médico logueado.
        private Paciente? paciente; // Objeto Paciente que está siendo atendido (puede ser nulo si no hay paciente válido).
        private RegistroMedico? registro; // Objeto RegistroMedico que se está procesando (puede ser nulo).
        
        // Campos de texto para que el médico ingrese la información.
        private TextBox? txtDiagnostico;
        private TextBox? txtTratamiento;
        private TextBox? txtObservaciones;

        // Controles para validación de enfermedad
        private ComboBox? cmbEnfermedadConfirmada;
        private TextBox? txtEnfermedadManual;
        private CheckBox? chkDiagnosticoCoincide;

        // Nuevo constructor: Para usar cuando el paciente y el registro ya están definidos (ej. desde ValidarDiagnosticos).
        public FormAtenderPaciente(Sistema sistemaParam, PersonalHospitalario medicoParam, Paciente pacienteParam, RegistroMedico registroParam)
        {
            sistema = sistemaParam;
            medico = medicoParam;
            paciente = pacienteParam;
            registro = registroParam;
            InitializeComponent(); // Inicializa la interfaz de usuario.
            CargarDatosPacienteYRegistro(); // Carga los datos iniciales en los controles UI.
        }

        // Constructor existente: Para cargar un paciente desde la cola de espera del hospital.
        public FormAtenderPaciente(Sistema sistemaParam, PersonalHospitalario medicoParam)
        {
            sistema = sistemaParam;
            medico = medicoParam;
            InitializeComponent(); // Inicializar la UI primero, antes de intentar cargar datos de la cola.

            // Lógica para cargar paciente y registro desde la cola DESPUÉS de InitializeComponent.
            // Verifica si el hospital del médico tiene una cola de pacientes y si esta no está vacía.
            if (!sistema.ColasPorHospital.ContainsKey(medico.IdHospital) ||
                !sistema.ColasPorHospital[medico.IdHospital].Any())
            {
                // Si no hay pacientes en cola, 'paciente' y 'registro' permanecerán como null.
                // La UI mostrará el mensaje "No hay pacientes en cola...".
            }
            else
            {
                // Desencola la clave del paciente (formato "IDPaciente|IDRegistro").
                string clave = sistema.ColasPorHospital[medico.IdHospital].Dequeue();
                string[] partes = clave.Split('|');
                string idPaciente = partes[0];
                string idRegistro = partes[1];

                // Busca el paciente en el sistema.
                paciente = sistema.Pacientes.FirstOrDefault(p => p.Id == idPaciente);
                
                // Si se encuentra el paciente, busca el registro médico asociado.
                if (paciente != null)
                {
                    registro = paciente.Historial.FirstOrDefault(r => r.IdRegistro == idRegistro);
                }
            }

            // Si después de intentar cargar, el paciente o el registro son nulos, cierra el formulario
            // y muestra un mensaje de error.
            if (paciente == null || registro == null)
            {
                MessageBox.Show("No se encontró un paciente o registro válido para atender de la cola.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Cierra el formulario si no hay datos válidos.
                return;
            }
            CargarDatosPacienteYRegistro(); // Carga los datos en la UI si el paciente y el registro existen.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 750); // Tamaño del formulario.
            this.Text = "Atender Paciente"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.AutoScroll = true; // Habilita el scroll vertical para ver todo el contenido.

            // Muestra un mensaje "No hay pacientes en cola" si no hay paciente o registro válidos.
            // Esto solo se activa si el formulario se abre sin un paciente/registro precargado desde la cola.
            if (paciente == null || registro == null)
            {
                Label lblSinPacientes = new Label();
                lblSinPacientes.Text = "No hay pacientes en cola para atender.";
                lblSinPacientes.Font = new Font("Segoe UI", 14);
                lblSinPacientes.Location = new Point(300, 300);
                lblSinPacientes.Size = new Size(300, 40);
                this.Controls.Add(lblSinPacientes);

                // Botón para cerrar el formulario si no hay pacientes.
                Button btnCerrar1 = new Button();
                btnCerrar1.Text = "Cerrar";
                btnCerrar1.Font = new Font("Segoe UI", 12);
                btnCerrar1.Location = new Point(390, 360);
                btnCerrar1.Size = new Size(120, 45);
                btnCerrar1.BackColor = Color.White;
                btnCerrar1.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
                this.Controls.Add(btnCerrar1);
                return; // Salir de InitializeComponent para no cargar el resto de la UI si no hay datos.
            }
            
            // Título del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Atender Paciente";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(330, 20);
            lblTitulo.Size = new Size(240, 40);
            this.Controls.Add(lblTitulo);

            // Información básica del paciente.
            Label lblInfoPaciente = new Label();
            lblInfoPaciente.Text =
                $"Paciente: {paciente.Nombre}\nID: {paciente.Id} | Edad: {paciente.Edad}";
            lblInfoPaciente.Font = new Font("Segoe UI", 11);
            lblInfoPaciente.Location = new Point(50, 80);
            lblInfoPaciente.Size = new Size(400, 50);
            this.Controls.Add(lblInfoPaciente);

            // Panel para mostrar los detalles del registro médico actual.
            Panel panelRegistro = new Panel();
            panelRegistro.Location = new Point(50, 140);
            panelRegistro.Size = new Size(800, 230);
            panelRegistro.BorderStyle = BorderStyle.FixedSingle;
            panelRegistro.BackColor = Color.White;
            panelRegistro.AutoScroll = true;
            this.Controls.Add(panelRegistro);

            int yPosPanel = 10;

            // Título de la sección
            Label lblInfoRegistro = new Label();
            lblInfoRegistro.Text = "INFORMACIÓN DEL REGISTRO";
            lblInfoRegistro.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblInfoRegistro.Location = new Point(10, yPosPanel);
            lblInfoRegistro.Size = new Size(750, 25);
            lblInfoRegistro.ForeColor = Color.FromArgb(70, 130, 180);
            panelRegistro.Controls.Add(lblInfoRegistro);
            yPosPanel += 35;

            // ID y Fecha
            Label lblIdFecha = new Label();
            lblIdFecha.Text = $"ID Registro: {registro.IdRegistro} | Fecha: {registro.Fecha:dd/MM/yyyy HH:mm}";
            lblIdFecha.Font = new Font("Segoe UI", 10);
            lblIdFecha.Location = new Point(10, yPosPanel);
            lblIdFecha.Size = new Size(750, 25);
            panelRegistro.Controls.Add(lblIdFecha);
            yPosPanel += 30;

            // Síntomas
            Label lblSintomasTitulo = new Label();
            lblSintomasTitulo.Text = "Síntomas:";
            lblSintomasTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSintomasTitulo.Location = new Point(10, yPosPanel);
            lblSintomasTitulo.Size = new Size(150, 25);
            panelRegistro.Controls.Add(lblSintomasTitulo);
            yPosPanel += 25;

            Label lblSintomas = new Label();
            lblSintomas.Text = string.Join(", ", registro.Sintomas);
            lblSintomas.Font = new Font("Segoe UI", 10);
            lblSintomas.Location = new Point(10, yPosPanel);
            lblSintomas.Size = new Size(750, 25);
            panelRegistro.Controls.Add(lblSintomas);
            yPosPanel += 30;

            // Separar diagnóstico y descripción del paciente
            string diagnosticoSolo = registro.Diagnostico;
            string descripcionPaciente = "";

            // Verificar si hay descripción del paciente en el diagnóstico
            if (registro.Diagnostico.Contains("Descripción del paciente:") ||
                registro.Diagnostico.Contains("Descripcion del paciente:"))
            {
                int indiceDescripcion = registro.Diagnostico.IndexOf("Descripción del paciente:", StringComparison.OrdinalIgnoreCase);
                if (indiceDescripcion == -1)
                {
                    indiceDescripcion = registro.Diagnostico.IndexOf("Descripcion del paciente:", StringComparison.OrdinalIgnoreCase);
                }

                if (indiceDescripcion >= 0)
                {
                    diagnosticoSolo = registro.Diagnostico.Substring(0, indiceDescripcion).Trim();
                    descripcionPaciente = registro.Diagnostico.Substring(indiceDescripcion).Replace("Descripción del paciente:", "").Replace("Descripcion del paciente:", "").Trim();
                }
            }

            // Diagnóstico Automático
            Label lblDiagnosticoTitulo = new Label();
            lblDiagnosticoTitulo.Text = "Diagnóstico Automático:";
            lblDiagnosticoTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDiagnosticoTitulo.Location = new Point(10, yPosPanel);
            lblDiagnosticoTitulo.Size = new Size(200, 25);
            panelRegistro.Controls.Add(lblDiagnosticoTitulo);
            yPosPanel += 25;

            TextBox txtDiagnosticoInfo = new TextBox();
            txtDiagnosticoInfo.Text = diagnosticoSolo;
            txtDiagnosticoInfo.Font = new Font("Segoe UI", 9);
            txtDiagnosticoInfo.Location = new Point(10, yPosPanel);
            txtDiagnosticoInfo.Size = new Size(750, 80);
            txtDiagnosticoInfo.Multiline = true;
            txtDiagnosticoInfo.ScrollBars = ScrollBars.Vertical;
            txtDiagnosticoInfo.ReadOnly = true;
            txtDiagnosticoInfo.BackColor = Color.FromArgb(240, 255, 240);
            panelRegistro.Controls.Add(txtDiagnosticoInfo);
            yPosPanel += 90;

            // Descripción del Paciente (si existe)
            if (!string.IsNullOrWhiteSpace(descripcionPaciente))
            {
                Label lblDescripcionTitulo = new Label();
                lblDescripcionTitulo.Text = "Descripción del Paciente:";
                lblDescripcionTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblDescripcionTitulo.Location = new Point(10, yPosPanel);
                lblDescripcionTitulo.Size = new Size(200, 25);
                panelRegistro.Controls.Add(lblDescripcionTitulo);
                yPosPanel += 25;

                TextBox txtDescripcionInfo = new TextBox();
                txtDescripcionInfo.Text = descripcionPaciente;
                txtDescripcionInfo.Font = new Font("Segoe UI", 9);
                txtDescripcionInfo.Location = new Point(10, yPosPanel);
                txtDescripcionInfo.Size = new Size(750, 60);
                txtDescripcionInfo.Multiline = true;
                txtDescripcionInfo.ScrollBars = ScrollBars.Vertical;
                txtDescripcionInfo.ReadOnly = true;
                txtDescripcionInfo.BackColor = Color.FromArgb(255, 250, 240);
                panelRegistro.Controls.Add(txtDescripcionInfo);
                yPosPanel += 70;
            }

            int yPos = 390; // Posición vertical inicial para los siguientes controles.

            // Campo para el "Diagnóstico Final".
            Label lblDiagnostico = new Label();
            lblDiagnostico.Text = "Diagnóstico Final:";
            lblDiagnostico.Font = new Font("Segoe UI", 11);
            lblDiagnostico.Location = new Point(50, yPos);
            lblDiagnostico.Size = new Size(150, 25);
            this.Controls.Add(lblDiagnostico);

            txtDiagnostico = new TextBox();
            txtDiagnostico.Font = new Font("Segoe UI", 10);
            txtDiagnostico.Location = new Point(220, yPos);
            txtDiagnostico.Size = new Size(630, 25);
            txtDiagnostico.Text = registro.Diagnostico; // Rellena con el diagnóstico inicial.
            this.Controls.Add(txtDiagnostico);

            yPos += 40;

            // Campo para el "Tratamiento".
            Label lblTratamiento = new Label();
            lblTratamiento.Text = "Tratamiento:";
            lblTratamiento.Font = new Font("Segoe UI", 11);
            lblTratamiento.Location = new Point(50, yPos);
            lblTratamiento.Size = new Size(150, 25);
            this.Controls.Add(lblTratamiento);

            txtTratamiento = new TextBox();
            txtTratamiento.Font = new Font("Segoe UI", 10);
            txtTratamiento.Location = new Point(220, yPos);
            txtTratamiento.Size = new Size(630, 25);
            this.Controls.Add(txtTratamiento);

            yPos += 40;

            // Campo para "Observaciones".
            Label lblObservaciones = new Label();
            lblObservaciones.Text = "Observaciones:";
            lblObservaciones.Font = new Font("Segoe UI", 11);
            lblObservaciones.Location = new Point(50, yPos);
            lblObservaciones.Size = new Size(150, 25);
            this.Controls.Add(lblObservaciones);

            txtObservaciones = new TextBox();
            txtObservaciones.Font = new Font("Segoe UI", 10);
            txtObservaciones.Location = new Point(220, yPos);
            txtObservaciones.Size = new Size(630, 80);
            txtObservaciones.Multiline = true; // Permite múltiples líneas de texto.
            this.Controls.Add(txtObservaciones);

            yPos += 100;

            // ===== VALIDACIÓN DE ENFERMEDAD =====
            // Separador visual
            Panel separador = new Panel();
            separador.Location = new Point(50, yPos);
            separador.Size = new Size(800, 2);
            separador.BackColor = Color.FromArgb(70, 130, 180);
            this.Controls.Add(separador);
            yPos += 15;

            Label lblValidacion = new Label();
            lblValidacion.Text = "VALIDACIÓN DE DIAGNÓSTICO";
            lblValidacion.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblValidacion.ForeColor = Color.FromArgb(70, 130, 180);
            lblValidacion.Location = new Point(50, yPos);
            lblValidacion.Size = new Size(300, 25);
            this.Controls.Add(lblValidacion);
            yPos += 35;

            // Checkbox para indicar si coincide
            chkDiagnosticoCoincide = new CheckBox();
            chkDiagnosticoCoincide.Text = "El diagnóstico automático es correcto";
            chkDiagnosticoCoincide.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            chkDiagnosticoCoincide.ForeColor = Color.FromArgb(0, 128, 0);
            chkDiagnosticoCoincide.Location = new Point(50, yPos);
            chkDiagnosticoCoincide.Size = new Size(400, 25);
            chkDiagnosticoCoincide.CheckedChanged += ChkDiagnosticoCoincide_CheckedChanged;
            this.Controls.Add(chkDiagnosticoCoincide);
            yPos += 35;

            // ComboBox para seleccionar enfermedad confirmada
            Label lblEnfermedad = new Label();
            lblEnfermedad.Text = "Enfermedad confirmada:";
            lblEnfermedad.Font = new Font("Segoe UI", 11);
            lblEnfermedad.Location = new Point(50, yPos);
            lblEnfermedad.Size = new Size(200, 25);
            this.Controls.Add(lblEnfermedad);

            cmbEnfermedadConfirmada = new ComboBox();
            cmbEnfermedadConfirmada.Font = new Font("Segoe UI", 10);
            cmbEnfermedadConfirmada.Location = new Point(260, yPos);
            cmbEnfermedadConfirmada.Size = new Size(590, 25);
            cmbEnfermedadConfirmada.DropDownStyle = ComboBoxStyle.DropDownList;

            // Lista de enfermedades
            cmbEnfermedadConfirmada.Items.Add("Neumonía / COVID-19");
            cmbEnfermedadConfirmada.Items.Add("Bronquitis Bacteriana");
            cmbEnfermedadConfirmada.Items.Add("Bronquitis Viral");
            cmbEnfermedadConfirmada.Items.Add("Gripe / Influenza");
            cmbEnfermedadConfirmada.Items.Add("Meningitis");
            cmbEnfermedadConfirmada.Items.Add("Migraña / Cefalea");
            cmbEnfermedadConfirmada.Items.Add("Dengue / Chikungunya");
            cmbEnfermedadConfirmada.Items.Add("Infección Viral");
            cmbEnfermedadConfirmada.Items.Add("Apendicitis");
            cmbEnfermedadConfirmada.Items.Add("Colitis");
            cmbEnfermedadConfirmada.Items.Add("Gastroenteritis");
            cmbEnfermedadConfirmada.Items.Add("Gastritis / Dispepsia");
            cmbEnfermedadConfirmada.Items.Add("Amigdalitis");
            cmbEnfermedadConfirmada.Items.Add("Faringitis");
            cmbEnfermedadConfirmada.Items.Add("Infección Urinaria");
            cmbEnfermedadConfirmada.Items.Add("Lumbalgia");
            cmbEnfermedadConfirmada.Items.Add("Artritis / Bursitis");
            cmbEnfermedadConfirmada.Items.Add("Sinusitis");
            cmbEnfermedadConfirmada.Items.Add("Condición Crítica");
            cmbEnfermedadConfirmada.Items.Add("Condición Moderada");
            cmbEnfermedadConfirmada.Items.Add("Condición Leve");
            cmbEnfermedadConfirmada.Items.Add("Otra (especificar abajo)");

            cmbEnfermedadConfirmada.SelectedIndexChanged += CmbEnfermedadConfirmada_SelectedIndexChanged;
            this.Controls.Add(cmbEnfermedadConfirmada);
            yPos += 40;

            // Campo para ingresar enfermedad manual
            Label lblEnfermedadManual = new Label();
            lblEnfermedadManual.Text = "Especificar otra:";
            lblEnfermedadManual.Font = new Font("Segoe UI", 11);
            lblEnfermedadManual.Location = new Point(50, yPos);
            lblEnfermedadManual.Size = new Size(200, 25);
            this.Controls.Add(lblEnfermedadManual);

            txtEnfermedadManual = new TextBox();
            txtEnfermedadManual.Font = new Font("Segoe UI", 10);
            txtEnfermedadManual.Location = new Point(260, yPos);
            txtEnfermedadManual.Size = new Size(590, 25);
            txtEnfermedadManual.Enabled = false;
            this.Controls.Add(txtEnfermedadManual);
            yPos += 50;

            // Botón "Confirmar y Finalizar".
            Button btnConfirmar = new Button();
            btnConfirmar.Text = "Confirmar y Finalizar";
            btnConfirmar.Font = new Font("Segoe UI", 12);
            btnConfirmar.Location = new Point(250, yPos);
            btnConfirmar.Size = new Size(200, 50);
            btnConfirmar.BackColor = Color.LightGreen;
            btnConfirmar.Click += BtnConfirmar_Click; // Asigna el evento de confirmación.
            this.Controls.Add(btnConfirmar);

            // Botón "Devolver a Cola".
            Button btnDevolver = new Button();
            btnDevolver.Text = "Devolver a Cola";
            btnDevolver.Font = new Font("Segoe UI", 12);
            btnDevolver.Location = new Point(470, yPos);
            btnDevolver.Size = new Size(160, 50);
            btnDevolver.BackColor = Color.LightYellow;
            btnDevolver.Click += (s, e) =>
            {
                // Re-encola la clave del paciente si se decide devolverlo.
                // Se verifica que 'paciente' y 'registro' no sean nulos antes de usarlos.
                if (paciente != null && registro != null)
                {
                    string claveReencolar = paciente.Id + "|" + registro.IdRegistro;
                    sistema.ColasPorHospital[medico.IdHospital].Enqueue(claveReencolar);
                }
                
                MessageBox.Show("Paciente devuelto a la cola", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario.
            };
            this.Controls.Add(btnDevolver);
        }

        // Método para cargar los datos del paciente y su registro en la interfaz de usuario.
        // Se llama después de que el paciente y el registro han sido asignados.
        private void CargarDatosPacienteYRegistro()
        {
            if (paciente != null && registro != null)
            {
                // Rellena los campos de texto editables con la información del registro.
                if (txtDiagnostico != null) txtDiagnostico.Text = registro.Diagnostico;
                if (txtTratamiento != null) txtTratamiento.Text = registro.Tratamiento;
                if (txtObservaciones != null) txtObservaciones.Text = registro.ObservacionDoctor;
            }
        }

        // Manejador para cuando cambia el checkbox de coincidencia
        private void ChkDiagnosticoCoincide_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDiagnosticoCoincide != null && cmbEnfermedadConfirmada != null)
            {
                // Si está marcado como correcto, desactiva la selección de enfermedad
                cmbEnfermedadConfirmada.Enabled = !chkDiagnosticoCoincide.Checked;
                if (txtEnfermedadManual != null)
                    txtEnfermedadManual.Enabled = false;
            }
        }

        // Manejador para cuando cambia la selección de enfermedad
        private void CmbEnfermedadConfirmada_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEnfermedadConfirmada != null && txtEnfermedadManual != null)
            {
                // Habilita el campo manual solo si se selecciona "Otra"
                if (cmbEnfermedadConfirmada.SelectedItem != null &&
                    cmbEnfermedadConfirmada.SelectedItem.ToString() == "Otra (especificar abajo)")
                {
                    txtEnfermedadManual.Enabled = true;
                    txtEnfermedadManual.Focus();
                }
                else
                {
                    txtEnfermedadManual.Enabled = false;
                    txtEnfermedadManual.Clear();
                }
            }
        }

        // Manejador de eventos para el botón "Confirmar y Finalizar".
        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            // Valida que el paciente y el registro sean válidos.
            if (paciente == null || registro == null)
            {
                MessageBox.Show("Error: No hay datos de paciente o registro válidos para guardar.", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Actualiza el diagnóstico, tratamiento y observaciones del registro con los valores de los campos de texto.
            if (!string.IsNullOrWhiteSpace(txtDiagnostico?.Text))
                registro.Diagnostico = txtDiagnostico.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtTratamiento?.Text))
                registro.Tratamiento = txtTratamiento.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtObservaciones?.Text))
                registro.ObservacionDoctor = txtObservaciones.Text.Trim();

            // ===== VALIDACIÓN DE ENFERMEDAD =====
            // Verifica si el médico marcó que el diagnóstico automático es correcto
            if (chkDiagnosticoCoincide != null && chkDiagnosticoCoincide.Checked)
            {
                registro.DiagnosticoCoincide = true;
                registro.DiagnosticoConfirmado = registro.Diagnostico; // El diagn\u00f3stico confirmado es el mismo que el autom\u00e1tico
            }
            else
            {
                // El m\u00e9dico corrigi\u00f3 el diagn\u00f3stico
                registro.DiagnosticoCoincide = false;

                // Obtiene la enfermedad confirmada por el m\u00e9dico
                string enfermedadConfirmada = "";

                if (cmbEnfermedadConfirmada != null && cmbEnfermedadConfirmada.SelectedItem != null)
                {
                    string seleccion = cmbEnfermedadConfirmada.SelectedItem.ToString();

                    if (seleccion == "Otra (especificar abajo)")
                    {
                        // Usa la enfermedad manual si fue especificada
                        if (!string.IsNullOrWhiteSpace(txtEnfermedadManual?.Text))
                        {
                            enfermedadConfirmada = txtEnfermedadManual.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Por favor, especifique la enfermedad si seleccion\u00f3 'Otra'.",
                                "Validaci\u00f3n requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        enfermedadConfirmada = seleccion;
                    }

                    registro.DiagnosticoConfirmado = enfermedadConfirmada;
                }
                else if (!chkDiagnosticoCoincide.Checked)
                {
                    MessageBox.Show("Por favor, seleccione la enfermedad confirmada o marque que el diagn\u00f3stico es correcto.",
                        "Validaci\u00f3n requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            registro.Confirmado = true; // Marca el registro como confirmado.
            registro.IdMedico = medico.Id; // Asigna el ID del médico al registro.

            // Si el paciente no está en la lista de pacientes asignados del médico, lo añade.
            if (!medico.PacientesAsignados.Contains(paciente.Id))
                medico.PacientesAsignados.Add(paciente.Id);

            // Actualiza el registro en el historial del paciente (buscando la entrada original y modificándola).
            var regEnHistorial = paciente.Historial.FirstOrDefault(r => r.IdRegistro == registro.IdRegistro);
            if (regEnHistorial != null)
            {
                regEnHistorial.Diagnostico = registro.Diagnostico;
                regEnHistorial.DiagnosticoConfirmado = registro.DiagnosticoConfirmado;
                regEnHistorial.DiagnosticoCoincide = registro.DiagnosticoCoincide;
                regEnHistorial.Tratamiento = registro.Tratamiento;
                regEnHistorial.ObservacionDoctor = registro.ObservacionDoctor;
                regEnHistorial.Confirmado = registro.Confirmado;
                regEnHistorial.IdMedico = registro.IdMedico;
            }

            // Guarda los datos actualizados del paciente y del médico en la persistencia.
            sistema.GuardarUsuario(paciente);
            sistema.GuardarUsuario(medico);

            MessageBox.Show("Atención completada y guardada exitosamente", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Cierra el formulario.
        }
    }
}
