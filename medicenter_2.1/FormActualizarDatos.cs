using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el paciente pueda actualizar sus datos personales (teléfono, email, contacto de emergencia),
    // cambiar su contraseña y actualizar su estado de seguro médico.
    public partial class FormActualizarDatos : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar cambios.
        private Paciente paciente; // Objeto Paciente cuyos datos se van a actualizar.

        // Campos de texto para la entrada de datos.
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TextBox txtContacto;
        private TextBox txtPasswordActual; // Campo para la contraseña actual del paciente.
        private TextBox txtPasswordNueva; // Campo para la nueva contraseña del paciente.

        // Campos para el seguro médico.
        private RadioButton rbSiSeguro;
        private RadioButton rbNoSeguro;
        private TextBox txtNumeroSeguro;
        private PictureBox picSeguroFrontal;
        private PictureBox picSeguroTrasera;
        private Button btnCargarFrontal;
        private Button btnCargarTrasera;
        private Panel panelSeguro;
        private byte[] _imagenSeguroFrontalBytes;
        private byte[] _imagenSeguroTraseraBytes;

        // Constructor del formulario.
        public FormActualizarDatos(Sistema sistemaParam, Paciente pacienteParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            paciente = pacienteParam; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(750, 850); // Tamaño del formulario aumentado para incluir seguro.
            this.Text = "Actualizar Datos Personales"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.AutoScroll = true; // Permite scroll si es necesario.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Actualizar Datos Personales";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(180, 20);
            lblTitulo.Size = new Size(340, 40);
            this.Controls.Add(lblTitulo);

            int yPos = 90; // Posición vertical inicial para los controles.

            // Campo para "Nuevo Teléfono".
            Label lblTelefono = new Label();
            lblTelefono.Text = "Nuevo Teléfono:";
            lblTelefono.Font = new Font("Segoe UI", 11);
            lblTelefono.Location = new Point(80, yPos);
            lblTelefono.Size = new Size(150, 30);
            lblTelefono.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblTelefono);

            txtTelefono = new TextBox();
            txtTelefono.Font = new Font("Segoe UI", 11);
            txtTelefono.Location = new Point(250, yPos);
            txtTelefono.Size = new Size(300, 30);
            txtTelefono.Text = paciente.Telefono; // Muestra el teléfono actual del paciente.
            txtTelefono.KeyPress += TxtNumeric_KeyPress; // Añade validación numérica para el campo.
            this.Controls.Add(txtTelefono);

            yPos += 50;

            // Campo para "Nuevo Email".
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
            txtEmail.Size = new Size(300, 30);
            txtEmail.Text = paciente.Email; // Muestra el email actual del paciente.
            this.Controls.Add(txtEmail);

            yPos += 50;

            // Campo para "Nuevo Contacto de Emergencia".
            Label lblContacto = new Label();
            lblContacto.Text = "Nuevo Contacto Emergencia:";
            lblContacto.Font = new Font("Segoe UI", 11);
            lblContacto.Location = new Point(50, yPos);
            lblContacto.Size = new Size(180, 30);
            lblContacto.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblContacto);

            txtContacto = new TextBox();
            txtContacto.Font = new Font("Segoe UI", 11);
            txtContacto.Location = new Point(250, yPos);
            txtContacto.Size = new Size(300, 30);
            txtContacto.Text = paciente.ContactoEmergencia; // Muestra el contacto de emergencia actual.
            txtContacto.KeyPress += TxtNumeric_KeyPress; // Añade validación numérica.
            this.Controls.Add(txtContacto);

            yPos += 60;

            // Separador y título para la sección de "Seguro Médico".
            Label lblSeparadorSeguro = new Label();
            lblSeparadorSeguro.Text = "Seguro Medico";
            lblSeparadorSeguro.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSeparadorSeguro.Location = new Point(280, yPos);
            lblSeparadorSeguro.Size = new Size(200, 30);
            this.Controls.Add(lblSeparadorSeguro);

            yPos += 35;

            // Pregunta si tiene seguro.
            Label lblTieneSeguro = new Label();
            lblTieneSeguro.Text = "Tiene seguro medico?";
            lblTieneSeguro.Font = new Font("Segoe UI", 11);
            lblTieneSeguro.Location = new Point(80, yPos);
            lblTieneSeguro.Size = new Size(180, 30);
            lblTieneSeguro.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblTieneSeguro);

            rbSiSeguro = new RadioButton();
            rbSiSeguro.Text = "Si";
            rbSiSeguro.Font = new Font("Segoe UI", 11);
            rbSiSeguro.Location = new Point(280, yPos);
            rbSiSeguro.Size = new Size(60, 30);
            rbSiSeguro.CheckedChanged += RbSeguro_CheckedChanged;
            this.Controls.Add(rbSiSeguro);

            rbNoSeguro = new RadioButton();
            rbNoSeguro.Text = "No";
            rbNoSeguro.Font = new Font("Segoe UI", 11);
            rbNoSeguro.Location = new Point(360, yPos);
            rbNoSeguro.Size = new Size(60, 30);
            rbNoSeguro.CheckedChanged += RbSeguro_CheckedChanged;
            this.Controls.Add(rbNoSeguro);

            yPos += 40;

            // Panel que contiene los campos de seguro (visible solo si tiene seguro).
            panelSeguro = new Panel();
            panelSeguro.Location = new Point(30, yPos);
            panelSeguro.Size = new Size(680, 220);
            panelSeguro.BackColor = Color.FromArgb(240, 240, 255);
            this.Controls.Add(panelSeguro);

            // Número de seguro.
            Label lblNumSeguro = new Label();
            lblNumSeguro.Text = "Numero de Seguro:";
            lblNumSeguro.Font = new Font("Segoe UI", 11);
            lblNumSeguro.Location = new Point(50, 10);
            lblNumSeguro.Size = new Size(150, 30);
            lblNumSeguro.TextAlign = ContentAlignment.MiddleRight;
            panelSeguro.Controls.Add(lblNumSeguro);

            txtNumeroSeguro = new TextBox();
            txtNumeroSeguro.Font = new Font("Segoe UI", 11);
            txtNumeroSeguro.Location = new Point(220, 10);
            txtNumeroSeguro.Size = new Size(300, 30);
            txtNumeroSeguro.Text = paciente.NumeroSeguro ?? "";
            panelSeguro.Controls.Add(txtNumeroSeguro);

            // Imagen frontal del seguro.
            Label lblFrontal = new Label();
            lblFrontal.Text = "Imagen Frontal:";
            lblFrontal.Font = new Font("Segoe UI", 10);
            lblFrontal.Location = new Point(50, 50);
            lblFrontal.Size = new Size(120, 25);
            panelSeguro.Controls.Add(lblFrontal);

            picSeguroFrontal = new PictureBox();
            picSeguroFrontal.Location = new Point(50, 75);
            picSeguroFrontal.Size = new Size(140, 90);
            picSeguroFrontal.BorderStyle = BorderStyle.FixedSingle;
            picSeguroFrontal.SizeMode = PictureBoxSizeMode.Zoom;
            panelSeguro.Controls.Add(picSeguroFrontal);

            btnCargarFrontal = new Button();
            btnCargarFrontal.Text = "Cargar";
            btnCargarFrontal.Font = new Font("Segoe UI", 9);
            btnCargarFrontal.Location = new Point(80, 170);
            btnCargarFrontal.Size = new Size(80, 30);
            btnCargarFrontal.Click += BtnCargarFrontal_Click;
            panelSeguro.Controls.Add(btnCargarFrontal);

            // Imagen trasera del seguro.
            Label lblTrasera = new Label();
            lblTrasera.Text = "Imagen Trasera:";
            lblTrasera.Font = new Font("Segoe UI", 10);
            lblTrasera.Location = new Point(250, 50);
            lblTrasera.Size = new Size(120, 25);
            panelSeguro.Controls.Add(lblTrasera);

            picSeguroTrasera = new PictureBox();
            picSeguroTrasera.Location = new Point(250, 75);
            picSeguroTrasera.Size = new Size(140, 90);
            picSeguroTrasera.BorderStyle = BorderStyle.FixedSingle;
            picSeguroTrasera.SizeMode = PictureBoxSizeMode.Zoom;
            panelSeguro.Controls.Add(picSeguroTrasera);

            btnCargarTrasera = new Button();
            btnCargarTrasera.Text = "Cargar";
            btnCargarTrasera.Font = new Font("Segoe UI", 9);
            btnCargarTrasera.Location = new Point(280, 170);
            btnCargarTrasera.Size = new Size(80, 30);
            btnCargarTrasera.Click += BtnCargarTrasera_Click;
            panelSeguro.Controls.Add(btnCargarTrasera);

            // Cargar estado actual del seguro del paciente.
            CargarEstadoSeguroActual();

            yPos += 230;

            // Separador y título para la sección de "Cambiar Contraseña".
            Label lblSeparador = new Label();
            lblSeparador.Text = "Cambiar Contraseña";
            lblSeparador.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSeparador.Location = new Point(240, yPos);
            lblSeparador.Size = new Size(220, 30);
            this.Controls.Add(lblSeparador);

            yPos += 40;

            // Campo para la "Contraseña Actual".
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
            txtPasswordActual.Size = new Size(300, 30);
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
            txtPasswordNueva.Size = new Size(300, 30);
            txtPasswordNueva.UseSystemPasswordChar = true; // Oculta la entrada.
            this.Controls.Add(txtPasswordNueva);

            yPos += 70;

            // Botón "Guardar Cambios".
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.Font = new Font("Segoe UI", 12);
            btnGuardar.Location = new Point(200, yPos);
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.BackColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click; // Asigna el evento para guardar.
            this.Controls.Add(btnGuardar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(380, yPos);
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);
        }

        // Manejador de eventos para restringir la entrada a solo números en campos de texto específicos.
        private void TxtNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite solo dígitos y la tecla de retroceso (borrar).
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignora cualquier otro carácter presionado.
            }
        }

        // Carga el estado actual del seguro del paciente en los controles.
        private void CargarEstadoSeguroActual()
        {
            // Si el paciente tiene seguro (cualquier tipo diferente de SinSeguro).
            bool tieneSeguro = paciente.TipoSeguro != TipoSeguro.SinSeguro;

            if (tieneSeguro)
            {
                rbSiSeguro.Checked = true;
                panelSeguro.Visible = true;

                // Cargar imágenes existentes si las tiene.
                if (paciente.ImagenSeguroFrontal != null && paciente.ImagenSeguroFrontal.Length > 0)
                {
                    _imagenSeguroFrontalBytes = paciente.ImagenSeguroFrontal;
                    using (var ms = new MemoryStream(paciente.ImagenSeguroFrontal))
                    {
                        picSeguroFrontal.Image = Image.FromStream(ms);
                    }
                }

                if (paciente.ImagenSeguroTrasera != null && paciente.ImagenSeguroTrasera.Length > 0)
                {
                    _imagenSeguroTraseraBytes = paciente.ImagenSeguroTrasera;
                    using (var ms = new MemoryStream(paciente.ImagenSeguroTrasera))
                    {
                        picSeguroTrasera.Image = Image.FromStream(ms);
                    }
                }
            }
            else
            {
                rbNoSeguro.Checked = true;
                panelSeguro.Visible = false;
            }
        }

        // Manejador de eventos para cambio de opción de seguro.
        private void RbSeguro_CheckedChanged(object sender, EventArgs e)
        {
            panelSeguro.Visible = rbSiSeguro.Checked;
        }

        // Manejador de eventos para cargar imagen frontal del seguro.
        private void BtnCargarFrontal_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Seleccionar imagen frontal del seguro";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _imagenSeguroFrontalBytes = File.ReadAllBytes(ofd.FileName);
                    using (var ms = new MemoryStream(_imagenSeguroFrontalBytes))
                    {
                        picSeguroFrontal.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        // Manejador de eventos para cargar imagen trasera del seguro.
        private void BtnCargarTrasera_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Seleccionar imagen trasera del seguro";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _imagenSeguroTraseraBytes = File.ReadAllBytes(ofd.FileName);
                    using (var ms = new MemoryStream(_imagenSeguroTraseraBytes))
                    {
                        picSeguroTrasera.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        // Manejador de eventos para el botón "Guardar Cambios".
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            bool cambios = false; // Flag para indicar si se realizaron cambios.

            // Actualiza el teléfono si el nuevo valor es diferente y no está vacío.
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && txtTelefono.Text != paciente.Telefono)
            {
                paciente.Telefono = txtTelefono.Text.Trim();
                cambios = true;
            }

            // Actualiza el email si el nuevo valor es diferente y no está vacío.
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && txtEmail.Text != paciente.Email)
            {
                paciente.Email = txtEmail.Text.Trim();
                cambios = true;
            }

            // Actualiza el contacto de emergencia si el nuevo valor es diferente y no está vacío.
            if (!string.IsNullOrWhiteSpace(txtContacto.Text) && txtContacto.Text != paciente.ContactoEmergencia)
            {
                paciente.ContactoEmergencia = txtContacto.Text.Trim();
                cambios = true;
            }

            // Lógica para actualizar el seguro médico.
            bool nuevoTieneSeguro = rbSiSeguro.Checked;
            bool tieneSeguroActual = paciente.TipoSeguro != TipoSeguro.SinSeguro;

            // Si cambió el estado del seguro.
            if (nuevoTieneSeguro != tieneSeguroActual)
            {
                if (nuevoTieneSeguro)
                {
                    // Ahora tiene seguro, asignar tipo básico por defecto.
                    paciente.TipoSeguro = TipoSeguro.SeguroBasico;
                }
                else
                {
                    // Ya no tiene seguro, limpiar datos.
                    paciente.TipoSeguro = TipoSeguro.SinSeguro;
                    paciente.NumeroSeguro = "";
                    paciente.ImagenSeguroFrontal = null;
                    paciente.ImagenSeguroTrasera = null;
                }
                cambios = true;
            }

            // Si tiene seguro, validar y actualizar datos del seguro.
            if (nuevoTieneSeguro)
            {
                // Validar que tenga número de seguro.
                if (string.IsNullOrWhiteSpace(txtNumeroSeguro.Text))
                {
                    MessageBox.Show("Debe ingresar el numero de seguro.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que tenga las imágenes del seguro.
                if (_imagenSeguroFrontalBytes == null || _imagenSeguroFrontalBytes.Length == 0)
                {
                    MessageBox.Show("Debe cargar la imagen frontal del seguro.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_imagenSeguroTraseraBytes == null || _imagenSeguroTraseraBytes.Length == 0)
                {
                    MessageBox.Show("Debe cargar la imagen trasera del seguro.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Actualizar número de seguro si cambió.
                if (txtNumeroSeguro.Text.Trim() != paciente.NumeroSeguro)
                {
                    paciente.NumeroSeguro = txtNumeroSeguro.Text.Trim();
                    cambios = true;
                }

                // Actualizar imágenes si cambiaron.
                if (_imagenSeguroFrontalBytes != paciente.ImagenSeguroFrontal)
                {
                    paciente.ImagenSeguroFrontal = _imagenSeguroFrontalBytes;
                    cambios = true;
                }

                if (_imagenSeguroTraseraBytes != paciente.ImagenSeguroTrasera)
                {
                    paciente.ImagenSeguroTrasera = _imagenSeguroTraseraBytes;
                    cambios = true;
                }
            }

            // Lógica para cambiar la contraseña.
            if (!string.IsNullOrWhiteSpace(txtPasswordActual.Text) &&
                !string.IsNullOrWhiteSpace(txtPasswordNueva.Text))
            {
                // Verifica que la contraseña actual ingresada coincida con la contraseña del paciente.
                if (txtPasswordActual.Text == paciente.Password)
                {
                    // Valida que la nueva contraseña tenga al menos 6 caracteres.
                    if (txtPasswordNueva.Text.Length >= 6)
                    {
                        paciente.Password = txtPasswordNueva.Text; // Actualiza la contraseña.
                        cambios = true;
                    }
                    else
                    {
                        MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Sale del método si la nueva contraseña es muy corta.
                    }
                }
                else
                {
                    MessageBox.Show("La contraseña actual es incorrecta.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Sale del método si la contraseña actual no coincide.
                }
            }

            // Si se realizaron cambios, guarda el usuario en el sistema.
            if (cambios)
            {
                sistema.GuardarUsuario(paciente); // Persiste los cambios del paciente.
                MessageBox.Show("Datos actualizados exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario.
            }
            else
            {
                // Si no se detectaron cambios, informa al usuario.
                MessageBox.Show("No se realizaron cambios.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}