using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    public class frmProductos : Form
    {
        #region Evento público

        public event Action<string, string, string, string, decimal, decimal, int, string> ProductoGuardado;

        #endregion

        #region Variables privadas

        private bool modoEdicion;

        private Label lblTitulo, lblNombre, lblCategoria, lblCodigo,
                      lblRutaImagen, lblPrecioCompra, lblPrecioVenta,
                      lblStock, lblDetalles;

        private TextBox txtNombre, txtCodigo, txtRutaImagen,
                        txtPrecioCompra, txtPrecioVenta,
                        txtStock, txtDetalles;

        private ComboBox cmbCategoria;
        private PictureBox picImagen;

        private Button btnBuscarImagen, btnGuardar, btnCancelar;
        private ErrorProvider errorProvider1;

        #endregion

        #region Colores

        private readonly Color fondo = Color.FromArgb(245, 248, 255);
        private readonly Color azulTitulo = Color.FromArgb(102, 153, 255);
        private readonly Color azulBoton = Color.FromArgb(137, 179, 255);

        #endregion

        #region Constructor

        public frmProductos()
        {
            InicializarFormulario();
            InicializarControles();
        }

        #endregion

        #region Inicialización formulario

        private void InicializarFormulario()
        {
            Text = "Gestión de Productos";
            ClientSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = fondo;
        }

        #endregion

        #region Inicialización controles

        private void InicializarControles()
        {
            lblTitulo = new Label
            {
                Text = "Registro de Producto",
                Font = new Font("Segoe UI Semibold", 18),
                ForeColor = azulTitulo,
                AutoSize = true,
                Location = new Point(330, 20)
            };

            int leftCol = 40;
            int rightCol = 480;
            int top = 80;
            int espacio = 55;
            int width = 350;

            lblNombre = CrearLabel("Nombre:", leftCol, top);
            txtNombre = CrearTextBox(leftCol, top + 22, width);

            lblCategoria = CrearLabel("Categoría:", leftCol, top + espacio);
            cmbCategoria = new ComboBox
            {
                Location = new Point(leftCol, top + espacio + 22),
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategoria.Items.AddRange(new[] { "Electrónica", "Alimentos", "Ropa", "Hogar" });

            lblCodigo = CrearLabel("Código Referencia:", leftCol, top + espacio * 2);
            txtCodigo = CrearTextBox(leftCol, top + espacio * 2 + 22, width);

            lblPrecioCompra = CrearLabel("Precio Compra:", leftCol, top + espacio * 3);
            txtPrecioCompra = CrearTextBox(leftCol, top + espacio * 3 + 22, width);
            txtPrecioCompra.KeyPress += SoloNumerosDecimal;

            lblPrecioVenta = CrearLabel("Precio Venta:", leftCol, top + espacio * 4);
            txtPrecioVenta = CrearTextBox(leftCol, top + espacio * 4 + 22, width);
            txtPrecioVenta.KeyPress += SoloNumerosDecimal;

            lblStock = CrearLabel("Cantidad Stock:", leftCol, top + espacio * 5);
            txtStock = CrearTextBox(leftCol, top + espacio * 5 + 22, width);
            txtStock.KeyPress += SoloNumerosEnteros;

            lblRutaImagen = CrearLabel("Imagen del Producto:", rightCol, top);
            txtRutaImagen = CrearTextBox(rightCol, top + 22, 260);

            btnBuscarImagen = CrearBoton("📁", new Point(rightCol + 270, top + 20), new Size(50, 28));
            btnBuscarImagen.Click += BtnBuscarImagen_Click;

            picImagen = new PictureBox
            {
                Location = new Point(rightCol, top + 60),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.White
            };

            lblDetalles = CrearLabel("Detalles:", rightCol, top + 280);
            txtDetalles = CrearTextBox(rightCol, top + 302, 320);
            txtDetalles.Multiline = true;
            txtDetalles.Height = 70;

            btnGuardar = CrearBoton("Guardar", new Point(260, 520), new Size(140, 45));
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = CrearBoton("Cancelar", new Point(420, 520), new Size(140, 45));
            btnCancelar.Click += (s, e) => Close();

            errorProvider1 = new ErrorProvider { ContainerControl = this };

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblNombre, txtNombre,
                lblCategoria, cmbCategoria,
                lblCodigo, txtCodigo,
                lblPrecioCompra, txtPrecioCompra,
                lblPrecioVenta, txtPrecioVenta,
                lblStock, txtStock,
                lblRutaImagen, txtRutaImagen, btnBuscarImagen,
                picImagen,
                lblDetalles, txtDetalles,
                btnGuardar, btnCancelar
            });
        }

        #endregion

        #region Edición

        public void CargarProducto(string nombre, string categoria, string codigo,
                                   string precioCompra, string precioVenta, string stock)
        {
            modoEdicion = true;
            lblTitulo.Text = "Editar Producto";
            btnGuardar.Text = "Actualizar";

            txtNombre.Text = nombre;
            cmbCategoria.Text = categoria;
            txtCodigo.Text = codigo;
            txtPrecioCompra.Text = precioCompra;
            txtPrecioVenta.Text = precioVenta;
            txtStock.Text = stock;
        }

        #endregion

        #region Guardar

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            ProductoGuardado?.Invoke(
                txtNombre.Text,
                cmbCategoria.Text,
                txtCodigo.Text,
                txtRutaImagen.Text,
                decimal.Parse(txtPrecioCompra.Text),
                decimal.Parse(txtPrecioVenta.Text),
                int.Parse(txtStock.Text),
                txtDetalles.Text
            );

            MessageBox.Show(
                modoEdicion ? "Producto actualizado correctamente" : "Producto guardado correctamente",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }

        private bool ValidarFormulario()
        {
            errorProvider1.Clear();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Ingrese el nombre");
                valido = false;
            }

            if (cmbCategoria.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbCategoria, "Seleccione una categoría");
                valido = false;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out _))
            {
                errorProvider1.SetError(txtPrecioCompra, "Precio inválido");
                valido = false;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                errorProvider1.SetError(txtPrecioVenta, "Precio inválido");
                valido = false;
            }

            if (!int.TryParse(txtStock.Text, out _))
            {
                errorProvider1.SetError(txtStock, "Stock inválido");
                valido = false;
            }

            return valido;
        }

        #endregion

        #region Validaciones teclado

        private void SoloNumerosDecimal(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && txt.Text.Contains("."))
                e.Handled = true;
        }

        private void SoloNumerosEnteros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        #endregion

        #region Imagen

        private void BtnBuscarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Imagen (*.jpg;*.png)|*.jpg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaImagen.Text = dialog.FileName;
                    picImagen.ImageLocation = dialog.FileName;
                }
            }
        }

        #endregion

        #region Helpers

        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private TextBox CrearTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = width
            };
        }

        private Button CrearBoton(string texto, Point location, Size size)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = size,
                Location = location,
                BackColor = azulBoton,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        #endregion
    }
}
