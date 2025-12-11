using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para mostrar información general de pacientes y personal hospitalario.
    // Utiliza un TabControl para separar la visualización de pacientes y doctores en DataGridViews.
    public class FormInfoPacientesDoctores : Form
    {
        private readonly Sistema sistema; // Instancia de la clase Sistema para acceder a todos los datos de pacientes y personal.

        // Constructor del formulario.
        public FormInfoPacientesDoctores(Sistema sistemaParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 600); // Tamaño del formulario.
            this.Text = "Información de Pacientes y Doctores"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.
            this.Icon = SystemIcons.Application; // Establece el icono de la aplicación.

            // Control de pestañas (TabControl) para organizar la información.
            TabControl tabs = new TabControl();
            tabs.Location = new Point(20, 20);
            tabs.Size = new Size(860, 540);
            this.Controls.Add(tabs); // Añade el TabControl al formulario.

            // Pestaña para "Pacientes".
            TabPage tabPacientes = new TabPage("Pacientes");
            // Pestaña para "Personal Médico".
            TabPage tabDoctores = new TabPage("Personal Médico");
            tabs.TabPages.Add(tabPacientes); // Añade la pestaña de pacientes.
            tabs.TabPages.Add(tabDoctores); // Añade la pestaña de doctores.

            // DataGridView para mostrar la lista de pacientes.
            DataGridView dgvPacientes = new DataGridView();
            dgvPacientes.Dock = DockStyle.Fill; // Ocupa todo el espacio de la pestaña.
            dgvPacientes.ReadOnly = true; // No permite edición directa.
            dgvPacientes.AllowUserToAddRows = false; // No permite añadir filas.
            dgvPacientes.AllowUserToDeleteRows = false; // No permite eliminar filas.
            dgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajusta el ancho de las columnas.
            // Define las columnas para los datos de los pacientes.
            dgvPacientes.Columns.Add("Id", "ID");
            dgvPacientes.Columns.Add("Nombre", "Nombre");
            dgvPacientes.Columns.Add("Email", "Email");
            dgvPacientes.Columns.Add("Edad", "Edad");
            dgvPacientes.Columns.Add("Genero", "Género");
            dgvPacientes.Columns.Add("Registros", "Registros");

            // Rellena el DataGridView con los datos de todos los pacientes.
            foreach (var p in sistema.Pacientes)
            {
                dgvPacientes.Rows.Add(p.Id, p.Nombre, p.Email, p.Edad, p.Genero, p.Historial.Count);
            }
            tabPacientes.Controls.Add(dgvPacientes); // Añade el DataGridView a la pestaña de pacientes.

            // DataGridView para mostrar la lista de personal (doctores/administradores).
            DataGridView dgvDoctores = new DataGridView();
            dgvDoctores.Dock = DockStyle.Fill; // Ocupa todo el espacio de la pestaña.
            dgvDoctores.ReadOnly = true;
            dgvDoctores.AllowUserToAddRows = false;
            dgvDoctores.AllowUserToDeleteRows = false;
            dgvDoctores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Define las columnas para los datos del personal.
            dgvDoctores.Columns.Add("Id", "ID");
            dgvDoctores.Columns.Add("Nombre", "Nombre");
            dgvDoctores.Columns.Add("Email", "Email");
            dgvDoctores.Columns.Add("Nivel", "Nivel");
            dgvDoctores.Columns.Add("Hospital", "Hospital");
            dgvDoctores.Columns.Add("Pacientes", "Pacientes atendidos");

            // Rellena el DataGridView con los datos de todo el personal.
            foreach (var d in sistema.Personal)
            {
                dgvDoctores.Rows.Add(d.Id, d.Nombre, d.Email, d.NivelAcceso, d.IdHospital, d.PacientesAsignados.Count);
            }
            tabDoctores.Controls.Add(dgvDoctores); // Añade el DataGridView a la pestaña de doctores.
        }
    }
}
