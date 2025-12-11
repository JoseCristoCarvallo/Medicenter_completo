using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace MEDICENTER
{
    // Formulario del menú principal para usuarios con nivel de acceso "Médico".
    // Proporciona acceso a funcionalidades específicas para médicos en el sistema MediCenter.
    public partial class FormMenuMedico : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio.
        private PersonalHospitalario medico; // Objeto PersonalHospitalario que representa al médico logueado.

        // Constructor del formulario de menú del médico.
        public FormMenuMedico(Sistema sistemaParam, PersonalHospitalario medicoParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            medico = medicoParam; // Asigna el objeto médico.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 700); // Tamaño del formulario.
            this.Text = "MediCenter - Menú Médico"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.

            // Busca el hospital al que el médico está asignado.
            Hospital hospital = sistema.BuscarHospital(medico.IdHospital);

            // Panel de información superior que muestra el nombre del médico y el hospital.
            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(0, 0);
            panelInfo.Size = new Size(900, 100);
            panelInfo.BackColor = Color.FromArgb(100, 150, 200); // Color distintivo para el panel superior.
            this.Controls.Add(panelInfo);

            // Etiqueta para mostrar el nombre del médico.
            Label lblNombreMedico = new Label();
            lblNombreMedico.Text = $"MÉDICO - {medico.Nombre}";
            lblNombreMedico.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblNombreMedico.ForeColor = Color.White;
            lblNombreMedico.Location = new Point(30, 20);
            lblNombreMedico.Size = new Size(600, 35);
            panelInfo.Controls.Add(lblNombreMedico);

            // Etiqueta para mostrar el nombre del hospital.
            Label lblHospital = new Label();
            lblHospital.Text = hospital?.Nombre ?? "Hospital"; // Muestra el nombre del hospital o "Hospital" si es nulo.
            lblHospital.Font = new Font("Segoe UI", 13);
            lblHospital.ForeColor = Color.White;
            lblHospital.Location = new Point(30, 55);
            lblHospital.Size = new Size(600, 30);
            panelInfo.Controls.Add(lblHospital);

            // Título central del panel médico.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Panel Médico";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(350, 120);
            lblTitulo.Size = new Size(200, 40);
            this.Controls.Add(lblTitulo);

            // Posiciones iniciales para organizar los botones en dos columnas.
            int yPos = 180;
            int xLeft = 100;
            int xRight = 500;

            // Botón para "Atender Paciente (Cola)".
            Button btnAtender = CrearBoton("1. Atender Paciente (Cola)", xLeft, yPos);
            btnAtender.Click += (s, e) => AtenderPaciente(); // Asigna el evento click.
            this.Controls.Add(btnAtender);

            // Botón para "Validar Diagnósticos".
            Button btnValidar = CrearBoton("2. Validar Diagnósticos", xRight, yPos);
            btnValidar.Click += (s, e) => ValidarDiagnosticos();
            this.Controls.Add(btnValidar);

            yPos += 70; // Incrementa la posición vertical para el siguiente par de botones.

            // Botón para "Ver Registros del Hospital".
            Button btnRegistros = CrearBoton("3. Ver Registros del Hospital", xLeft, yPos);
            btnRegistros.Click += (s, e) => VerRegistros();
            this.Controls.Add(btnRegistros);
            
            // Botón para "Ver Paciente" (permite buscar y ver detalles de un paciente específico).
            Button btnVerPaciente = CrearBoton("4. Ver Paciente", xRight, yPos);
            btnVerPaciente.Click += (s, e) => VerPaciente(); // Nuevo método para ver paciente.
            this.Controls.Add(btnVerPaciente);

            yPos += 70;

            // Botón para "Ver mi Información" (detalles del médico logueado).
            Button btnInfo = CrearBoton("5. Ver mi Información", xLeft, yPos);
            btnInfo.Click += (s, e) => VerInformacion();
            this.Controls.Add(btnInfo);

            // Botón para "Cambiar Contraseña".
            Button btnPassword = CrearBoton("6. Cambiar Contraseña", xRight, yPos);
            btnPassword.Click += (s, e) => CambiarPassword();
            this.Controls.Add(btnPassword);

            yPos += 70;

            // Botón para "Ver Enfermedades" (estadísticas de diagnósticos del hospital).
            Button btnEnfermedades = CrearBoton("7. Ver Enfermedades", xLeft, yPos);
            btnEnfermedades.Click += (s, e) => VerEnfermedades();
            this.Controls.Add(btnEnfermedades);

            yPos += 70;

            // Botón para "Cerrar Sesión".
            Button btnCerrarSesion = CrearBoton("0. Cerrar Sesión", xLeft, yPos);
            btnCerrarSesion.BackColor = Color.FromArgb(200, 100, 100); // Color distintivo para el botón de cerrar sesión.
            btnCerrarSesion.Click += (s, e) => CerrarSesion();
            this.Controls.Add(btnCerrarSesion);
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

        // Abre el formulario para gestionar la cola de pacientes a atender por el médico.
        private void AtenderPaciente()
        {
            FormListaColaPacientes formCola = new FormListaColaPacientes(sistema, medico);
            formCola.ShowDialog(); // Muestra el formulario como diálogo modal.
        }

        // Abre el formulario para que el médico valide diagnósticos.
        private void ValidarDiagnosticos()
        {
            FormValidarDiagnosticos formValidar = new FormValidarDiagnosticos(sistema, medico);
            formValidar.ShowDialog();
        }

        // Abre el formulario para ver los registros (usuarios) del hospital actual.
        private void VerRegistros()
        {
            FormVerUsuarios formVerUsuarios = new FormVerUsuarios(sistema, medico.IdHospital);
            formVerUsuarios.ShowDialog();
        }
        
        // El método original 'VerMisPacientes()' ha sido eliminado o su funcionalidad
        // ha sido integrada en 'VerPaciente()' o ya no es directamente necesaria en este menú.

        // Muestra la información detallada del médico actual.
        private void VerInformacion()
        {
            string info = medico.ObtenerInformacionFormateada(); // Usa el método de la clase PersonalHospitalario.
            MessageBox.Show(info, "Mi Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Abre el formulario para que el médico cambie su contraseña.
        private void CambiarPassword()
        {
            FormCambiarPasswordPersonal formCambio = new FormCambiarPasswordPersonal(sistema, medico);
            formCambio.ShowDialog();
        }

        // Permite al médico buscar un paciente por ID y ver sus detalles.
        private void VerPaciente()
        {
            // Solicita al usuario el ID del paciente mediante un cuadro de entrada.
            string idPaciente = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el ID del paciente:", "Ver Paciente", "");

            if (string.IsNullOrWhiteSpace(idPaciente))
            {
                MessageBox.Show("ID de paciente no puede estar vacío.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Busca el paciente en la lista de pacientes del sistema.
            Paciente paciente = sistema.Pacientes.Find(p => p.Id == idPaciente);

            if (paciente == null)
            {
                MessageBox.Show("Paciente no encontrado.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Abre el formulario FormVerDetallePaciente para mostrar los detalles del paciente.
            FormVerDetallePaciente formVerDetalle = new FormVerDetallePaciente(sistema, paciente);
            formVerDetalle.ShowDialog();
        }

        // Abre el formulario de estadísticas de enfermedades del hospital.
        private void VerEnfermedades()
        {
            FormEstadisticasEnfermedades formEstadisticas = new FormEstadisticasEnfermedades(sistema, medico);
            formEstadisticas.ShowDialog();
        }

        // Cierra la sesión del médico y el formulario actual.
        private void CerrarSesion()
        {
            MessageBox.Show("Hasta pronto", "Cerrando sesión",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Cierra el formulario.
        }
    }
}