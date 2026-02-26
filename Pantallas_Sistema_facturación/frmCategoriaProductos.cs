using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para la gestión de categorías de productos.
    /// Permite crear y editar categorías.
    /// </summary>
    public partial class frmCategoriaProductos : Form
    {
        private Panel panelCard;
        private Label lblTitulo;
        private Label lblNombre;
        private Label lblDescripcion;

        private TextBox txtNombre;
        private TextBox txtDescripcion;

        private Button btnGuardar;
        private Button btnCancelar;

        private ErrorProvider errorProvider1;

        /// <summary>
        /// Modelo de categoría.
        /// </summary>
        public class Categoria
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }

        /// <summary>
        /// Categoría resultante luego de guardar.
        /// </summary>
        public Categoria CategoriaGuardada { get; private set; }

        /// <summary>
        /// Indica si el formulario está en modo edición.
        /// </summary>
        public bool EsEdicion { get; set; } = false;

        /// <summary>
        /// Datos usados al editar una categoría.
        /// </summary>
        public string NombreCategoria { get; set; }
        public string DescripcionCategoria { get; set; }

        /// <summary>
        /// Constructor del formulario.
        /// </summary>
        public frmCategoriaProductos()
        {
            InicializarComponentes();
            this.Load += FrmCategoriaProductos_Load;
        }

        /// <summary>
        /// Inicializa y configura los controles del formulario.
        /// </summary>
        private void InicializarComponentes()
        {
            this.Text = "Gestión de Categorías";
            this.Size = new Size(520, 460);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(232, 241, 255);

            panelCard = new Panel()
            {
                Size = new Size(460, 380),
                Location = new Point(20, 20),
                BackColor = Color.White
            };
            panelCard.Paint += PanelCard_Paint;

            lblTitulo = new Label()
            {
                Text = "Registro de Categoría",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 95, 180),
                AutoSize = true,
                Location = new Point(110, 25)
            };

            lblNombre = new Label()
            {
                Text = "Nombre de la categoría",
                Location = new Point(40, 90),
                AutoSize = true
            };

            txtNombre = new TextBox()
            {
                Size = new Size(360, 28),
                Location = new Point(40, 115),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };

            lblDescripcion = new Label()
            {
                Text = "Descripción",
                Location = new Point(40, 155),
                AutoSize = true
            };

            txtDescripcion = new TextBox()
            {
                Size = new Size(360, 80),
                Location = new Point(40, 180),
                Multiline = true,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };

            btnGuardar = new Button()
            {
                Text = "Guardar",
                Size = new Size(130, 38),
                Location = new Point(90, 290),
                BackColor = Color.FromArgb(86, 128, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button()
            {
                Text = "Cancelar",
                Size = new Size(130, 38),
                Location = new Point(240, 290),
                BackColor = Color.FromArgb(210, 224, 255),
                ForeColor = Color.FromArgb(45, 95, 180),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;

            errorProvider1 = new ErrorProvider
            {
                ContainerControl = this
            };

            panelCard.Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblNombre,
                txtNombre,
                lblDescripcion,
                txtDescripcion,
                btnGuardar,
                btnCancelar
            });

            Controls.Add(panelCard);
        }

        /// <summary>
        /// Carga los datos cuando el formulario está en modo edición.
        /// </summary>
        private void FrmCategoriaProductos_Load(object sender, EventArgs e)
        {
            if (EsEdicion)
            {
                lblTitulo.Text = "Editar Categoría";
                txtNombre.Text = NombreCategoria;
                txtDescripcion.Text = DescripcionCategoria;
            }
        }

        /// <summary>
        /// Dibuja el panel con bordes redondeados.
        /// </summary>
        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            int radius = 25;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panelCard.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panelCard.Width - radius, panelCard.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panelCard.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            panelCard.Region = new Region(path);
        }

        /// <summary>
        /// Valida y guarda la categoría.
        /// </summary>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Ingrese el nombre de la categoría");
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                errorProvider1.SetError(txtDescripcion, "Ingrese una descripción");
                txtDescripcion.Focus();
                return;
            }

            CategoriaGuardada = new Categoria
            {
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim()
            };

            MessageBox.Show(
                "Categoría guardada correctamente",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cierra el formulario sin guardar.
        /// </summary>
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
