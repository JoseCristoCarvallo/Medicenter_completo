using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario principal de la aplicación MediCenter.
    // Sirve como el punto de entrada inicial de la interfaz de usuario, permitiendo al usuario seleccionar entre
    // iniciar sesión como paciente o como personal hospitalario.
    public partial class FormPrincipal : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a la lógica de negocio y datos.

        // Constructor del formulario principal.
        public FormPrincipal()
        {
            sistema = new Sistema(); // Inicializa una nueva instancia del sistema central.
            InitializeComponent(); // Llama al método para inicializar y configurar los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            this.SuspendLayout(); // Suspende la lógica de diseño para una construcción más eficiente.

            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 600); // Tamaño del formulario.
            this.Text = "MediCenter - Sistema de Gestión Médica Integral"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro de la pantalla.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo del formulario.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo, no redimensionable.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.
            this.Icon = WC_MediCenter.Properties.Resources.Medicenter_2_ico != null ?
                        new Icon(new System.IO.MemoryStream(WC_MediCenter.Properties.Resources.Medicenter_2_ico)) : null;

            // Título principal.
            Label lblTitulo = new Label();
            lblTitulo.Text = "¡Bienvenido a MediCenter!";
            lblTitulo.Font = new Font("Segoe UI", 24, FontStyle.Bold | FontStyle.Italic);
            lblTitulo.Location = new Point(200, 50);
            lblTitulo.Size = new Size(500, 50);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo); // Añade el título al formulario.

            // Subtítulo.
            Label lblSubtitulo = new Label();
            lblSubtitulo.Text = "Sistema de Gestión Médica Integral";
            lblSubtitulo.Font = new Font("Segoe UI", 16, FontStyle.Italic);
            lblSubtitulo.Location = new Point(200, 100);
            lblSubtitulo.Size = new Size(500, 40);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblSubtitulo); // Añade el subtítulo al formulario.

            // Panel para la opción de Paciente.
            Panel panelPaciente = new Panel();
            panelPaciente.Location = new Point(100, 180);
            panelPaciente.Size = new Size(300, 300);
            panelPaciente.BorderStyle = BorderStyle.FixedSingle;
            panelPaciente.BackColor = Color.White;
            panelPaciente.Cursor = Cursors.Hand; // Cambia el cursor para indicar que es clickeable.
            this.Controls.Add(panelPaciente); // Añade el panel de paciente al formulario.

            // PictureBox para imagen de Paciente (placeholder).
            PictureBox picPaciente = new PictureBox();
            picPaciente.Location = new Point(75, 30);
            picPaciente.Size = new Size(150, 150);
            picPaciente.BorderStyle = BorderStyle.FixedSingle;
            picPaciente.SizeMode = PictureBoxSizeMode.Zoom;
            panelPaciente.Controls.Add(picPaciente); // Añade la imagen al panel de paciente.
            picPaciente.Image = WC_MediCenter.Properties.Resources.Medicenter_1;

            // Etiqueta para "Paciente".
            Label lblPaciente = new Label();
            lblPaciente.Text = "Paciente";
            lblPaciente.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblPaciente.Location = new Point(0, 220);
            lblPaciente.Size = new Size(300, 40);
            lblPaciente.TextAlign = ContentAlignment.MiddleCenter;
            panelPaciente.Controls.Add(lblPaciente); // Añade la etiqueta al panel de paciente.

            // Asigna el evento de clic para abrir el login de paciente a los controles del panel de paciente.
            panelPaciente.Click += (s, e) => AbrirLoginPaciente();
            picPaciente.Click += (s, e) => AbrirLoginPaciente();
            lblPaciente.Click += (s, e) => AbrirLoginPaciente();

            // Panel para la opción de Personal Hospitalario.
            Panel panelPersonal = new Panel();
            panelPersonal.Location = new Point(500, 180);
            panelPersonal.Size = new Size(300, 300);
            panelPersonal.BorderStyle = BorderStyle.FixedSingle;
            panelPersonal.BackColor = Color.White;
            panelPersonal.Cursor = Cursors.Hand; // Cambia el cursor para indicar que es clickeable.
            this.Controls.Add(panelPersonal); // Añade el panel de personal al formulario.

            // PictureBox para imagen de Personal Hospitalario (placeholder).
            PictureBox picPersonal = new PictureBox();
            picPersonal.Location = new Point(75, 30);
            picPersonal.Size = new Size(150, 150);
            picPersonal.BorderStyle = BorderStyle.FixedSingle;
            picPersonal.SizeMode = PictureBoxSizeMode.Zoom;
            panelPersonal.Controls.Add(picPersonal); // Añade la imagen al panel de personal.
            picPersonal.Image = WC_MediCenter.Properties.Resources.Personal_Hosp;


            // Etiqueta para "Personal Hospitalario".
            Label lblPersonal = new Label();
            lblPersonal.Text = "Personal Hospitalario";
            lblPersonal.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblPersonal.Location = new Point(0, 220);
            lblPersonal.Size = new Size(300, 40);
            lblPersonal.TextAlign = ContentAlignment.MiddleCenter;
            panelPersonal.Controls.Add(lblPersonal); // Añade la etiqueta al panel de personal.
            // 

            // Asigna el evento de clic para abrir el login de personal a los controles del panel de personal.
            panelPersonal.Click += (s, e) => AbrirLoginPersonal();
            picPersonal.Click += (s, e) => AbrirLoginPersonal();
            lblPersonal.Click += (s, e) => AbrirLoginPersonal();

            // Etiqueta de instrucción.
            Label lblInstruccion = new Label();
            lblInstruccion.Text = "Seleccione una opción para continuar";
            lblInstruccion.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            lblInstruccion.Location = new Point(200, 500);
            lblInstruccion.Size = new Size(500, 30);
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInstruccion); // Añade la instrucción al formulario.

            this.ResumeLayout(false); // Reanuda la lógica de diseño.
        }

        // Método para abrir el formulario de inicio de sesión de pacientes.
        private void AbrirLoginPaciente()
        {
            FormLoginPaciente formLogin = new FormLoginPaciente(sistema); // Crea una instancia del formulario de login de paciente.
            formLogin.ShowDialog(); // Muestra el formulario de login de paciente como un diálogo modal.
        }

        // Método para abrir el formulario de inicio de sesión de personal hospitalario.
        private void AbrirLoginPersonal()
        {
            FormLoginPersonal formLogin = new FormLoginPersonal(sistema); // Crea una instancia del formulario de login de personal.
            formLogin.ShowDialog(); // Muestra el formulario de login de personal como un diálogo modal.
        }

        // Sobrescribe el método OnFormClosing para manejar eventos al cerrar el formulario.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e); // Llama al método base OnFormClosing.
            // Aquí se podría añadir lógica para guardar el estado del sistema si fuera necesario
            // antes de que la aplicación se cierre por completo.
        }
    }
}