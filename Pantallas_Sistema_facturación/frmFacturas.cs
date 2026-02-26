using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para creación y edición de facturas.
    /// </summary>
    public class frmFacturas : Form
    {
        #region Controles

        private TextBox txtNumeroFactura;
        private TextBox txtDescuento;
        private TextBox txtIVA;
        private TextBox txtTotal;
        private ComboBox cmbCliente;
        private ComboBox cmbEmpleado;
        private DateTimePicker dtpFechaRegistro;
        private DateTimePicker dtpFechaFactura;
        private DataGridView dgvDetalle;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider1;
        private Panel panelCardDatos;
        private Panel panelCardDetalle;

        #endregion

        #region Propiedades edición

        public bool EsEdicion { get; set; } = false;

        public string NumeroFacturaEditar { get; set; }
        public string ClienteEditar { get; set; }
        public string EmpleadoEditar { get; set; }
        public DateTime FechaFacturaEditar { get; set; }
        public decimal TotalEditar { get; set; }

        public string ProductosEditar { get; set; }
        public string CantidadesEditar { get; set; }
        public string PreciosEditar { get; set; }
        public string SubtotalesEditar { get; set; }

        #endregion

        #region Constructor

        public frmFacturas()
        {
            InicializarComponentes();
            CargarDatos();
        }

        #endregion

        #region Eventos ciclo de vida

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (EsEdicion)
            {
                txtNumeroFactura.Text = NumeroFacturaEditar;
                cmbCliente.Text = ClienteEditar;
                cmbEmpleado.Text = EmpleadoEditar;
                dtpFechaFactura.Value = FechaFacturaEditar;
                txtTotal.Text = TotalEditar.ToString("N2");
                txtNumeroFactura.Enabled = false;

                CargarDetalleEditar();
            }
            else
            {
                txtNumeroFactura.Text = frmListarFacturas.ObtenerSiguienteNumero();
            }
        }

        #endregion

        #region Guardar factura

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            StringBuilder productos = new StringBuilder();
            StringBuilder cantidades = new StringBuilder();
            StringBuilder precios = new StringBuilder();
            StringBuilder subtotales = new StringBuilder();

            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (row.IsNewRow) continue;

                productos.Append(row.Cells[0].Value).Append("|");
                precios.Append(row.Cells[1].Value).Append("|");
                cantidades.Append(row.Cells[2].Value).Append("|");
                subtotales.Append(row.Cells[3].Value).Append("|");
            }

            QuitarUltimoSeparador(productos, cantidades, precios, subtotales);

            if (EsEdicion)
            {
                foreach (DataGridViewRow row in frmListarFacturas.dgvFacturas.Rows)
                {
                    if (row.Cells["Numero"].Value.ToString() == txtNumeroFactura.Text)
                    {
                        row.Cells["Cliente"].Value = cmbCliente.Text;
                        row.Cells["Empleado"].Value = cmbEmpleado.Text;
                        row.Cells["Fecha"].Value = dtpFechaFactura.Value.ToShortDateString();
                        row.Cells["Total"].Value = txtTotal.Text;
                        row.Cells["Productos"].Value = productos.ToString();
                        row.Cells["Cantidades"].Value = cantidades.ToString();
                        row.Cells["Precios"].Value = precios.ToString();
                        row.Cells["Subtotales"].Value = subtotales.ToString();
                        break;
                    }
                }
            }
            else
            {
                frmListarFacturas.dgvFacturas.Rows.Add(
                    txtNumeroFactura.Text,
                    cmbCliente.Text,
                    cmbEmpleado.Text,
                    dtpFechaFactura.Value.ToShortDateString(),
                    txtTotal.Text,
                    productos.ToString(),
                    cantidades.ToString(),
                    precios.ToString(),
                    subtotales.ToString()
                );
            }

            MessageBox.Show(
                "Factura guardada correctamente",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }

        #endregion

        #region Cálculos

        private void CalcularFila(int fila)
        {
            if (fila < 0) return;

            if (decimal.TryParse(Convert.ToString(dgvDetalle.Rows[fila].Cells[1].Value), out decimal precio) &&
                decimal.TryParse(Convert.ToString(dgvDetalle.Rows[fila].Cells[2].Value), out decimal cantidad))
            {
                dgvDetalle.Rows[fila].Cells[3].Value = (precio * cantidad).ToString("N2");
            }

            CalcularTotal();
        }

        private void CalcularTotal()
        {
            decimal suma = 0;

            foreach (DataGridViewRow row in dgvDetalle.Rows)
            {
                if (row.IsNewRow) continue;

                if (decimal.TryParse(Convert.ToString(row.Cells[3].Value), out decimal sub))
                    suma += sub;
            }

            decimal.TryParse(txtDescuento.Text, out decimal descuento);
            decimal.TryParse(txtIVA.Text, out decimal iva);

            decimal total = suma - (suma * descuento / 100);
            total += total * iva / 100;

            txtTotal.Text = total.ToString("N2");
        }

        #endregion

        #region Carga edición

        private void CargarDetalleEditar()
        {
            if (string.IsNullOrWhiteSpace(ProductosEditar))
                return;

            string[] productos = ProductosEditar.Split('|');
            string[] cantidades = CantidadesEditar.Split('|');
            string[] precios = PreciosEditar.Split('|');
            string[] subtotales = SubtotalesEditar.Split('|');

            dgvDetalle.Rows.Clear();

            for (int i = 0; i < productos.Length; i++)
            {
                dgvDetalle.Rows.Add(
                    productos[i],
                    precios[i],
                    cantidades[i],
                    subtotales[i]
                );
            }
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Facturación";
            ClientSize = new Size(960, 760);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(232, 241, 255);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            AutoScroll = true;

            errorProvider1 = new ErrorProvider { ContainerControl = this };

            panelCardDatos = CrearCard(new Size(900, 240), new Point(30, 60));
            panelCardDetalle = CrearCard(new Size(900, 340), new Point(30, 320));

            txtNumeroFactura = CrearTextBox(25, 45, 180);
            cmbCliente = CrearComboBox(240, 45, 240);
            cmbEmpleado = CrearComboBox(500, 45, 240);
            txtDescuento = CrearTextBox(440, 122, 90);
            txtIVA = CrearTextBox(550, 122, 90);
            txtTotal = CrearTextBox(660, 122, 120);
            txtTotal.ReadOnly = true;

            dtpFechaRegistro = new DateTimePicker { Location = new Point(25, 122), Width = 180 };
            dtpFechaFactura = new DateTimePicker { Location = new Point(240, 122), Width = 180 };

            panelCardDatos.Controls.AddRange(new Control[]
            {
                CrearLabel("Nro Factura",25,25), txtNumeroFactura,
                CrearLabel("Cliente",240,25), cmbCliente,
                CrearLabel("Empleado",500,25), cmbEmpleado,
                CrearLabel("Fecha Registro",25,100), dtpFechaRegistro,
                CrearLabel("Fecha Factura",240,100), dtpFechaFactura,
                CrearLabel("Descuento (%)",440,100), txtDescuento,
                CrearLabel("IVA (%)",550,100), txtIVA,
                CrearLabel("Total",660,100), txtTotal
            });

            dgvDetalle = new DataGridView
            {
                Location = new Point(20, 45),
                Size = new Size(860, 260),
                AllowUserToAddRows = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            dgvDetalle.Columns.Add("Producto", "Producto");
            dgvDetalle.Columns.Add("Precio", "Precio");
            dgvDetalle.Columns.Add("Cantidad", "Cantidad");
            dgvDetalle.Columns.Add("Subtotal", "Subtotal");

            dgvDetalle.CellEndEdit += (s, e) => CalcularFila(e.RowIndex);
            dgvDetalle.RowsRemoved += (s, e) => CalcularTotal();

            panelCardDetalle.Controls.Add(dgvDetalle);

            btnGuardar = CrearBoton("Guardar", new Point(350, 690), Color.FromArgb(86, 128, 255), Color.White);
            btnCancelar = CrearBoton("Cancelar", new Point(510, 690), Color.LightGray, Color.Black);

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                panelCardDatos, panelCardDetalle, btnGuardar, btnCancelar
            });
        }

        private void CargarDatos()
        {
            cmbCliente.Items.AddRange(new object[] { "Cliente 1", "Cliente 2", "Cliente 3" });
            cmbEmpleado.Items.AddRange(new object[] { "Empleado 1", "Empleado 2", "Empleado 3" });
            txtDescuento.Text = "0";
            txtIVA.Text = "19";
        }

        #endregion

        #region Utilidades

        private bool ValidarFormulario()
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtNumeroFactura.Text))
                return false;

            if (cmbCliente.SelectedIndex < 0 || cmbEmpleado.SelectedIndex < 0)
                return false;

            if (dgvDetalle.Rows.Count <= 1)
            {
                MessageBox.Show("Debe agregar al menos un producto");
                return false;
            }

            return true;
        }

        private void QuitarUltimoSeparador(params StringBuilder[] builders)
        {
            foreach (var sb in builders)
            {
                if (sb.Length > 0)
                    sb.Length--;
            }
        }

        private Panel CrearCard(Size size, Point location)
        {
            Panel panel = new Panel
            {
                Size = size,
                Location = location,
                BackColor = Color.White
            };

            panel.Paint += (s, e) =>
            {
                int r = 25;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(panel.Width - r, 0, r, r, 270, 90);
                path.AddArc(panel.Width - r, panel.Height - r, r, r, 0, 90);
                path.AddArc(0, panel.Height - r, r, r, 90, 90);
                path.CloseFigure();
                panel.Region = new Region(path);
            };

            return panel;
        }

        private Label CrearLabel(string texto, int x, int y) =>
            new Label { Text = texto, Location = new Point(x, y), AutoSize = true };

        private TextBox CrearTextBox(int x, int y, int w) =>
            new TextBox { Location = new Point(x, y), Width = w };

        private ComboBox CrearComboBox(int x, int y, int w) =>
            new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList };

        private Button CrearBoton(string texto, Point location, Color back, Color fore) =>
            new Button
            {
                Text = texto,
                Location = location,
                Size = new Size(130, 40),
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat
            };

        #endregion
    }
}

