using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MEDICENTER
{
    // Formulario para mostrar estadísticas de enfermedades diagnosticadas en el hospital
    // Muestra un gráfico de barras con porcentajes y una lista detallada
    public partial class FormEstadisticasEnfermedades : Form
    {
        private Sistema sistema;
        private PersonalHospitalario medico;
        private Hospital hospital;
        private Dictionary<string, int> estadisticasEnfermedades;
        private int totalDiagnosticos;

        public FormEstadisticasEnfermedades(Sistema sistemaParam, PersonalHospitalario medicoParam)
        {
            sistema = sistemaParam;
            medico = medicoParam;
            hospital = sistema.BuscarHospital(medico.IdHospital);

            // Analiza los registros y genera las estadísticas
            GenerarEstadisticas();

            InitializeComponent();
        }

        // Genera estadísticas de enfermedades a partir de los diagnósticos del hospital
        private void GenerarEstadisticas()
        {
            estadisticasEnfermedades = new Dictionary<string, int>();
            totalDiagnosticos = 0;

            // Verifica si hay registros para este hospital
            if (!sistema.RegistrosPorHospital.ContainsKey(hospital.Id))
            {
                return;
            }

            var registros = sistema.RegistrosPorHospital[hospital.Id];

            // Analiza cada registro y extrae la enfermedad principal del diagnóstico
            // Prioriza el diagnóstico confirmado por el médico si existe
            foreach (var registro in registros)
            {
                string diagnosticoAUsar = "";

                // Si hay diagnóstico confirmado por médico, usa ese; si no, usa el automático
                if (!string.IsNullOrWhiteSpace(registro.DiagnosticoConfirmado))
                {
                    diagnosticoAUsar = registro.DiagnosticoConfirmado;
                }
                else if (!string.IsNullOrWhiteSpace(registro.Diagnostico))
                {
                    diagnosticoAUsar = registro.Diagnostico;
                }

                if (string.IsNullOrWhiteSpace(diagnosticoAUsar))
                    continue;

                string enfermedad = ExtraerEnfermedadPrincipal(diagnosticoAUsar);

                if (!string.IsNullOrWhiteSpace(enfermedad))
                {
                    if (estadisticasEnfermedades.ContainsKey(enfermedad))
                    {
                        estadisticasEnfermedades[enfermedad]++;
                    }
                    else
                    {
                        estadisticasEnfermedades[enfermedad] = 1;
                    }
                    totalDiagnosticos++;
                }
            }
        }

        // Extrae la enfermedad principal del texto del diagnóstico
        private string ExtraerEnfermedadPrincipal(string diagnostico)
        {
            // Elimina la descripción del paciente si existe
            if (diagnostico.Contains("Descripción del paciente:") ||
                diagnostico.Contains("Descripcion del paciente:"))
            {
                int indice = diagnostico.IndexOf("Descripción del paciente:", StringComparison.OrdinalIgnoreCase);
                if (indice == -1)
                    indice = diagnostico.IndexOf("Descripcion del paciente:", StringComparison.OrdinalIgnoreCase);

                if (indice >= 0)
                    diagnostico = diagnostico.Substring(0, indice).Trim();
            }

            // Extrae la parte más relevante del diagnóstico
            string diagnosticoLimpio = diagnostico.ToUpper();

            // Busca palabras clave de enfermedades
            if (diagnosticoLimpio.Contains("NEUMONÍA") || diagnosticoLimpio.Contains("NEUMONIA"))
                return "Neumonía / COVID-19";
            if (diagnosticoLimpio.Contains("BRONQUITIS"))
                return "Bronquitis";
            if (diagnosticoLimpio.Contains("GRIPE") || diagnosticoLimpio.Contains("INFLUENZA"))
                return "Gripe / Influenza";
            if (diagnosticoLimpio.Contains("MENINGITIS"))
                return "Meningitis";
            if (diagnosticoLimpio.Contains("MIGRAÑA") || diagnosticoLimpio.Contains("MIGRANA") || diagnosticoLimpio.Contains("CEFALEA"))
                return "Migraña / Cefalea";
            if (diagnosticoLimpio.Contains("DENGUE"))
                return "Dengue";
            if (diagnosticoLimpio.Contains("CHIKUNGUNYA"))
                return "Chikungunya";
            if (diagnosticoLimpio.Contains("INFECCIÓN VIRAL") || diagnosticoLimpio.Contains("INFECCION VIRAL"))
                return "Infección Viral";
            if (diagnosticoLimpio.Contains("APENDICITIS"))
                return "Apendicitis";
            if (diagnosticoLimpio.Contains("COLITIS"))
                return "Colitis";
            if (diagnosticoLimpio.Contains("GASTROENTERITIS"))
                return "Gastroenteritis";
            if (diagnosticoLimpio.Contains("GASTRITIS") || diagnosticoLimpio.Contains("DISPEPSIA"))
                return "Gastritis / Dispepsia";
            if (diagnosticoLimpio.Contains("AMIGDALITIS"))
                return "Amigdalitis";
            if (diagnosticoLimpio.Contains("FARINGITIS"))
                return "Faringitis";
            if (diagnosticoLimpio.Contains("INFECCIÓN URINARIA") || diagnosticoLimpio.Contains("INFECCION URINARIA") ||
                diagnosticoLimpio.Contains("CISTITIS") || diagnosticoLimpio.Contains("PIELONEFRITIS"))
                return "Infección Urinaria";
            if (diagnosticoLimpio.Contains("LUMBALGIA"))
                return "Lumbalgia";
            if (diagnosticoLimpio.Contains("ARTRITIS") || diagnosticoLimpio.Contains("BURSITIS"))
                return "Artritis / Bursitis";

            // Si no se identifica una enfermedad específica
            if (diagnosticoLimpio.Contains("CRÍTICO") || diagnosticoLimpio.Contains("CRITICO"))
                return "Condición Crítica";
            if (diagnosticoLimpio.Contains("MODERADO"))
                return "Condición Moderada";
            if (diagnosticoLimpio.Contains("LEVE"))
                return "Condición Leve";

            return "Otras Condiciones";
        }

        private void InitializeComponent()
        {
            this.ClientSize = new Size(1000, 700);
            this.Text = "Estadísticas de Enfermedades";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 230, 250);

            // Panel superior con información
            Panel panelHeader = new Panel();
            panelHeader.Location = new Point(0, 0);
            panelHeader.Size = new Size(1000, 80);
            panelHeader.BackColor = Color.FromArgb(70, 130, 180);
            this.Controls.Add(panelHeader);

            Label lblTitulo = new Label();
            lblTitulo.Text = "ESTADÍSTICAS DE ENFERMEDADES";
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(30, 15);
            lblTitulo.Size = new Size(500, 30);
            panelHeader.Controls.Add(lblTitulo);

            Label lblHospital = new Label();
            lblHospital.Text = $"Hospital: {hospital.Nombre}";
            lblHospital.Font = new Font("Segoe UI", 12);
            lblHospital.ForeColor = Color.White;
            lblHospital.Location = new Point(30, 45);
            lblHospital.Size = new Size(500, 25);
            panelHeader.Controls.Add(lblHospital);

            Label lblTotal = new Label();
            lblTotal.Text = $"Total de diagnósticos: {totalDiagnosticos}";
            lblTotal.Font = new Font("Segoe UI", 11);
            lblTotal.ForeColor = Color.White;
            lblTotal.Location = new Point(700, 30);
            lblTotal.Size = new Size(250, 25);
            panelHeader.Controls.Add(lblTotal);

            // Panel para el gráfico
            Panel panelGrafico = new Panel();
            panelGrafico.Location = new Point(20, 100);
            panelGrafico.Size = new Size(950, 400);
            panelGrafico.BackColor = Color.White;
            panelGrafico.BorderStyle = BorderStyle.FixedSingle;
            panelGrafico.AutoScroll = true;
            panelGrafico.Paint += PanelGrafico_Paint;
            this.Controls.Add(panelGrafico);

            // Lista detallada de enfermedades
            Label lblListaTitulo = new Label();
            lblListaTitulo.Text = "DETALLE POR ENFERMEDAD:";
            lblListaTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblListaTitulo.Location = new Point(20, 515);
            lblListaTitulo.Size = new Size(300, 25);
            this.Controls.Add(lblListaTitulo);

            ListView listViewEnfermedades = new ListView();
            listViewEnfermedades.Location = new Point(20, 545);
            listViewEnfermedades.Size = new Size(950, 100);
            listViewEnfermedades.View = View.Details;
            listViewEnfermedades.FullRowSelect = true;
            listViewEnfermedades.GridLines = true;
            listViewEnfermedades.Font = new Font("Segoe UI", 10);

            listViewEnfermedades.Columns.Add("Enfermedad", 400);
            listViewEnfermedades.Columns.Add("Casos", 150);
            listViewEnfermedades.Columns.Add("Porcentaje", 150);
            listViewEnfermedades.Columns.Add("Gráfico", 230);

            // Evento para hacer clic en "Otras Condiciones"
            listViewEnfermedades.MouseDoubleClick += ListViewEnfermedades_MouseDoubleClick;

            // Ordena las enfermedades por cantidad de casos (descendente)
            var enfermedadesOrdenadas = estadisticasEnfermedades.OrderByDescending(x => x.Value);

            foreach (var kvp in enfermedadesOrdenadas)
            {
                double porcentaje = totalDiagnosticos > 0 ? (kvp.Value * 100.0 / totalDiagnosticos) : 0;
                ListViewItem item = new ListViewItem(kvp.Key);
                item.SubItems.Add(kvp.Value.ToString());
                item.SubItems.Add($"{porcentaje:F1}%");

                // Crea una barra visual simple
                int barLength = (int)(porcentaje / 2); // Máximo 50 caracteres
                string barra = new string('█', Math.Min(barLength, 50));
                item.SubItems.Add(barra);

                listViewEnfermedades.Items.Add(item);
            }

            this.Controls.Add(listViewEnfermedades);

            // Botón cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 11);
            btnCerrar.Location = new Point(430, 655);
            btnCerrar.Size = new Size(140, 35);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCerrar);
        }

        // Dibuja el gráfico de barras
        private void PanelGrafico_Paint(object sender, PaintEventArgs e)
        {
            if (totalDiagnosticos == 0 || estadisticasEnfermedades.Count == 0)
            {
                // Muestra mensaje si no hay datos
                e.Graphics.DrawString("No hay datos de diagnósticos para mostrar",
                    new Font("Segoe UI", 14), Brushes.Gray, new PointF(250, 180));
                return;
            }

            Panel panel = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Configuración del gráfico
            int margenIzq = 250;
            int margenTop = 40;
            int margenBottom = 30;
            int altoBarra = 35;
            int espacioEntreBarra = 10;
            int anchoMaxBarra = panel.Width - margenIzq - 100;

            // Ordena las enfermedades por cantidad
            var enfermedadesOrdenadas = estadisticasEnfermedades.OrderByDescending(x => x.Value).ToList();

            // Título del gráfico
            Font fuenteTitulo = new Font("Segoe UI", 14, FontStyle.Bold);
            g.DrawString("DISTRIBUCIÓN DE DIAGNÓSTICOS POR ENFERMEDAD",
                fuenteTitulo, Brushes.Black, new PointF(margenIzq - 100, 10));

            // Colores para las barras
            Color[] colores = new Color[]
            {
                Color.FromArgb(70, 130, 180),   // Azul acero
                Color.FromArgb(220, 20, 60),    // Crimson
                Color.FromArgb(50, 205, 50),    // Verde lima
                Color.FromArgb(255, 140, 0),    // Naranja oscuro
                Color.FromArgb(138, 43, 226),   // Violeta
                Color.FromArgb(0, 191, 255),    // Azul cielo
                Color.FromArgb(255, 99, 71),    // Tomate
                Color.FromArgb(34, 139, 34),    // Verde bosque
                Color.FromArgb(184, 134, 11),   // Dorado oscuro
                Color.FromArgb(218, 112, 214)   // Orquídea
            };

            Font fuenteLabel = new Font("Segoe UI", 9);
            Font fuentePorcentaje = new Font("Segoe UI", 10, FontStyle.Bold);

            int yPos = margenTop;
            int colorIndex = 0;

            foreach (var kvp in enfermedadesOrdenadas)
            {
                double porcentaje = (kvp.Value * 100.0 / totalDiagnosticos);
                int anchoBarra = (int)(anchoMaxBarra * kvp.Value / (double)totalDiagnosticos);

                // Dibuja el nombre de la enfermedad (alineado a la derecha)
                string nombreEnfermedad = kvp.Key;
                if (nombreEnfermedad.Length > 30)
                    nombreEnfermedad = nombreEnfermedad.Substring(0, 27) + "...";

                SizeF tamTexto = g.MeasureString(nombreEnfermedad, fuenteLabel);
                g.DrawString(nombreEnfermedad, fuenteLabel, Brushes.Black,
                    new PointF(margenIzq - tamTexto.Width - 10, yPos + 10));

                // Dibuja la barra
                Color colorBarra = colores[colorIndex % colores.Length];
                Brush brushBarra = new SolidBrush(colorBarra);
                g.FillRectangle(brushBarra, margenIzq, yPos, anchoBarra, altoBarra);

                // Borde de la barra
                g.DrawRectangle(Pens.Gray, margenIzq, yPos, anchoBarra, altoBarra);

                // Dibuja el porcentaje y cantidad dentro de la barra o al lado
                string textoCantidad = $"{kvp.Value} ({porcentaje:F1}%)";
                SizeF tamCantidad = g.MeasureString(textoCantidad, fuentePorcentaje);

                float xTexto = margenIzq + anchoBarra + 10;
                if (anchoBarra > tamCantidad.Width + 20)
                {
                    // Si la barra es suficientemente ancha, pone el texto dentro
                    xTexto = margenIzq + 10;
                    g.DrawString(textoCantidad, fuentePorcentaje, Brushes.White,
                        new PointF(xTexto, yPos + 8));
                }
                else
                {
                    // Si no, pone el texto al lado
                    g.DrawString(textoCantidad, fuentePorcentaje, Brushes.Black,
                        new PointF(xTexto, yPos + 8));
                }

                yPos += altoBarra + espacioEntreBarra;
                colorIndex++;
            }

            // Ajusta el tamaño del panel para scroll si hay muchas enfermedades
            if (yPos > panel.Height)
            {
                panel.AutoScrollMinSize = new Size(0, yPos + margenBottom);
            }
        }

        // Manejador de eventos para el doble clic en el ListView de enfermedades.
        // Muestra los detalles de "Otras Condiciones" si se hace doble clic en esa categoría.
        private void ListViewEnfermedades_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 0)
                return;

            ListViewItem itemSeleccionado = listView.SelectedItems[0];
            string enfermedadSeleccionada = itemSeleccionado.Text;

            // Solo muestra detalles si es "Otras Condiciones"
            if (enfermedadSeleccionada == "Otras Condiciones")
            {
                MostrarDetallesOtrasCondiciones();
            }
        }

        // Muestra un diálogo con los detalles de todos los casos clasificados como "Otras Condiciones"
        private void MostrarDetallesOtrasCondiciones()
        {
            // Obtiene todos los registros que fueron clasificados como "Otras Condiciones"
            List<RegistroMedico> otrasCondiciones = new List<RegistroMedico>();

            if (!sistema.RegistrosPorHospital.ContainsKey(hospital.Id))
                return;

            var registros = sistema.RegistrosPorHospital[hospital.Id];

            foreach (var registro in registros)
            {
                string diagnosticoAUsar = "";

                // Si hay diagnóstico confirmado por médico, usa ese; si no, usa el automático
                if (!string.IsNullOrWhiteSpace(registro.DiagnosticoConfirmado))
                {
                    diagnosticoAUsar = registro.DiagnosticoConfirmado;
                }
                else if (!string.IsNullOrWhiteSpace(registro.Diagnostico))
                {
                    diagnosticoAUsar = registro.Diagnostico;
                }

                if (string.IsNullOrWhiteSpace(diagnosticoAUsar))
                    continue;

                string enfermedad = ExtraerEnfermedadPrincipal(diagnosticoAUsar);

                if (enfermedad == "Otras Condiciones")
                {
                    otrasCondiciones.Add(registro);
                }
            }

            if (otrasCondiciones.Count == 0)
            {
                MessageBox.Show("No hay casos clasificados como 'Otras Condiciones'.", "Sin datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crea un formulario de diálogo para mostrar los detalles
            Form formDetalles = new Form();
            formDetalles.Text = "Detalles de Otras Condiciones";
            formDetalles.Size = new Size(800, 600);
            formDetalles.StartPosition = FormStartPosition.CenterParent;
            formDetalles.BackColor = Color.FromArgb(230, 230, 250);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = $"DETALLES DE OTRAS CONDICIONES ({otrasCondiciones.Count} casos)";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Size = new Size(760, 30);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.ForeColor = Color.FromArgb(70, 130, 180);
            formDetalles.Controls.Add(lblTitulo);

            // ListBox para mostrar los detalles
            ListBox listBoxDetalles = new ListBox();
            listBoxDetalles.Location = new Point(20, 60);
            listBoxDetalles.Size = new Size(760, 450);
            listBoxDetalles.Font = new Font("Consolas", 9);
            listBoxDetalles.BackColor = Color.White;

            // Agrupa diagnósticos similares
            Dictionary<string, int> diagnosticosAgrupados = new Dictionary<string, int>();

            foreach (var registro in otrasCondiciones)
            {
                string diagnostico = !string.IsNullOrWhiteSpace(registro.DiagnosticoConfirmado)
                    ? registro.DiagnosticoConfirmado
                    : registro.Diagnostico;

                // Limpia el diagnóstico
                if (diagnostico.Contains("Descripción del paciente:") ||
                    diagnostico.Contains("Descripcion del paciente:"))
                {
                    int indice = diagnostico.IndexOf("Descripción del paciente:", StringComparison.OrdinalIgnoreCase);
                    if (indice == -1)
                        indice = diagnostico.IndexOf("Descripcion del paciente:", StringComparison.OrdinalIgnoreCase);

                    if (indice >= 0)
                        diagnostico = diagnostico.Substring(0, indice).Trim();
                }

                // Agrupa diagnósticos similares
                if (diagnosticosAgrupados.ContainsKey(diagnostico))
                {
                    diagnosticosAgrupados[diagnostico]++;
                }
                else
                {
                    diagnosticosAgrupados[diagnostico] = 1;
                }
            }

            // Ordena por frecuencia
            var diagnosticosOrdenados = diagnosticosAgrupados.OrderByDescending(x => x.Value);

            listBoxDetalles.Items.Add("═══════════════════════════════════════════════════════════════");
            listBoxDetalles.Items.Add("CONDICIONES NO CATEGORIZADAS - LISTADO DETALLADO");
            listBoxDetalles.Items.Add("═══════════════════════════════════════════════════════════════");
            listBoxDetalles.Items.Add("");

            int contador = 1;
            foreach (var kvp in diagnosticosOrdenados)
            {
                double porcentaje = (kvp.Value * 100.0 / otrasCondiciones.Count);
                listBoxDetalles.Items.Add($"{contador}. [{kvp.Value} caso{(kvp.Value > 1 ? "s" : "")} - {porcentaje:F1}%]");
                listBoxDetalles.Items.Add($"   {kvp.Key}");
                listBoxDetalles.Items.Add("");
                contador++;
            }

            listBoxDetalles.Items.Add("═══════════════════════════════════════════════════════════════");
            listBoxDetalles.Items.Add($"Total: {otrasCondiciones.Count} casos sin categoría específica");

            formDetalles.Controls.Add(listBoxDetalles);

            // Botón cerrar
            Button btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Font = new Font("Segoe UI", 11);
            btnCerrar.Location = new Point(340, 520);
            btnCerrar.Size = new Size(120, 35);
            btnCerrar.BackColor = Color.White;
            btnCerrar.Click += (s, e) => formDetalles.Close();
            formDetalles.Controls.Add(btnCerrar);

            formDetalles.ShowDialog();
        }
    }
}
