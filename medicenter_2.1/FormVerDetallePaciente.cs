using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para mostrar el detalle completo de la información de un paciente.
    // Incluye información personal, detalles del seguro (con imágenes) y su historial médico.
    public partial class FormVerDetallePaciente : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para acceder a datos generales.
        private Paciente _paciente; // El objeto Paciente cuyos detalles se están visualizando.

        // Controles de UI para mostrar la información del paciente.
        private Label lblNombre;
        private Label lblID;
        private Label lblEmail;
        private Label lblEdad;
        private Label lblGenero;
        private Label lblTelefono;
        private Label lblTipoSangre;
        private Label lblSeguro;
        private Label lblNumeroSeguro;
        private Label lblContactoEmergencia;
        private PictureBox picFrontal; // PictureBox para la imagen frontal del seguro.
        private PictureBox picTrasera; // PictureBox para la imagen trasera del seguro.
        private ListBox listBoxHistorial; // ListBox para mostrar el historial médico.

        // Constructor del formulario.
        public FormVerDetallePaciente(Sistema sistema, Paciente paciente)
        {
            _sistema = sistema; // Asigna la instancia del sistema.
            _paciente = paciente; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(1000, 750); // Tamaño del formulario.
            this.Text = $"Detalle del Paciente: {_paciente.Nombre}"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Detalle Completo del Paciente";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(1000, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            int yPosInfo = 80; // Posición vertical inicial para los paneles de información.
            int xPosInfoLeft = 50;
            // int xPosInfoRight = 350; // Variable no usada directamente.

            // Panel de Información Básica.
            Panel panelInfoBasica = new Panel();
            panelInfoBasica.Location = new Point(xPosInfoLeft - 20, yPosInfo - 10);
            panelInfoBasica.Size = new Size(400, 300);
            panelInfoBasica.BorderStyle = BorderStyle.FixedSingle;
            panelInfoBasica.BackColor = Color.White;
            this.Controls.Add(panelInfoBasica);

            // Título del panel de Información Básica.
            Label lblInfoBasicaTitulo = new Label();
            lblInfoBasicaTitulo.Text = "Información Básica";
            lblInfoBasicaTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblInfoBasicaTitulo.Location = new Point(0, 10);
            lblInfoBasicaTitulo.Size = new Size(400, 30);
            lblInfoBasicaTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelInfoBasica.Controls.Add(lblInfoBasicaTitulo);

            // Labels con los datos básicos del paciente, creados dinámicamente.
            yPosInfo = 50;
            lblID = CrearLabelInfo(panelInfoBasica, "ID:", _paciente.Id, xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblNombre = CrearLabelInfo(panelInfoBasica, "Nombre:", _paciente.Nombre, xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblEmail = CrearLabelInfo(panelInfoBasica, "Email:", _paciente.Email, xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblEdad = CrearLabelInfo(panelInfoBasica, "Edad:", $"{_paciente.Edad} años", xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblGenero = CrearLabelInfo(panelInfoBasica, "Género:", _paciente.Genero.ToString(), xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblTelefono = CrearLabelInfo(panelInfoBasica, "Teléfono:", _paciente.Telefono, xPosInfoLeft, yPosInfo); yPosInfo += 30;
            lblContactoEmergencia = CrearLabelInfo(panelInfoBasica, "Contacto Emergencia:", _paciente.ContactoEmergencia, xPosInfoLeft, yPosInfo); yPosInfo += 30;
            
            // Panel de Seguro e Imágenes.
            Panel panelSeguroImagenes = new Panel();
            panelSeguroImagenes.Location = new Point(xPosInfoLeft + 420, 70);
            panelSeguroImagenes.Size = new Size(500, 310);
            panelSeguroImagenes.BorderStyle = BorderStyle.FixedSingle;
            panelSeguroImagenes.BackColor = Color.White;
            this.Controls.Add(panelSeguroImagenes);

            // Título del panel de Información del Seguro.
            Label lblSeguroTitulo = new Label();
            lblSeguroTitulo.Text = "Información del Seguro";
            lblSeguroTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblSeguroTitulo.Location = new Point(0, 10);
            lblSeguroTitulo.Size = new Size(500, 30);
            lblSeguroTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelSeguroImagenes.Controls.Add(lblSeguroTitulo);

            // Labels con los datos del seguro.
            int yPosSeguro = 50;
            lblTipoSangre = CrearLabelInfo(panelSeguroImagenes, "Tipo Sangre:", FormatearTipoSangre(_paciente.TipoSangre), xPosInfoLeft - 20, yPosSeguro); yPosSeguro += 30;
            lblSeguro = CrearLabelInfo(panelSeguroImagenes, "Tipo Seguro:", FormatearSeguro(_paciente.TipoSeguro), xPosInfoLeft - 20, yPosSeguro); yPosSeguro += 30;
            lblNumeroSeguro = CrearLabelInfo(panelSeguroImagenes, "Número Seguro:", _paciente.NumeroSeguro, xPosInfoLeft - 20, yPosSeguro); yPosSeguro += 30;

            // Título para la sección de Imágenes del seguro.
            Label lblImagenesTitulo = new Label();
            lblImagenesTitulo.Text = "Imágenes:";
            lblImagenesTitulo.Font = new Font("Segoe UI", 11);
            lblImagenesTitulo.Location = new Point(50, yPosSeguro);
            lblImagenesTitulo.Size = new Size(100, 25);
            panelSeguroImagenes.Controls.Add(lblImagenesTitulo);

            // PictureBox para la imagen frontal del seguro.
            picFrontal = new PictureBox();
            picFrontal.Location = new Point(160, yPosSeguro);
            picFrontal.Size = new Size(120, 100);
            picFrontal.BorderStyle = BorderStyle.FixedSingle;
            picFrontal.SizeMode = PictureBoxSizeMode.Zoom;
            panelSeguroImagenes.Controls.Add(picFrontal);

            // PictureBox para la imagen trasera del seguro.
            picTrasera = new PictureBox();
            picTrasera.Location = new Point(300, yPosSeguro);
            picTrasera.Size = new Size(120, 100);
            picTrasera.BorderStyle = BorderStyle.FixedSingle;
            picTrasera.SizeMode = PictureBoxSizeMode.Zoom;
            panelSeguroImagenes.Controls.Add(picTrasera);
            
            CargarImagenesSeguro(); // Carga las imágenes del seguro del paciente.

            // Panel de Historial Médico.
            Panel panelHistorial = new Panel();
            panelHistorial.Location = new Point(xPosInfoLeft - 20, 390);
            panelHistorial.Size = new Size(920, 280);
            panelHistorial.BorderStyle = BorderStyle.FixedSingle;
            panelHistorial.BackColor = Color.White;
            this.Controls.Add(panelHistorial);

            // Título del panel de Historial Médico.
            Label lblHistorialTitulo = new Label();
            lblHistorialTitulo.Text = "Historial Médico";
            lblHistorialTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblHistorialTitulo.Location = new Point(0, 10);
            lblHistorialTitulo.Size = new Size(920, 30);
            lblHistorialTitulo.TextAlign = ContentAlignment.MiddleCenter;
            panelHistorial.Controls.Add(lblHistorialTitulo);

            // ListBox para mostrar el historial médico.
            listBoxHistorial = new ListBox();
            listBoxHistorial.Location = new Point(20, 50);
            listBoxHistorial.Size = new Size(880, 210);
            listBoxHistorial.Font = new Font("Consolas", 9); // Fuente monoespaciada para mejor formato.
            panelHistorial.Controls.Add(listBoxHistorial);

            CargarHistorial(); // Carga el historial médico del paciente.

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(440, 680);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Método auxiliar para crear etiquetas de información con formato consistente.
        private Label CrearLabelInfo(Panel parent, string textoLabel, string textoValor, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = $"{textoLabel} {textoValor}"; // Formato "Etiqueta: Valor".
            lbl.Font = new Font("Segoe UI", 11);
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(300, 25);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            parent.Controls.Add(lbl);
            return lbl;
        }

        // Carga las imágenes del seguro del paciente y las muestra en los PictureBox.
        private void CargarImagenesSeguro()
        {
            // Carga la imagen frontal si existe.
            if (_paciente.ImagenSeguroFrontal != null && _paciente.ImagenSeguroFrontal.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(_paciente.ImagenSeguroFrontal))
                {
                    picFrontal.Image = Image.FromStream(ms);
                }
            }

            // Carga la imagen trasera si existe.
            if (_paciente.ImagenSeguroTrasera != null && _paciente.ImagenSeguroTrasera.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(_paciente.ImagenSeguroTrasera))
                {
                    picTrasera.Image = Image.FromStream(ms);
                }
            }
        }

        // Carga el historial médico del paciente y lo muestra en el ListBox.
        private void CargarHistorial()
        {
            // Muestra un mensaje si no hay registros.
            if (_paciente.Historial.Count == 0)
            {
                listBoxHistorial.Items.Add("No hay registros médicos para este paciente.");
                return;
            }

            // Itera sobre cada registro en el historial del paciente.
            foreach (var registro in _paciente.Historial)
            {
                // Busca el nombre del médico asociado al registro.
                PersonalHospitalario medicoAtendio = _sistema.Personal.OfType<PersonalHospitalario>()
                                                        .FirstOrDefault(p => p.Id == registro.IdMedico);
                string nombreMedico = medicoAtendio?.Nombre ?? "Desconocido";

                // Añade la información formateada del registro al ListBox.
                listBoxHistorial.Items.Add("───────────────────────────────────────────────");
                listBoxHistorial.Items.Add($"ID Registro: {registro.IdRegistro}");
                listBoxHistorial.Items.Add($"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm}");
                listBoxHistorial.Items.Add($"Hospital: {registro.IdHospital}");
                listBoxHistorial.Items.Add($"Síntomas: {string.Join(", ", registro.Sintomas)}");
                listBoxHistorial.Items.Add($"Diagnóstico: {registro.Diagnostico}");
                if (!string.IsNullOrEmpty(registro.Tratamiento))
                    listBoxHistorial.Items.Add($"Tratamiento: {registro.Tratamiento}");
                listBoxHistorial.Items.Add($"Estado: {(registro.Confirmado ? "Confirmado" : "Pendiente")}");
                listBoxHistorial.Items.Add($"Médico: {nombreMedico} (ID: {registro.IdMedico})");
                if (!string.IsNullOrEmpty(registro.ObservacionDoctor))
                    listBoxHistorial.Items.Add($"Observaciones: {registro.ObservacionDoctor}");
                listBoxHistorial.Items.Add(""); // Línea en blanco para separar entradas.
            }
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
