using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que un administrador registre nuevos miembros del personal hospitalario (médicos o administradores).
    // Permite especificar el nombre, email, hospital, nivel de acceso y especialidad, generando un ID y contraseña por defecto.
    public partial class FormRegistrarPersonalAdmin : Form
    {
        private readonly Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar datos.
        private readonly PersonalHospitalario admin; // El objeto PersonalHospitalario (administrador) que realiza el registro.

        // Campos de texto y ComboBoxes para la entrada de datos del nuevo personal.
        private TextBox txtNombre;
        private TextBox txtEmail;
        private ComboBox cmbHospital; // ComboBox para seleccionar el hospital de asignación.
        private ComboBox cmbNivelAcceso; // ComboBox para seleccionar el nivel de acceso (Médico o Administrador).
        private ComboBox cmbEspecialidad; // ComboBox para seleccionar la especialidad predefinida.
        private TextBox txtEspecialidadManual; // Campo para ingresar especialidad personalizada.

        // Constructor del formulario.
        public FormRegistrarPersonalAdmin(Sistema sistemaParam, PersonalHospitalario adminParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            admin = adminParam; // Asigna el objeto administrador.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(700, 620); // Tamaño del formulario aumentado.
            this.Text = "Registrar Nuevo Personal Hospitalario"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Registrar Nuevo Personal Hospitalario";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(120, 20);
            lblTitulo.Size = new Size(460, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            int yPos = 90; // Posición vertical inicial para los controles.

            // Campo para el Nombre completo.
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre completo:";
            lblNombre.Font = new Font("Segoe UI", 11);
            lblNombre.Location = new Point(60, yPos);
            lblNombre.Size = new Size(150, 30);
            lblNombre.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNombre);

            txtNombre = new TextBox();
            txtNombre.Font = new Font("Segoe UI", 11);
            txtNombre.Location = new Point(220, yPos);
            txtNombre.Size = new Size(380, 30);
            this.Controls.Add(txtNombre);

            yPos += 45;

            // Campo para el Email.
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 11);
            lblEmail.Location = new Point(60, yPos);
            lblEmail.Size = new Size(150, 30);
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 11);
            txtEmail.Location = new Point(220, yPos);
            txtEmail.Size = new Size(380, 30);
            this.Controls.Add(txtEmail);

            yPos += 45;

            // ComboBox para seleccionar el Hospital.
            Label lblHospital = new Label();
            lblHospital.Text = "Hospital:";
            lblHospital.Font = new Font("Segoe UI", 11);
            lblHospital.Location = new Point(60, yPos);
            lblHospital.Size = new Size(150, 30);
            lblHospital.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblHospital);

            cmbHospital = new ComboBox();
            cmbHospital.Font = new Font("Segoe UI", 11);
            cmbHospital.Location = new Point(220, yPos);
            cmbHospital.Size = new Size(380, 30);
            cmbHospital.DropDownStyle = ComboBoxStyle.DropDownList; // No permite edición manual.

            // Llena el ComboBox con los nombres y IDs de los hospitales disponibles.
            foreach (var hosp in sistema.Hospitales)
            {
                cmbHospital.Items.Add($"{hosp.Id} - {hosp.Nombre}");
            }

            // Selecciona el primer elemento por defecto si hay alguno.
            if (cmbHospital.Items.Count > 0)
                cmbHospital.SelectedIndex = 0;

            this.Controls.Add(cmbHospital);

            yPos += 45;

            // ComboBox para seleccionar el Nivel de Acceso.
            Label lblNivel = new Label();
            lblNivel.Text = "Nivel de acceso:";
            lblNivel.Font = new Font("Segoe UI", 11);
            lblNivel.Location = new Point(60, yPos);
            lblNivel.Size = new Size(150, 30);
            lblNivel.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNivel);

            cmbNivelAcceso = new ComboBox();
            cmbNivelAcceso.Font = new Font("Segoe UI", 11);
            cmbNivelAcceso.Location = new Point(220, yPos);
            cmbNivelAcceso.Size = new Size(250, 30);
            cmbNivelAcceso.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNivelAcceso.Items.Add("Médico General"); // Opciones de nivel de acceso.
            cmbNivelAcceso.Items.Add("Administrador");
            cmbNivelAcceso.SelectedIndex = 0; // Selecciona "Médico General" por defecto.
            this.Controls.Add(cmbNivelAcceso);

            yPos += 45;

            // Campo para la Especialidad (se muestra solo si el nivel de acceso es médico).
            Label lblEsp = new Label();
            lblEsp.Text = "Especialidad (si es médico):";
            lblEsp.Font = new Font("Segoe UI", 11);
            lblEsp.Location = new Point(10, yPos);
            lblEsp.Size = new Size(200, 30);
            lblEsp.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEsp);

            // ComboBox para seleccionar especialidad predefinida
            cmbEspecialidad = new ComboBox();
            cmbEspecialidad.Font = new Font("Segoe UI", 11);
            cmbEspecialidad.Location = new Point(220, yPos);
            cmbEspecialidad.Size = new Size(380, 30);
            cmbEspecialidad.DropDownStyle = ComboBoxStyle.DropDownList;

            // Lista de especialidades médicas comunes en un hospital general
            cmbEspecialidad.Items.Add("Medicina General");
            cmbEspecialidad.Items.Add("Medicina Interna");
            cmbEspecialidad.Items.Add("Pediatría");
            cmbEspecialidad.Items.Add("Ginecología y Obstetricia");
            cmbEspecialidad.Items.Add("Cirugía General");
            cmbEspecialidad.Items.Add("Traumatología y Ortopedia");
            cmbEspecialidad.Items.Add("Cardiología");
            cmbEspecialidad.Items.Add("Neurología");
            cmbEspecialidad.Items.Add("Dermatología");
            cmbEspecialidad.Items.Add("Oftalmología");
            cmbEspecialidad.Items.Add("Otorrinolaringología");
            cmbEspecialidad.Items.Add("Psiquiatría");
            cmbEspecialidad.Items.Add("Radiología");
            cmbEspecialidad.Items.Add("Anestesiología");
            cmbEspecialidad.Items.Add("Medicina de Emergencias");
            cmbEspecialidad.Items.Add("Otra (especificar abajo)");

            cmbEspecialidad.SelectedIndex = 0; // Selecciona "Medicina General" por defecto
            cmbEspecialidad.SelectedIndexChanged += CmbEspecialidad_SelectedIndexChanged;
            this.Controls.Add(cmbEspecialidad);

            yPos += 45;

            // Campo para ingresar especialidad manual (solo se habilita si selecciona "Otra")
            Label lblEspManual = new Label();
            lblEspManual.Text = "Especifique otra especialidad:";
            lblEspManual.Font = new Font("Segoe UI", 11);
            lblEspManual.Location = new Point(10, yPos);
            lblEspManual.Size = new Size(200, 30);
            lblEspManual.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEspManual);

            txtEspecialidadManual = new TextBox();
            txtEspecialidadManual.Font = new Font("Segoe UI", 11);
            txtEspecialidadManual.Location = new Point(220, yPos);
            txtEspecialidadManual.Size = new Size(380, 30);
            txtEspecialidadManual.Enabled = false; // Deshabilitado por defecto
            this.Controls.Add(txtEspecialidadManual);

            yPos += 60;

            // Información sobre la contraseña por defecto.
            Label lblInfoPass = new Label();
            lblInfoPass.Text = "La contraseña por defecto será: medicenter2025\n" +
                               "El usuario deberá cambiarla al iniciar sesión.";
            lblInfoPass.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblInfoPass.Location = new Point(120, yPos);
            lblInfoPass.Size = new Size(460, 40);
            lblInfoPass.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInfoPass);

            yPos += 70;

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 11);
            btnCancelar.Location = new Point(190, yPos);
            btnCancelar.Size = new Size(140, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);

            // Botón "Registrar".
            Button btnRegistrar = new Button();
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Font = new Font("Segoe UI", 11);
            btnRegistrar.Location = new Point(360, yPos);
            btnRegistrar.Size = new Size(140, 45);
            btnRegistrar.BackColor = Color.White;
            btnRegistrar.Click += BtnRegistrar_Click; // Asigna el evento de registro.
            this.Controls.Add(btnRegistrar);
        }

        // Manejador de eventos para cuando cambia la selección de especialidad
        private void CmbEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita el campo manual solo si se selecciona "Otra (especificar abajo)"
            if (cmbEspecialidad.SelectedItem != null &&
                cmbEspecialidad.SelectedItem.ToString() == "Otra (especificar abajo)")
            {
                txtEspecialidadManual.Enabled = true;
                txtEspecialidadManual.Focus();
            }
            else
            {
                txtEspecialidadManual.Enabled = false;
                txtEspecialidadManual.Clear();
            }
        }

        // Manejador de eventos para el botón "Registrar".
        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            // Validaciones básicas de campos obligatorios.
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre completo", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de formato de email.
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Ingrese un email válido", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de selección de hospital.
            if (cmbHospital.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un hospital", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de selección de nivel de acceso.
            if (cmbNivelAcceso.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el nivel de acceso", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtiene el ID del hospital del elemento seleccionado en el ComboBox.
            string seleccionado = cmbHospital.SelectedItem.ToString();
            string idHospital = seleccionado.Split('-')[0].Trim();

            // Busca el objeto Hospital correspondiente al ID.
            Hospital hospital = sistema.BuscarHospital(idHospital);
            if (hospital == null)
            {
                MessageBox.Show("No se encontró el hospital seleccionado", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Determina el nivel de acceso según la selección del ComboBox.
            NivelAcceso nivel;
            if (cmbNivelAcceso.SelectedItem.ToString().StartsWith("Médico"))
                nivel = NivelAcceso.MedicoGeneral;
            else
                nivel = NivelAcceso.Administrador;

            // Genera un nuevo ID para el personal y establece una contraseña por defecto.
            string nuevoId = sistema.GenerarIdPersonal();
            string passwordDefecto = "medicenter2025";

            // Crea una nueva instancia de PersonalHospitalario con los datos ingresados.
            PersonalHospitalario nuevo = new PersonalHospitalario
            {
                Id = nuevoId,
                Nombre = txtNombre.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = passwordDefecto,
                IdHospital = idHospital,
                NivelAcceso = nivel
            };

            // Si el personal es médico, asigna la especialidad.
            if (nivel == NivelAcceso.MedicoGeneral)
            {
                // Verifica si se seleccionó "Otra" y usa el campo manual, sino usa la selección del ComboBox
                if (cmbEspecialidad.SelectedItem != null)
                {
                    string especialidadSeleccionada = cmbEspecialidad.SelectedItem.ToString();

                    if (especialidadSeleccionada == "Otra (especificar abajo)")
                    {
                        // Valida que haya ingresado una especialidad personalizada
                        if (string.IsNullOrWhiteSpace(txtEspecialidadManual.Text))
                        {
                            MessageBox.Show("Por favor, especifique la especialidad personalizada.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        nuevo.Especialidad = txtEspecialidadManual.Text.Trim();
                    }
                    else
                    {
                        nuevo.Especialidad = especialidadSeleccionada;
                    }
                }
                else
                {
                    nuevo.Especialidad = "No especificada";
                }
            }

            // Añade el nuevo personal a la lista del sistema y al hospital.
            sistema.Personal.Add(nuevo);
            hospital.PersonalIds.Add(nuevo.Id);
            sistema.GuardarUsuario(nuevo); // Guarda el nuevo personal en la persistencia.

            // Muestra un mensaje de éxito con los datos del nuevo personal.
            string mensaje = "Personal registrado exitosamente\n\n";
            mensaje += $"ID: {nuevo.Id}\n";
            mensaje += $"Nivel: {nuevo.NivelAcceso}\n";
            mensaje += $"Hospital: {hospital.Nombre}\n\n";
            mensaje += $"Contraseña por defecto: {passwordDefecto}\n";
            mensaje += "El usuario deberá cambiarla al iniciar sesión.";

            MessageBox.Show(mensaje, "Registro exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Cierra el formulario.
        }
    }
}