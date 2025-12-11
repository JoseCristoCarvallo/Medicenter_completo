using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para forzar el cambio de contraseña a un usuario (generalmente un administrador
    // con contraseña por defecto) al iniciar sesión por primera vez.
    public partial class FormCambiarPassword : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar cambios.
        private PersonalHospitalario personal; // Objeto PersonalHospitalario que requiere el cambio de contraseña.
        
        // Campos de texto para la nueva contraseña.
        private TextBox txtNuevaPassword;
        private TextBox txtConfirmarPassword;

        // Constructor del formulario.
        public FormCambiarPassword(Sistema sistemaParam, PersonalHospitalario personalParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            personal = personalParam; // Asigna el objeto personal.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(600, 400); // Tamaño del formulario.
            this.Text = "Cambio de Contraseña Requerido"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.ControlBox = false; // Deshabilita los botones de control (cerrar, minimizar, maximizar) para forzar la acción.

            // Título principal.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Cambio de Contraseña Requerido";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(120, 30);
            lblTitulo.Size = new Size(360, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Mensaje informativo para el usuario.
            Label lblInfo = new Label();
            lblInfo.Text = "Por seguridad, debe cambiar su contraseña";
            lblInfo.Font = new Font("Segoe UI", 11);
            lblInfo.Location = new Point(130, 80);
            lblInfo.Size = new Size(340, 30);
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInfo);

            // Campo para la "Nueva Contraseña".
            Label lblNueva = new Label();
            lblNueva.Text = "Nueva Contraseña:";
            lblNueva.Font = new Font("Segoe UI", 11);
            lblNueva.Location = new Point(80, 140);
            lblNueva.Size = new Size(150, 30);
            lblNueva.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNueva);

            txtNuevaPassword = new TextBox();
            txtNuevaPassword.Font = new Font("Segoe UI", 11);
            txtNuevaPassword.Location = new Point(240, 140);
            txtNuevaPassword.Size = new Size(250, 30);
            txtNuevaPassword.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtNuevaPassword);

            // Campo para "Confirmar Contraseña".
            Label lblConfirmar = new Label();
            lblConfirmar.Text = "Confirmar Contraseña:";
            lblConfirmar.Font = new Font("Segoe UI", 11);
            lblConfirmar.Location = new Point(80, 190);
            lblConfirmar.Size = new Size(150, 30);
            lblConfirmar.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblConfirmar);

            txtConfirmarPassword = new TextBox();
            txtConfirmarPassword.Font = new Font("Segoe UI", 11);
            txtConfirmarPassword.Location = new Point(240, 190);
            txtConfirmarPassword.Size = new Size(250, 30);
            txtConfirmarPassword.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtConfirmarPassword);

            // Botón "Cambiar Contraseña".
            Button btnCambiar = new Button();
            btnCambiar.Text = "Cambiar Contraseña";
            btnCambiar.Font = new Font("Segoe UI", 12);
            btnCambiar.Location = new Point(200, 270);
            btnCambiar.Size = new Size(200, 50);
            btnCambiar.BackColor = Color.White;
            btnCambiar.Click += BtnCambiar_Click; // Asigna el evento para cambiar la contraseña.
            this.Controls.Add(btnCambiar);
        }

        // Manejador de eventos para el botón "Cambiar Contraseña".
        private void BtnCambiar_Click(object sender, EventArgs e)
        {
            string nueva = txtNuevaPassword.Text; // Obtiene la nueva contraseña.
            string confirmar = txtConfirmarPassword.Text; // Obtiene la confirmación de la contraseña.

            // Valida que ambos campos no estén vacíos.
            if (string.IsNullOrWhiteSpace(nueva) || string.IsNullOrWhiteSpace(confirmar))
            {
                MessageBox.Show("Por favor complete todos los campos", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida que la nueva contraseña tenga al menos 6 caracteres.
            if (nueva.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida que las contraseñas coincidan.
            if (nueva != confirmar)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            personal.Password = nueva; // Asigna la nueva contraseña al objeto personal.
            personal.CambioPassword = true; // Marca que la contraseña ya ha sido cambiada.
            sistema.GuardarUsuario(personal); // Guarda los cambios del personal en la persistencia.

            MessageBox.Show("Contraseña actualizada exitosamente", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK; // Establece el resultado del diálogo a OK.
            this.Close(); // Cierra el formulario.
        }
    }
}