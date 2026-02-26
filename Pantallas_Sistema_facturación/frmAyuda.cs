using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario de ayuda del sistema de facturación.
    /// Muestra contenido de soporte mediante un navegador integrado.
    /// </summary>
    public class frmAyuda : Form
    {
        private Label lblTitulo;
        private WebBrowser webAyuda;
        private Button btnCerrar;

        private Color azulFondo = Color.FromArgb(240, 246, 255);
        private Color azulPrincipal = Color.FromArgb(86, 128, 233);
        private Color azulHover = Color.FromArgb(120, 160, 255);
        private Color grisTexto = Color.FromArgb(60, 60, 60);

        /// <summary>
        /// Constructor del formulario de ayuda.
        /// </summary>
        public frmAyuda()
        {
            InicializarComponentes();
        }

        /// <summary>
        /// Inicializa y configura los controles del formulario.
        /// </summary>
        private void InicializarComponentes()
        {
            this.Text = "Ayuda del Sistema";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = azulFondo;
            this.Font = new Font("Segoe UI", 10);

            lblTitulo = new Label()
            {
                Text = "Centro de Ayuda - Sistema de Facturación",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Location = new Point(20, 15),
                AutoSize = true
            };

            Panel panelWeb = new Panel()
            {
                Location = new Point(20, 60),
                Size = new Size(840, 450),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            webAyuda = new WebBrowser()
            {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };

            webAyuda.Navigate("https://www.microsoft.com/es-co/microsoft-365/support");

            panelWeb.Controls.Add(webAyuda);

            btnCerrar = new Button()
            {
                Text = "Cerrar",
                Size = new Size(140, 38),
                Location = new Point(720, 525),
                BackColor = azulPrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.MouseEnter += (s, e) => btnCerrar.BackColor = azulHover;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.BackColor = azulPrincipal;
            btnCerrar.Click += BtnCerrar_Click;

            Controls.Add(lblTitulo);
            Controls.Add(panelWeb);
            Controls.Add(btnCerrar);
        }

        /// <summary>
        /// Cierra el formulario de ayuda.
        /// </summary>
        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
