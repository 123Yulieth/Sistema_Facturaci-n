using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario de listado y mantenimiento de productos.
    /// </summary>
    public class frmListaProductos : Form
    {
        #region Controles

        private DataGridView dgvProductos;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Label lblTitulo;
        private Label lblBuscar;
        private TextBox txtBuscar;

        private int filaEditar = -1;

        #endregion

        #region Constructor

        public frmListaProductos()
        {
            InicializarComponentes();
            CargarProductosEjemplo();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Color azulFondo = Color.FromArgb(232, 241, 255);
            Color azulTitulo = Color.FromArgb(45, 95, 180);
            Color azulBoton = Color.FromArgb(86, 128, 255);

            Text = "Lista de Productos";
            Size = new Size(1100, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = azulFondo;

            lblTitulo = new Label
            {
                Text = "Listado de Productos",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = azulTitulo,
                AutoSize = true,
                Location = new Point(380, 20)
            };

            lblBuscar = new Label
            {
                Text = "Buscar:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 75)
            };

            txtBuscar = new TextBox
            {
                Location = new Point(110, 72),
                Width = 300,
                Font = new Font("Segoe UI", 11)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            dgvProductos = CrearGrid();

            btnNuevo = CrearBoton("Nuevo", new Point(350, 480), azulBoton);
            btnEditar = CrearBoton("Editar", new Point(500, 480), azulBoton);
            btnEliminar = CrearBoton("Eliminar", new Point(650, 480), azulBoton);

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblBuscar,
                txtBuscar,
                dgvProductos,
                btnNuevo,
                btnEditar,
                btnEliminar
            });
        }

        private DataGridView CrearGrid()
        {
            DataGridView dgv = new DataGridView
            {
                Location = new Point(40, 110),
                Size = new Size(1000, 340),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };

            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 11, FontStyle.Bold);

            dgv.Columns.Add("Nombre", "Nombre");
            dgv.Columns.Add("Categoria", "Categoría");
            dgv.Columns.Add("Codigo", "Código");
            dgv.Columns.Add("PrecioCompra", "Precio Compra");
            dgv.Columns.Add("PrecioVenta", "Precio Venta");
            dgv.Columns.Add("Stock", "Stock");

            return dgv;
        }

        #endregion

        #region Eventos Buscar

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (fila.IsNewRow) continue;

                bool visible = false;

                foreach (DataGridViewCell celda in fila.Cells)
                {
                    if (celda.Value != null &&
                        celda.Value.ToString().ToLower().Contains(filtro))
                    {
                        visible = true;
                        break;
                    }
                }

                fila.Visible = visible;
            }
        }

        #endregion

        #region Botones

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            frmProductos frm = new frmProductos();
            frm.ProductoGuardado += AgregarProducto;
            frm.ShowDialog();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            filaEditar = dgvProductos.SelectedRows[0].Index;

            DataGridViewRow fila = dgvProductos.Rows[filaEditar];

            frmProductos frm = new frmProductos();

            frm.CargarProducto(
                fila.Cells["Nombre"].Value?.ToString(),
                fila.Cells["Categoria"].Value?.ToString(),
                fila.Cells["Codigo"].Value?.ToString(),
                fila.Cells["PrecioCompra"].Value?.ToString(),
                fila.Cells["PrecioVenta"].Value?.ToString(),
                fila.Cells["Stock"].Value?.ToString()
            );

            frm.ProductoGuardado += ActualizarProducto;
            frm.ShowDialog();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            DialogResult r = MessageBox.Show(
                "¿Desea eliminar el producto seleccionado?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                dgvProductos.Rows.RemoveAt(
                    dgvProductos.SelectedRows[0].Index
                );
            }
        }

        #endregion

        #region CRUD Productos

        private void AgregarProducto(string nombre, string categoria, string codigo,
                                     string rutaImagen,
                                     decimal precioCompra,
                                     decimal precioVenta,
                                     int stock,
                                     string detalles)
        {
            dgvProductos.Rows.Add(
                nombre,
                categoria,
                codigo,
                precioCompra.ToString("N2"),
                precioVenta.ToString("N2"),
                stock
            );
        }

        private void ActualizarProducto(string nombre, string categoria, string codigo,
                                        string rutaImagen,
                                        decimal precioCompra,
                                        decimal precioVenta,
                                        int stock,
                                        string detalles)
        {
            if (filaEditar < 0 || filaEditar >= dgvProductos.Rows.Count)
                return;

            DataGridViewRow fila = dgvProductos.Rows[filaEditar];

            fila.Cells["Nombre"].Value = nombre;
            fila.Cells["Categoria"].Value = categoria;
            fila.Cells["Codigo"].Value = codigo;
            fila.Cells["PrecioCompra"].Value = precioCompra.ToString("N2");
            fila.Cells["PrecioVenta"].Value = precioVenta.ToString("N2");
            fila.Cells["Stock"].Value = stock;
        }

        #endregion

        #region Utilidades

        private bool HayFilaSeleccionada()
        {
            if (dgvProductos.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos registrados.");
                return false;
            }

            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto.");
                return false;
            }

            return true;
        }

        private Button CrearBoton(string texto, Point location, Color color)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(120, 45),
                Location = location,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        #endregion

        #region Datos Ejemplo

        private void CargarProductosEjemplo()
        {
            dgvProductos.Rows.Add(
                "Laptop HP",
                "Electrónica",
                "HP123",
                "2000000",
                "2500000",
                5
            );

            dgvProductos.Rows.Add(
                "Camisa Azul",
                "Ropa",
                "RP456",
                "50000",
                "80000",
                15
            );
        }

        #endregion
    }
}
