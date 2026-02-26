using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario de inicio de sesión del sistema.
    /// </summary>
    public class frmLogin : Form
    {
        #region Controles

        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblPassword;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnIngresar;
        private ErrorProvider errorProvider1;

        #endregion

        #region Colores

        private readonly Color azulFondo = Color.FromArgb(240, 248, 255);
        private readonly Color azulPrincipal = Color.FromArgb(0, 102, 204);
        private readonly Color azulHover = Color.FromArgb(0, 85, 170);
        private readonly Color azulTexto = Color.FromArgb(33, 37, 41);

        #endregion

        #region Constructor

        public frmLogin()
        {
            InicializarComponentes();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Login - Sistema de Facturación";
            Size = new Size(400, 350);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = azulFondo;
            Font = new Font("Segoe UI", 10);

            lblTitulo = new Label
            {
                Text = "INICIAR SESIÓN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = azulPrincipal,
                AutoSize = true,
                Location = new Point(95, 30)
            };

            lblUsuario = new Label
            {
                Text = "Usuario",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = azulTexto,
                Location = new Point(50, 100),
                AutoSize = true
            };

            txtUsuario = new TextBox
            {
                Name = "txtUsuario",
                Size = new Size(250, 30),
                Location = new Point(50, 125),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtUsuario.TextChanged += (s, e) => errorProvider1.SetError(txtUsuario, "");

            lblPassword = new Label
            {
                Text = "Contraseña",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = azulTexto,
                Location = new Point(50, 165),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Name = "txtPassword",
                Size = new Size(250, 30),
                Location = new Point(50, 190),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };
            txtPassword.TextChanged += (s, e) => errorProvider1.SetError(txtPassword, "");

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Size = new Size(250, 40),
                Location = new Point(50, 240),
                BackColor = azulPrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;

            btnIngresar.MouseEnter += (s, e) => btnIngresar.BackColor = azulHover;
            btnIngresar.MouseLeave += (s, e) => btnIngresar.BackColor = azulPrincipal;
            btnIngresar.Click += BtnIngresar_Click;

            errorProvider1 = new ErrorProvider
            {
                ContainerControl = this
            };

            // Permite Enter para iniciar sesión
            this.AcceptButton = btnIngresar;

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblUsuario,
                txtUsuario,
                lblPassword,
                txtPassword,
                btnIngresar
            });
        }

        #endregion

        #region Lógica Login

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            if (Autenticar())
            {
                frmPrincipal principal = new frmPrincipal();
                principal.Show();
                Hide();
            }
            else
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private bool ValidarCampos()
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "Debe ingresar el usuario");
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Debe ingresar la contraseña");
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private bool Autenticar()
        {
            // 🔐 Credenciales quemadas (según tu indicación)
            return txtUsuario.Text == "admin" && txtPassword.Text == "123";
        }

        #endregion
    }
}

