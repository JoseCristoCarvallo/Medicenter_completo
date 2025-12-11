using System;
using System.Drawing;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario de diálogo para que el personal médico valide un registro médico preliminar.
    // Permite al médico agregar observaciones y un tratamiento.
    public partial class FormDialogoValidacion : Form
    {
        private RegistroMedico _registro; // El registro médico que se va a validar.
        
        // Campos de texto para que el médico ingrese sus datos.
        private TextBox txtObservaciones;
        private TextBox txtTratamiento;

        // Propiedades públicas para acceder a los datos ingresados por el médico.
        public string Observaciones { get; private set; }
        public string Tratamiento { get; private set; }

        // Constructor del formulario de diálogo.
        public FormDialogoValidacion(RegistroMedico registro)
        {
            _registro = registro; // Asigna el registro médico a validar.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(600, 450); // Tamaño del formulario.
            this.Text = "Validar Registro Médico"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal, mostrando el ID del registro a validar.
            Label lblTitulo = new Label();
            lblTitulo.Text = $"Validar Registro: {_registro.IdRegistro}";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(600, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Etiqueta para mostrar el diagnóstico previo generado automáticamente.
            Label lblDiagnosticoPrevio = new Label();
            lblDiagnosticoPrevio.Text = $"Diagnóstico Previo: {_registro.Diagnostico}";
            lblDiagnosticoPrevio.Font = new Font("Segoe UI", 11);
            lblDiagnosticoPrevio.Location = new Point(50, 70);
            lblDiagnosticoPrevio.Size = new Size(500, 25);
            this.Controls.Add(lblDiagnosticoPrevio);

            int yPos = 110; // Posición vertical inicial para los siguientes controles.

            // Campo para "Observaciones del Médico".
            Label lblObservaciones = new Label();
            lblObservaciones.Text = "Observaciones del Médico:";
            lblObservaciones.Font = new Font("Segoe UI", 11);
            lblObservaciones.Location = new Point(50, yPos);
            lblObservaciones.Size = new Size(200, 25);
            this.Controls.Add(lblObservaciones);

            txtObservaciones = new TextBox();
            txtObservaciones.Font = new Font("Segoe UI", 11);
            txtObservaciones.Location = new Point(50, yPos + 30);
            txtObservaciones.Size = new Size(500, 80);
            txtObservaciones.Multiline = true; // Permite múltiples líneas.
            txtObservaciones.ScrollBars = ScrollBars.Vertical; // Añade barras de desplazamiento verticales.
            this.Controls.Add(txtObservaciones);

            yPos += 130;

            // Campo para "Tratamiento".
            Label lblTratamiento = new Label();
            lblTratamiento.Text = "Tratamiento:";
            lblTratamiento.Font = new Font("Segoe UI", 11);
            lblTratamiento.Location = new Point(50, yPos);
            lblTratamiento.Size = new Size(200, 25);
            this.Controls.Add(lblTratamiento);

            txtTratamiento = new TextBox();
            txtTratamiento.Font = new Font("Segoe UI", 11);
            txtTratamiento.Location = new Point(50, yPos + 30);
            txtTratamiento.Size = new Size(500, 80);
            txtTratamiento.Multiline = true; // Permite múltiples líneas.
            txtTratamiento.ScrollBars = ScrollBars.Vertical; // Añade barras de desplazamiento verticales.
            this.Controls.Add(txtTratamiento);

            yPos += 130;

            // Botón "Aceptar y Validar".
            Button btnAceptar = new Button();
            btnAceptar.Text = "Aceptar y Validar";
            btnAceptar.Font = new Font("Segoe UI", 12);
            btnAceptar.Location = new Point(170, yPos);
            btnAceptar.Size = new Size(150, 45);
            btnAceptar.BackColor = Color.LightGreen;
            btnAceptar.Click += BtnAceptar_Click; // Asigna el evento de aceptación.
            this.Controls.Add(btnAceptar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 12);
            btnCancelar.Location = new Point(340, yPos);
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); }; // Cierra el formulario con resultado Cancel.
            this.Controls.Add(btnCancelar);
        }

        // Manejador de eventos para el botón "Aceptar y Validar".
        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            // Valida que los campos de observaciones y tratamiento no estén vacíos.
            if (string.IsNullOrWhiteSpace(txtObservaciones.Text) || string.IsNullOrWhiteSpace(txtTratamiento.Text))
            {
                MessageBox.Show("Por favor, ingrese Observaciones y Tratamiento.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Impide el cierre del diálogo si los campos están vacíos.
            }

            // Asigna los valores ingresados a las propiedades públicas.
            Observaciones = txtObservaciones.Text;
            Tratamiento = txtTratamiento.Text;
            this.DialogResult = DialogResult.OK; // Establece el resultado del diálogo a OK.
            this.Close(); // Cierra el formulario.
        }
    }
}
