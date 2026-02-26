using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    public class frmPrincipal : Form
    {
        #region Controles

        private MenuStrip menuStrip;
        private Panel panelHome;
        private Label lblTitulo, lblDescripcion, lblFeatures;
        private Button btnSalir;

        #endregion

        #region Colores

        private readonly Color azulFondo = Color.FromArgb(232, 241, 255);
        private readonly Color azulTitulo = Color.FromArgb(45, 95, 180);
        private readonly Color azulTexto = Color.Black;
        private readonly Color azulBoton = Color.FromArgb(86, 128, 255);
        private readonly Color azulHover = Color.FromArgb(130, 180, 245);

        #endregion

        #region Constructor

        public frmPrincipal()
        {
            InicializarFormulario();
            InicializarMenu();
            InicializarPanelHome();
        }

        #endregion

        #region Inicialización Formulario

        private void InicializarFormulario()
        {
            Text = "Sistema de Facturación";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = azulFondo;
        }

        #endregion

        #region Menú

        private void InicializarMenu()
        {
            menuStrip = new MenuStrip
            {
                BackColor = Color.White,
                ForeColor = azulTexto,
                Font = new Font("Segoe UI", 11),
                Padding = new Padding(10, 6, 10, 6),
                Renderer = new MenuPastelRenderer(azulHover)
            };

            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            menuStrip.Items.AddRange(new ToolStripItem[]
            {
                CrearMenuInicio(),
                CrearMenuTablas(),
                CrearMenuFacturacion(),
                CrearMenuSeguridad(),
                CrearMenuAyuda()
            });
        }

        private ToolStripMenuItem CrearMenuInicio()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Inicio");
            menu.Click += (s, e) => CentrarPanel();
            return menu;
        }

        private ToolStripMenuItem CrearMenuTablas()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Tablas");
            menu.DropDownItems.Add("Clientes", null, (s, e) => AbrirDialogo(new frmListaClientes()));
            menu.DropDownItems.Add("Productos", null, (s, e) => AbrirDialogo(new frmListaProductos()));
            menu.DropDownItems.Add("Categorías", null, (s, e) => AbrirDialogo(new frmListarCategoria()));
            return menu;
        }

        private ToolStripMenuItem CrearMenuFacturacion()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Facturación");
            menu.DropDownItems.Add("Facturas", null, (s, e) => AbrirDialogo(new frmListarFacturas()));
            menu.DropDownItems.Add("Informes", null, (s, e) => AbrirDialogo(new frmInformes()));
            return menu;
        }

        private ToolStripMenuItem CrearMenuSeguridad()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Seguridad");
            menu.DropDownItems.Add("Empleados", null, (s, e) => AbrirDialogo(new frmListarEmpleados()));
            menu.DropDownItems.Add("Roles", null, (s, e) => AbrirDialogo(new frmRolEmpleados()));
            menu.DropDownItems.Add("Administración", null, (s, e) => AbrirDialogo(new frmAdminSeguridad()));
            return menu;
        }

        private ToolStripMenuItem CrearMenuAyuda()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("Ayuda");
            menu.DropDownItems.Add("Ayuda", null, (s, e) => AbrirDialogo(new frmAyuda()));
            menu.DropDownItems.Add("Acerca de", null, (s, e) => AbrirDialogo(new frmAcercaDe()));
            return menu;
        }

        #endregion

        #region Panel Home

        private void InicializarPanelHome()
        {
            panelHome = CrearCard(new Size(900, 420));

            lblTitulo = new Label
            {
                Text = "Bienvenido al Sistema de Facturación",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = azulTitulo,
                AutoSize = true,
                Location = new Point(40, 30)
            };

            lblDescripcion = new Label
            {
                Text = "Gestiona tu negocio de forma rápida, clara y profesional.",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(40, 80)
            };

            lblFeatures = new Label
            {
                Text =
                "• Facturación rápida y ordenada\n" +
                "• Control de clientes y empleados\n" +
                "• Gestión de productos y categorías\n" +
                "• Informes de ventas\n" +
                "• Seguridad y roles de usuario",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Location = new Point(40, 130)
            };

            btnSalir = CrearBoton("Cerrar sesión", new Point(40, 300));
            btnSalir.Click += BtnSalir_Click;

            panelHome.Controls.AddRange(new Control[]
            {
                lblTitulo, lblDescripcion, lblFeatures, btnSalir
            });

            Controls.Add(panelHome);

            Resize += (s, e) => CentrarPanel();
            CentrarPanel();
        }

        #endregion

        #region Métodos Auxiliares

        private void AbrirDialogo(Form frm)
        {
            using (frm)
            {
                frm.ShowDialog();
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
                "¿Desea cerrar la sesión?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                Hide();
                new frmLogin().Show();
            }
        }

        private void CentrarPanel()
        {
            panelHome.Left = (ClientSize.Width - panelHome.Width) / 2;
            panelHome.Top = 120;
        }

        private Panel CrearCard(Size size)
        {
            Panel panel = new Panel
            {
                Size = size,
                BackColor = Color.White
            };

            panel.Paint += (s, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    int r = 28;
                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(panel.Width - r, 0, r, r, 270, 90);
                    path.AddArc(panel.Width - r, panel.Height - r, r, r, 0, 90);
                    path.AddArc(0, panel.Height - r, r, r, 90, 90);
                    path.CloseFigure();
                    panel.Region = new Region(path);
                }
            };

            return panel;
        }

        private Button CrearBoton(string texto, Point location)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(160, 42),
                Location = location,
                BackColor = azulBoton,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        #endregion
    }

    public class MenuPastelRenderer : ToolStripProfessionalRenderer
    {
        private readonly Color hover;

        public MenuPastelRenderer(Color hoverColor)
        {
            hover = hoverColor;
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(
                e.Item.Selected ? hover : Color.White))
            {
                e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.AffectedBounds);
        }
    }
}
