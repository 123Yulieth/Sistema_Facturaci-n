using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para listar y administrar clientes.
    /// </summary>
    public class frmListaClientes : Form
    {
        #region Controles

        private DataGridView dgvClientes;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private TextBox txtBuscar;

        #endregion

        #region Colores

        private readonly Color azulFondo = Color.FromArgb(240, 246, 255);
        private readonly Color azulPrincipal = Color.FromArgb(86, 128, 233);
        private readonly Color azulHover = Color.FromArgb(120, 160, 255);

        #endregion

        #region Constructor

        public frmListaClientes()
        {
            InicializarComponentes();
            CargarDatosEjemplo();
        }

        #endregion

        #region Inicialización

        private void InicializarComponentes()
        {
            Text = "Lista de Clientes";
            Size = new Size(950, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = azulFondo;
            Font = new Font("Segoe UI", 10);

            Label lblTitulo = new Label
            {
                Text = "Lista de Clientes",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = azulPrincipal,
                AutoSize = true,
                Location = new Point(350, 10)
            };

            Label lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(20, 50),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(90, 45),
                Width = 300
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            dgvClientes = CrearGrid();

            btnNuevo = CrearBoton("Nuevo", 200);
            btnEditar = CrearBoton("Editar", 400);
            btnEliminar = CrearBoton("Eliminar", 600);

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblBuscar,
                txtBuscar,
                dgvClientes,
                btnNuevo,
                btnEditar,
                btnEliminar
            });
        }

        #endregion

        #region Grid

        private DataGridView CrearGrid()
        {
            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(900, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = azulPrincipal;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;

            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(230, 238, 255);

            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("Documento", "Documento");
            dgv.Columns.Add("Direccion", "Dirección");
            dgv.Columns.Add("Telefono", "Teléfono");
            dgv.Columns.Add("Email", "Email");

            return dgv;
        }

        #endregion

        #region Datos

        private void CargarDatosEjemplo()
        {
            dgvClientes.Rows.Add(
                "Carlos Perez", "123456", "Calle 10",
                "3001234567", "carlos@gmail.com");

            dgvClientes.Rows.Add(
                "Maria Gomez", "789456", "Carrera 20",
                "3019876543", "maria@hotmail.com");

            dgvClientes.Rows.Add(
                "Andres Ruiz", "456123", "Av 30",
                "3024567890", "andres@yahoo.com");
        }

        #endregion

        #region Búsqueda

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim().ToLower();

            foreach (DataGridViewRow fila in dgvClientes.Rows)
            {
                if (fila.IsNewRow) continue;

                string nombre = fila.Cells[0].Value?.ToString().ToLower() ?? string.Empty;
                string documento = fila.Cells[1].Value?.ToString().ToLower() ?? string.Empty;

                fila.Visible =
                    nombre.Contains(texto) ||
                    documento.Contains(texto);
            }
        }

        #endregion

        #region Botones

        private Button CrearBoton(string texto, int x)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(x, 420),
                Size = new Size(130, 45),
                BackColor = azulPrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
            btn.MouseLeave += (s, e) => btn.BackColor = azulPrincipal;

            return btn;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            frmClientes frm = new frmClientes();
            frm.ClienteGuardado += AgregarCliente;
            frm.ShowDialog();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            DataGridViewRow fila = dgvClientes.SelectedRows[0];

            frmClientes frm = new frmClientes(
                fila.Cells[0].Value?.ToString(),
                fila.Cells[1].Value?.ToString(),
                fila.Cells[2].Value?.ToString(),
                fila.Cells[3].Value?.ToString(),
                fila.Cells[4].Value?.ToString());

            frm.ClienteGuardado += (n, d, di, t, e2) =>
            {
                fila.Cells[0].Value = n;
                fila.Cells[1].Value = d;
                fila.Cells[2].Value = di;
                fila.Cells[3].Value = t;
                fila.Cells[4].Value = e2;
            };

            frm.ShowDialog();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                dgvClientes.Rows.Remove(dgvClientes.SelectedRows[0]);
            }
        }

        #endregion

        #region Utilidades

        private bool HayFilaSeleccionada()
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Seleccione un cliente.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void AgregarCliente(
            string nombre,
            string documento,
            string direccion,
            string telefono,
            string email)
        {
            dgvClientes.Rows.Add(
                nombre,
                documento,
                direccion,
                telefono,
                email);
        }

        #endregion
    }
}

