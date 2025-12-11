using System.Collections.Generic;
﻿using System.Drawing;
﻿using System.Windows.Forms;
﻿
﻿namespace MEDICENTER
﻿{
﻿    // Formulario para mostrar la cola de pacientes en espera para un hospital específico.
﻿    // Utiliza un ListBox para visualizar los pacientes que están aguardando atención.
﻿    public partial class FormColaPacientes : Form
﻿    {
﻿        private Sistema sistema; // Instancia de la clase Sistema para acceder a las colas de hospitales y datos de pacientes.
﻿        private string idHospital; // ID del hospital cuya cola de pacientes se va a mostrar.
﻿        private ListBox listBoxCola; // Control ListBox para mostrar los pacientes en cola.
﻿
﻿        // Constructor del formulario.
﻿        public FormColaPacientes(Sistema sistemaParam, string idHospitalParam)
﻿        {
﻿            sistema = sistemaParam; // Asigna la instancia del sistema.
﻿            idHospital = idHospitalParam; // Asigna el ID del hospital.
﻿            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
﻿        }
﻿
﻿        // Método que inicializa programáticamente todos los componentes visuales del formulario.
﻿        private void InitializeComponent()
﻿        {
﻿            // Configuración básica del formulario.
﻿            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
﻿            this.Text = "Cola de Pacientes"; // Título de la ventana.
﻿            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
﻿            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
﻿
﻿            // Título principal del formulario.
﻿            Label lblTitulo = new Label();
﻿            lblTitulo.Text = "Cola de Pacientes";
﻿            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
﻿            lblTitulo.Location = new Point(280, 20);
﻿            lblTitulo.Size = new Size(240, 40);
﻿            this.Controls.Add(lblTitulo);
﻿
﻿            // ListBox para mostrar los elementos de la cola.
﻿            listBoxCola = new ListBox();
﻿            listBoxCola.Location = new Point(50, 80);
﻿            listBoxCola.Size = new Size(700, 450);
﻿            listBoxCola.Font = new Font("Consolas", 10); // Fuente monoespaciada para mejor alineación.
﻿            this.Controls.Add(listBoxCola);
﻿
﻿            CargarCola(); // Llama al método para cargar y mostrar los pacientes en la cola.
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
﻿        // Método para cargar y mostrar la información de los pacientes en la cola del hospital.
﻿        private void CargarCola()
﻿        {
﻿            // Verifica si el hospital tiene una cola de pacientes y si está vacía.
﻿            if (!sistema.ColasPorHospital.ContainsKey(idHospital) ||
﻿                sistema.ColasPorHospital[idHospital].Count == 0)
﻿            {
﻿                listBoxCola.Items.Add("No hay pacientes en cola"); // Muestra un mensaje si no hay pacientes.
﻿                return;
﻿            }
﻿
﻿            Queue<string> cola = sistema.ColasPorHospital[idHospital]; // Obtiene la cola de pacientes del hospital.
﻿            string[] colaArray = cola.ToArray(); // Convierte la cola a un array para poder iterar sin desencolar.
﻿
﻿            listBoxCola.Items.Add($"Total de pacientes en espera: {colaArray.Length}"); // Muestra el número total.
﻿            listBoxCola.Items.Add("═════════════════════════════════════════════════"); // Separador.
﻿            listBoxCola.Items.Add(""); // Línea en blanco para formato.
﻿
﻿            // Itera sobre los elementos de la cola (que son claves "IDPaciente|IDRegistro").
﻿            for (int i = 0; i < colaArray.Length; i++)
﻿            {
﻿                string[] partes = colaArray[i].Split('|'); // Divide la clave para obtener ID de paciente y registro.
﻿                string idPaciente = partes[0];
﻿                string idRegistro = partes[1];
﻿
﻿                // Busca el objeto Paciente correspondiente al ID.
﻿                Paciente paciente = sistema.Pacientes.Find(p => p.Id == idPaciente);
﻿
﻿                // Muestra la información del paciente y su registro.
﻿                listBoxCola.Items.Add($"{i + 1}. Paciente: {paciente?.Nombre ?? idPaciente}"); // Muestra el nombre o ID si no se encuentra el paciente.
﻿                listBoxCola.Items.Add($"   ID: {idPaciente} | Registro: {idRegistro}");
﻿                listBoxCola.Items.Add(""); // Línea en blanco para formato.
﻿            }
﻿        }
﻿    }
﻿}
﻿