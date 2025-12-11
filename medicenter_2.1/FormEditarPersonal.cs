using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que un administrador edite los datos de un miembro del personal hospitalario.
    // Permite modificar el nombre, email, contraseña, hospital asignado, nivel de acceso y especialidad,
    // así como eliminar al usuario del sistema.
    public partial class FormEditarPersonal : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y datos.
        private PersonalHospitalario _personal; // El objeto PersonalHospitalario (miembro del personal) a editar.

        // Controles de UI para los campos de datos del personal.
        private TextBox txtId;
        private TextBox txtNombre;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cmbHospital; // ComboBox para seleccionar el hospital.
        private ComboBox cmbNivelAcceso; // ComboBox para seleccionar el nivel de acceso.
        private TextBox txtEspecialidad; // Campo para la especialidad del personal (principalmente médicos).

        // Constructor del formulario.
        public FormEditarPersonal(Sistema sistema, PersonalHospitalario personal)
        {
            _sistema = sistema; // Asigna la instancia del sistema.
            _personal = personal; // Asigna el objeto de personal a editar.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
            CargarDatosPersonal(); // Carga los datos actuales del personal en los campos del formulario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(700, 650); // Tamaño del formulario.
            this.Text = $"Editar Personal: {_personal.Nombre}"; // Título de la ventana con el nombre del personal.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Editar Datos del Personal Hospitalario";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(700, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            int yPos = 80; // Posición vertical inicial para los controles.

            // Campo para el ID (solo lectura).
            Label lblId = new Label();
            lblId.Text = "ID:";
            lblId.Font = new Font("Segoe UI", 11);
            lblId.Location = new Point(80, yPos);
            lblId.Size = new Size(150, 30);
            lblId.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblId);

            txtId = new TextBox();
            txtId.Font = new Font("Segoe UI", 11);
            txtId.Location = new Point(250, yPos);
            txtId.Size = new Size(300, 30);
            txtId.ReadOnly = true; // El ID no se puede cambiar, es un identificador único.
            txtId.BackColor = Color.LightGray; // Fondo gris para indicar que es de solo lectura.
            this.Controls.Add(txtId);

            yPos += 45;

            // Campo para el Nombre.
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre:";
            lblNombre.Font = new Font("Segoe UI", 11);
            lblNombre.Location = new Point(80, yPos);
            lblNombre.Size = new Size(150, 30);
            lblNombre.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNombre);

            txtNombre = new TextBox();
            txtNombre.Font = new Font("Segoe UI", 11);
            txtNombre.Location = new Point(250, yPos);
            txtNombre.Size = new Size(300, 30);
            this.Controls.Add(txtNombre);

            yPos += 45;

            // Campo para el Email.
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 11);
            lblEmail.Location = new Point(80, yPos);
            lblEmail.Size = new Size(150, 30);
            lblEmail.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 11);
            txtEmail.Location = new Point(250, yPos);
            txtEmail.Size = new Size(300, 30);
            this.Controls.Add(txtEmail);

            yPos += 45;

            // Campo para la Contraseña.
            Label lblPassword = new Label();
            lblPassword.Text = "Contraseña:";
            lblPassword.Font = new Font("Segoe UI", 11);
            lblPassword.Location = new Point(80, yPos);
            lblPassword.Size = new Size(150, 30);
            lblPassword.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.Location = new Point(250, yPos);
            txtPassword.Size = new Size(300, 30);
            txtPassword.UseSystemPasswordChar = true; // Oculta la contraseña por defecto.
            this.Controls.Add(txtPassword);

            // CheckBox para ver la contraseña.
            CheckBox chkVerPassword = new CheckBox();
            chkVerPassword.Text = "Ver Contraseña";
            chkVerPassword.Font = new Font("Segoe UI", 10);
            chkVerPassword.Location = new Point(560, yPos);
            chkVerPassword.Size = new Size(130, 30);
            // Evento para alternar la visibilidad de la contraseña.
            chkVerPassword.CheckedChanged += (s, e) => { txtPassword.UseSystemPasswordChar = !chkVerPassword.Checked; };
            this.Controls.Add(chkVerPassword);

            yPos += 45;

            // ComboBox para seleccionar el Hospital.
            Label lblHospital = new Label();
            lblHospital.Text = "Hospital:";
            lblHospital.Font = new Font("Segoe UI", 11);
            lblHospital.Location = new Point(80, yPos);
            lblHospital.Size = new Size(150, 30);
            lblHospital.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblHospital);

            cmbHospital = new ComboBox();
            cmbHospital.Font = new Font("Segoe UI", 11);
            cmbHospital.Location = new Point(250, yPos);
            cmbHospital.Size = new Size(300, 30);
            cmbHospital.DropDownStyle = ComboBoxStyle.DropDownList; // No permite edición manual.
            // Llena el ComboBox con los hospitales disponibles en el sistema.
            foreach (var hosp in _sistema.Hospitales)
            {
                cmbHospital.Items.Add($"{hosp.Id} - {hosp.Nombre}");
            }
            this.Controls.Add(cmbHospital);

            yPos += 45;

            // ComboBox para seleccionar el Nivel de Acceso.
            Label lblNivelAcceso = new Label();
            lblNivelAcceso.Text = "Nivel de Acceso:";
            lblNivelAcceso.Font = new Font("Segoe UI", 11);
            lblNivelAcceso.Location = new Point(80, yPos);
            lblNivelAcceso.Size = new Size(150, 30);
            lblNivelAcceso.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblNivelAcceso);

            cmbNivelAcceso = new ComboBox();
            cmbNivelAcceso.Font = new Font("Segoe UI", 11);
            cmbNivelAcceso.Location = new Point(250, yPos);
            cmbNivelAcceso.Size = new Size(200, 30);
            cmbNivelAcceso.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNivelAcceso.Items.Add("Médico General"); // Opciones de nivel de acceso.
            cmbNivelAcceso.Items.Add("Administrador");
            this.Controls.Add(cmbNivelAcceso);

            yPos += 45;

            // Campo para la Especialidad.
            Label lblEspecialidad = new Label();
            lblEspecialidad.Text = "Especialidad:";
            lblEspecialidad.Font = new Font("Segoe UI", 11);
            lblEspecialidad.Location = new Point(80, yPos);
            lblEspecialidad.Size = new Size(150, 30);
            lblEspecialidad.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblEspecialidad);

            txtEspecialidad = new TextBox();
            txtEspecialidad.Font = new Font("Segoe UI", 11);
            txtEspecialidad.Location = new Point(250, yPos);
            txtEspecialidad.Size = new Size(300, 30);
            this.Controls.Add(txtEspecialidad);

            yPos += 60;

            // Botones de acción.
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.Font = new Font("Segoe UI", 12);
            btnGuardar.Location = new Point(150, yPos);
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.BackColor = Color.White;
            btnGuardar.Click += BtnGuardar_Click; // Asigna el evento para guardar cambios.
            this.Controls.Add(btnGuardar);

            Button btnEliminar = new Button();
            btnEliminar.Text = "Eliminar Usuario";
            btnEliminar.Font = new Font("Segoe UI", 12);
            btnEliminar.Location = new Point(320, yPos);
            btnEliminar.Size = new Size(160, 45);
            btnEliminar.BackColor = Color.Red; // Color distintivo para eliminar.
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Click += BtnEliminar_Click; // Asigna el evento para eliminar el usuario.
            this.Controls.Add(btnEliminar);

            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(490, yPos);
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);
        }

        // Carga los datos actuales del objeto PersonalHospitalario en los controles del formulario.
        private void CargarDatosPersonal()
        {
            txtId.Text = _personal.Id; // Muestra el ID (solo lectura).
            txtNombre.Text = _personal.Nombre;
            txtEmail.Text = _personal.Email;
            txtPassword.Text = _personal.Password; // Muestra la contraseña (oculta por UseSystemPasswordChar).

            // Selecciona el hospital asignado en el ComboBox.
            if (!string.IsNullOrEmpty(_personal.IdHospital))
            {
                // Busca el formato de string que coincide con el hospital del personal.
                string itemToSelect = _sistema.Hospitales
                    .Where(h => h.Id == _personal.IdHospital)
                    .Select(h => $"{h.Id} - {h.Nombre}")
                    .FirstOrDefault();
                if (itemToSelect != null)
                {
                    cmbHospital.SelectedItem = itemToSelect;
                }
            }

            // Selecciona el nivel de acceso en el ComboBox.
            cmbNivelAcceso.SelectedItem = _personal.NivelAcceso == NivelAcceso.MedicoGeneral ? "Médico General" : "Administrador";

            txtEspecialidad.Text = _personal.Especialidad; // Muestra la especialidad.
        }

        // Manejador de eventos para el botón "Guardar Cambios".
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones de campos obligatorios.
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Todos los campos obligatorios deben estar completos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de formato de email.
            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Ingrese un email válido.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación de longitud de contraseña.
            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validación de selección de hospital.
            if (cmbHospital.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un hospital.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validación de selección de nivel de acceso.
            if (cmbNivelAcceso.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un nivel de acceso.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Actualiza las propiedades del objeto PersonalHospitalario con los nuevos valores de los campos.
            _personal.Nombre = txtNombre.Text.Trim();
            _personal.Email = txtEmail.Text.Trim();
            _personal.Password = txtPassword.Text; // Guarda la contraseña directamente (ya está oculta en UI).
            // Extrae el ID del hospital del elemento seleccionado en el ComboBox.
            _personal.IdHospital = cmbHospital.SelectedItem.ToString().Split('-')[0].Trim();
            // Asigna el NivelAcceso según la selección del ComboBox.
            _personal.NivelAcceso = cmbNivelAcceso.SelectedItem.ToString() == "Médico General" ? NivelAcceso.MedicoGeneral : NivelAcceso.Administrador;
            _personal.Especialidad = txtEspecialidad.Text.Trim();

            // Persiste los cambios del objeto PersonalHospitalario en el sistema.
            _sistema.GuardarUsuario(_personal);

            MessageBox.Show("Datos del personal actualizados exitosamente.", "Actualización Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK; // Establece el resultado del diálogo a OK.
            this.Close(); // Cierra el formulario.
        }

        // Manejador de eventos para el botón "Eliminar Usuario".
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            // Pide confirmación al usuario antes de eliminar.
            DialogResult confirmacion = MessageBox.Show($"¿Está seguro que desea eliminar al usuario {_personal.Nombre} ({_personal.Id})?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                // Elimina el personal de la lista en memoria del sistema.
                _sistema.Personal.Remove(_personal);
                // Elimina el personal de la persistencia (archivos).
                _sistema.EliminarUsuario(_personal.Id);

                MessageBox.Show("Usuario eliminado exitosamente.", "Eliminación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Indica que se realizó una modificación.
                this.Close(); // Cierra el formulario.
            }
        }
    }
}
