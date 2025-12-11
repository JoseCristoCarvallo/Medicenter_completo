using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el personal hospitalario (médicos o administradores)
    // pueda cambiar su contraseña de forma voluntaria.
    public partial class FormCambiarPasswordPersonal : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar cambios.
        private PersonalHospitalario personal; // Objeto PersonalHospitalario que desea cambiar su contraseña.

        // Constructor del formulario.
        public FormCambiarPasswordPersonal(Sistema sistemaParam, PersonalHospitalario personalParam)
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
            this.Text = "Cambiar Contraseña"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Cambiar Contraseña";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(180, 30);
            lblTitulo.Size = new Size(240, 40);
            this.Controls.Add(lblTitulo);

            // Campo para la "Contraseña Actual".
            Label lblActual = new Label();
            lblActual.Text = "Contraseña Actual:";
            lblActual.Font = new Font("Segoe UI", 11);
            lblActual.Location = new Point(80, 100);
            lblActual.Size = new Size(150, 30);
            lblActual.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblActual);

            TextBox txtActual = new TextBox();
            txtActual.Font = new Font("Segoe UI", 11);
            txtActual.Location = new Point(240, 100);
            txtActual.Size = new Size(250, 30);
            txtActual.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtActual);

            // Campo para la "Nueva Contraseña".
            Label lblNueva = new Label();
            lblNueva.Text = "Nueva Contraseña:";
            lblNueva.Font = new Font("Segoe UI", 11);
            lblNueva.Location = new Point(80, 150);
            lblNueva.Size = new Size(150, 30);
            lblNueva.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNueva);

            TextBox txtNueva = new TextBox();
            txtNueva.Font = new Font("Segoe UI", 11);
            txtNueva.Location = new Point(240, 150);
            txtNueva.Size = new Size(250, 30);
            txtNueva.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtNueva);

            // Campo para "Confirmar Contraseña".
            Label lblConfirmar = new Label();
            lblConfirmar.Text = "Confirmar Contraseña:";
            lblConfirmar.Font = new Font("Segoe UI", 11);
            lblConfirmar.Location = new Point(80, 200);
            lblConfirmar.Size = new Size(150, 30);
            lblConfirmar.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblConfirmar);

            TextBox txtConfirmar = new TextBox();
            txtConfirmar.Font = new Font("Segoe UI", 11);
            txtConfirmar.Location = new Point(240, 200);
            txtConfirmar.Size = new Size(250, 30);
            txtConfirmar.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtConfirmar);

            // Botón "Cambiar".
            Button btnCambiar = new Button();
            btnCambiar.Text = "Cambiar";
            btnCambiar.Font = new Font("Segoe UI", 12);
            btnCambiar.Location = new Point(190, 280);
            btnCambiar.Size = new Size(120, 50);
            btnCambiar.BackColor = Color.White;
            btnCambiar.Click += (s, e) =>
            {
                // Valida que la contraseña actual ingresada coincida con la del personal.
                if (txtActual.Text != personal.Password)
                {
                    MessageBox.Show("Contraseña actual incorrecta", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Valida que la nueva contraseña tenga al menos 6 caracteres.
                if (txtNueva.Text.Length < 6)
                {
                    MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Valida que la nueva contraseña y su confirmación coincidan.
                if (txtNueva.Text != txtConfirmar.Text)
                {
                    MessageBox.Show("Las contraseñas no coinciden", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                personal.Password = txtNueva.Text; // Asigna la nueva contraseña al objeto personal.
                personal.CambioPassword = true; // Marca que la contraseña ya ha sido cambiada.
                sistema.GuardarUsuario(personal); // Guarda los cambios del personal en la persistencia.

                MessageBox.Show("Contraseña actualizada exitosamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario.
            };
            this.Controls.Add(btnCambiar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(330, 280);
            btnCancelar.Size = new Size(120, 50);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);
        }
    }
}