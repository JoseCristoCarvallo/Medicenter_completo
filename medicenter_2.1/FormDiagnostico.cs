using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario de Diagnóstico Automático: Guía al paciente a través de un árbol de decisiones
    // para obtener un diagnóstico preliminar basado en sus síntomas utilizando el algoritmo Naive Bayes.
    public partial class FormDiagnostico : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio.
        private Paciente paciente; // El paciente que está realizando el diagnóstico.
        private Hospital hospital; // El hospital donde se registra la consulta.
        private RegistroMedico registro; // El registro médico que se está creando/actualizando.
        private DecisionNode nodoActual; // El nodo actual en el árbol de decisiones.
        private StringBuilder respuestasArbol; // Almacena las respuestas del cuestionario para el historial.

        // Controles de UI para mostrar preguntas y recibir respuestas.
        private Label lblPregunta;
        private Button btnSi;
        private Button btnNo;
        private Panel panelPregunta; // Panel que contiene la pregunta y los botones de respuesta.
        private Panel panelDescripcion; // Panel para que el paciente describa su malestar.
        private TextBox txtDescripcion; // Campo de texto para la descripción del paciente.
        private Panel panelResultado; // Panel que muestra el resultado del diagnóstico.

        // Constructor del formulario de diagnóstico.
        public FormDiagnostico(Sistema sistemaParam, Paciente pacienteParam, Hospital hospitalParam, RegistroMedico registroParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            paciente = pacienteParam; // Asigna el objeto paciente.
            hospital = hospitalParam; // Asigna el objeto hospital.
            registro = registroParam; // Asigna el objeto registro médico.
            nodoActual = sistema.ArbolDiagnostico; // Inicia el diagnóstico en la raíz del árbol.
            respuestasArbol = new StringBuilder(); // Inicializa el StringBuilder para las respuestas.

            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 650); // Tamaño del formulario.
            this.Text = "Diagnóstico Automático"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Diagnóstico Automático";
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitulo.Location = new Point(250, 30);
            lblTitulo.Size = new Size(400, 50);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Instrucción para el usuario.
            Label lblInstruccion = new Label();
            lblInstruccion.Text = "Responda las siguientes preguntas:";
            lblInstruccion.Font = new Font("Segoe UI", 13);
            lblInstruccion.Location = new Point(250, 90);
            lblInstruccion.Size = new Size(400, 30);
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInstruccion);

            // Panel para mostrar la pregunta actual y las opciones de respuesta.
            panelPregunta = new Panel();
            panelPregunta.Location = new Point(100, 150);
            panelPregunta.Size = new Size(700, 350);
            panelPregunta.BackColor = Color.White;
            panelPregunta.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelPregunta);

            // Etiqueta donde se muestra la pregunta del nodo actual.
            lblPregunta = new Label();
            lblPregunta.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblPregunta.Location = new Point(50, 80);
            lblPregunta.Size = new Size(600, 100);
            lblPregunta.TextAlign = ContentAlignment.MiddleCenter;
            panelPregunta.Controls.Add(lblPregunta);

            // Botón "SÍ" para responder a la pregunta.
            btnSi = new Button();
            btnSi.Text = "SÍ";
            btnSi.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnSi.Location = new Point(150, 220);
            btnSi.Size = new Size(150, 60);
            btnSi.BackColor = Color.LightGreen;
            btnSi.Click += (s, e) => ProcesarRespuesta("si"); // Asigna el evento de respuesta "sí".
            panelPregunta.Controls.Add(btnSi);

            // Botón "NO" para responder a la pregunta.
            btnNo = new Button();
            btnNo.Text = "NO";
            btnNo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnNo.Location = new Point(400, 220);
            btnNo.Size = new Size(150, 60);
            btnNo.BackColor = Color.LightCoral;
            btnNo.Click += (s, e) => ProcesarRespuesta("no"); // Asigna el evento de respuesta "no".
            panelPregunta.Controls.Add(btnNo);

            // Panel para que el paciente describa su malestar manualmente.
            panelDescripcion = new Panel();
            panelDescripcion.Location = new Point(100, 150);
            panelDescripcion.Size = new Size(700, 350);
            panelDescripcion.BackColor = Color.White;
            panelDescripcion.BorderStyle = BorderStyle.FixedSingle;
            panelDescripcion.Visible = false; // Inicialmente oculto.
            this.Controls.Add(panelDescripcion);

            // Título del panel de descripción.
            Label lblDescripcionTitulo = new Label();
            lblDescripcionTitulo.Text = "Describa su malestar";
            lblDescripcionTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblDescripcionTitulo.Location = new Point(50, 20);
            lblDescripcionTitulo.Size = new Size(600, 40);
            lblDescripcionTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelDescripcion.Controls.Add(lblDescripcionTitulo);

            // Instrucción para el paciente.
            Label lblDescripcionInstruccion = new Label();
            lblDescripcionInstruccion.Text = "Por favor, describa detalladamente cómo se siente, cualquier síntoma adicional o información relevante:";
            lblDescripcionInstruccion.Font = new Font("Segoe UI", 11);
            lblDescripcionInstruccion.Location = new Point(30, 70);
            lblDescripcionInstruccion.Size = new Size(640, 40);
            panelDescripcion.Controls.Add(lblDescripcionInstruccion);

            // Campo de texto para la descripción.
            txtDescripcion = new TextBox();
            txtDescripcion.Multiline = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.Font = new Font("Segoe UI", 11);
            txtDescripcion.Location = new Point(30, 120);
            txtDescripcion.Size = new Size(640, 150);
            panelDescripcion.Controls.Add(txtDescripcion);

            // Botón "Generar Diagnóstico Final".
            Button btnGenerarDiagnostico = new Button();
            btnGenerarDiagnostico.Text = "Generar Diagnóstico Final";
            btnGenerarDiagnostico.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnGenerarDiagnostico.Location = new Point(200, 290);
            btnGenerarDiagnostico.Size = new Size(300, 45);
            btnGenerarDiagnostico.BackColor = Color.FromArgb(70, 130, 180);
            btnGenerarDiagnostico.ForeColor = Color.White;
            btnGenerarDiagnostico.FlatStyle = FlatStyle.Flat;
            btnGenerarDiagnostico.Click += BtnGenerarDiagnostico_Click;
            panelDescripcion.Controls.Add(btnGenerarDiagnostico);

            // Panel para mostrar el resultado final del diagnóstico.
            panelResultado = new Panel();
            panelResultado.Location = new Point(100, 150);
            panelResultado.Size = new Size(700, 350);
            panelResultado.BackColor = Color.White;
            panelResultado.BorderStyle = BorderStyle.FixedSingle;
            panelResultado.Visible = false; // Inicialmente oculto.
            panelResultado.AutoScroll = true; // Habilita scroll para contenido largo.
            this.Controls.Add(panelResultado);

            MostrarPregunta(); // Inicia el proceso mostrando la primera pregunta.
        }

        // Muestra la pregunta del nodo actual o pasa al panel de descripción si es un nodo hoja.
        private void MostrarPregunta()
        {
            if (nodoActual.EsHoja())
            {
                MostrarPanelDescripcion(); // Si es una hoja, muestra el panel de descripción.
            }
            else
            {
                lblPregunta.Text = nodoActual.Pregunta; // Si no es hoja, muestra la pregunta.
            }
        }

        // Procesa la respuesta del usuario y avanza en el árbol de decisiones.
        private void ProcesarRespuesta(string respuesta)
        {
            // Guarda la pregunta y respuesta para el historial.
            respuestasArbol.AppendLine($"Pregunta: {nodoActual.Pregunta}");
            respuestasArbol.AppendLine($"Respuesta: {respuesta.ToUpper()}");
            respuestasArbol.AppendLine();

            // Busca un hijo cuyo RespuestaEsperada coincida con la respuesta del usuario.
            foreach (DecisionNode hijo in nodoActual.Hijos)
            {
                if (hijo.RespuestaEsperada == respuesta)
                {
                    nodoActual = hijo; // Avanza al siguiente nodo.
                    MostrarPregunta(); // Muestra la pregunta o pasa al panel de descripción.
                    return;
                }
            }

            // En caso de que la respuesta no tenga un camino definido en el árbol (error en la configuración del árbol).
            MessageBox.Show("Error en el árbol de decisión", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Muestra el panel para que el paciente describa su malestar.
        private void MostrarPanelDescripcion()
        {
            panelPregunta.Visible = false; // Oculta el panel de preguntas.
            panelDescripcion.Visible = true; // Muestra el panel de descripción.

            // Guarda el diagnóstico preliminar del Naive Bayes.
            string diagnosticoNaiveBayes = nodoActual.Diagnostico;
            registro.Diagnostico = diagnosticoNaiveBayes;
        }

        // Manejador para el botón "Generar Diagnóstico Final".
        private void BtnGenerarDiagnostico_Click(object sender, EventArgs e)
        {
            string descripcionPaciente = txtDescripcion.Text.Trim();

            // Valida que el paciente haya ingresado una descripción.
            if (string.IsNullOrWhiteSpace(descripcionPaciente))
            {
                MessageBox.Show("Por favor, describa su malestar antes de continuar.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enriquece el diagnóstico con la descripción del paciente
            string diagnosticoCompleto = registro.Diagnostico + "\n\nDescripción del paciente: " + descripcionPaciente;
            registro.Diagnostico = diagnosticoCompleto;
            registro.Tratamiento = "Diagnóstico automático - Pendiente de validación médica";

            // Muestra el resultado final.
            MostrarResultado();
        }

        // Muestra el panel con el resultado del diagnóstico automático.
        private void MostrarResultado()
        {
            panelDescripcion.Visible = false;
            panelResultado.Visible = true;
            panelResultado.Controls.Clear();

            int yPos = 10;

            // Separar diagnóstico y descripción del paciente
            string diagnosticoCompleto = registro.Diagnostico;
            string diagnosticoSolo = diagnosticoCompleto;
            string descripcionPaciente = "";

            // Verificar si hay descripción del paciente
            if (diagnosticoCompleto.Contains("Descripción del paciente:") ||
                diagnosticoCompleto.Contains("Descripcion del paciente:"))
            {
                int indice = diagnosticoCompleto.IndexOf("Descripción del paciente:", StringComparison.OrdinalIgnoreCase);
                if (indice == -1)
                    indice = diagnosticoCompleto.IndexOf("Descripcion del paciente:", StringComparison.OrdinalIgnoreCase);

                if (indice >= 0)
                {
                    diagnosticoSolo = diagnosticoCompleto.Substring(0, indice).Trim();
                    descripcionPaciente = diagnosticoCompleto.Substring(indice)
                        .Replace("Descripción del paciente:", "")
                        .Replace("Descripcion del paciente:", "").Trim();
                }
            }

            // Título principal
            Label lblTitulo = new Label();
            lblTitulo.Text = "RESULTADO DEL DIAGNÓSTICO";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(150, yPos);
            lblTitulo.Size = new Size(400, 35);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.ForeColor = Color.FromArgb(70, 130, 180);
            panelResultado.Controls.Add(lblTitulo);
            yPos += 50;

            // Detectar nivel de urgencia
            string nivelUrgencia = "LEVE";
            Color colorUrgencia = Color.FromArgb(50, 205, 50); // Verde

            if (diagnosticoSolo.Contains("CRÍTICO") || diagnosticoSolo.Contains("CRITICO"))
            {
                nivelUrgencia = "CRÍTICO";
                colorUrgencia = Color.FromArgb(220, 20, 60); // Rojo
            }
            else if (diagnosticoSolo.Contains("MODERADO"))
            {
                nivelUrgencia = "MODERADO";
                colorUrgencia = Color.FromArgb(255, 140, 0); // Naranja
            }

            // Panel de nivel de urgencia
            Panel panelUrgencia = new Panel();
            panelUrgencia.Location = new Point(30, yPos);
            panelUrgencia.Size = new Size(640, 50);
            panelUrgencia.BackColor = colorUrgencia;
            panelUrgencia.BorderStyle = BorderStyle.FixedSingle;
            panelResultado.Controls.Add(panelUrgencia);

            Label lblUrgencia = new Label();
            lblUrgencia.Text = $"NIVEL DE URGENCIA: {nivelUrgencia}";
            lblUrgencia.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblUrgencia.ForeColor = Color.White;
            lblUrgencia.Location = new Point(10, 12);
            lblUrgencia.Size = new Size(620, 30);
            lblUrgencia.TextAlign = ContentAlignment.MiddleCenter;
            panelUrgencia.Controls.Add(lblUrgencia);
            yPos += 60;

            // ===== DIAGNÓSTICO AUTOMÁTICO =====
            Label lblDiagTitulo = new Label();
            lblDiagTitulo.Text = "DIAGNÓSTICO AUTOMÁTICO (Naive Bayes + Árbol de Decisión):";
            lblDiagTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDiagTitulo.Location = new Point(30, yPos);
            lblDiagTitulo.Size = new Size(640, 25);
            lblDiagTitulo.ForeColor = Color.FromArgb(0, 100, 0);
            panelResultado.Controls.Add(lblDiagTitulo);
            yPos += 30;

            TextBox txtDiagnostico = new TextBox();
            txtDiagnostico.Text = diagnosticoSolo;
            txtDiagnostico.Font = new Font("Segoe UI", 10);
            txtDiagnostico.Location = new Point(30, yPos);
            txtDiagnostico.Size = new Size(640, 100);
            txtDiagnostico.Multiline = true;
            txtDiagnostico.ScrollBars = ScrollBars.Vertical;
            txtDiagnostico.ReadOnly = true;
            txtDiagnostico.BackColor = Color.FromArgb(240, 255, 240);
            panelResultado.Controls.Add(txtDiagnostico);
            yPos += 110;

            // ===== DESCRIPCIÓN DEL PACIENTE (si existe) =====
            if (!string.IsNullOrWhiteSpace(descripcionPaciente))
            {
                Label lblDescTitulo = new Label();
                lblDescTitulo.Text = "DESCRIPCIÓN ADICIONAL DEL PACIENTE:";
                lblDescTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblDescTitulo.Location = new Point(30, yPos);
                lblDescTitulo.Size = new Size(640, 25);
                lblDescTitulo.ForeColor = Color.FromArgb(138, 43, 226);
                panelResultado.Controls.Add(lblDescTitulo);
                yPos += 30;

                TextBox txtDescripcion = new TextBox();
                txtDescripcion.Text = descripcionPaciente;
                txtDescripcion.Font = new Font("Segoe UI", 10);
                txtDescripcion.Location = new Point(30, yPos);
                txtDescripcion.Size = new Size(640, 70);
                txtDescripcion.Multiline = true;
                txtDescripcion.ScrollBars = ScrollBars.Vertical;
                txtDescripcion.ReadOnly = true;
                txtDescripcion.BackColor = Color.FromArgb(255, 250, 240);
                panelResultado.Controls.Add(txtDescripcion);
                yPos += 80;
            }

            // Alerta si es crítico
            if (nivelUrgencia == "CRÍTICO")
            {
                Panel panelAlerta = new Panel();
                panelAlerta.Location = new Point(30, yPos);
                panelAlerta.Size = new Size(640, 60);
                panelAlerta.BackColor = Color.FromArgb(255, 240, 240);
                panelAlerta.BorderStyle = BorderStyle.FixedSingle;
                panelResultado.Controls.Add(panelAlerta);

                Label lblAlerta = new Label();
                lblAlerta.Text = "ATENCIÓN: Situación crítica detectada\n" +
                                "Busque atención médica de EMERGENCIA inmediatamente";
                lblAlerta.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblAlerta.ForeColor = Color.FromArgb(220, 20, 60);
                lblAlerta.Location = new Point(10, 10);
                lblAlerta.Size = new Size(620, 40);
                lblAlerta.TextAlign = ContentAlignment.MiddleCenter;
                panelAlerta.Controls.Add(lblAlerta);
                yPos += 70;
            }

            // Información adicional
            Label lblInfo = new Label();
            lblInfo.Text = "Este diagnóstico es preliminar y debe ser validado por un médico.\n" +
                          "Será agregado a la cola de atención del hospital.";
            lblInfo.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblInfo.ForeColor = Color.Gray;
            lblInfo.Location = new Point(30, yPos);
            lblInfo.Size = new Size(640, 35);
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            panelResultado.Controls.Add(lblInfo);
            yPos += 45;

            // Botón Finalizar
            Button btnFinalizar = new Button();
            btnFinalizar.Text = "Finalizar y Registrar Consulta";
            btnFinalizar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnFinalizar.Location = new Point(220, yPos);
            btnFinalizar.Size = new Size(260, 45);
            btnFinalizar.BackColor = Color.FromArgb(70, 130, 180);
            btnFinalizar.ForeColor = Color.White;
            btnFinalizar.FlatStyle = FlatStyle.Flat;
            btnFinalizar.Click += BtnFinalizar_Click;
            panelResultado.Controls.Add(btnFinalizar);
        }

        // Manejador de eventos para el botón "Finalizar Consulta".
        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            // Asegura que el diccionario de registros por hospital exista para el hospital actual.
            if (!sistema.RegistrosPorHospital.ContainsKey(hospital.Id))
            {
                sistema.RegistrosPorHospital[hospital.Id] = new List<RegistroMedico>();
            }
            // Añade el registro médico al historial del hospital.
            sistema.RegistrosPorHospital[hospital.Id].Add(registro);

            // Añade el registro médico al historial personal del paciente.
            paciente.Historial.Add(registro);

            // Asegura que la cola de pacientes del hospital exista.
            if (!sistema.ColasPorHospital.ContainsKey(hospital.Id))
            {
                sistema.ColasPorHospital[hospital.Id] = new Queue<string>();
            }
            // Encola al paciente para revisión médica. La clave combina ID de paciente y ID de registro.
            string clave = paciente.Id + "|" + registro.IdRegistro;
            sistema.ColasPorHospital[hospital.Id].Enqueue(clave);
            sistema.GuardarUsuario(paciente); // Guarda los datos actualizados del paciente.

            // Prepara un mensaje de confirmación para el usuario.
            string mensaje = $"---------------------------------------\n";
            mensaje += "Consulta registrada exitosamente\n";
            mensaje += "---------------------------------------\n\n";
            mensaje += $"ID de Registro: {registro.IdRegistro}\n";
            mensaje += "Hospital: " + hospital.Nombre + "\n";
            mensaje += "Diagnóstico Preliminar: " + registro.Diagnostico + "\n\n";
            mensaje += "Un médico revisará su caso pronto\n";
            mensaje += "Posición en cola: " + sistema.ColasPorHospital[hospital.Id].Count + "\n";

            MessageBox.Show(mensaje, "Consulta Completada",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Cierra el formulario.
        }
    }
}