using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para editar un registro médico existente.
    // Permite al médico modificar el diagnóstico, tratamiento, observaciones y enfermedad confirmada.
    public partial class FormEditarRegistroMedico : Form
    {
        private Sistema sistema;
        private Paciente paciente;
        private RegistroMedico registro;

        // Controles de UI
        private TextBox txtDiagnostico;
        private TextBox txtTratamiento;
        private TextBox txtObservaciones;
        private ComboBox cmbEnfermedadConfirmada;
        private TextBox txtEnfermedadManual;
        private CheckBox chkDiagnosticoCoincide;
        private Label lblFecha;
        private Label lblSintomas;

        // Constructor del formulario.
        public FormEditarRegistroMedico(Sistema sistemaParam, Paciente pacienteParam, RegistroMedico registroParam)
        {
            sistema = sistemaParam;
            paciente = pacienteParam;
            registro = registroParam;
            InitializeComponent();
        }

        // Inicializa los componentes del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(800, 850);
            this.Text = "Editar Registro Médico";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 230, 250);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            int yPos = 20;

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Editar Registro Médico";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(250, yPos);
            lblTitulo.Size = new Size(300, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);
            yPos += 50;

            // Panel de información básica del registro
            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(30, yPos);
            panelInfo.Size = new Size(740, 120);
            panelInfo.BackColor = Color.White;
            panelInfo.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelInfo);

            // ID y Fecha
            Label lblIdRegistro = new Label();
            lblIdRegistro.Text = $"ID Registro: {registro.IdRegistro}";
            lblIdRegistro.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblIdRegistro.Location = new Point(15, 10);
            lblIdRegistro.Size = new Size(350, 25);
            panelInfo.Controls.Add(lblIdRegistro);

            lblFecha = new Label();
            lblFecha.Text = $"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm}";
            lblFecha.Font = new Font("Segoe UI", 10);
            lblFecha.Location = new Point(15, 40);
            lblFecha.Size = new Size(350, 25);
            panelInfo.Controls.Add(lblFecha);

            // Síntomas
            Label lblSintomasTitulo = new Label();
            lblSintomasTitulo.Text = "Síntomas:";
            lblSintomasTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSintomasTitulo.Location = new Point(15, 70);
            lblSintomasTitulo.Size = new Size(100, 25);
            panelInfo.Controls.Add(lblSintomasTitulo);

            lblSintomas = new Label();
            lblSintomas.Text = string.Join(", ", registro.Sintomas);
            lblSintomas.Font = new Font("Segoe UI", 9);
            lblSintomas.Location = new Point(120, 70);
            lblSintomas.Size = new Size(600, 40);
            panelInfo.Controls.Add(lblSintomas);

            yPos += 140;

            // Campo Diagnóstico
            Label lblDiagnostico = new Label();
            lblDiagnostico.Text = "Diagnóstico:";
            lblDiagnostico.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDiagnostico.Location = new Point(30, yPos);
            lblDiagnostico.Size = new Size(150, 25);
            this.Controls.Add(lblDiagnostico);
            yPos += 30;

            txtDiagnostico = new TextBox();
            txtDiagnostico.Font = new Font("Segoe UI", 10);
            txtDiagnostico.Location = new Point(30, yPos);
            txtDiagnostico.Size = new Size(740, 80);
            txtDiagnostico.Multiline = true;
            txtDiagnostico.ScrollBars = ScrollBars.Vertical;
            txtDiagnostico.Text = registro.Diagnostico;
            this.Controls.Add(txtDiagnostico);
            yPos += 90;

            // Campo Tratamiento
            Label lblTratamiento = new Label();
            lblTratamiento.Text = "Tratamiento:";
            lblTratamiento.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTratamiento.Location = new Point(30, yPos);
            lblTratamiento.Size = new Size(150, 25);
            this.Controls.Add(lblTratamiento);
            yPos += 30;

            txtTratamiento = new TextBox();
            txtTratamiento.Font = new Font("Segoe UI", 10);
            txtTratamiento.Location = new Point(30, yPos);
            txtTratamiento.Size = new Size(740, 60);
            txtTratamiento.Multiline = true;
            txtTratamiento.ScrollBars = ScrollBars.Vertical;
            txtTratamiento.Text = registro.Tratamiento;
            this.Controls.Add(txtTratamiento);
            yPos += 70;

            // Campo Observaciones
            Label lblObservaciones = new Label();
            lblObservaciones.Text = "Observaciones del Doctor:";
            lblObservaciones.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblObservaciones.Location = new Point(30, yPos);
            lblObservaciones.Size = new Size(250, 25);
            this.Controls.Add(lblObservaciones);
            yPos += 30;

            txtObservaciones = new TextBox();
            txtObservaciones.Font = new Font("Segoe UI", 10);
            txtObservaciones.Location = new Point(30, yPos);
            txtObservaciones.Size = new Size(740, 80);
            txtObservaciones.Multiline = true;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Text = registro.ObservacionDoctor;
            this.Controls.Add(txtObservaciones);
            yPos += 100;

            // Separador
            Panel separador = new Panel();
            separador.Location = new Point(30, yPos);
            separador.Size = new Size(740, 2);
            separador.BackColor = Color.FromArgb(70, 130, 180);
            this.Controls.Add(separador);
            yPos += 15;

            // Validación de enfermedad
            Label lblValidacion = new Label();
            lblValidacion.Text = "VALIDACIÓN DE ENFERMEDAD";
            lblValidacion.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblValidacion.ForeColor = Color.FromArgb(70, 130, 180);
            lblValidacion.Location = new Point(30, yPos);
            lblValidacion.Size = new Size(300, 25);
            this.Controls.Add(lblValidacion);
            yPos += 35;

            // Checkbox coincidencia
            chkDiagnosticoCoincide = new CheckBox();
            chkDiagnosticoCoincide.Text = "El diagnóstico automático es correcto";
            chkDiagnosticoCoincide.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            chkDiagnosticoCoincide.ForeColor = Color.FromArgb(0, 128, 0);
            chkDiagnosticoCoincide.Location = new Point(30, yPos);
            chkDiagnosticoCoincide.Size = new Size(350, 25);
            chkDiagnosticoCoincide.Checked = registro.DiagnosticoCoincide;
            chkDiagnosticoCoincide.CheckedChanged += ChkDiagnosticoCoincide_CheckedChanged;
            this.Controls.Add(chkDiagnosticoCoincide);
            yPos += 35;

            // ComboBox enfermedad
            Label lblEnfermedad = new Label();
            lblEnfermedad.Text = "Enfermedad confirmada:";
            lblEnfermedad.Font = new Font("Segoe UI", 11);
            lblEnfermedad.Location = new Point(30, yPos);
            lblEnfermedad.Size = new Size(200, 25);
            this.Controls.Add(lblEnfermedad);

            cmbEnfermedadConfirmada = new ComboBox();
            cmbEnfermedadConfirmada.Font = new Font("Segoe UI", 10);
            cmbEnfermedadConfirmada.Location = new Point(240, yPos);
            cmbEnfermedadConfirmada.Size = new Size(530, 25);
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

            // Selecciona la enfermedad actual si existe
            if (!string.IsNullOrEmpty(registro.DiagnosticoConfirmado))
            {
                int index = cmbEnfermedadConfirmada.Items.IndexOf(registro.DiagnosticoConfirmado);
                if (index >= 0)
                {
                    cmbEnfermedadConfirmada.SelectedIndex = index;
                }
                else
                {
                    // Si no está en la lista, es personalizada
                    cmbEnfermedadConfirmada.SelectedIndex = cmbEnfermedadConfirmada.Items.IndexOf("Otra (especificar abajo)");
                }
            }

            cmbEnfermedadConfirmada.SelectedIndexChanged += CmbEnfermedadConfirmada_SelectedIndexChanged;
            this.Controls.Add(cmbEnfermedadConfirmada);
            yPos += 40;

            // Campo manual
            Label lblEnfermedadManual = new Label();
            lblEnfermedadManual.Text = "Especificar otra:";
            lblEnfermedadManual.Font = new Font("Segoe UI", 11);
            lblEnfermedadManual.Location = new Point(30, yPos);
            lblEnfermedadManual.Size = new Size(200, 25);
            this.Controls.Add(lblEnfermedadManual);

            txtEnfermedadManual = new TextBox();
            txtEnfermedadManual.Font = new Font("Segoe UI", 10);
            txtEnfermedadManual.Location = new Point(240, yPos);
            txtEnfermedadManual.Size = new Size(530, 25);
            txtEnfermedadManual.Enabled = false;

            // Si la enfermedad actual no está en la lista, mostrarla en el campo manual
            if (!string.IsNullOrEmpty(registro.DiagnosticoConfirmado))
            {
                int index = cmbEnfermedadConfirmada.Items.IndexOf(registro.DiagnosticoConfirmado);
                if (index < 0 && cmbEnfermedadConfirmada.SelectedIndex == cmbEnfermedadConfirmada.Items.IndexOf("Otra (especificar abajo)"))
                {
                    txtEnfermedadManual.Text = registro.DiagnosticoConfirmado;
                    txtEnfermedadManual.Enabled = true;
                }
            }

            this.Controls.Add(txtEnfermedadManual);
            yPos += 50;

            // Botones
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnGuardar.Location = new Point(240, yPos);
            btnGuardar.Size = new Size(180, 50);
            btnGuardar.BackColor = Color.LightGreen;
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(440, yPos);
            btnCancelar.Size = new Size(140, 50);
            btnCancelar.BackColor = Color.LightGray;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancelar);
        }

        // Manejador para checkbox de coincidencia
        private void ChkDiagnosticoCoincide_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDiagnosticoCoincide != null && cmbEnfermedadConfirmada != null)
            {
                cmbEnfermedadConfirmada.Enabled = !chkDiagnosticoCoincide.Checked;
                if (txtEnfermedadManual != null)
                    txtEnfermedadManual.Enabled = false;
            }
        }

        // Manejador para cambio de selección de enfermedad
        private void CmbEnfermedadConfirmada_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEnfermedadConfirmada != null && txtEnfermedadManual != null)
            {
                if (cmbEnfermedadConfirmada.SelectedItem != null &&
                    cmbEnfermedadConfirmada.SelectedItem.ToString() == "Otra (especificar abajo)")
                {
                    txtEnfermedadManual.Enabled = true;
                    txtEnfermedadManual.Focus();
                }
                else
                {
                    txtEnfermedadManual.Enabled = false;
                }
            }
        }

        // Manejador para guardar cambios
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Actualiza el diagnóstico
            if (!string.IsNullOrWhiteSpace(txtDiagnostico?.Text))
                registro.Diagnostico = txtDiagnostico.Text.Trim();

            // Actualiza el tratamiento
            if (!string.IsNullOrWhiteSpace(txtTratamiento?.Text))
                registro.Tratamiento = txtTratamiento.Text.Trim();

            // Actualiza las observaciones
            if (!string.IsNullOrWhiteSpace(txtObservaciones?.Text))
                registro.ObservacionDoctor = txtObservaciones.Text.Trim();

            // Actualiza la validación de enfermedad
            if (chkDiagnosticoCoincide != null && chkDiagnosticoCoincide.Checked)
            {
                registro.DiagnosticoCoincide = true;
                registro.DiagnosticoConfirmado = registro.Diagnostico;
            }
            else
            {
                registro.DiagnosticoCoincide = false;

                string enfermedadConfirmada = "";

                if (cmbEnfermedadConfirmada != null && cmbEnfermedadConfirmada.SelectedItem != null)
                {
                    string seleccion = cmbEnfermedadConfirmada.SelectedItem.ToString();

                    if (seleccion == "Otra (especificar abajo)")
                    {
                        if (!string.IsNullOrWhiteSpace(txtEnfermedadManual?.Text))
                        {
                            enfermedadConfirmada = txtEnfermedadManual.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Por favor, especifique la enfermedad si seleccionó 'Otra'.",
                                "Validación requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Por favor, seleccione la enfermedad confirmada o marque que el diagnóstico es correcto.",
                        "Validación requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Marca como confirmado si no lo estaba
            if (!registro.Confirmado)
                registro.Confirmado = true;

            // Actualiza el registro en el historial del paciente
            var regEnHistorial = paciente.Historial.FirstOrDefault(r => r.IdRegistro == registro.IdRegistro);
            if (regEnHistorial != null)
            {
                regEnHistorial.Diagnostico = registro.Diagnostico;
                regEnHistorial.DiagnosticoConfirmado = registro.DiagnosticoConfirmado;
                regEnHistorial.DiagnosticoCoincide = registro.DiagnosticoCoincide;
                regEnHistorial.Tratamiento = registro.Tratamiento;
                regEnHistorial.ObservacionDoctor = registro.ObservacionDoctor;
                regEnHistorial.Confirmado = registro.Confirmado;
            }

            // También actualiza en RegistrosPorHospital si existe
            if (sistema.RegistrosPorHospital.ContainsKey(registro.IdHospital))
            {
                var regEnHospital = sistema.RegistrosPorHospital[registro.IdHospital]
                    .FirstOrDefault(r => r.IdRegistro == registro.IdRegistro);
                if (regEnHospital != null)
                {
                    regEnHospital.Diagnostico = registro.Diagnostico;
                    regEnHospital.DiagnosticoConfirmado = registro.DiagnosticoConfirmado;
                    regEnHospital.DiagnosticoCoincide = registro.DiagnosticoCoincide;
                    regEnHospital.Tratamiento = registro.Tratamiento;
                    regEnHospital.ObservacionDoctor = registro.ObservacionDoctor;
                    regEnHospital.Confirmado = registro.Confirmado;
                }
            }

            // Guarda los cambios
            sistema.GuardarUsuario(paciente);

            MessageBox.Show("Registro médico actualizado exitosamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
