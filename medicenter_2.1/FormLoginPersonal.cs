using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario de inicio de sesión para el personal hospitalario (médicos y administradores).
    // Permite al personal autenticarse y ser redirigido a su menú correspondiente.
    public partial class FormLoginPersonal : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para gestionar datos y lógica de negocio.
        private TextBox txtID; // Campo de texto para el ID del personal.
        private TextBox txtPassword; // Campo de texto para la contraseña del personal.

        // Constructor del formulario de login de personal.
        public FormLoginPersonal(Sistema sistemaParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            this.SuspendLayout(); // Suspende la lógica de diseño.

            // Configuración básica del formulario.
            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
            this.Text = "Login - Personal Hospitalario"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Control de pestañas (TabControl). Originalmente tenía dos, pero la de registro fue eliminada.
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(30, 50);
            tabControl.Size = new Size(740, 500);
            tabControl.Font = new Font("Segoe UI", 10);
            this.Controls.Add(tabControl); // Añade el TabControl al formulario.

            // Pestaña para "Iniciar Sesión".
            TabPage tabLogin = new TabPage("Iniciar Sesión");
            tabControl.TabPages.Add(tabLogin);
            CrearTabLogin(tabLogin); // Llama al método para construir el contenido de la pestaña de login.

            // NOTA: La pestaña de "Registrarse" y su método asociado (CrearTabRegistro) han sido eliminados
            // para este formulario, ya que el registro de personal se gestiona a través del administrador.

            this.ResumeLayout(false); // Reanuda la lógica de diseño.
        }

        // Método para construir el contenido de la pestaña de "Iniciar Sesión".
        private void CrearTabLogin(TabPage tab)
        {
            // Título de la pestaña de login.
            Label lblTitulo = new Label();
            lblTitulo.Text = "¡Bienvenido Doctor!"; // Título genérico para el personal.
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold | FontStyle.Italic);
            lblTitulo.Location = new Point(200, 50);
            lblTitulo.Size = new Size(400, 50);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            tab.Controls.Add(lblTitulo);

            // Etiqueta y campo de texto para el ID.
            Label lblID = new Label();
            lblID.Text = "ID:";
            lblID.Font = new Font("Segoe UI", 12);
            lblID.Location = new Point(150, 150);
            lblID.Size = new Size(100, 30);
            lblID.TextAlign = ContentAlignment.MiddleRight;
            tab.Controls.Add(lblID);

            txtID = new TextBox();
            txtID.Font = new Font("Segoe UI", 12);
            txtID.Location = new Point(270, 150);
            txtID.Size = new Size(250, 30);
            tab.Controls.Add(txtID);

            // Etiqueta y campo de texto para la contraseña.
            Label lblPassword = new Label();
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Segoe UI", 12);
            lblPassword.Location = new Point(150, 210);
            lblPassword.Size = new Size(110, 30);
            lblPassword.TextAlign = ContentAlignment.MiddleRight;
            tab.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 12);
            txtPassword.Location = new Point(270, 210);
            txtPassword.Size = new Size(250, 30);
            txtPassword.UseSystemPasswordChar = true; // Oculta la contraseña.
            tab.Controls.Add(txtPassword);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnCancelar.Location = new Point(220, 290);
            btnCancelar.Size = new Size(130, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            tab.Controls.Add(btnCancelar);

            // Botón "Iniciar".
            Button btnIniciar = new Button();
            btnIniciar.Text = "Iniciar";
            btnIniciar.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnIniciar.Location = new Point(380, 290);
            btnIniciar.Size = new Size(130, 45);
            btnIniciar.BackColor = Color.White;
            btnIniciar.Click += BtnIniciar_Click; // Asigna el evento de login.
            tab.Controls.Add(btnIniciar);

            // Etiqueta de instrucción para contactar al administrador para el registro.
            PersonalHospitalario adminGeneral = sistema.Personal.FirstOrDefault(p => p.Id == "ADMIN001"); // Busca al admin general.
            string adminEmail = adminGeneral?.Email ?? "admin@medicenter.com"; // Obtiene el email del admin o un fallback.
            
            Label lblInstruccion = new Label();
            lblInstruccion.Text = $"Contacte al administrador general para registrar su usuario doctor: {adminEmail}";
            lblInstruccion.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblInstruccion.Location = new Point(100, 370);
            lblInstruccion.Size = new Size(600, 50);
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;
            tab.Controls.Add(lblInstruccion);
        }

        // Manejador de eventos para el botón "Iniciar".
        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            string id = txtID.Text.Trim(); // Obtiene el ID del campo de texto.
            string password = txtPassword.Text; // Obtiene la contraseña.

            // Valida que ambos campos no estén vacíos.
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor complete todos los campos", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Busca al personal en el sistema usando las credenciales proporcionadas.
            PersonalHospitalario personal = sistema.BuscarPersonal(id, password);

            if (personal != null)
            {
                MessageBox.Show($"Acceso concedido\nBienvenido, {personal.Nombre}", "Inicio exitoso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Verifica si el personal debe cambiar su contraseña (ej. primer login del admin por defecto).
                if (!personal.CambioPassword && password == "medicenter2025") // Condición específica para forzar cambio de password
                {
                    // Abre el formulario para cambiar la contraseña.
                    FormCambiarPassword formCambio = new FormCambiarPassword(sistema, personal);
                    if (formCambio.ShowDialog() == DialogResult.OK)
                    {
                        AbrirMenuPersonal(personal); // Si el cambio es exitoso, abre el menú.
                    }
                }
                else
                {
                    AbrirMenuPersonal(personal); // Abre directamente el menú si no necesita cambiar la contraseña.
                }
            }
            else
            {
                // Muestra un mensaje de error si las credenciales son incorrectas.
                MessageBox.Show("Credenciales incorrectas", "Error de autenticación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para abrir el menú correspondiente al nivel de acceso del personal.
        private void AbrirMenuPersonal(PersonalHospitalario personal)
        {
            this.Hide(); // Oculta el formulario de login actual.

            // Redirige al menú de administrador o médico según el nivel de acceso.
            if (personal.NivelAcceso == NivelAcceso.Administrador)
            {
                FormMenuAdministrador formMenu = new FormMenuAdministrador(sistema, personal);
                formMenu.ShowDialog();
            }
            else // Asume que cualquier otro nivel de acceso es de médico (MedicoGeneral).
            {
                FormMenuMedico formMenu = new FormMenuMedico(sistema, personal);
                formMenu.ShowDialog();
            }

            this.Close(); // Cierra el formulario de login una vez que el menú se ha cerrado.
        }
    }
}
