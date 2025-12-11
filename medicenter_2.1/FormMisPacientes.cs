using System.Drawing;
﻿using System.Windows.Forms;
﻿
﻿namespace MEDICENTER
﻿{
﻿    // Formulario para que un médico visualice la lista de pacientes que le han sido asignados.
﻿    // Muestra los detalles básicos de cada paciente en un ListBox.
﻿    public partial class FormMisPacientes : Form
﻿    {
﻿        private Sistema sistema; // Instancia de la clase Sistema para acceder a los datos de los pacientes.
﻿        private PersonalHospitalario medico; // El objeto PersonalHospitalario (médico) logueado.
﻿        private ListBox listBoxPacientes; // Control ListBox para mostrar la lista de pacientes.
﻿
﻿        // Constructor del formulario.
﻿        public FormMisPacientes(Sistema sistemaParam, PersonalHospitalario medicoParam)
﻿        {
﻿            sistema = sistemaParam; // Asigna la instancia del sistema.
﻿            medico = medicoParam; // Asigna el objeto médico.
﻿            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
﻿        }
﻿
﻿        // Método que inicializa programáticamente todos los componentes visuales del formulario.
﻿        private void InitializeComponent()
﻿        {
﻿            // Configuración básica del formulario.
﻿            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
﻿            this.Text = "Mis Pacientes Asignados"; // Título de la ventana.
﻿            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
﻿            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
﻿
﻿            // Título principal del formulario.
﻿            Label lblTitulo = new Label();
﻿            lblTitulo.Text = "Mis Pacientes Asignados";
﻿            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
﻿            lblTitulo.Location = new Point(250, 20);
﻿            lblTitulo.Size = new Size(300, 40);
﻿            this.Controls.Add(lblTitulo);
﻿
﻿            // ListBox para mostrar los pacientes asignados.
﻿            listBoxPacientes = new ListBox();
﻿            listBoxPacientes.Location = new Point(50, 80);
﻿            listBoxPacientes.Size = new Size(700, 450);
﻿            listBoxPacientes.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
﻿            this.Controls.Add(listBoxPacientes);
﻿
﻿            CargarPacientes(); // Llama al método para cargar y mostrar la lista de pacientes.
﻿
﻿            // Botón "Cerrar".
﻿            Button btnCerrar = new Button();
﻿            btnCerrar.Text = "Cerrar";
﻿            btnCerrar.Font = new Font("Segoe UI", 12);
﻿            btnCerrar.Location = new Point(340, 540);
﻿            btnCerrar.Size = new Size(120, 45);
﻿            btnCerrar.BackColor = Color.White;
﻿            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
﻿            this.Controls.Add(btnCerrar);
﻿        }
﻿
﻿        // Método para cargar y mostrar la información de los pacientes asignados al médico en el ListBox.
﻿        private void CargarPacientes()
﻿        {
﻿            // Verifica si el médico tiene pacientes asignados.
﻿            if (medico.PacientesAsignados.Count == 0)
﻿            {
﻿                listBoxPacientes.Items.Add("No tiene pacientes asignados"); // Muestra un mensaje si no hay pacientes.
﻿                return;
﻿            }
﻿
﻿            // Muestra el total de pacientes asignados.
﻿            listBoxPacientes.Items.Add($"Total de pacientes: {medico.PacientesAsignados.Count}");
﻿            listBoxPacientes.Items.Add("═══════════════════════════════════════════════"); // Separador.
﻿            listBoxPacientes.Items.Add(""); // Línea en blanco.
﻿
﻿            // Itera sobre los IDs de los pacientes asignados al médico.
﻿            foreach (string idPaciente in medico.PacientesAsignados)
﻿            {
﻿                // Busca el objeto Paciente correspondiente al ID.
﻿                Paciente paciente = sistema.Pacientes.Find(p => p.Id == idPaciente);
﻿                if (paciente != null)
﻿                {
﻿                    // Añade la información formateada del paciente al ListBox.
﻿                    listBoxPacientes.Items.Add($"[{paciente.Id}] {paciente.Nombre}");
﻿                    listBoxPacientes.Items.Add(
﻿                        $"Edad: {paciente.Edad} | Registros: {paciente.Historial.Count}");
﻿                    listBoxPacientes.Items.Add(""); // Línea en blanco para separar entradas.
﻿                }
﻿            }
﻿        }
﻿    }
﻿}
﻿