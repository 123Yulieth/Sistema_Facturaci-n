using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para listar y administrar facturas.
    /// </summary>
    public class frmListarFacturas : Form
    {
        #region Controles

        public static DataGridView dgvFacturas;

        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;

        #endregion

        #region Constructor

        public frmListarFacturas()
        {
            InicializarComponentes();
            CargarDatosIniciales();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Listado de Facturas";
            ClientSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(232, 241, 255);

            dgvFacturas = CrearGrid();

            btnNuevo = CrearBoton("Nuevo", new Point(220, 480));
            btnEditar = CrearBoton("Editar", new Point(380, 480));
            btnEliminar = CrearBoton("Eliminar", new Point(540, 480));

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[]
            {
                dgvFacturas,
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
                Location = new Point(30, 30),
                Size = new Size(840, 420),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            dgv.Columns.Add("Numero", "N° Factura");
            dgv.Columns.Add("Cliente", "Cliente");
            dgv.Columns.Add("Empleado", "Empleado");
            dgv.Columns.Add("Fecha", "Fecha");
            dgv.Columns.Add("Total", "Total");

            dgv.Columns.Add("Productos", "Productos");
            dgv.Columns.Add("Cantidades", "Cantidades");
            dgv.Columns.Add("Precios", "Precios");
            dgv.Columns.Add("Subtotales", "Subtotales");

            dgv.Columns["Productos"].Visible = false;
            dgv.Columns["Cantidades"].Visible = false;
            dgv.Columns["Precios"].Visible = false;
            dgv.Columns["Subtotales"].Visible = false;

            return dgv;
        }

        #endregion

        #region Datos

        private void CargarDatosIniciales()
        {
            dgvFacturas.Rows.Add(
                "F001",
                "Cliente 1",
                "Empleado 1",
                DateTime.Today.AddDays(-2).ToShortDateString(),
                "1,428.00",
                "Laptop|Mouse",
                "1|2",
                "1200|50",
                "1200|100"
            );

            dgvFacturas.Rows.Add(
                "F002",
                "Cliente 2",
                "Empleado 2",
                DateTime.Today.AddDays(-1).ToShortDateString(),
                "595.00",
                "Teclado|Monitor",
                "1|1",
                "95|500",
                "95|500"
            );

            dgvFacturas.Rows.Add(
                "F003",
                "Cliente 3",
                "Empleado 1",
                DateTime.Today.ToShortDateString(),
                "238.00",
                "Mouse|USB",
                "2|3",
                "50|12",
                "100|36"
            );
        }

        #endregion

        #region Botones

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            frmFacturas frm = new frmFacturas();
            frm.ShowDialog();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            DataGridViewRow row = dgvFacturas.CurrentRow;

            frmFacturas frm = new frmFacturas
            {
                EsEdicion = true,
                NumeroFacturaEditar = row.Cells["Numero"].Value?.ToString(),
                ClienteEditar = row.Cells["Cliente"].Value?.ToString(),
                EmpleadoEditar = row.Cells["Empleado"].Value?.ToString(),
                FechaFacturaEditar = DateTime.Parse(
                    row.Cells["Fecha"].Value?.ToString()
                ),
                TotalEditar = ConvertirDecimalSeguro(
                    row.Cells["Total"].Value?.ToString()
                ),
                ProductosEditar = row.Cells["Productos"].Value?.ToString(),
                CantidadesEditar = row.Cells["Cantidades"].Value?.ToString(),
                PreciosEditar = row.Cells["Precios"].Value?.ToString(),
                SubtotalesEditar = row.Cells["Subtotales"].Value?.ToString()
            };

            frm.ShowDialog();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (!HayFilaSeleccionada())
                return;

            dgvFacturas.Rows.Remove(dgvFacturas.CurrentRow);
        }

        #endregion

        #region Utilidades

        private bool HayFilaSeleccionada()
        {
            if (dgvFacturas.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione una factura.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private decimal ConvertirDecimalSeguro(string valor)
        {
            if (decimal.TryParse(
                valor,
                NumberStyles.Any,
                CultureInfo.CurrentCulture,
                out decimal resultado))
            {
                return resultado;
            }

            return 0;
        }

        public static string ObtenerSiguienteNumero()
        {
            if (dgvFacturas == null || dgvFacturas.Rows.Count == 0)
                return "F001";

            int max = 0;

            foreach (DataGridViewRow row in dgvFacturas.Rows)
            {
                string valor = row.Cells["Numero"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(valor))
                    continue;

                valor = valor.Replace("F", "");

                if (int.TryParse(valor, out int numero) && numero > max)
                    max = numero;
            }

            return "F" + (max + 1).ToString("000");
        }

        private Button CrearBoton(string texto, Point location)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(130, 40),
                Location = location,
                BackColor = Color.FromArgb(86, 128, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        #endregion
    }
}
