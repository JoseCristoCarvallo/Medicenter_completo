using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace MEDICENTER
{
    // Formulario de inicio de sesión y registro para pacientes.
    // Permite a los usuarios existentes acceder al sistema y a los nuevos usuarios crear una cuenta.
    public partial class FormLoginPaciente : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para gestionar datos y lógica de negocio.
        private TextBox txtID; // Campo de texto para el ID del paciente en el login.
        private TextBox txtPassword; // Campo de texto para la contraseña del paciente en el login.
        private CheckBox chkVerPassword; // Checkbox para mostrar/ocultar la contraseña.

        // Variables para almacenar las imágenes del seguro cargadas por el usuario como arreglos de bytes.
        private byte[] _imagenSeguroFrontalBytes;
        private byte[] _imagenSeguroTraseraBytes;

        // Controles que necesitan ser accesibles globalmente para habilitar/deshabilitar según la lógica.
        private PictureBox picSeguro1; // PictureBox para mostrar la imagen frontal del seguro.
        private PictureBox picSeguro2; // PictureBox para mostrar la imagen trasera del seguro.
        private Button btnCargarFrontal; // Botón para cargar la imagen frontal del seguro.
        private Button btnCargarTrasera; // Botón para cargar la imagen trasera del seguro.
        private RadioButton rbSiSeguro; // RadioButton para indicar si el paciente tiene seguro.
        private RadioButton rbNoSeguro; // RadioButton para indicar si el paciente no tiene seguro.

        // Constructor del formulario de login de paciente.
        public FormLoginPaciente(Sistema sistemaParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            this.SuspendLayout(); // Suspende la lógica de diseño para una construcción más eficiente.

            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "Login - Paciente"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro de la pantalla.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Control de pestañas (TabControl) para separar el login del registro.
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(30, 50);
            tabControl.Size = new Size(840, 600);
            tabControl.Font = new Font("Segoe UI", 10);
            this.Controls.Add(tabControl); // Añade el TabControl al formulario.

            // Pestaña para "Iniciar Sesión".
            TabPage tabLogin = new TabPage("Iniciar Sesión");
            tabControl.TabPages.Add(tabLogin);
            CrearTabLogin(tabLogin); // Llama al método para construir el contenido de la pestaña de login.

            // Pestaña para "Registrarse".
            TabPage tabRegistro = new TabPage("Registrarse");
            tabControl.TabPages.Add(tabRegistro);
            CrearTabRegistro(tabRegistro); // Llama al método para construir el contenido de la pestaña de registro.

            this.ResumeLayout(false); // Reanuda la lógica de diseño.
        }

        // Método para construir el contenido de la pestaña de "Iniciar Sesión".
        private void CrearTabLogin(TabPage tab)
        {
            // Título de la pestaña de login.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Bienvenido Paciente";
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold | FontStyle.Italic);
            lblTitulo.Location = new Point(200, 50);
            lblTitulo.Size = new Size(400, 50);
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            tab.Controls.Add(lblTitulo);

            // PictureBox para el logo del login.
            PictureBox picLogoLogin = new PictureBox();
            picLogoLogin.Location = new Point(650, 120);
            picLogoLogin.Size = new Size(150, 150);
            picLogoLogin.BorderStyle = BorderStyle.FixedSingle;
            picLogoLogin.SizeMode = PictureBoxSizeMode.Zoom;
            tab.Controls.Add(picLogoLogin);
            picLogoLogin.Image = WC_MediCenter.Properties.Resources.Medicenter_2;

            // Etiqueta para el banner del sistema.
            Label lblLogo = new Label();
            lblLogo.Text = "MEDICENTER\nSISTEMA DE DIAGNÓSTICO MÉDICO";
            lblLogo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblLogo.Location = new Point(640, 280);
            lblLogo.Size = new Size(170, 50);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            tab.Controls.Add(lblLogo);

            // Etiqueta y campo de texto para el ID del usuario.
            Label lblID = new Label();
            lblID.Text = "ID Usuario:";
            lblID.Font = new Font("Segoe UI", 12);
            lblID.Location = new Point(150, 180);
            lblID.Size = new Size(120, 30);
            lblID.TextAlign = ContentAlignment.MiddleRight;
            tab.Controls.Add(lblID);

            txtID = new TextBox();
            txtID.Font = new Font("Segoe UI", 12);
            txtID.Location = new Point(280, 180);
            txtID.Size = new Size(250, 30);
            tab.Controls.Add(txtID);

            // Etiqueta y campo de texto para la contraseña.
            Label lblPassword = new Label();
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Segoe UI", 12);
            lblPassword.Location = new Point(150, 240);
            lblPassword.Size = new Size(120, 30);
            lblPassword.TextAlign = ContentAlignment.MiddleRight;
            tab.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 12);
            txtPassword.Location = new Point(280, 240);
            txtPassword.Size = new Size(250, 30);
            txtPassword.UseSystemPasswordChar = true; // Oculta la contraseña.
            tab.Controls.Add(txtPassword);

            // Checkbox para ver la contraseña.
            chkVerPassword = new CheckBox();
            chkVerPassword.Text = "Ver Contraseña";
            chkVerPassword.Font = new Font("Segoe UI", 10);
            chkVerPassword.Location = new Point(535, 240);
            chkVerPassword.Size = new Size(150, 30);
            chkVerPassword.CheckedChanged += ChkVerPassword_CheckedChanged; // Asigna evento para mostrar/ocultar.
            tab.Controls.Add(chkVerPassword);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnCancelar.Location = new Point(230, 370);
            btnCancelar.Size = new Size(130, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            tab.Controls.Add(btnCancelar);

            // Botón "Ingresar".
            Button btnIngresar = new Button();
            btnIngresar.Text = "Ingresar";
            btnIngresar.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnIngresar.Location = new Point(400, 370);
            btnIngresar.Size = new Size(130, 45);
            btnIngresar.BackColor = Color.White;
            btnIngresar.Click += BtnIngresar_Click; // Asigna el evento de login.
            tab.Controls.Add(btnIngresar);

            // Instrucción para el usuario.
            Label lblInstruccion = new Label();
            lblInstruccion.Text = "Ingresa tus credenciales para acceder al sistema";
            lblInstruccion.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblInstruccion.Location = new Point(200, 460);
            lblInstruccion.Size = new Size(400, 25);
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;
            tab.Controls.Add(lblInstruccion);
        }

        // Método para construir el contenido de la pestaña "Registrarse".
        private void CrearTabRegistro(TabPage tab)
        {
            // Panel con scroll para contener el formulario de registro (permite más campos de los visibles).
            Panel scrollPanel = new Panel();
            scrollPanel.Location = new Point(10, 10);
            scrollPanel.Size = new Size(790, 480);
            scrollPanel.AutoScroll = true;
            tab.Controls.Add(scrollPanel);

            int yPos = 20; // Posición vertical inicial para los controles.

            // Título de la pestaña de registro.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Crea tu cuenta";
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold | FontStyle.Italic);
            lblTitulo.Location = new Point(30, yPos);
            lblTitulo.Size = new Size(400, 50);
            scrollPanel.Controls.Add(lblTitulo);

            // PictureBox para el logo en registro.
            PictureBox picLogoRegistro = new PictureBox();
            picLogoRegistro.Location = new Point(620, yPos);
            picLogoRegistro.Size = new Size(150, 150);
            picLogoRegistro.BorderStyle = BorderStyle.FixedSingle;
            picLogoRegistro.SizeMode = PictureBoxSizeMode.Zoom;
            scrollPanel.Controls.Add(picLogoRegistro);
            picLogoRegistro.Image = WC_MediCenter.Properties.Resources.Medicenter_2;

            // Etiqueta para el banner del sistema en registro.
            Label lblLogo = new Label();
            lblLogo.Text = "MEDICENTER\nSISTEMA DE DIAGNÓSTICO MÉDICO";
            lblLogo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblLogo.Location = new Point(610, yPos + 160);
            lblLogo.Size = new Size(170, 50);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            scrollPanel.Controls.Add(lblLogo);

            yPos += 80;

            // Campo de texto para el ID (auto-generado).
            Label lblID = new Label();
            lblID.Text = "ID:";
            lblID.Font = new Font("Segoe UI", 11);
            lblID.Location = new Point(30, yPos);
            lblID.Size = new Size(150, 30);
            lblID.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblID);

            TextBox txtIDReg = new TextBox();
            txtIDReg.Font = new Font("Segoe UI", 11);
            txtIDReg.Location = new Point(190, yPos);
            txtIDReg.Size = new Size(300, 30);
            txtIDReg.ReadOnly = true; // Solo lectura.
            txtIDReg.BackColor = Color.LightGray;
            txtIDReg.Text = "(Auto-generado)";
            scrollPanel.Controls.Add(txtIDReg);

            yPos += 45;

            // Campo de texto para el Nombre Completo.
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre Completo:";
            lblNombre.Font = new Font("Segoe UI", 11);
            lblNombre.Location = new Point(30, yPos);
            lblNombre.Size = new Size(150, 30);
            lblNombre.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblNombre);

            TextBox txtNombre = new TextBox();
            txtNombre.Font = new Font("Segoe UI", 11);
            txtNombre.Location = new Point(190, yPos);
            txtNombre.Size = new Size(300, 30);
            txtNombre.Tag = "nombre"; // Usa Tag para identificar el control fácilmente.
            scrollPanel.Controls.Add(txtNombre);

            yPos += 45;

            // Campo de texto para el Email.
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 11);
            lblEmail.Location = new Point(30, yPos);
            lblEmail.Size = new Size(150, 30);
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 11);
            txtEmail.Location = new Point(190, yPos);
            txtEmail.Size = new Size(300, 30);
            txtEmail.Tag = "email";
            scrollPanel.Controls.Add(txtEmail);

            yPos += 45;

            // Campo de texto para la Contraseña de registro.
            Label lblPassReg = new Label();
            lblPassReg.Text = "Contraseña:";
            lblPassReg.Font = new Font("Segoe UI", 11);
            lblPassReg.Location = new Point(30, yPos);
            lblPassReg.Size = new Size(150, 30);
            lblPassReg.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblPassReg);

            TextBox txtPassReg = new TextBox();
            txtPassReg.Font = new Font("Segoe UI", 11);
            txtPassReg.Location = new Point(190, yPos);
            txtPassReg.Size = new Size(300, 30);
            txtPassReg.UseSystemPasswordChar = true;
            txtPassReg.Tag = "password";
            scrollPanel.Controls.Add(txtPassReg);

            yPos += 45;

            // Campo numérico para la Edad.
            Label lblEdad = new Label();
            lblEdad.Text = "Edad:";
            lblEdad.Font = new Font("Segoe UI", 11);
            lblEdad.Location = new Point(30, yPos);
            lblEdad.Size = new Size(150, 30);
            lblEdad.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblEdad);

            NumericUpDown numEdad = new NumericUpDown();
            numEdad.Font = new Font("Segoe UI", 11);
            numEdad.Location = new Point(190, yPos);
            numEdad.Size = new Size(100, 30);
            numEdad.Minimum = 0;
            numEdad.Maximum = 120;
            numEdad.Tag = "edad";
            scrollPanel.Controls.Add(numEdad);

            // Checkbox para ver la contraseña de registro.
            CheckBox chkVerPassReg = new CheckBox();
            chkVerPassReg.Text = "Ver Contraseña";
            chkVerPassReg.Font = new Font("Segoe UI", 10);
            chkVerPassReg.Location = new Point(320, yPos);
            chkVerPassReg.Size = new Size(150, 30);
            chkVerPassReg.CheckedChanged += (s, e) =>
            {
                txtPassReg.UseSystemPasswordChar = !chkVerPassReg.Checked;
            };
            scrollPanel.Controls.Add(chkVerPassReg);

            yPos += 45;

            // ComboBox para seleccionar el Género.
            Label lblGenero = new Label();
            lblGenero.Text = "Género:";
            lblGenero.Font = new Font("Segoe UI", 11);
            lblGenero.Location = new Point(30, yPos);
            lblGenero.Size = new Size(150, 30);
            lblGenero.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblGenero);

            ComboBox cmbGenero = new ComboBox();
            cmbGenero.Font = new Font("Segoe UI", 11);
            cmbGenero.Location = new Point(190, yPos);
            cmbGenero.Size = new Size(200, 30);
            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList; // Lista desplegable no editable.
            cmbGenero.Items.AddRange(new object[] { "Masculino", "Femenino" }); // Opciones de género.
            cmbGenero.Tag = "genero";
            scrollPanel.Controls.Add(cmbGenero);

            yPos += 45;

            // Campo de texto para el Teléfono.
            Label lblTelefono = new Label();
            lblTelefono.Text = "Teléfono:";
            lblTelefono.Font = new Font("Segoe UI", 11);
            lblTelefono.Location = new Point(30, yPos);
            lblTelefono.Size = new Size(150, 30);
            lblTelefono.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblTelefono);

            TextBox txtTelefono = new TextBox();
            txtTelefono.Font = new Font("Segoe UI", 11);
            txtTelefono.Location = new Point(190, yPos);
            txtTelefono.Size = new Size(200, 30);
            txtTelefono.Tag = "telefono";
            txtTelefono.KeyPress += TxtNumeric_KeyPress; // Añade validación para solo permitir números.
            scrollPanel.Controls.Add(txtTelefono);

            // Campo de texto para el Contacto de Emergencia.
            Label lblContactoEmerg = new Label();
            lblContactoEmerg.Text = "Contacto Emergencia:";
            lblContactoEmerg.Font = new Font("Segoe UI", 11);
            lblContactoEmerg.Location = new Point(400, yPos);
            lblContactoEmerg.Size = new Size(170, 30);
            lblContactoEmerg.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblContactoEmerg);

            TextBox txtContactoEmerg = new TextBox();
            txtContactoEmerg.Font = new Font("Segoe UI", 11);
            txtContactoEmerg.Location = new Point(580, yPos);
            txtContactoEmerg.Size = new Size(200, 30);
            txtContactoEmerg.Tag = "contacto";
            txtContactoEmerg.KeyPress += TxtNumeric_KeyPress; // Añade validación para solo permitir números.
            scrollPanel.Controls.Add(txtContactoEmerg);

            yPos += 45;

            // ComboBox para seleccionar el Tipo de Sangre.
            Label lblTipoSangre = new Label();
            lblTipoSangre.Text = "Tipo De Sangre:";
            lblTipoSangre.Font = new Font("Segoe UI", 11);
            lblTipoSangre.Location = new Point(30, yPos);
            lblTipoSangre.Size = new Size(150, 30);
            lblTipoSangre.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblTipoSangre);

            ComboBox cmbTipoSangre = new ComboBox();
            cmbTipoSangre.Font = new Font("Segoe UI", 11);
            cmbTipoSangre.Location = new Point(190, yPos);
            cmbTipoSangre.Size = new Size(150, 30);
            cmbTipoSangre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoSangre.Items.AddRange(new object[] {
                "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-"
            }); // Opciones de tipo de sangre.
            cmbTipoSangre.Tag = "tipoSangre";
            scrollPanel.Controls.Add(cmbTipoSangre);

            // Campo de texto para la Cédula (DNI).
            Label lblCedula = new Label();
            lblCedula.Text = "Cédula:";
            lblCedula.Font = new Font("Segoe UI", 11);
            lblCedula.Location = new Point(400, yPos);
            lblCedula.Size = new Size(170, 30);
            lblCedula.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblCedula);

            TextBox txtCedula = new TextBox();
            txtCedula.Font = new Font("Segoe UI", 11);
            txtCedula.Location = new Point(580, yPos);
            txtCedula.Size = new Size(200, 30);
            txtCedula.KeyPress += TxtNumeric_KeyPress; // Añade validación para solo permitir números.
            scrollPanel.Controls.Add(txtCedula);

            yPos += 45;

            // RadioButtons para indicar si tiene Seguro Médico.
            Label lblSeguro = new Label();
            lblSeguro.Text = "Seguro Médico:";
            lblSeguro.Font = new Font("Segoe UI", 11);
            lblSeguro.Location = new Point(30, yPos);
            lblSeguro.Size = new Size(150, 30);
            lblSeguro.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblSeguro);

            rbSiSeguro = new RadioButton(); // Referencia global.
            rbSiSeguro.Text = "Sí";
            rbSiSeguro.Font = new Font("Segoe UI", 11);
            rbSiSeguro.Location = new Point(190, yPos);
            rbSiSeguro.Size = new Size(60, 30);
            rbSiSeguro.Tag = "seguro";
            rbSiSeguro.CheckedChanged += RbSeguro_CheckedChanged; // Asigna evento para gestionar la visibilidad de carga de imágenes.
            scrollPanel.Controls.Add(rbSiSeguro);

            rbNoSeguro = new RadioButton(); // Referencia global.
            rbNoSeguro.Text = "No";
            rbNoSeguro.Font = new Font("Segoe UI", 11);
            rbNoSeguro.Location = new Point(270, yPos);
            rbNoSeguro.Size = new Size(60, 30);
            rbNoSeguro.Checked = true; // Por defecto, no tiene seguro.
            rbNoSeguro.CheckedChanged += RbSeguro_CheckedChanged;
            scrollPanel.Controls.Add(rbNoSeguro);

            yPos += 50;
            
            // Etiqueta para la sección de Imágenes del Seguro.
            Label lblImagenesSeguro = new Label();
            lblImagenesSeguro.Text = "Imágenes del Seguro:";
            lblImagenesSeguro.Font = new Font("Segoe UI", 11);
            lblImagenesSeguro.Location = new Point(30, yPos);
            lblImagenesSeguro.Size = new Size(150, 30);
            lblImagenesSeguro.TextAlign = ContentAlignment.MiddleRight;
            scrollPanel.Controls.Add(lblImagenesSeguro);

            // PictureBox para la imagen frontal del seguro.
            picSeguro1 = new PictureBox(); // Referencia global.
            picSeguro1.Location = new Point(190, yPos);
            picSeguro1.Size = new Size(120, 100);
            picSeguro1.BorderStyle = BorderStyle.FixedSingle;
            picSeguro1.SizeMode = PictureBoxSizeMode.Zoom;
            scrollPanel.Controls.Add(picSeguro1);

            // Botón para cargar la imagen frontal del seguro.
            btnCargarFrontal = new Button(); // Referencia global.
            btnCargarFrontal.Text = "Cargar Frontal";
            btnCargarFrontal.Font = new Font("Segoe UI", 9);
            btnCargarFrontal.Location = new Point(190, yPos + 110);
            btnCargarFrontal.Size = new Size(120, 30);
            btnCargarFrontal.Click += (s, e) => CargarImagenSeguro(picSeguro1, true); // Asigna evento para cargar imagen frontal.
            scrollPanel.Controls.Add(btnCargarFrontal);

            // PictureBox para la imagen trasera del seguro.
            picSeguro2 = new PictureBox(); // Referencia global.
            picSeguro2.Location = new Point(320, yPos);
            picSeguro2.Size = new Size(120, 100);
            picSeguro2.BorderStyle = BorderStyle.FixedSingle;
            picSeguro2.SizeMode = PictureBoxSizeMode.Zoom;
            scrollPanel.Controls.Add(picSeguro2);

            // Botón para cargar la imagen trasera del seguro.
            btnCargarTrasera = new Button(); // Referencia global.
            btnCargarTrasera.Text = "Cargar Trasera";
            btnCargarTrasera.Font = new Font("Segoe UI", 9);
            btnCargarTrasera.Location = new Point(320, yPos + 110);
            btnCargarTrasera.Size = new Size(120, 30);
            btnCargarTrasera.Click += (s, e) => CargarImagenSeguro(picSeguro2, false); // Asigna evento para cargar imagen trasera.
            scrollPanel.Controls.Add(btnCargarTrasera);

            // Botón "Cancelar" (fuera del scrollPanel para que siempre sea visible).
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnCancelar.Location = new Point(250, 540);
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            tab.Controls.Add(btnCancelar);

            // Botón "Registrar" (fuera del scrollPanel para que siempre sea visible).
            Button btnRegistrarFinal = new Button();
            btnRegistrarFinal.Text = "Registrar";
            btnRegistrarFinal.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            btnRegistrarFinal.Location = new Point(400, 540);
            btnRegistrarFinal.Size = new Size(150, 40);
            btnRegistrarFinal.BackColor = Color.LightGreen;
            btnRegistrarFinal.Click += (s, e) => RegistrarPaciente(scrollPanel.Controls); // Asigna el evento de registro.
            tab.Controls.Add(btnRegistrarFinal);
            
            // Llama al método para establecer el estado inicial de los controles de imagen según el seguro.
            RbSeguro_CheckedChanged(null, null);
        }

        // Manejador de eventos para restringir la entrada a solo números en campos de texto.
        private void TxtNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite dígitos y la tecla de retroceso (borrar).
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignora cualquier otro carácter presionado.
            }
        }

        // Método para cargar una imagen del seguro desde un archivo.
        private void CargarImagenSeguro(PictureBox pictureBox, bool esFrontal)
        {
            // Muestra una advertencia si se intenta cargar una imagen sin seleccionar "Sí, tengo seguro".
            if (rbNoSeguro.Checked)
            {
                MessageBox.Show("Solo puede agregar imágenes si selecciona 'Sí, tengo seguro'.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"; // Filtro de archivos permitidos.
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image img = Image.FromFile(ofd.FileName); // Carga la imagen del archivo seleccionado.
                        pictureBox.Image = img; // Muestra la imagen en el PictureBox.
                        
                        // Convierte la imagen a un arreglo de bytes para su almacenamiento.
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, img.RawFormat); // Guarda la imagen en el MemoryStream.
                            if (esFrontal)
                                _imagenSeguroFrontalBytes = ms.ToArray(); // Almacena bytes de la imagen frontal.
                            else
                                _imagenSeguroTraseraBytes = ms.ToArray(); // Almacena bytes de la imagen trasera.
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores si la carga de la imagen falla.
                        MessageBox.Show("No se pudo cargar la imagen seleccionada: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Manejador de eventos para el cambio de estado de los RadioButtons de seguro.
        // Habilita/deshabilita los controles de carga de imagen según si el paciente tiene seguro.
        private void RbSeguro_CheckedChanged(object sender, EventArgs e)
        {
            bool tieneSeguro = rbSiSeguro.Checked; // Verifica si "Sí, tengo seguro" está seleccionado.

            // Habilita o deshabilita los PictureBox y botones de carga de imagen.
            picSeguro1.Enabled = tieneSeguro;
            picSeguro2.Enabled = tieneSeguro; 
            btnCargarFrontal.Enabled = tieneSeguro;
            btnCargarTrasera.Enabled = tieneSeguro;

            // Limpia las imágenes si se selecciona "No tengo seguro".
            if (!tieneSeguro)
            {
                picSeguro1.Image = null; // Elimina la imagen del PictureBox frontal.
                picSeguro2.Image = null; // Elimina la imagen del PictureBox trasero.
                _imagenSeguroFrontalBytes = null; // Borra los bytes de la imagen frontal.
                _imagenSeguroTraseraBytes = null; // Borra los bytes de la imagen trasera.
            }
        }

        // Manejador de eventos para el checkbox "Ver Contraseña" en el login.
        private void ChkVerPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Alterna la visibilidad de la contraseña en el campo txtPassword.
            txtPassword.UseSystemPasswordChar = !chkVerPassword.Checked;
        }

        // Manejador de eventos para el botón "Ingresar" en la pestaña de login.
        private void BtnIngresar_Click(object sender, EventArgs e)
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

            // Busca al paciente en el sistema usando las credenciales proporcionadas.
            Paciente paciente = sistema.BuscarPaciente(id, password);

            if (paciente != null)
            {
                // Si las credenciales son correctas, muestra un mensaje de bienvenida.
                MessageBox.Show($"Bienvenido, {paciente.Nombre}", "Acceso concedido",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abre el menú principal del paciente, ocultando el formulario de login.
                FormMenuPaciente formMenu = new FormMenuPaciente(sistema, paciente);
                this.Hide(); // Oculta el formulario actual.
                formMenu.ShowDialog(); // Muestra el menú del paciente como diálogo modal.
                this.Close(); // Cierra el formulario actual una vez que el menú del paciente se cierra.
            }
            else
            {
                // Si las credenciales son incorrectas, muestra un mensaje de error.
                MessageBox.Show("Credenciales incorrectas", "Error de autenticación",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para registrar un nuevo paciente a partir de los datos ingresados en la pestaña de registro.
        private void RegistrarPaciente(Control.ControlCollection controles)
        {
            try
            {
                Paciente nuevoPaciente = new Paciente(); // Crea una nueva instancia de Paciente.
                nuevoPaciente.Id = sistema.GenerarIdPaciente(); // Genera un ID único para el nuevo paciente.

                // Itera sobre los controles del formulario para recopilar los datos.
                foreach (Control ctrl in controles)
                {
                    // Procesa campos de texto.
                    if (ctrl is TextBox txt && txt.Tag != null)
                    {
                        string tag = txt.Tag.ToString();
                        if (tag == "nombre") nuevoPaciente.Nombre = txt.Text.Trim();
                        else if (tag == "email") nuevoPaciente.Email = txt.Text.Trim();
                        else if (tag == "password") nuevoPaciente.Password = txt.Text;
                        else if (tag == "telefono") nuevoPaciente.Telefono = txt.Text.Trim();
                        else if (tag == "contacto") nuevoPaciente.ContactoEmergencia = txt.Text.Trim();
                    }
                    // Procesa el campo numérico de edad.
                    else if (ctrl is NumericUpDown num && num.Tag != null && num.Tag.ToString() == "edad")
                    {
                        nuevoPaciente.Edad = (int)num.Value;
                    }
                    // Procesa los ComboBox de género y tipo de sangre.
                    else if (ctrl is ComboBox cmb && cmb.Tag != null)
                    {
                        string tag = cmb.Tag.ToString();
                        if (tag == "genero" && cmb.SelectedIndex >= 0)
                        {
                            nuevoPaciente.Genero = (Genero)cmb.SelectedIndex; // Asigna el género según el índice seleccionado.
                        }
                        else if (tag == "tipoSangre" && cmb.SelectedIndex >= 0)
                        {
                            nuevoPaciente.TipoSangre = (TipoSangre)cmb.SelectedIndex; // Asigna el tipo de sangre.
                        }
                    }
                }

                // Asigna el tipo de seguro según la selección de los RadioButtons.
                if (rbSiSeguro.Checked)
                    nuevoPaciente.TipoSeguro = TipoSeguro.SeguroCompleto; // Asume seguro completo si selecciona "Sí".
                else
                    nuevoPaciente.TipoSeguro = TipoSeguro.SinSeguro; // Sin seguro si selecciona "No".


                // Asigna las imágenes del seguro cargadas solo si el paciente tiene seguro.
                if (nuevoPaciente.TipoSeguro != TipoSeguro.SinSeguro)
                {
                    nuevoPaciente.ImagenSeguroFrontal = _imagenSeguroFrontalBytes;
                    nuevoPaciente.ImagenSeguroTrasera = _imagenSeguroTraseraBytes;
                }
                else
                {
                    // Asegura que no se guarden bytes de imagen si el paciente no tiene seguro.
                    nuevoPaciente.ImagenSeguroFrontal = null;
                    nuevoPaciente.ImagenSeguroTrasera = null;
                }

                // Valida que los campos obligatorios no estén vacíos.
                if (string.IsNullOrWhiteSpace(nuevoPaciente.Nombre) ||
                    string.IsNullOrWhiteSpace(nuevoPaciente.Email) ||
                    string.IsNullOrWhiteSpace(nuevoPaciente.Password))
                {
                    MessageBox.Show("Por favor complete todos los campos obligatorios", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                sistema.Pacientes.Add(nuevoPaciente); // Añade el nuevo paciente a la lista de pacientes del sistema.
                sistema.GuardarUsuario(nuevoPaciente); // Guarda el nuevo paciente en la persistencia.

                // Muestra un mensaje de éxito con el ID generado.
                MessageBox.Show($"Paciente registrado exitosamente!\n\nSu ID es: {nuevoPaciente.Id}\n\nGuarde su ID y contraseña para iniciar sesión.",
                    "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // Cierra el formulario de login/registro.
            }
            catch (Exception ex)
            {
                // Manejo de errores durante el proceso de registro.
                MessageBox.Show("Error al registrar paciente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
