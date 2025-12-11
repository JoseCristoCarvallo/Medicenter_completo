using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el personal médico visualice y gestione la cola de pacientes de su hospital.
    // Permite ver los pacientes en espera y atender al primero de la cola.
    public partial class FormListaColaPacientes : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para interactuar con las colas y los datos del paciente.
        private PersonalHospitalario _medico; // El objeto PersonalHospitalario (médico) logueado.
        private ListBox listBoxCola; // Control ListBox para mostrar los pacientes en la cola.

        // Constructor del formulario.
        public FormListaColaPacientes(Sistema sistemaParam, PersonalHospitalario medicoParam)
        {
            _sistema = sistemaParam; // Asigna la instancia del sistema.
            _medico = medicoParam; // Asigna el objeto médico.
            InitializeComponent(); // Llama al InitializeComponent generado por el diseñador (si existe).
            SetupFormControls(); // Llama a mi método para configurar los controles y la lógica del formulario.
        }

        // Método que configura programáticamente todos los componentes visuales del formulario.
        private void SetupFormControls()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
            this.Text = $"Cola de Pacientes - Hospital {_medico.IdHospital}"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = $"Cola de Pacientes para {_medico.IdHospital}";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // ListBox para mostrar los pacientes en cola.
            listBoxCola = new ListBox();
            listBoxCola.Location = new Point(50, 80);
            listBoxCola.Size = new Size(700, 400);
            listBoxCola.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
            listBoxCola.MouseDoubleClick += ListBoxCola_MouseDoubleClick; // Asigna el evento de doble clic.
            this.Controls.Add(listBoxCola);

            CargarCola(); // Llama al método para cargar la cola inicial.

            // Botón "Atender Paciente Seleccionado".
            Button btnAtender = new Button();
            btnAtender.Text = "Atender Paciente Seleccionado";
            btnAtender.Font = new Font("Segoe UI", 12);
            btnAtender.Location = new Point(200, 500);
            btnAtender.Size = new Size(220, 45);
            btnAtender.BackColor = Color.LightGreen;
            btnAtender.Click += BtnAtender_Click; // Asigna el evento de atender paciente.
            this.Controls.Add(btnAtender);

            // Botón "Refrescar Cola".
            Button btnRefrescar = new Button();
            btnRefrescar.Text = "Refrescar Cola";
            btnRefrescar.Font = new Font("Segoe UI", 12);
            btnRefrescar.Location = new Point(440, 500);
            btnRefrescar.Size = new Size(160, 45);
            btnRefrescar.BackColor = Color.LightBlue;
            btnRefrescar.Click += (s, e) => CargarCola(); // Refresca la cola al hacer clic.
            this.Controls.Add(btnRefrescar);

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(620, 500);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Carga la información de los pacientes en la cola del hospital y la muestra en el ListBox.
        private void CargarCola()
        {
            listBoxCola.Items.Clear(); // Limpia la lista antes de cargar.

            // Verifica si el hospital tiene una cola y si está vacía.
            if (!_sistema.ColasPorHospital.ContainsKey(_medico.IdHospital) || _sistema.ColasPorHospital[_medico.IdHospital].Count == 0)
            {
                listBoxCola.Items.Add("No hay pacientes en cola para este hospital.");
                return;
            }

            Queue<string> colaHospital = _sistema.ColasPorHospital[_medico.IdHospital]; // Obtiene la cola.
            listBoxCola.Items.Add($"Pacientes en cola: {colaHospital.Count}"); // Muestra el total de pacientes.
            listBoxCola.Items.Add("══════════════════════════════════════════════════"); // Separador.
            listBoxCola.Items.Add(""); // Línea en blanco.

            // Para mostrar los elementos de la cola sin modificarlos, se copia a una lista temporal.
            List<string> tempColaList = colaHospital.ToList(); 

            // Itera sobre los elementos temporales para mostrarlos.
            foreach (string clave in tempColaList)
            {
                string[] partes = clave.Split('|');
                string idPaciente = partes[0];
                string idRegistro = partes[1];

                // Busca el paciente y el registro médico asociado.
                Paciente? pacienteEnCola = _sistema.Pacientes.FirstOrDefault(p => p.Id == idPaciente);
                RegistroMedico? registroEnCola = pacienteEnCola?.Historial.FirstOrDefault(r => r.IdRegistro == idRegistro);

                // Formatea y añade la información a la lista.
                string infoDisplay = $"ID Paciente: {idPaciente} | Nombre: {pacienteEnCola?.Nombre ?? "Desconocido"} | Reg: {idRegistro}";
                listBoxCola.Items.Add(infoDisplay);
            }
        }

        // Manejador de eventos para el doble clic del ratón en un elemento del ListBox.
        // Reutiliza la lógica del botón "Atender".
        private void ListBoxCola_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnAtender_Click(sender, e); // Llama al manejador del botón "Atender".
        }

        // Manejador de eventos para el botón "Atender Paciente Seleccionado".
        // Procesa el paciente que se encuentra al inicio de la cola.
        private void BtnAtender_Click(object sender, EventArgs e)
        {
            // Valida que haya un elemento seleccionado y que no sea una de las líneas informativas.
            if (listBoxCola.SelectedItem == null || listBoxCola.SelectedIndex < 2) 
            {
                MessageBox.Show("Por favor seleccione un paciente de la cola.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // En un sistema real de colas FIFO, solo se atendería al primer elemento.
            // Para fines de demostración, este botón intenta atender el elemento visualmente seleccionado,
            // pero la lógica subyacente de la Queue asegura que el 'Dequeue' siempre es el primero.
            // Por lo tanto, se verifica que el elemento seleccionado visualmente sea el primero en la cola real.

            string? primerElementoColaKey = _sistema.ColasPorHospital[_medico.IdHospital].Any() ? _sistema.ColasPorHospital[_medico.IdHospital].Peek() : null;

            if (primerElementoColaKey == null)
            {
                MessageBox.Show("No hay pacientes en cola para atender.", "Cola Vacía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Extrae los IDs del primer elemento de la cola real.
            string[] partesPrimerElemento = primerElementoColaKey.Split('|');
            string idPacientePrimer = partesPrimerElemento[0];
            string idRegistroPrimer = partesPrimerElemento[1];

            // Busca el paciente y el registro médico asociado al primer elemento de la cola.
            Paciente? pacienteAAtender = _sistema.Pacientes.FirstOrDefault(p => p.Id == idPacientePrimer);
            RegistroMedico? registroAAtender = pacienteAAtender?.Historial.FirstOrDefault(r => r.IdRegistro == idRegistroPrimer);

            if (pacienteAAtender == null || registroAAtender == null)
            {
                MessageBox.Show("No se encontró el paciente o el registro correspondiente al primer elemento de la cola.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Si hay un error, se intenta recargar la cola.
                CargarCola(); 
                return;
            }

            // Desencola el primer paciente de la cola real del sistema.
            _sistema.ColasPorHospital[_medico.IdHospital].Dequeue(); 
            
            // Abre el formulario FormAtenderPaciente para que el médico lo atienda.
            FormAtenderPaciente formAtender = new FormAtenderPaciente(_sistema, _medico, pacienteAAtender, registroAAtender);
            this.Hide(); // Oculta el formulario actual.
            formAtender.ShowDialog(); // Muestra el formulario de atención al paciente.
            this.Close(); // Cierra este formulario al terminar la atención del paciente.
        }
    }
}