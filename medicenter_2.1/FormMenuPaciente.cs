using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario del menú principal para usuarios con nivel de acceso "Paciente".
    // Proporciona al paciente acceso a sus datos personales, historial médico,
    // y funcionalidades para interactuar con el sistema (seleccionar hospital, etc.).
    public partial class FormMenuPaciente : Form
    {
        private Sistema sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio.
        private Paciente paciente; // Objeto Paciente que representa al paciente logueado.

        // Constructor del formulario de menú del paciente.
        public FormMenuPaciente(Sistema sistemaParam, Paciente pacienteParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            paciente = pacienteParam; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            this.SuspendLayout(); // Suspende la lógica de diseño.

            // Configuración básica del formulario.
            this.ClientSize = new Size(900, 650); // Tamaño del formulario.
            this.Text = "MediCenter - Menú Paciente"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.
            this.Icon = SystemIcons.Application; // Establece el icono de la aplicación.

            // Panel de información superior que muestra el nombre del paciente.
            Panel panelInfo = new Panel();
            panelInfo.Location = new Point(0, 0);
            panelInfo.Size = new Size(900, 80);
            panelInfo.BackColor = Color.FromArgb(100, 150, 200); // Color distintivo para el panel superior.
            this.Controls.Add(panelInfo);

            // Etiqueta para mostrar el nombre del paciente logueado.
            Label lblNombrePaciente = new Label();
            lblNombrePaciente.Text = $"Paciente: {paciente.Nombre}";
            lblNombrePaciente.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblNombrePaciente.ForeColor = Color.White;
            lblNombrePaciente.Location = new Point(30, 25);
            lblNombrePaciente.Size = new Size(500, 35);
            panelInfo.Controls.Add(lblNombrePaciente);

            // Título central del menú del paciente.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Menú Principal - Paciente";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(275, 100);
            lblTitulo.Size = new Size(400, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Posiciones iniciales para organizar los botones en dos columnas.
            int yPos = 160;
            int xLeft = 100;
            int xRight = 500;

            // Botón para "Seleccionar Hospital y Solicitar Consulta".
            Button btnSeleccionarHospital = CrearBoton("1. Seleccionar Hospital y Solicitar Consulta", xLeft, yPos);
            btnSeleccionarHospital.Click += (s, e) => SeleccionarHospitalYConsultar(); // Asigna el evento click.
            this.Controls.Add(btnSeleccionarHospital);

            // Botón para "Ver mi Historial Médico".
            Button btnHistorial = CrearBoton("2. Ver mi Historial Médico", xRight, yPos);
            btnHistorial.Click += (s, e) => VerHistorial();
            this.Controls.Add(btnHistorial);

            yPos += 70; // Incrementa la posición vertical para el siguiente par de botones.

            // Botón para "Ver mi Información Personal".
            Button btnInfoPersonal = CrearBoton("3. Ver mi Información Personal", xLeft, yPos);
            btnInfoPersonal.Click += (s, e) => VerInformacionPersonal();
            this.Controls.Add(btnInfoPersonal);

            // Botón para "Actualizar mi Información".
            Button btnActualizarDatos = CrearBoton("4. Actualizar mi Información", xRight, yPos);
            btnActualizarDatos.Click += (s, e) => ActualizarDatos();
            this.Controls.Add(btnActualizarDatos);

            yPos += 70;

            // Botón para "Ver Imágenes del Seguro".
            Button btnVerImagenesSeguro = CrearBoton("5. Ver Imágenes del Seguro", xLeft, yPos);
            btnVerImagenesSeguro.Click += (s, e) => VerImagenesSeguro();
            this.Controls.Add(btnVerImagenesSeguro);

            yPos += 70;

            // Botón para "Cerrar Sesión".
            Button btnCerrarSesion = CrearBoton("0. Cerrar Sesión", xLeft, yPos);
            btnCerrarSesion.BackColor = Color.FromArgb(200, 100, 100); // Color distintivo para el botón de cerrar sesión.
            btnCerrarSesion.Click += (s, e) => CerrarSesion();
            this.Controls.Add(btnCerrarSesion);

            this.ResumeLayout(false); // Reanuda la lógica de diseño.
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

        // Abre el formulario para que el paciente seleccione un hospital y solicite una consulta.
        private void SeleccionarHospitalYConsultar()
        {
            FormSeleccionarHospital formHospital = new FormSeleccionarHospital(sistema, paciente);
            formHospital.ShowDialog(); // Muestra el formulario como diálogo modal.
        }

        // Abre el formulario para que el paciente vea su historial médico.
        private void VerHistorial()
        {
            FormHistorialPaciente formHistorial = new FormHistorialPaciente(sistema, paciente); // Pasa la instancia del sistema y del paciente.
            formHistorial.ShowDialog();
        }

        // Muestra la información personal detallada del paciente en un MessageBox.
        private void VerInformacionPersonal()
        {
            string info = $"═══════════════════════════════════════\n";
            info += $"       INFORMACIÓN DEL PACIENTE\n";
            info += $"═══════════════════════════════════════\n\n";
            info += $"ID: {paciente.Id}\n";
            info += $"Nombre: {paciente.Nombre}\n";
            info += $"Email: {paciente.Email}\n";
            info += $"Edad: {paciente.Edad} años\n";
            info += $"Teléfono: {paciente.Telefono}\n";
            info += $"Género: {paciente.Genero}\n";
            info += $"Tipo de Sangre: {FormatearTipoSangre(paciente.TipoSangre)}\n"; // Usa el método auxiliar para formatear.
            info += $"Seguro Médico: {FormatearSeguro(paciente.TipoSeguro)}\n"; // Usa el método auxiliar para formatear.
            if (!string.IsNullOrEmpty(paciente.NumeroSeguro))
                info += $"Número de Seguro: {paciente.NumeroSeguro}\n";
            info += $"Contacto Emergencia: {paciente.ContactoEmergencia}\n";
            info += $"Registros Médicos: {paciente.Historial.Count}\n";

            MessageBox.Show(info, "Información Personal", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Abre el formulario para que el paciente actualice sus datos personales.
        private void ActualizarDatos()
        {
            FormActualizarDatos formActualizar = new FormActualizarDatos(sistema, paciente);
            formActualizar.ShowDialog();
        }

        // Abre el formulario para que el paciente vea las imágenes de su seguro.
        private void VerImagenesSeguro()
        {
            FormVerImagenesSeguro formVerImagenes = new FormVerImagenesSeguro(sistema, paciente); // Pasa la instancia del sistema y del paciente.
            formVerImagenes.ShowDialog();
        }

        // Cierra la sesión del paciente y el formulario actual.
        private void CerrarSesion()
        {
            MessageBox.Show("Hasta pronto", "Cerrando sesión",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Cierra el formulario.
        }

        // Método auxiliar para formatear la enumeración TipoSangre a una cadena más legible.
        private string FormatearTipoSangre(TipoSangre tipo)
        {
            return tipo.ToString().Replace("_Positivo", "+").Replace("_Negativo", "-");
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
