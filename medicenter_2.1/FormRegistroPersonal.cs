using System.Drawing;
﻿using System.Windows.Forms;
﻿
﻿namespace MEDICENTER
﻿{
﻿    // Formulario informativo que indica cómo se debe registrar el personal hospitalario.
﻿    // Advierte que la funcionalidad de registro directo está deshabilitada y redirige al usuario
﻿    // a la opción de registro a través del menú del Administrador General.
﻿    public partial class FormRegistroPersonal : Form
﻿    {
﻿        private Sistema sistema; // Instancia de la clase Sistema (aunque no se usa directamente en este formulario informativo).
﻿
﻿        // Constructor del formulario.
﻿        public FormRegistroPersonal(Sistema sistemaParam)
﻿        {
﻿            sistema = sistemaParam; // Asigna la instancia del sistema.
﻿            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
﻿        }
﻿
﻿        // Método que inicializa programáticamente todos los componentes visuales del formulario.
﻿        private void InitializeComponent()
﻿        {
﻿            // Configuración básica del formulario.
﻿            this.ClientSize = new Size(800, 700); // Tamaño del formulario.
﻿            this.Text = "Registro de Personal (Administrador General)"; // Título de la ventana.
﻿            this.StartPosition = FormStartPosition.CenterScreen; // Posiciona el formulario en el centro.
﻿            this.BackColor = Color.FromArgb(230, 230, 250); // Color de fondo.
﻿
﻿            // Título principal del formulario.
﻿            Label lblTitulo = new Label();
﻿            lblTitulo.Text = "Registro de Personal Hospitalario";
﻿            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
﻿            lblTitulo.Location = new Point(200, 20);
﻿            lblTitulo.Size = new Size(400, 40);
﻿            this.Controls.Add(lblTitulo);
﻿
﻿            // Mensaje informativo explicando la forma correcta de registrar personal.
﻿            Label lblInfo = new Label();
﻿            lblInfo.Text =
﻿                "Esta funcionalidad solo puede ser usada por el ADMINISTRADOR GENERAL.\n" +
﻿                "Primero inicie sesión como Personal Hospitalario con la cuenta global,\n" +
﻿                "y luego use la opción 'Registrar Nuevo Personal' en el menú de administración.";
﻿            lblInfo.Font = new Font("Segoe UI", 11);
﻿            lblInfo.Location = new Point(100, 100);
﻿            lblInfo.Size = new Size(600, 100);
﻿            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
﻿            this.Controls.Add(lblInfo);
﻿
﻿            // Mensaje que muestra las credenciales por defecto del Administrador General.
﻿            Label lblCredenciales = new Label();
﻿            lblCredenciales.Text =
﻿                "Credencial de ADMINISTRADOR GENERAL por defecto:\n" +
﻿                "ID: ADMIN001\nPassword: admin123";
﻿            lblCredenciales.Font = new Font("Consolas", 11, FontStyle.Bold); // Fuente monoespaciada para credenciales.
﻿            lblCredenciales.Location = new Point(250, 230);
﻿            lblCredenciales.Size = new Size(300, 80);
﻿            lblCredenciales.TextAlign = ContentAlignment.MiddleCenter;
﻿            lblCredenciales.ForeColor = Color.DarkBlue; // Color distintivo para las credenciales.
﻿            this.Controls.Add(lblCredenciales);
﻿
﻿            // Botón "Entendido" para cerrar el formulario.
﻿            Button btnCerrar = new Button();
﻿            btnCerrar.Text = "Entendido";
﻿            btnCerrar.Font = new Font("Segoe UI", 12);
﻿            btnCerrar.Location = new Point(320, 360);
﻿            btnCerrar.Size = new Size(160, 50);
﻿            btnCerrar.BackColor = Color.White;
﻿            btnCerrar.Click += (s, e) => this.Close(); // Cierra el formulario al hacer clic.
﻿            this.Controls.Add(btnCerrar);
﻿        }
﻿    }
﻿}
﻿