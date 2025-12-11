using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario del menú principal para usuarios con nivel de acceso "Administrador".
    // Proporciona acceso a las funcionalidades de gestión del sistema MediCenter.
    public partial class FormMenuAdministrador : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio.
        private PersonalHospitalario admin; // Objeto PersonalHospitalario que representa al administrador logueado.

        // Constructor del formulario de menú del administrador.
        public FormMenuAdministrador(Sistema sistemaParam, PersonalHospitalario adminParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            admin = adminParam; // Asigna el objeto administrador.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "MediCenter - Menú Administrador"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Busca el hospital asociado al administrador (si aplica).
            Hospital hospital = sistema.BuscarHospital(admin.IdHospital);

            // Panel de información superior que muestra el nombre del administrador y el hospital.
            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(0, 0);
            panelInfo.Size = new Size(900, 100);
            panelInfo.BackColor = Color.FromArgb(200, 100, 100); // Color distintivo para el panel superior.
            this.Controls.Add(panelInfo);

            // Etiqueta para mostrar el nombre del administrador y su rol (General o de Hospital).
            Label lblNombreAdmin = new Label();
            if (admin.Id == "ADMIN001")
            {
                lblNombreAdmin.Text = $"ADMINISTRADOR GENERAL - {admin.Nombre}";
            }
            else
            {
                lblNombreAdmin.Text = $"ADMINISTRADOR DEL HOSPITAL - {admin.Nombre}";
            }
            lblNombreAdmin.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblNombreAdmin.ForeColor = Color.White;
            lblNombreAdmin.Location = new Point(30, 20);
            lblNombreAdmin.Size = new Size(600, 35);
            panelInfo.Controls.Add(lblNombreAdmin);

            // Etiqueta para mostrar el nombre del hospital (solo si no es el administrador general).
            Label lblHospital = new Label();
            lblHospital.Font = new Font("Segoe UI", 13);
            lblHospital.ForeColor = Color.White;
            lblHospital.Location = new Point(30, 55);
            lblHospital.Size = new Size(600, 30);

            if (admin.Id == "ADMIN001")
            {
                lblHospital.Text = string.Empty; // El administrador general no está asociado a un hospital específico.
            }
            else
            {
                lblHospital.Text = hospital?.Nombre ?? "Hospital"; // Muestra el nombre del hospital asignado.
            }
            panelInfo.Controls.Add(lblHospital);

            // Título central del panel de administración.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Panel de Administración";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(300, 120);
            lblTitulo.Size = new Size(300, 40);
            this.Controls.Add(lblTitulo);

            // Posiciones iniciales para organizar los botones en dos columnas.
            int yPos = 180;
            int xLeft = 100;
            int xRight = 500;

            // Botón para "Ver Información de los Hospitales".
            Button btnEstadisticas = CrearBoton("1. Ver Información de los Hospitales", xLeft, yPos);
            btnEstadisticas.Click += (s, e) => VerEstadisticas(); // Asigna el evento click.
            this.Controls.Add(btnEstadisticas);

            // Botón para "Cambiar mis datos" (información personal del administrador).
            Button btnCambiarDatos = CrearBoton("2. Cambiar mis datos", xRight, yPos);
            btnCambiarDatos.Click += (s, e) => CambiarMisDatos();
            this.Controls.Add(btnCambiarDatos);

            yPos += 70; // Incrementa la posición vertical para el siguiente par de botones.

            // Botón para "Registrar Nuevo Personal Médico".
            Button btnRegistrar = CrearBoton("3. Registrar Nuevo Personal Médico", xLeft, yPos);
            btnRegistrar.Click += (s, e) => RegistrarNuevoPersonal();
            this.Controls.Add(btnRegistrar);

            // Botón para "Ver mi Información" (detalles del administrador logueado).
            Button btnInfo = CrearBoton("4. Ver mi Información", xRight, yPos);
            btnInfo.Click += (s, e) => VerInformacion();
            this.Controls.Add(btnInfo);

            yPos += 70;

            // Botón para "Gestionar Personal" (gestión del personal del hospital).
            Button btnGestionarPersonal = CrearBoton("5. Gestionar Personal", xLeft, yPos);
            btnGestionarPersonal.Click += (s, e) => GestionarPersonal();
            this.Controls.Add(btnGestionarPersonal);

            // Botón para "Listado General de Usuarios".
            Button btnVerUsuarios = CrearBoton("6. Listado General de Usuarios", xRight, yPos);
            btnVerUsuarios.Click += (s, e) => VerRegistrosMedicos(); // Reutiliza el método que abre FormVerUsuarios.
            this.Controls.Add(btnVerUsuarios);

            yPos += 70;

            // Botón para "Trasladar Paciente".
            Button btnTraslado = CrearBoton("7. Trasladar Paciente", xLeft, yPos);
            btnTraslado.Click += (s, e) => TrasladarPacienteDesdeAdmin();
            this.Controls.Add(btnTraslado);

            yPos += 70;

            // Botón para "Cerrar Sesión".
            Button btnCerrarSesion = CrearBoton("0. Cerrar Sesión", xLeft, yPos);
            btnCerrarSesion.BackColor = Color.FromArgb(200, 100, 100); // Color distintivo para el botón de cerrar sesión.
            btnCerrarSesion.Click += (s, e) => CerrarSesion();
            this.Controls.Add(btnCerrarSesion);
        }

        // Abre el formulario para registrar nuevo personal médico.
        private void RegistrarNuevoPersonal()
        {
            FormRegistrarPersonalAdmin form = new FormRegistrarPersonalAdmin(sistema, admin);
            form.ShowDialog(); // Muestra el formulario como diálogo modal.
        }

        // Método auxiliar para crear botones de menú con un estilo uniforme.
        private Button CrearBoton(string texto, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 11);
            btn.Location = new Point(x, y);
            btn.Size = new Size(350, 55);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat; // Estilo plano para el botón.
            btn.Cursor = Cursors.Hand; // Cambia el cursor para indicar que es clickeable.
            return btn;
        }

        // Abre el formulario para gestionar el personal del hospital.
        private void GestionarPersonal()
        {
            FormGestionPersonal formGestion = new FormGestionPersonal(sistema, admin);
            formGestion.ShowDialog();
        }

        // Abre el formulario para ver y comparar información de hospitales (estadísticas).
        private void VerEstadisticas()
        {
            FormCompararHospitales form = new FormCompararHospitales(sistema);
            form.ShowDialog();
        }

        // Abre el formulario para ver un listado general de todos los usuarios (pacientes y personal).
        private void VerRegistrosMedicos() // Lógicamente renombrado para reflejar su función actual de ver usuarios.
        {
            FormVerUsuarios formVerUsuarios = new FormVerUsuarios(sistema);
            formVerUsuarios.ShowDialog();
        }
        
        // El método original 'VerInformacionPacientesYDoctores' fue eliminado y su funcionalidad
        // fue reemplazada por 'VerRegistrosMedicos' (que ahora abre FormVerUsuarios).

        // Muestra la información detallada del administrador actual.
        private void VerInformacion()
        {
            string info = admin.ObtenerInformacionFormateada(); // Usa el método de la clase PersonalHospitalario.
            MessageBox.Show(info, "Mi Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Abre el formulario para que el administrador cambie sus propios datos (email y contraseña).
        private void CambiarMisDatos()
        {
            FormEditarAdminData formEditarAdmin = new FormEditarAdminData(sistema, admin);
            formEditarAdmin.ShowDialog();

            // Después de cerrar el formulario, se podría recargar el objeto 'admin' si los cambios no se reflejan automáticamente.
            // Actualmente, se asume que el objeto 'admin' se actualiza por referencia si se modificó en FormEditarAdminData.
            if (formEditarAdmin.DialogResult == DialogResult.OK)
            {
                // Lógica de refresco si fuera necesario (ej. recargar 'admin' desde la persistencia o la lista de sistema).
            }
        }

        // Cierra la sesión del administrador y el formulario actual.
        private void CerrarSesion()
        {
            MessageBox.Show("Hasta pronto", "Cerrando sesión",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Cierra el formulario.
        }

        // Abre el formulario para trasladar pacientes entre hospitales.
        private void TrasladarPacienteDesdeAdmin()
        {
            using (FormTrasladoPaciente form = new FormTrasladoPaciente(sistema))
            {
                form.ShowDialog();
            }
        }
    }
}