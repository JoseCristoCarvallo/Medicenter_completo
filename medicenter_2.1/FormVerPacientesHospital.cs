using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para visualizar los pacientes asociados a un hospital específico.
    // Muestra los pacientes cuyo último registro médico fue realizado en el hospital seleccionado.
    public partial class FormVerPacientesHospital : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para acceder a los datos de los pacientes.
        private Hospital _hospital; // El objeto Hospital cuyos pacientes se van a visualizar.
        private ListBox listBoxPacientes; // Control ListBox para mostrar la lista de pacientes.

        // Constructor del formulario.
        public FormVerPacientesHospital(Sistema sistema, Hospital hospital)
        {
            _sistema = sistema; // Asigna la instancia del sistema.
            _hospital = hospital; // Asigna el objeto hospital.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
            this.Text = $"Pacientes del Hospital: {_hospital.Nombre}"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario, mostrando el nombre del hospital.
            Label lblTitulo = new Label();
            lblTitulo.Text = $"Pacientes Asignados a: {_hospital.Nombre}";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // ListBox para mostrar los pacientes.
            listBoxPacientes = new ListBox();
            listBoxPacientes.Location = new Point(50, 80);
            listBoxPacientes.Size = new Size(700, 450);
            listBoxPacientes.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
            this.Controls.Add(listBoxPacientes);

            CargarPacientes(); // Llama al método para cargar y mostrar la lista de pacientes.

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(340, 540);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Carga y muestra los pacientes asociados al hospital actual en el ListBox.
        private void CargarPacientes()
        {
            listBoxPacientes.Items.Clear(); // Limpia los elementos existentes antes de recargar.

            // Filtra los pacientes cuyo último registro médico pertenece al hospital actual.
            var pacientesEnHospital = _sistema.Pacientes
                                        .Where(p => p.Historial.Any() && p.Historial.Last().IdHospital == _hospital.Id)
                                        .ToList();

            // Si no hay pacientes asociados al hospital.
            if (!pacientesEnHospital.Any())
            {
                listBoxPacientes.Items.Add($"No hay pacientes registrados con último historial en {_hospital.Nombre}.");
                return;
            }

            // Muestra el total de pacientes y un separador.
            listBoxPacientes.Items.Add($"Total de pacientes en {_hospital.Nombre}: {pacientesEnHospital.Count}");
            listBoxPacientes.Items.Add("═════════════════════════════════════════════════");
            listBoxPacientes.Items.Add("");

            int contador = 1; // Contador para enumerar las entradas.
            // Itera sobre cada paciente y lo muestra en el ListBox.
            foreach (var paciente in pacientesEnHospital)
            {
                listBoxPacientes.Items.Add($"{contador}. ID: {paciente.Id} | Nombre: {paciente.Nombre}");
                listBoxPacientes.Items.Add($"   Edad: {paciente.Edad} | Género: {paciente.Genero}");
                // Muestra el último diagnóstico del paciente.
                listBoxPacientes.Items.Add($"   Último Diagnóstico: {paciente.Historial.Last().Diagnostico}");
                listBoxPacientes.Items.Add(""); // Línea en blanco para separar entradas.
                contador++;
            }
        }
    }
}
