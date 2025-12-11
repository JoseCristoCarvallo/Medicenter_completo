using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que un usuario con perfil de Administrador edite sus propios datos,
    // incluyendo el email y la contraseña.
    public partial class FormEditarAdminData : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar cambios.
        private PersonalHospitalario _admin; // Referencia al objeto PersonalHospitalario (administrador) que se va a editar.

        // Campos de texto para la entrada de datos.
        private TextBox txtEmail;
        private TextBox txtPasswordActual;
        private TextBox txtPasswordNueva;
        private TextBox txtPasswordConfirmar;

        // Constructor del formulario.
        public FormEditarAdminData(Sistema sistema, PersonalHospitalario admin)
        {
            _sistema = sistema; // Asigna la instancia del sistema.
            _admin = admin; // Asigna el objeto administrador logueado.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
            CargarDatosAdmin(); // Carga los datos actuales del administrador en los campos.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(600, 500); // Tamaño del formulario.
            this.Text = "Editar Mis Datos de Administrador"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Editar Datos de Administrador";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(600, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            int yPos = 80; // Posición vertical inicial para los controles.

            // Campo para el "Nuevo Email".
            Label lblEmail = new Label();
            lblEmail.Text = "Nuevo Email:";
            lblEmail.Font = new Font("Segoe UI", 11);
            lblEmail.Location = new Point(80, yPos);
            lblEmail.Size = new Size(150, 30);
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 11);
            txtEmail.Location = new Point(250, yPos);
            txtEmail.Size = new Size(250, 30);
            this.Controls.Add(txtEmail);

            yPos += 60;

            // Campo para la "Contraseña Actual" (requerida para cambiar la contraseña).
            Label lblPassActual = new Label();
            lblPassActual.Text = "Contraseña Actual:";
            lblPassActual.Font = new Font("Segoe UI", 11);
            lblPassActual.Location = new Point(80, yPos);
            lblPassActual.Size = new Size(150, 30);
            lblPassActual.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblPassActual);

            txtPasswordActual = new TextBox();
            txtPasswordActual.Font = new Font("Segoe UI", 11);
            txtPasswordActual.Location = new Point(250, yPos);
            txtPasswordActual.Size = new Size(250, 30);
            txtPasswordActual.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtPasswordActual);

            yPos += 50;

            // Campo para la "Nueva Contraseña".
            Label lblPassNueva = new Label();
            lblPassNueva.Text = "Nueva Contraseña:";
            lblPassNueva.Font = new Font("Segoe UI", 11);
            lblPassNueva.Location = new Point(80, yPos);
            lblPassNueva.Size = new Size(150, 30);
            lblPassNueva.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblPassNueva);

            txtPasswordNueva = new TextBox();
            txtPasswordNueva.Font = new Font("Segoe UI", 11);
            txtPasswordNueva.Location = new Point(250, yPos);
            txtPasswordNueva.Size = new Size(250, 30);
            txtPasswordNueva.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtPasswordNueva);

            yPos += 50;

            // Campo para "Confirmar Nueva Contraseña".
            Label lblPassConfirmar = new Label();
            lblPassConfirmar.Text = "Confirmar Nueva:";
            lblPassConfirmar.Font = new Font("Segoe UI", 11);
            lblPassConfirmar.Location = new Point(80, yPos);
            lblPassConfirmar.Size = new Size(150, 30);
            lblPassConfirmar.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblPassConfirmar);

            txtPasswordConfirmar = new TextBox();
            txtPasswordConfirmar.Font = new Font("Segoe UI", 11);
            txtPasswordConfirmar.Location = new Point(250, yPos);
            txtPasswordConfirmar.Size = new Size(250, 30);
            txtPasswordConfirmar.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtPasswordConfirmar);

            yPos += 60;

            // Botón "Guardar Cambios".
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.Font = new Font("Segoe UI", 12);
            btnGuardar.Location = new Point(160, yPos);
            btnGuardar.Size = new Size(150, 50);
            btnGuardar.BackColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click; // Asigna el evento de guardado.
            this.Controls.Add(btnGuardar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(330, yPos);
            btnCancelar.Size = new Size(120, 50);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); }; // Cierra el formulario con resultado Cancel.
            this.Controls.Add(btnCancelar);
        }

        // Carga los datos actuales del administrador en los campos del formulario.
        private void CargarDatosAdmin()
        {
            txtEmail.Text = _admin.Email; // Muestra el email actual del administrador.
        }

        // Manejador de eventos para el botón "Guardar Cambios".
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            bool cambiosRealizados = false; // Flag para indicar si se realizaron cambios.

            // 1. Validar y actualizar Email.
            if (txtEmail.Text.Trim() != _admin.Email)
            {
                // Valida que el email no esté vacío y contenga "@".
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
                {
                    MessageBox.Show("Ingrese un email válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Detiene el proceso si el email no es válido.
                }
                _admin.Email = txtEmail.Text.Trim(); // Actualiza el email del administrador.
                cambiosRealizados = true;
            }

            // 2. Validar y actualizar Contraseña.
            // Se realiza solo si alguno de los campos de contraseña ha sido llenado.
            if (!string.IsNullOrWhiteSpace(txtPasswordActual.Text) ||
                !string.IsNullOrWhiteSpace(txtPasswordNueva.Text) ||
                !string.IsNullOrWhiteSpace(txtPasswordConfirmar.Text))
            {
                // Verifica que la contraseña actual ingresada sea correcta.
                if (txtPasswordActual.Text != _admin.Password)
                {
                    MessageBox.Show("La contraseña actual ingresada es incorrecta.", "Error de Contraseña", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Valida que la nueva contraseña tenga al menos 6 caracteres.
                if (txtPasswordNueva.Text.Length < 6)
                {
                    MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Error de Contraseña", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Valida que la nueva contraseña y su confirmación coincidan.
                if (txtPasswordNueva.Text != txtPasswordConfirmar.Text)
                {
                    MessageBox.Show("La nueva contraseña y su confirmación no coinciden.", "Error de Contraseña", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _admin.Password = txtPasswordNueva.Text; // Asigna la nueva contraseña al objeto administrador.
                _admin.CambioPassword = true; // Marca que la contraseña fue cambiada.
                cambiosRealizados = true;
            }

            // Si se realizaron cambios (email o contraseña), guarda el administrador en el sistema.
            if (cambiosRealizados)
            {
                _sistema.GuardarUsuario(_admin); // Persiste los cambios del administrador.
                MessageBox.Show("Datos actualizados exitosamente.", "Actualización Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Establece el resultado del diálogo a OK.
                this.Close(); // Cierra el formulario.
            }
            else
            {
                MessageBox.Show("No se realizaron cambios en los datos.", "Sin Cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
