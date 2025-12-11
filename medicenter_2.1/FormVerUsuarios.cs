using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para visualizar un listado general de todos los usuarios del sistema (pacientes y personal).
    // Permite filtrar los usuarios por hospital y los presenta en dos pestañas separadas usando DataGridViews.
    public partial class FormVerUsuarios : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a todos los datos de usuarios.
        private string _idHospitalFiltro; // Campo opcional para filtrar usuarios por un ID de hospital específico.
        private DataGridView dgvPacientes; // DataGridView para mostrar la lista de pacientes.
        private DataGridView dgvPersonal; // DataGridView para mostrar la lista de personal hospitalario.

        // Constructor del formulario. Puede recibir un ID de hospital para filtrar los resultados.
        public FormVerUsuarios(Sistema sistemaParam, string idHospitalFiltro = null) 
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            _idHospitalFiltro = idHospitalFiltro; // Asigna el ID del hospital para el filtro.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(1100, 700); // Tamaño del formulario.
            this.Text = "Listado de Usuarios"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario, que puede incluir el nombre del hospital si hay filtro.
            Label lblTitulo = new Label();
            string tituloBase = "Listado de Usuarios";
            // Si hay un ID de hospital para filtrar, el título se adapta.
            if (!string.IsNullOrEmpty(_idHospitalFiltro))
            {
                Hospital hospitalFiltro = sistema.Hospitales.FirstOrDefault(h => h.Id == _idHospitalFiltro);
                tituloBase = $"Usuarios del Hospital: {hospitalFiltro?.Nombre ?? _idHospitalFiltro}";
            }
            lblTitulo.Text = tituloBase;
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(1100, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Control de pestañas (TabControl) para separar pacientes y personal.
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(50, 80);
            tabControl.Size = new Size(1000, 550);
            tabControl.Font = new Font("Segoe UI", 10);
            this.Controls.Add(tabControl);

            // Pestaña de Pacientes.
            TabPage tabPacientes = new TabPage("Pacientes");
            tabControl.TabPages.Add(tabPacientes);
            
            // DataGridView para los pacientes.
            dgvPacientes = new DataGridView();
            dgvPacientes.Dock = DockStyle.Fill; // Ocupa todo el espacio de la pestaña.
            dgvPacientes.ReadOnly = true; // Solo lectura.
            dgvPacientes.AllowUserToAddRows = false; // No permite añadir filas.
            dgvPacientes.AllowUserToDeleteRows = false; // No permite eliminar filas.
            dgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajusta el ancho de las columnas.
            // Define las columnas para los datos de los pacientes.
            dgvPacientes.Columns.Add("ID", "ID");
            dgvPacientes.Columns.Add("Nombre", "Nombre");
            dgvPacientes.Columns.Add("Género", "Género");
            dgvPacientes.Columns.Add("Email", "Email");
            dgvPacientes.Columns.Add("Teléfono", "Teléfono");
            dgvPacientes.Columns.Add("Seguro", "Seguro");
            dgvPacientes.Columns.Add("Hospital", "Hospital"); // Columna para el hospital del último registro del paciente.
            dgvPacientes.Columns.Add("Registros", "Registros");
            tabPacientes.Controls.Add(dgvPacientes);

            // Pestaña de Personal Hospitalario.
            TabPage tabPersonal = new TabPage("Personal Hospitalario");
            tabControl.TabPages.Add(tabPersonal);

            // DataGridView para el personal hospitalario.
            dgvPersonal = new DataGridView();
            dgvPersonal.Dock = DockStyle.Fill; // Ocupa todo el espacio de la pestaña.
            dgvPersonal.ReadOnly = true;
            dgvPersonal.AllowUserToAddRows = false;
            dgvPersonal.AllowUserToDeleteRows = false;
            dgvPersonal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Define las columnas para los datos del personal.
            dgvPersonal.Columns.Add("ID", "ID");
            dgvPersonal.Columns.Add("Nombre", "Nombre");
            dgvPersonal.Columns.Add("Email", "Email");
            dgvPersonal.Columns.Add("Rol", "Rol");
            dgvPersonal.Columns.Add("Hospital", "Hospital");
            dgvPersonal.Columns.Add("Especialidad", "Especialidad");
            tabPersonal.Controls.Add(dgvPersonal);

            CargarUsuarios(); // Llama al método para cargar los datos de los usuarios.

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(490, 640);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Carga los datos de pacientes y personal en los DataGridViews, aplicando el filtro si existe.
        private void CargarUsuarios()
        {
            dgvPacientes.Rows.Clear(); // Limpia filas existentes de pacientes.
            dgvPersonal.Rows.Clear(); // Limpia filas existentes de personal.

            // Cargar Pacientes: filtra si _idHospitalFiltro no es nulo o vacío.
            var pacientesFiltrados = string.IsNullOrEmpty(_idHospitalFiltro)
                ? sistema.Pacientes // Si no hay filtro, toma todos los pacientes.
                // Si hay filtro, toma pacientes cuyo último registro fue en el hospital especificado.
                : sistema.Pacientes.Where(p => p.Historial.Any() && p.Historial.Last().IdHospital == _idHospitalFiltro);

            // Rellena el DataGridView de pacientes.
            foreach (var paciente in pacientesFiltrados)
            {
                string nombreHospitalPaciente = "N/A";
                if (paciente.Historial.Any())
                {
                    string idUltimoHospital = paciente.Historial.Last().IdHospital;
                    // Busca el nombre del hospital del último registro o usa el ID si no se encuentra el nombre.
                    nombreHospitalPaciente = sistema.Hospitales.FirstOrDefault(h => h.Id == idUltimoHospital)?.Nombre ?? idUltimoHospital;
                }

                dgvPacientes.Rows.Add(
                    paciente.Id,
                    paciente.Nombre,
                    paciente.Genero.ToString(),
                    paciente.Email,
                    paciente.Telefono,
                    FormatearSeguro(paciente.TipoSeguro), // Muestra el tipo de seguro formateado.
                    nombreHospitalPaciente, // Nombre del hospital asociado al paciente.
                    paciente.Historial.Count
                );
            }

            // Cargar Personal Hospitalario: filtra si _idHospitalFiltro no es nulo o vacío.
            var personalFiltrado = string.IsNullOrEmpty(_idHospitalFiltro)
                ? sistema.Personal // Si no hay filtro, toma todo el personal.
                : sistema.Personal.Where(p => p.IdHospital == _idHospitalFiltro); // Si hay filtro, toma personal de ese hospital.

            // Rellena el DataGridView de personal.
            foreach (var personal in personalFiltrado)
            {
                string nombreHospital;
                if (personal.Id == "ADMIN001")
                {
                    nombreHospital = "N/A"; // El ADMIN001 (general) no está asignado a un hospital específico.
                }
                else
                {
                    // Busca el nombre del hospital asignado al personal.
                    nombreHospital = sistema.Hospitales.FirstOrDefault(h => h.Id == personal.IdHospital)?.Nombre ?? personal.IdHospital;
                }

                dgvPersonal.Rows.Add(
                    personal.Id,
                    personal.Nombre,
                    personal.Email,
                    personal.NivelAcceso.ToString(),
                    nombreHospital,
                    personal.Especialidad
                );
            }
        }

        // Método auxiliar para formatear la enumeración TipoSeguro a una cadena más legible.
        private string FormatearSeguro(TipoSeguro tipo)
        {
            switch (tipo)
            {
                case TipoSeguro.SinSeguro: return "Sin Seguro";
                case TipoSeguro.SeguroBasico: return "Seguro Básico";
                case TipoSeguro.SeguroCompleto: return "Seguro Completo";
                default: return "No especificado";
            }
        }
    }
}