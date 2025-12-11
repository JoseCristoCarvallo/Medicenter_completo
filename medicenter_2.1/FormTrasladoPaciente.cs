using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para gestionar el traslado de un paciente (específicamente su último registro médico)
    // de un hospital a otro. Esta funcionalidad es utilizada por el personal hospitalario o administradores.
    public class FormTrasladoPaciente : Form
    {
        private readonly Sistema sistema; // Instancia de la clase Sistema para acceder y modificar datos de pacientes y hospitales.
        private ComboBox cmbPaciente; // ComboBox para seleccionar el paciente a trasladar.
        private ComboBox cmbHospitalDestino; // ComboBox para seleccionar el hospital de destino.

        // Constructor del formulario.
        public FormTrasladoPaciente(Sistema sistemaParam)
        {
            sistema = sistemaParam; // Asigna la instancia del sistema.
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
        }

        // Método que inicializa programáticamente todos los componentes visuales del formulario.
        private void InitializeComponent()
        {
            // Configuración básica del formulario.
            this.ClientSize = new Size(650, 360); // Tamaño del formulario.
            this.Text = "Traslado de Paciente a Otro Hospital"; // Título de la ventana.
            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
            this.BackColor = Color.FromArgb(240, 240, 255); // Color de fondo.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Borde fijo.
            this.MaximizeBox = false; // Deshabilita el botón de maximizar.
            this.Icon = SystemIcons.Application; // Establece el icono de la aplicación.

            // Título principal del formulario.
            Label lblTitulo = new Label();
            lblTitulo.Text = "Traslado de paciente";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.Location = new Point(0, 20);
            lblTitulo.Size = new Size(650, 40);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // Etiqueta para seleccionar el paciente.
            Label lblPaciente = new Label();
            lblPaciente.Text = "Seleccione el paciente a trasladar:";
            lblPaciente.Font = new Font("Segoe UI", 11);
            lblPaciente.Location = new Point(30, 80);
            lblPaciente.Size = new Size(580, 25);
            this.Controls.Add(lblPaciente);

            // ComboBox para listar y seleccionar pacientes.
            cmbPaciente = new ComboBox();
            cmbPaciente.DropDownStyle = ComboBoxStyle.DropDownList; // Solo permite selección, no edición.
            cmbPaciente.Location = new Point(30, 110);
            cmbPaciente.Size = new Size(580, 28);
            cmbPaciente.Font = new Font("Segoe UI", 10);
            this.Controls.Add(cmbPaciente);

            // Llena el ComboBox con todos los pacientes registrados en el sistema.
            foreach (var pac in sistema.Pacientes)
            {
                cmbPaciente.Items.Add(pac);
            }
            cmbPaciente.DisplayMember = nameof(Paciente.Nombre); // Muestra el nombre del paciente en el ComboBox.

            // Etiqueta para seleccionar el hospital de destino.
            Label lblDestino = new Label();
            lblDestino.Text = "Seleccione el hospital de destino:";
            lblDestino.Font = new Font("Segoe UI", 11);
            lblDestino.Location = new Point(30, 150);
            lblDestino.Size = new Size(580, 25);
            this.Controls.Add(lblDestino);

            // ComboBox para listar y seleccionar hospitales de destino.
            cmbHospitalDestino = new ComboBox();
            cmbHospitalDestino.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHospitalDestino.Location = new Point(30, 180);
            cmbHospitalDestino.Size = new Size(540, 28);
            cmbHospitalDestino.Font = new Font("Segoe UI", 10);
            this.Controls.Add(cmbHospitalDestino);

            // Carga los hospitales disponibles en el ComboBox de destino.
            foreach (var hosp in sistema.Hospitales)
            {
                cmbHospitalDestino.Items.Add(hosp);
            }

            // Botón "Trasladar".
            Button btnTrasladar = new Button();
            btnTrasladar.Text = "Trasladar";
            btnTrasladar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnTrasladar.Location = new Point(170, 240);
            btnTrasladar.Size = new Size(130, 45);
            btnTrasladar.BackColor = Color.FromArgb(180, 220, 180); // Color verde claro.
            btnTrasladar.Click += (s, e) => EjecutarTraslado(); // Asigna el evento para ejecutar el traslado.
            this.Controls.Add(btnTrasladar);

            // Botón "Cancelar".
            Button btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Segoe UI", 11);
            btnCancelar.Location = new Point(340, 240);
            btnCancelar.Size = new Size(130, 45);
            btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close(); // Cierra el formulario al cancelar.
            this.Controls.Add(btnCancelar);
        }

        // Método que ejecuta la lógica del traslado del paciente.
        private void EjecutarTraslado()
        {
            // Validaciones: verifica que se haya seleccionado un paciente.
            if (cmbPaciente.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un paciente.", "Traslado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validaciones: verifica que se haya seleccionado un hospital de destino.
            if (cmbHospitalDestino.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un hospital de destino.", "Traslado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var paciente = (Paciente)cmbPaciente.SelectedItem; // Obtiene el objeto Paciente seleccionado.

            // Valida si el paciente tiene registros médicos para trasladar.
            if (paciente.Historial.Count == 0)
            {
                MessageBox.Show("El paciente no tiene registros medicos para trasladar.", "Traslado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Obtiene el último registro médico del paciente y el hospital de destino seleccionado.
            var ultimoRegistro = paciente.Historial.Last();
            string idHospitalOrigen = ultimoRegistro.IdHospital; // ID del hospital actual del último registro.
            var hospitalDestino = (Hospital)cmbHospitalDestino.SelectedItem; // Objeto Hospital de destino.

            // Valida que el hospital de destino no sea el mismo que el actual del último registro.
            if (hospitalDestino.Id == idHospitalOrigen)
            {
                MessageBox.Show("El hospital de destino es el mismo que el hospital actual del ultimo registro.", "Traslado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pide confirmación al usuario antes de realizar el traslado.
            var resultado = MessageBox.Show(
                $"Se trasladara el ultimo registro (ID: {ultimoRegistro.IdRegistro})\n" +
                $"del hospital {idHospitalOrigen} al hospital {hospitalDestino.Id}.\n\n" +
                "¿Desea continuar?",
                "Confirmar traslado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return; // Si el usuario cancela, no procede con el traslado.

            // Lógica para mover el registro entre los diccionarios de registros por hospital.
            // Primero, elimina el registro del hospital de origen (si existe).
            if (!string.IsNullOrEmpty(idHospitalOrigen) &&
                sistema.RegistrosPorHospital.ContainsKey(idHospitalOrigen))
            {
                sistema.RegistrosPorHospital[idHospitalOrigen].Remove(ultimoRegistro);
            }

            // Elimina al paciente de la lista de PacientesAtendidos del hospital de origen.
            Hospital hospitalOrigen = sistema.Hospitales.FirstOrDefault(h => h.Id == idHospitalOrigen);
            if (hospitalOrigen != null && hospitalOrigen.PacientesAtendidos.Contains(paciente.Id))
            {
                hospitalOrigen.PacientesAtendidos.Remove(paciente.Id);
            }

            // Asegura que exista una lista de registros para el hospital de destino.
            if (!sistema.RegistrosPorHospital.ContainsKey(hospitalDestino.Id))
            {
                sistema.RegistrosPorHospital[hospitalDestino.Id] = new System.Collections.Generic.List<RegistroMedico>();
            }

            // Actualiza el IdHospital en el último registro y lo añade al nuevo hospital.
            ultimoRegistro.IdHospital = hospitalDestino.Id;
            sistema.RegistrosPorHospital[hospitalDestino.Id].Add(ultimoRegistro);

            // Registra al paciente en la lista de pacientes atendidos del hospital de destino (si no está ya).
            if (!hospitalDestino.PacientesAtendidos.Contains(paciente.Id))
            {
                hospitalDestino.PacientesAtendidos.Add(paciente.Id);
            }

            MessageBox.Show("Traslado realizado correctamente.", "Traslado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            sistema.GuardarUsuario(paciente); // Guarda los datos actualizados del paciente después del traslado.
            this.Close(); // Cierra el formulario.
        }
    }
}
