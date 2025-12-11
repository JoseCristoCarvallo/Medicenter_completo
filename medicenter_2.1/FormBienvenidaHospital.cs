using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario de bienvenida al hospital seleccionado.
    // Proporciona información del hospital antes de iniciar el cuestionario de diagnóstico.
    public partial class FormBienvenidaHospital : Form
    {
        private Sistema sistema;
        private Paciente paciente;
        private Hospital hospital;

        // Constructor del formulario.
        public FormBienvenidaHospital(Sistema sistemaParam, Paciente pacienteParam, Hospital hospitalParam)
        {
            sistema = sistemaParam;
            paciente = pacienteParam;
            hospital = hospitalParam;
            InitializeComponent();
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700);
            this.Text = "Bienvenida - " + hospital.Nombre;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 230, 250);

            // Panel principal con fondo blanco.
            Panel panelPrincipal = new Panel();
            panelPrincipal.Location = new Point(50, 50);
            panelPrincipal.Size = new Size(800, 600);
            panelPrincipal.BackColor = Color.White;
            panelPrincipal.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelPrincipal);

            // Icono o imagen del hospital (simulado con un panel de color).
            Panel panelIcono = new Panel();
            panelIcono.Location = new Point(325, 30);
            panelIcono.Size = new Size(150, 150);
            panelIcono.BackColor = Color.FromArgb(100, 149, 237);
            panelIcono.BorderStyle = BorderStyle.FixedSingle;
            panelPrincipal.Controls.Add(panelIcono);

            // Símbolo médico en el centro del icono.
            Label lblSimboloMedico = new Label();
            lblSimboloMedico.Text = "⚕";
            lblSimboloMedico.Font = new Font("Segoe UI", 72, FontStyle.Bold);
            lblSimboloMedico.ForeColor = Color.White;
            lblSimboloMedico.Location = new Point(30, 20);
            lblSimboloMedico.Size = new Size(90, 110);
            lblSimboloMedico.TextAlign = ContentAlignment.MiddleCenter;
            panelIcono.Controls.Add(lblSimboloMedico);

            // Título de bienvenida.
            Label lblBienvenida = new Label();
            lblBienvenida.Text = $"Bienvenido a {hospital.Nombre}";
            lblBienvenida.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblBienvenida.Location = new Point(50, 200);
            lblBienvenida.Size = new Size(700, 40);
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            lblBienvenida.ForeColor = Color.FromArgb(70, 130, 180);
            panelPrincipal.Controls.Add(lblBienvenida);

            // Mensaje de presentación.
            Label lblMensaje1 = new Label();
            lblMensaje1.Text = $"Estimado(a) {paciente.Nombre},";
            lblMensaje1.Font = new Font("Segoe UI", 13);
            lblMensaje1.Location = new Point(80, 260);
            lblMensaje1.Size = new Size(640, 30);
            panelPrincipal.Controls.Add(lblMensaje1);

            Label lblMensaje2 = new Label();
            lblMensaje2.Text = "Gracias por confiar en nuestros servicios médicos.";
            lblMensaje2.Font = new Font("Segoe UI", 12);
            lblMensaje2.Location = new Point(80, 295);
            lblMensaje2.Size = new Size(640, 30);
            panelPrincipal.Controls.Add(lblMensaje2);

            // Información del hospital.
            Label lblInfoTitulo = new Label();
            lblInfoTitulo.Text = "Información del Hospital:";
            lblInfoTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblInfoTitulo.Location = new Point(80, 345);
            lblInfoTitulo.Size = new Size(640, 30);
            panelPrincipal.Controls.Add(lblInfoTitulo);

            // Detalles del hospital.
            Label lblTipo = new Label();
            lblTipo.Text = $"• Tipo: {(hospital.EsPublico ? "Hospital Público" : "Hospital Privado")}";
            lblTipo.Font = new Font("Segoe UI", 11);
            lblTipo.Location = new Point(100, 380);
            lblTipo.Size = new Size(600, 25);
            panelPrincipal.Controls.Add(lblTipo);

            Label lblCosto = new Label();
            lblCosto.Text = $"• Costo de consulta: {(hospital.EsPublico ? "Gratuito" : "$" + hospital.CostoConsulta.ToString("F2"))}";
            lblCosto.Font = new Font("Segoe UI", 11);
            lblCosto.Location = new Point(100, 410);
            lblCosto.Size = new Size(600, 25);
            panelPrincipal.Controls.Add(lblCosto);

            // Cuenta el número real de doctores en este hospital
            int numeroDoctores = sistema.Personal.Count(p =>
                p.IdHospital == hospital.Id &&
                p.NivelAcceso == NivelAcceso.MedicoGeneral);

            Label lblDoctores = new Label();
            lblDoctores.Text = $"• Doctores disponibles: {numeroDoctores} médico{(numeroDoctores != 1 ? "s" : "")}";
            lblDoctores.Font = new Font("Segoe UI", 11);
            lblDoctores.Location = new Point(100, 440);
            lblDoctores.Size = new Size(600, 25);
            panelPrincipal.Controls.Add(lblDoctores);

            // Instrucción para continuar.
            Label lblInstruccion = new Label();
            lblInstruccion.Text = "A continuación, responderá un cuestionario médico para evaluar su condición.";
            lblInstruccion.Font = new Font("Segoe UI", 11, FontStyle.Italic);
            lblInstruccion.Location = new Point(80, 510);
            lblInstruccion.Size = new Size(640, 30);
            lblInstruccion.ForeColor = Color.Gray;
            panelPrincipal.Controls.Add(lblInstruccion);

            // Botón "Comenzar Cuestionario".
            Button btnComenzar = new Button();
            btnComenzar.Text = "Comenzar Cuestionario";
            btnComenzar.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btnComenzar.Location = new Point(250, 545);
            btnComenzar.Size = new Size(250, 50);
            btnComenzar.BackColor = Color.FromArgb(70, 130, 180);
            btnComenzar.ForeColor = Color.White;
            btnComenzar.FlatStyle = FlatStyle.Flat;
            btnComenzar.FlatAppearance.BorderSize = 0;
            btnComenzar.Cursor = Cursors.Hand;
            btnComenzar.Click += BtnComenzar_Click;
            panelPrincipal.Controls.Add(btnComenzar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 11);
            btnCancelar.Location = new Point(520, 555);
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.BackColor = Color.LightGray;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.Click += (s, e) => this.Close();
            panelPrincipal.Controls.Add(btnCancelar);
        }

        // Manejador de eventos para el botón "Comenzar Cuestionario".
        private void BtnComenzar_Click(object sender, EventArgs e)
        {
            // Agrega el paciente a la lista de pacientes atendidos del hospital si no está ya.
            if (!hospital.PacientesAtendidos.Contains(paciente.Id))
            {
                hospital.PacientesAtendidos.Add(paciente.Id);
            }

            // Crea un nuevo registro médico.
            RegistroMedico nuevoRegistro = new RegistroMedico
            {
                IdRegistro = sistema.GenerarIdRegistro(),
                IdPaciente = paciente.Id,
                IdHospital = hospital.Id
            };

            // Abre el formulario de diagnóstico (cuestionario).
            FormDiagnostico formDiagnostico = new FormDiagnostico(sistema, paciente, hospital, nuevoRegistro);
            this.Hide();
            formDiagnostico.ShowDialog();
            this.Close();
        }
    }
}
