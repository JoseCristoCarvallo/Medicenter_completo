using System.Collections.Generic;
using System.Drawing;
using System.Linq; // Necesario para .Count, .Where y .Any
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el paciente seleccione un hospital donde solicitar una consulta.
    // La lista de hospitales disponibles se filtra según el tipo de seguro del paciente.
    public partial class FormSeleccionarHospital : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para acceder a los datos de hospitales y pacientes.
        private Paciente paciente; // El objeto Paciente que está seleccionando el hospital.
        private ListView listViewHospitales; // Control ListView para mostrar la lista de hospitales.

        // Constructor del formulario.
        public FormSeleccionarHospital(Sistema sistemaParam, Paciente pacienteParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            paciente = pacienteParam; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "Seleccionar Hospital"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Seleccionar Hospital";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.Location = new Point(300, 20);
            lblTitulo.Size = new Size(300, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Etiqueta informativa sobre la disponibilidad de hospitales.
            Label lblDisponibles = new Label();
            lblDisponibles.Text = "Hospitales Disponibles (según su seguro):";
            lblDisponibles.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblDisponibles.Location = new Point(50, 80);
            lblDisponibles.Size = new Size(400, 30);
            this.Controls.Add(lblDisponibles);

            // ListView para mostrar la lista de hospitales.
            listViewHospitales = new ListView();
            listViewHospitales.Location = new Point(50, 120);
            listViewHospitales.Size = new Size(800, 450);
            listViewHospitales.View = View.Details; // Muestra los elementos en un formato de tabla.
            listViewHospitales.FullRowSelect = true; // Permite seleccionar la fila completa.
            listViewHospitales.GridLines = true; // Muestra las líneas de la cuadrícula.
            listViewHospitales.Font = new Font("Segoe UI", 10);

            // Define las columnas del ListView.
            listViewHospitales.Columns.Add("ID", 80);
            listViewHospitales.Columns.Add("Hospital", 350);
            listViewHospitales.Columns.Add("Tipo", 120);
            listViewHospitales.Columns.Add("Doctores", 100); 
            listViewHospitales.Columns.Add("Costo", 150);

            CargarHospitales(); // Llama al método para cargar la lista de hospitales.
            this.Controls.Add(listViewHospitales);

            // Botón "Seleccionar y Continuar".
            Button btnSeleccionar = new Button();
            btnSeleccionar.Text = "Seleccionar y Continuar";
            btnSeleccionar.Font = new Font("Segoe UI", 12);
            btnSeleccionar.Location = new Point(300, 600);
            btnSeleccionar.Size = new Size(200, 45);
            btnSeleccionar.BackColor = Color.White;
            btnSeleccionar.Click += BtnSeleccionar_Click; // Asigna el evento de selección.
            this.Controls.Add(btnSeleccionar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(520, 600);
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);
        }

        // Carga y muestra los hospitales disponibles en el ListView, filtrados por el tipo de seguro del paciente.
        private void CargarHospitales()
        {
            listViewHospitales.Items.Clear(); // Limpia los elementos existentes.

            // Obtiene todos los hospitales del sistema.
            var todosLosHospitales = sistema.Hospitales;

            // Lista para almacenar los hospitales que el paciente puede seleccionar directamente.
            List<Hospital> hospitalesVisibles = new List<Hospital>();

            // Filtra los hospitales según el tipo de seguro del paciente.
            if (paciente.TipoSeguro == TipoSeguro.SeguroCompleto)
            {
                hospitalesVisibles.AddRange(todosLosHospitales); // Con seguro completo, todos son visibles.
            }
            else // Si el seguro es Básico o Sin Seguro, solo puede ver hospitales públicos.
            {
                hospitalesVisibles.AddRange(todosLosHospitales.Where(h => h.EsPublico));
            }

            // Añade los hospitales visibles a la lista.
            foreach (var hospital in hospitalesVisibles)
            {
                // Cuenta el número de doctores (PersonalHospitalario con NivelAcceso.MedicoGeneral) en este hospital.
                int numDoctores = sistema.Personal.Count(p => p.IdHospital == hospital.Id && p.NivelAcceso == NivelAcceso.MedicoGeneral);

                ListViewItem item = new ListViewItem(hospital.Id); // Crea un nuevo item para el ListView.
                item.SubItems.Add(hospital.Nombre);
                item.SubItems.Add(hospital.EsPublico ? "Público" : "Privado"); // Muestra el tipo de hospital.
                item.SubItems.Add(numDoctores.ToString()); // Muestra el número de doctores.
                // Muestra el costo de consulta (Gratis para públicos, o el precio para privados).
                item.SubItems.Add(hospital.EsPublico ? "Gratis" :
                    "$" + hospital.CostoConsulta.ToString("F2"));
                item.Tag = hospital; // Almacena el objeto Hospital completo en la propiedad Tag.
                listViewHospitales.Items.Add(item);
            }

            // Si el paciente no tiene seguro completo, muestra los hospitales privados como "no disponibles"
            // para que el paciente sepa que existen, pero no puede seleccionarlos.
            if (paciente.TipoSeguro != TipoSeguro.SeguroCompleto)
            {
                foreach (var hospitalPrivado in todosLosHospitales.Where(h => !h.EsPublico))
                {
                    // Solo añade los hospitales privados que aún no han sido añadidos a la lista (evitar duplicados).
                    if (!hospitalesVisibles.Any(h => h.Id == hospitalPrivado.Id))
                    {
                        int numDoctores = sistema.Personal.Count(p => p.IdHospital == hospitalPrivado.Id && p.NivelAcceso == NivelAcceso.MedicoGeneral);

                        ListViewItem item = new ListViewItem(hospitalPrivado.Id);
                        item.SubItems.Add(hospitalPrivado.Nombre + " (No disponible con su seguro)"); // Indicación de no disponibilidad.
                        item.SubItems.Add("Privado");
                        item.SubItems.Add(numDoctores.ToString());
                        item.SubItems.Add("$" + hospitalPrivado.CostoConsulta.ToString("F2"));
                        item.Tag = hospitalPrivado;
                        item.BackColor = Color.LightGray; // Estilo visual para indicar no seleccionable.
                        item.ForeColor = Color.DarkGray;
                        listViewHospitales.Items.Add(item);
                    }
                }
            }
        }

        // Manejador de eventos para el botón "Seleccionar y Continuar".
        private void BtnSeleccionar_Click(object sender, System.EventArgs e)
        {
            // Valida que se haya seleccionado un hospital.
            if (listViewHospitales.SelectedItems.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un hospital", "Selección requerida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtiene el objeto Hospital completo del elemento seleccionado.
            Hospital hospitalSeleccionado = (Hospital)listViewHospitales.SelectedItems[0].Tag; 

            if (hospitalSeleccionado == null)
            {
                MessageBox.Show("Error al seleccionar hospital. Por favor intente de nuevo.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Valida que un paciente con seguro básico o sin seguro no pueda seleccionar un hospital privado.
            if (!hospitalSeleccionado.EsPublico && paciente.TipoSeguro != TipoSeguro.SeguroCompleto)
            {
                MessageBox.Show("No puede seleccionar hospitales privados con su tipo de seguro.", "Acceso Restringido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si es un hospital privado y el paciente tiene seguro completo, pide confirmación de pago.
            if (!hospitalSeleccionado.EsPublico && paciente.TipoSeguro == TipoSeguro.SeguroCompleto)
            {
                DialogResult result = MessageBox.Show(
                    $"Este hospital requiere pago de consulta: {hospitalSeleccionado.CostoConsulta:F2}\n\n¿Desea continuar?",
                    "Confirmación de pago",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return; // Si el paciente cancela, no procede.
            }

            // Abre el formulario de bienvenida al hospital, pasando los datos necesarios.
            FormBienvenidaHospital formBienvenida =
                new FormBienvenidaHospital(sistema, paciente, hospitalSeleccionado);
            this.Hide(); // Oculta el formulario actual.
            formBienvenida.ShowDialog(); // Muestra el formulario de bienvenida como diálogo modal.
            this.Close(); // Cierra el formulario de selección de hospital.
        }
    }
}