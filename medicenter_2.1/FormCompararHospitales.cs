using System.Drawing;
using System.Windows.Forms;
using System.Linq; // Necesario para .FirstOrDefault() y .Count()

namespace MEDICENTER
{
    // Formulario para la comparación y visualización de información de los hospitales.
    // Muestra una tabla con detalles de cada hospital y permite ver los pacientes de un hospital específico.
    public partial class FormCompararHospitales : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a los datos de los hospitales.
        private DataGridView dgvHospitales; // Control DataGridView para mostrar la lista de hospitales.

        // Constructor del formulario.
        public FormCompararHospitales(Sistema sistemaParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(1000, 600); // Tamaño del formulario.
            this.Text = "Informacion de Hospitales"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Informacion de Hospitales";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.Location = new Point(320, 20);
            lblTitulo.Size = new Size(360, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // DataGridView para mostrar los datos de los hospitales.
            dgvHospitales = new DataGridView();
            dgvHospitales.Location = new Point(50, 80);
            dgvHospitales.Size = new Size(900, 420);
            dgvHospitales.ReadOnly = true; // Los datos no son editables directamente.
            dgvHospitales.AllowUserToAddRows = false; // No permite añadir filas.
            dgvHospitales.AllowUserToDeleteRows = false; // No permite eliminar filas.
            dgvHospitales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajusta el ancho de las columnas.
            dgvHospitales.Font = new Font("Segoe UI", 10);
            dgvHospitales.CellDoubleClick += DgvHospitales_CellDoubleClick; // Asigna el evento de doble clic en una celda.

            // Define las columnas del DataGridView.
            dgvHospitales.Columns.Add("ID", "ID");
            dgvHospitales.Columns.Add("Hospital", "Hospital");
            dgvHospitales.Columns.Add("Tipo", "Tipo");
            dgvHospitales.Columns.Add("Doctores", "Doctores");
            dgvHospitales.Columns.Add("Pacientes", "Pacientes");

            CargarDatosHospitales(); // Llama al método para cargar los datos en el DataGridView.
            this.Controls.Add(dgvHospitales);

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(440, 520);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Método para cargar la información de los hospitales en el DataGridView.
        private void CargarDatosHospitales()
        {
            dgvHospitales.Rows.Clear(); // Limpia las filas existentes antes de cargar nuevos datos.

            // Itera sobre cada hospital en el sistema.
            foreach (var hospital in sistema.Hospitales)
            {
                // Cuenta el número de doctores asignados a este hospital desde PersonalIds.
                int numDoctores = hospital.PersonalIds.Count(idPersonal =>
                {
                    var personal = sistema.Personal.FirstOrDefault(p => p.Id == idPersonal);
                    return personal != null && personal.NivelAcceso == NivelAcceso.MedicoGeneral;
                });

                // Cuenta el número de pacientes atendidos en este hospital desde PacientesAtendidos.
                int numPacientes = hospital.PacientesAtendidos.Count;

                // Añade una nueva fila al DataGridView con la información del hospital.
                dgvHospitales.Rows.Add(
                    hospital.Id,
                    hospital.Nombre,
                    hospital.EsPublico ? "Público" : "Privado", // Muestra "Público" o "Privado".
                    numDoctores, // Número de doctores asociados al hospital.
                    numPacientes // Número de pacientes atendidos en este hospital.
                );
            }
        }

        // Manejador de eventos para el doble clic en una celda del DataGridView.
        // Permite abrir un formulario con los pacientes de un hospital seleccionado.
        private void DgvHospitales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita procesar clics en las cabeceras de fila o columna.
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; 

            // Obtiene el ID del hospital de la fila seleccionada.
            string idHospitalSeleccionado = dgvHospitales.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            // Busca el objeto Hospital correspondiente en el sistema.
            Hospital hospitalSeleccionado = sistema.Hospitales.FirstOrDefault(h => h.Id == idHospitalSeleccionado);

            // Si se encuentra el hospital, abre el formulario para ver sus pacientes.
            if (hospitalSeleccionado != null)
            {
                FormVerPacientesHospital formPacientes = new FormVerPacientesHospital(sistema, hospitalSeleccionado);
                formPacientes.ShowDialog(); // Muestra el formulario como diálogo modal.
            }
        }
    }
}