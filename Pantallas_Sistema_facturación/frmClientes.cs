using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para la gestión de clientes.
    /// Permite crear y editar información del cliente.
    /// </summary>
    public class frmClientes : Form
    {
        /// <summary>
        /// Evento que se dispara al guardar un cliente.
        /// </summary>
        public event Action<string, string, string, string, string> ClienteGuardado;

        private TextBox txtNombre;
        private TextBox txtDocumento;
        private TextBox txtDireccion;
        private TextBox txtTelefono;
        private TextBox txtEmail;

        // Paleta de colores (diseño original)
        private readonly Color azulFondo = Color.FromArgb(245, 249, 255);
        private readonly Color azulPastel = Color.FromArgb(168, 206, 255);
        private readonly Color azulHover = Color.FromArgb(130, 180, 245);
        private readonly Color azulTexto = Color.FromArgb(60, 70, 90);
        private readonly Color grisBorde = Color.FromArgb(220, 230, 245);

        /// <summary>
        /// Constructor del formulario.
        /// Permite cargar datos cuando se edita un cliente.
        /// </summary>
        public frmClientes(
            string nombre = "",
            string documento = "",
            string direccion = "",
            string telefono = "",
            string email = "")
        {
            InicializarComponentes();

            txtNombre.Text = nombre;
            txtDocumento.Text = documento;
            txtDireccion.Text = direccion;
            txtTelefono.Text = telefono;
            txtEmail.Text = email;
        }

        /// <summary>
        /// Inicializa los controles del formulario.
        /// </summary>
        private void InicializarComponentes()
        {
            this.Text = "Registro de Cliente";
            this.Size = new Size(520, 440);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = azulFondo;

            Label lblTitulo = new Label()
            {
                Text = "Datos del Cliente",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = azulPastel,
                AutoSize = true,
                Location = new Point(40, 15)
            };

            Label lblNombre = CrearLabel("Nombre", 40, 70);
            Label lblDocumento = CrearLabel("Documento", 40, 120);
            Label lblDireccion = CrearLabel("Dirección", 40, 170);
            Label lblTelefono = CrearLabel("Teléfono", 40, 220);
            Label lblEmail = CrearLabel("Email", 40, 270);

            txtNombre = CrearTextBox(150, 65);
            txtDocumento = CrearTextBox(150, 115);
            txtDireccion = CrearTextBox(150, 165);
            txtTelefono = CrearTextBox(150, 215);
            txtEmail = CrearTextBox(150, 265);

            // Validaciones de entrada
            txtNombre.KeyPress += ValidarSoloLetras;
            txtDocumento.KeyPress += ValidarSoloNumeros;
            txtTelefono.KeyPress += ValidarSoloNumeros;

            Button btnGuardar = CrearBoton(
                "Guardar",
                new Point(150, 330),
                azulPastel,
                Color.White,
                true);

            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = CrearBoton(
                "Cancelar",
                new Point(290, 330),
                Color.White,
                azulTexto,
                false);

            btnCancelar.FlatAppearance.BorderColor = grisBorde;
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = Color.FromArgb(235, 242, 252);
            btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.White;
            btnCancelar.Click += (s, e) => this.Close();

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblNombre, lblDocumento, lblDireccion, lblTelefono, lblEmail,
                txtNombre, txtDocumento, txtDireccion, txtTelefono, txtEmail,
                btnGuardar, btnCancelar
            });
        }

        /// <summary>
        /// Crea un label con estilo estándar.
        /// </summary>
        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label()
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = azulTexto
            };
        }

        /// <summary>
        /// Crea un textbox con estilo estándar.
        /// </summary>
        private TextBox CrearTextBox(int x, int y)
        {
            return new TextBox()
            {
                Location = new Point(x, y),
                Width = 280,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        /// <summary>
        /// Crea un botón con estilo estándar.
        /// </summary>
        private Button CrearBoton(string texto, Point location, Color back, Color fore, bool principal)
        {
            Button btn = new Button()
            {
                Text = texto,
                Location = location,
                Size = new Size(120, 42),
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", principal ? 10 : 10, principal ? FontStyle.Bold : FontStyle.Regular),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = principal ? 0 : 1;

            if (principal)
            {
                btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
                btn.MouseLeave += (s, e) => btn.BackColor = azulPastel;
            }

            return btn;
        }

        /// <summary>
        /// Valida que solo se ingresen números.
        /// </summary>
        private void ValidarSoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        /// <summary>
        /// Valida que solo se ingresen letras.
        /// </summary>
        private void ValidarSoloLetras(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        /// <summary>
        /// Guarda el cliente y dispara el evento ClienteGuardado.
        /// </summary>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Complete todos los campos",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email no válido",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            ClienteGuardado?.Invoke(
                txtNombre.Text.Trim(),
                txtDocumento.Text.Trim(),
                txtDireccion.Text.Trim(),
                txtTelefono.Text.Trim(),
                txtEmail.Text.Trim());

            this.Close();
        }
    }
}
