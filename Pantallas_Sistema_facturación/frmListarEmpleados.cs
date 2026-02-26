using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario de listado y mantenimiento de empleados.
    /// </summary>
    public class frmListarEmpleados : Form
    {
        #region Controles

        public static DataGridView dgvEmpleados;

        private TextBox txtBuscar;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Panel panelCard;

        #endregion

        #region Colores

        private readonly Color azulFondo = Color.FromArgb(232, 241, 255);
        private readonly Color azulPrincipal = Color.FromArgb(86, 128, 255);
        private readonly Color azulHover = Color.FromArgb(115, 150, 255);

        #endregion

        #region Constructor

        public frmListarEmpleados()
        {
            InicializarComponentes();
            CargarEmpleadosIniciales();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Listado de Empleados";
            ClientSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = azulFondo;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label lblTitulo = new Label
            {
                Text = "Empleados Registrados",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Location = new Point(30, 20),
                AutoSize = true
            };
            Controls.Add(lblTitulo);

            panelCard = new Panel
            {
                Size = new Size(840, 430),
                Location = new Point(30, 60),
                BackColor = Color.White
            };
            panelCard.Paint += PanelCard_Paint;
            Controls.Add(panelCard);

            Label lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(25, 20),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(90, 16),
                Width = 260,
                Font = new Font("Segoe UI", 10)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            dgvEmpleados = CrearGrid();

            btnNuevo = CrearBoton("Nuevo", new Point(180, 350));
            btnEditar = CrearBoton("Editar", new Point(350, 350));
            btnEliminar = CrearBoton("Eliminar", new Point(520, 350));

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            panelCard.Controls.AddRange(new Control[]
            {
                lblBuscar,
                txtBuscar,
                dgvEmpleados,
                btnNuevo,
                btnEditar,
                btnEliminar
            });
        }

        private DataGridView CrearGrid()
        {
            DataGridView dgv = new DataGridView
            {
                Location = new Point(25, 55),
                Size = new Size(790, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = azulPrincipal;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 224, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("Documento", "Documento");
            dgv.Columns.Add("Telefono", "Teléfono");
            dgv.Columns.Add("Rol", "Rol");
            dgv.Columns.Add("Fecha", "Fecha Ingreso");

            return dgv;
        }

        #endregion

        #region Datos Iniciales

        private void CargarEmpleadosIniciales()
        {
            dgvEmpleados.Rows.Add(
                "Carlos Pérez", "123456", "3001234567", "Administrador",
                DateTime.Today.AddYears(-2).ToShortDateString());

            dgvEmpleados.Rows.Add(
                "Laura Gómez", "654321", "3109876543", "Vendedor",
                DateTime.Today.AddYears(-1).ToShortDateString());

            dgvEmpleados.Rows.Add(
                "Andrés Ruiz", "112233", "3204567890", "Cajero",
                DateTime.Today.AddMonths(-6).ToShortDateString());
        }

        #endregion

        #region Botones

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            frmEmpleados frm = new frmEmpleados();
            frm.ShowDialog();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
                return;

            DataGridViewRow fila = dgvEmpleados.CurrentRow;

            frmEmpleados frm = new frmEmpleados
            {
                EsEdicion = true,
                FilaEditar = fila
            };

            frm.CargarDatos(
                fila.Cells["Nombre"].Value?.ToString(),
                fila.Cells["Documento"].Value?.ToString(),
                fila.Cells["Telefono"].Value?.ToString(),
                fila.Cells["Rol"].Value?.ToString(),
                DateTime.Parse(fila.Cells["Fecha"].Value.ToString())
            );

            frm.ShowDialog();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!HaySeleccion())
                return;

            dgvEmpleados.Rows.Remove(dgvEmpleados.CurrentRow);
        }

        #endregion

        #region Buscar

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            foreach (DataGridViewRow row in dgvEmpleados.Rows)
            {
                if (row.IsNewRow) continue;

                string nombre = row.Cells["Nombre"].Value?.ToString().ToLower() ?? "";
                row.Visible = nombre.Contains(filtro);
            }
        }

        #endregion

        #region Utilidades

        private bool HaySeleccion()
        {
            if (dgvEmpleados.Rows.Count == 0)
            {
                MessageBox.Show("No hay empleados registrados.");
                return false;
            }

            if (dgvEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado.");
                return false;
            }

            return true;
        }

        private Button CrearBoton(string texto, Point location)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(130, 40),
                Location = location,
                BackColor = azulPrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
            btn.MouseLeave += (s, e) => btn.BackColor = azulPrincipal;

            return btn;
        }

        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            int r = 28;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(panelCard.Width - r, 0, r, r, 270, 90);
            path.AddArc(panelCard.Width - r, panelCard.Height - r, r, r, 0, 90);
            path.AddArc(0, panelCard.Height - r, r, r, 90, 90);
            path.CloseFigure();
            panelCard.Region = new Region(path);
        }

        #endregion
    }
}

