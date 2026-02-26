using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario que muestra información general del sistema de facturación.
    /// Incluye datos del producto, componentes y utilidades del sistema.
    /// </summary>
    public class frmAcercaDe : Form
    {
        private Label lblTitulo;
        private GroupBox gbDetalles;
        private Label lblProducto, lblVersion, lblFramework, lblAutor, lblFecha;
        private ListBox lstComponentes;
        private Label lblLegal;
        private Button btnCopiar, btnInfoSistema, btnDxDiag, btnAceptar;

        private Color azulFondo = Color.FromArgb(245, 248, 255);
        private Color azulPrincipal = Color.FromArgb(95, 135, 255);
        private Color azulHover = Color.FromArgb(130, 165, 255);
        private Color azulClaro = Color.FromArgb(230, 238, 255);
        private Font fuenteGeneral = new Font("Segoe UI", 10);

        /// <summary>
        /// Constructor del formulario Acerca de.
        /// </summary>
        public frmAcercaDe()
        {
            InicializarComponentes();
        }

        /// <summary>
        /// Inicializa y configura todos los controles del formulario.
        /// </summary>
        private void InicializarComponentes()
        {
            this.Text = "Acerca del Sistema";
            this.Size = new Size(820, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = azulFondo;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = fuenteGeneral;

            lblTitulo = new Label()
            {
                Text = "Acerca del Sistema de Facturación",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Location = new Point(20, 15),
                AutoSize = true
            };

            gbDetalles = new GroupBox()
            {
                Text = "Detalles del producto",
                Location = new Point(20, 60),
                Size = new Size(470, 190),
                ForeColor = azulPrincipal,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblProducto = CrearLabel("Producto: Sistema de Facturación", 35);
            lblVersion = CrearLabel("Versión: 1.0.0", 65);
            lblFramework = CrearLabel(".NET Framework: 4.8", 95);
            lblAutor = CrearLabel("Autor: Yulieth Vidales", 125);
            lblFecha = CrearLabel("Año: 2025", 155);

            gbDetalles.Controls.AddRange(new Control[]
            {
                lblProducto, lblVersion, lblFramework, lblAutor, lblFecha
            });

            Label lblComponentes = new Label()
            {
                Text = "Componentes del sistema",
                Location = new Point(20, 265),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = azulPrincipal
            };

            lstComponentes = new ListBox()
            {
                Location = new Point(20, 295),
                Size = new Size(470, 140),
                Font = fuenteGeneral,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lstComponentes.Items.AddRange(new object[]
            {
                "Clientes",
                "Productos",
                "Categorías",
                "Facturación",
                "Empleados y Roles",
                "Informes",
                "Seguridad"
            });

            lblLegal = new Label()
            {
                Text = "© 2025 Yulieth Vidales. Todos los derechos reservados.\n" +
                       "Proyecto académico - Uso no comercial.",
                Location = new Point(20, 445),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            btnCopiar = CrearBoton("Copiar información", 80);
            btnCopiar.Click += BtnCopiar_Click;

            btnInfoSistema = CrearBoton("Información del sistema", 130);
            btnInfoSistema.Click += BtnInfoSistema_Click;

            btnDxDiag = CrearBoton("Diagnóstico DirectX", 180);
            btnDxDiag.Click += BtnDxDiag_Click;

            btnAceptar = CrearBoton("Aceptar", 440);
            btnAceptar.Location = new Point(600, 445);
            btnAceptar.Click += (s, e) => this.Close();

            Controls.Add(lblTitulo);
            Controls.Add(gbDetalles);
            Controls.Add(lblComponentes);
            Controls.Add(lstComponentes);
            Controls.Add(lblLegal);
            Controls.Add(btnCopiar);
            Controls.Add(btnInfoSistema);
            Controls.Add(btnDxDiag);
            Controls.Add(btnAceptar);
        }

        /// <summary>
        /// Crea una etiqueta con formato estándar.
        /// </summary>
        private Label CrearLabel(string texto, int y)
        {
            return new Label()
            {
                Text = texto,
                Location = new Point(15, y),
                AutoSize = true,
                Font = fuenteGeneral,
                ForeColor = Color.FromArgb(70, 70, 70)
            };
        }

        /// <summary>
        /// Crea un botón con estilo azul estándar del sistema.
        /// </summary>
        private Button CrearBoton(string texto, int y)
        {
            Button btn = new Button()
            {
                Text = texto,
                Size = new Size(220, 40),
                Location = new Point(560, y),
                BackColor = azulPrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = azulHover;
            btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
            btn.MouseLeave += (s, e) => btn.BackColor = azulPrincipal;

            return btn;
        }

        /// <summary>
        /// Copia la información del sistema al portapapeles.
        /// </summary>
        private void BtnCopiar_Click(object sender, EventArgs e)
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("Sistema de Facturación");
            info.AppendLine("Versión: 1.0.0");
            info.AppendLine(".NET Framework: 4.8");
            info.AppendLine("Autor: Yulieth Vidales");
            info.AppendLine("Año: 2025");
            info.AppendLine("Módulos: Clientes, Productos, Categorías, Facturación, Empleados y Roles, Informes, Seguridad");

            Clipboard.SetText(info.ToString());

            MessageBox.Show("Información copiada al portapapeles 📋",
                "Listo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnInfoSistema_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("msinfo32");
            }
            catch
            {
                MessageBox.Show("No se pudo abrir la información del sistema.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnDxDiag_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("dxdiag");
            }
            catch
            {
                MessageBox.Show("No se pudo abrir el diagnóstico DirectX.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
