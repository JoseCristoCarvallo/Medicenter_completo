using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para que el paciente pueda ver y actualizar las imágenes de su seguro médico.
    // Muestra la vista frontal y trasera del seguro y permite cargar nuevas imágenes.
    public partial class FormVerImagenesSeguro : Form
    {
        private Sistema _sistema; // Instancia de la clase Sistema para interactuar con la lógica de negocio y guardar cambios.
        private Paciente _paciente; // El objeto Paciente cuyas imágenes de seguro se están gestionando.
        
        // Controles PictureBox para mostrar las imágenes.
        private PictureBox picFrontal;
        private PictureBox picTrasera;

        // Variables temporales para almacenar las nuevas imágenes cargadas antes de guardarlas definitivamente.
        private byte[]? _nuevaImagenFrontalBytes;
        private byte[]? _nuevaImagenTraseraBytes;

        // Constructor del formulario.
        public FormVerImagenesSeguro(Sistema sistemaParam, Paciente pacienteParam) // Constructor modificado
        {
            _sistema = sistemaParam; // Asigna la instancia del sistema.
            _paciente = pacienteParam; // Asigna el objeto paciente.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(800, 600); // Tamaño del formulario.
            this.Text = $"Imágenes de Seguro - {_paciente.Nombre}"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = $"Imágenes de Seguro de {_paciente.Nombre}";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(800, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Sección para la Imagen Frontal del seguro.
            Label lblFrontal = new Label();
            lblFrontal.Text = "Vista Frontal";
            lblFrontal.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblFrontal.Location = new Point(100, 80);
            lblFrontal.Size = new Size(200, 30);
            lblFrontal.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblFrontal);

            picFrontal = new PictureBox();
            picFrontal.Location = new Point(100, 120);
            picFrontal.Size = new Size(250, 250);
            picFrontal.BorderStyle = BorderStyle.FixedSingle;
            picFrontal.SizeMode = PictureBoxSizeMode.Zoom; // Ajusta la imagen al tamaño del PictureBox.
            this.Controls.Add(picFrontal);

            // Botón para cambiar la imagen frontal.
            Button btnCambiarFrontal = new Button();
            btnCambiarFrontal.Text = "Cambiar Frontal";
            btnCambiarFrontal.Font = new Font("Segoe UI", 10);
            btnCambiarFrontal.Location = new Point(130, 380);
            btnCambiarFrontal.Size = new Size(180, 40);
            btnCambiarFrontal.Click += (s, e) => CargarNuevaImagen(picFrontal, true); // Asigna el evento de carga.
            this.Controls.Add(btnCambiarFrontal);


            // Sección para la Imagen Trasera del seguro.
            Label lblTrasera = new Label();
            lblTrasera.Text = "Vista Trasera";
            lblTrasera.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTrasera.Location = new Point(450, 80);
            lblTrasera.Size = new Size(200, 30);
            lblTrasera.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTrasera);

            picTrasera = new PictureBox();
            picTrasera.Location = new Point(450, 120);
            picTrasera.Size = new Size(250, 250);
            picTrasera.BorderStyle = BorderStyle.FixedSingle;
            picTrasera.SizeMode = PictureBoxSizeMode.Zoom; // Ajusta la imagen al tamaño del PictureBox.
            this.Controls.Add(picTrasera);

            // Botón para cambiar la imagen trasera.
            Button btnCambiarTrasera = new Button();
            btnCambiarTrasera.Text = "Cambiar Trasera";
            btnCambiarTrasera.Font = new Font("Segoe UI", 10);
            btnCambiarTrasera.Location = new Point(480, 380);
            btnCambiarTrasera.Size = new Size(180, 40);
            btnCambiarTrasera.Click += (s, e) => CargarNuevaImagen(picTrasera, false); // Asigna el evento de carga.
            this.Controls.Add(btnCambiarTrasera);


            CargarImagenesExistente(); // Carga las imágenes existentes del paciente al iniciar el formulario.

            // Botón "Guardar Cambios".
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar Cambios";
            btnGuardar.Font = new Font("Segoe UI", 12);
            btnGuardar.Location = new Point(220, 450);
            btnGuardar.Size = new Size(160, 45);
            btnGuardar.BackColor = Color.LightGreen;
            btnGuardar.Click += BtnGuardar_Click; // Asigna el evento para guardar las imágenes.
            this.Controls.Add(btnGuardar);

            // Botón "Cerrar".
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 12);
            btnCerrar.Location = new Point(400, 450);
            btnCerrar.Size = new Size(120, 45);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
            this.Controls.Add(btnCerrar);
        }

        // Carga las imágenes de seguro existentes del paciente en los PictureBox.
        private void CargarImagenesExistente()
        {
            // Carga la imagen frontal si está disponible.
            if (_paciente.ImagenSeguroFrontal != null && _paciente.ImagenSeguroFrontal.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(_paciente.ImagenSeguroFrontal))
                {
                    picFrontal.Image = Image.FromStream(ms);
                }
            }

            // Carga la imagen trasera si está disponible.
            if (_paciente.ImagenSeguroTrasera != null && _paciente.ImagenSeguroTrasera.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(_paciente.ImagenSeguroTrasera))
                {
                    picTrasera.Image = Image.FromStream(ms);
                }
            }
        }

        // Permite al usuario seleccionar una nueva imagen desde el sistema de archivos y la carga en el PictureBox.
        private void CargarNuevaImagen(PictureBox pictureBox, bool esFrontal)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png;*.bmp"; // Filtra por tipos de imagen.
                ofd.Title = "Seleccionar Imagen de Seguro";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image nuevaImagen = Image.FromFile(ofd.FileName); // Carga la imagen seleccionada.
                        pictureBox.Image = nuevaImagen; // Muestra la imagen en el PictureBox.

                        // Convierte la imagen a un arreglo de bytes para su almacenamiento temporal.
                        using (MemoryStream ms = new MemoryStream())
                        {
                            nuevaImagen.Save(ms, nuevaImagen.RawFormat);
                            if (esFrontal)
                                _nuevaImagenFrontalBytes = ms.ToArray(); // Almacena bytes de la nueva imagen frontal.
                            else
                                _nuevaImagenTraseraBytes = ms.ToArray(); // Almacena bytes de la nueva imagen trasera.
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Manejador de eventos para el botón "Guardar Cambios".
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            bool cambiosRealizados = false; // Flag para indicar si se realizaron cambios.

            // Si se cargó una nueva imagen frontal, la asigna al paciente.
            if (_nuevaImagenFrontalBytes != null)
            {
                _paciente.ImagenSeguroFrontal = _nuevaImagenFrontalBytes;
                cambiosRealizados = true;
            }

            // Si se cargó una nueva imagen trasera, la asigna al paciente.
            if (_nuevaImagenTraseraBytes != null)
            {
                _paciente.ImagenSeguroTrasera = _nuevaImagenTraseraBytes;
                cambiosRealizados = true;
            }

            // Si se realizaron cambios, guarda el objeto paciente actualizado en el sistema.
            if (cambiosRealizados)
            {
                _sistema.GuardarUsuario(_paciente); // Persiste los cambios del paciente.
                MessageBox.Show("Imágenes del seguro actualizadas exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Cierra el formulario.
            }
            else
            {
                MessageBox.Show("No se realizaron cambios en las imágenes.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}