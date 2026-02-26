using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para registrar y editar empleados.
    /// </summary>
    public class frmEmpleados : Form
    {
        #region Propiedades públicas

        /// <summary>
        /// Indica si el formulario está en modo edición.
        /// </summary>
        public bool EsEdicion { get; set; } = false;

        /// <summary>
        /// Fila del DataGridView que se editará.
        /// </summary>
        public DataGridViewRow FilaEditar { get; set; }

        #endregion

        #region Controles

        private Panel panelCard;
        private TextBox txtNombre;
        private TextBox txtDocumento;
        private TextBox txtDireccion;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TextBox txtDatosAdicionales;
        private ComboBox cmbRol;
        private DateTimePicker dtpFechaIngreso;
        private Button btnGuardar;
        private Button btnCancelar;
        private ErrorProvider errorProvider1;

        #endregion

        #region Estilos

        private readonly Color azulFondo = Color.FromArgb(245, 248, 255);
        private readonly Color azulPrincipal = Color.FromArgb(95, 135, 255);
        private readonly Color azulHover = Color.FromArgb(130, 165, 255);
        private readonly Font fuenteGeneral = new Font("Segoe UI", 10);

        #endregion

        #region Constructor

        public frmEmpleados()
        {
            InicializarComponentes();
        }

        #endregion

        #region Inicialización

        private void InicializarComponentes()
        {
            Text = "Gestión de Empleados";
            ClientSize = new Size(600, 720);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = azulFondo;
            Font = fuenteGeneral;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            panelCard = new Panel
            {
                Size = new Size(520, 650),
                Location = new Point(40, 30),
                BackColor = Color.White
            };
            panelCard.Paint += PanelCard_Paint;

            int left = 40;
            int width = 430;
            int top = 25;
            int espacio = 58;

            Label lblTitulo = new Label
            {
                Text = "Registro de Empleado",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = azulPrincipal,
                AutoSize = true,
                Location = new Point(145, top)
            };

            panelCard.Controls.Add(lblTitulo);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Nombre completo", left, top));
            txtNombre = CrearTextBox(left, top + 22, width);
            panelCard.Controls.Add(txtNombre);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Documento", left, top));
            txtDocumento = CrearTextBox(left, top + 22, width);
            txtDocumento.KeyPress += SoloNumeros;
            panelCard.Controls.Add(txtDocumento);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Dirección", left, top));
            txtDireccion = CrearTextBox(left, top + 22, width);
            panelCard.Controls.Add(txtDireccion);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Teléfono", left, top));
            txtTelefono = CrearTextBox(left, top + 22, width);
            txtTelefono.KeyPress += SoloNumeros;
            panelCard.Controls.Add(txtTelefono);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Email", left, top));
            txtEmail = CrearTextBox(left, top + 22, width);
            panelCard.Controls.Add(txtEmail);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Rol", left, top));
            cmbRol = new ComboBox
            {
                Location = new Point(left, top + 22),
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRol.Items.AddRange(new object[]
            {
                "Seleccione rol",
                "Administrador",
                "Vendedor",
                "Cajero"
            });
            cmbRol.SelectedIndex = 0;
            panelCard.Controls.Add(cmbRol);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Fecha de ingreso", left, top));
            dtpFechaIngreso = new DateTimePicker
            {
                Location = new Point(left, top + 22),
                Width = width,
                Format = DateTimePickerFormat.Short
            };
            panelCard.Controls.Add(dtpFechaIngreso);
            top += espacio;

            panelCard.Controls.Add(CrearLabel("Datos adicionales", left, top));
            txtDatosAdicionales = new TextBox
            {
                Location = new Point(left, top + 22),
                Width = width,
                Height = 70,
                Multiline = true
            };
            panelCard.Controls.Add(txtDatosAdicionales);
            top += 95;

            btnGuardar = CrearBoton("Guardar", new Point(110, top), azulPrincipal, Color.White);
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = CrearBoton("Cancelar", new Point(270, top), Color.LightGray, Color.Black);
            btnCancelar.Click += (s, e) => Close();

            panelCard.Controls.Add(btnGuardar);
            panelCard.Controls.Add(btnCancelar);

            errorProvider1 = new ErrorProvider { ContainerControl = this };

            Controls.Add(panelCard);
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Carga datos cuando el formulario se usa en modo edición.
        /// </summary>
        public void CargarDatos(
            string nombre,
            string documento,
            string telefono,
            string rol,
            DateTime fecha)
        {
            txtNombre.Text = nombre;
            txtDocumento.Text = documento;
            txtTelefono.Text = telefono;
            cmbRol.SelectedItem = rol;
            dtpFechaIngreso.Value = fecha;
        }

        #endregion

        #region Eventos

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Ingrese el nombre");
                return;
            }

            if (cmbRol.SelectedIndex == 0)
            {
                errorProvider1.SetError(cmbRol, "Seleccione un rol");
                return;
            }

            // 🔴 CLAVE: NO duplicar registros
            if (EsEdicion && FilaEditar != null)
            {
                FilaEditar.Cells["Nombre"].Value = txtNombre.Text.Trim();
                FilaEditar.Cells["Documento"].Value = txtDocumento.Text.Trim();
                FilaEditar.Cells["Telefono"].Value = txtTelefono.Text.Trim();
                FilaEditar.Cells["Rol"].Value = cmbRol.Text;
                FilaEditar.Cells["Fecha"].Value = dtpFechaIngreso.Value.ToShortDateString();
            }
            else
            {
                frmListarEmpleados.dgvEmpleados.Rows.Add(
                    txtNombre.Text.Trim(),
                    txtDocumento.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    cmbRol.Text,
                    dtpFechaIngreso.Value.ToShortDateString()
                );
            }

            MessageBox.Show(
                "Empleado guardado correctamente",
                "Éxito",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }

        #endregion

        #region Métodos auxiliares

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

        private Button CrearBoton(string texto, Point location, Color back, Color fore)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(130, 40),
                Location = location,
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
            btn.MouseLeave += (s, e) => btn.BackColor = back;

            return btn;
        }

        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            int radius = 25;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panelCard.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panelCard.Width - radius, panelCard.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panelCard.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panelCard.Region = new Region(path);
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        #endregion
    }
}
