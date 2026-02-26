using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para la administración de usuarios del sistema.
    /// Permite crear, editar y eliminar usuarios con roles y estado.
    /// </summary>
    public class frmAdminSeguridad : Form
    {
        private DataGridView dgvUsuarios;

        private Label lblTitulo, lblUsuario, lblClave, lblRol, lblEstado;
        private TextBox txtUsuario, txtClave;
        private ComboBox cmbRol;
        private CheckBox chkActivo;

        private Button btnGuardar, btnEditar, btnEliminar;

        private int indiceSeleccionado = -1;

        private Color azulFondo = Color.FromArgb(240, 246, 255);
        private Color azulPrincipal = Color.FromArgb(86, 128, 233);
        private Color azulHover = Color.FromArgb(120, 160, 255);
        private Color grisTexto = Color.FromArgb(60, 60, 60);

        /// <summary>
        /// Constructor del formulario de administración de seguridad.
        /// </summary>
        public frmAdminSeguridad()
        {
            InicializarComponentes();
            CargarUsuariosIniciales();
        }

        /// <summary>
        /// Inicializa y configura todos los controles visuales del formulario.
        /// </summary>
        private void InicializarComponentes()
        {
            this.Text = "Administración de Seguridad";
            this.Size = new Size(900, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = azulFondo;
            this.Font = new Font("Segoe UI", 10);

            lblTitulo = new Label()
            {
                Text = "Administración de Usuarios",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Location = new Point(30, 20),
                AutoSize = true
            };

            lblUsuario = CrearLabel("Usuario:", 40, 70);
            lblClave = CrearLabel("Clave:", 40, 110);
            lblRol = CrearLabel("Rol:", 40, 150);
            lblEstado = CrearLabel("Activo:", 40, 190);

            txtUsuario = CrearTextBox(150, 65);
            txtClave = CrearTextBox(150, 105);
            txtClave.UseSystemPasswordChar = true;

            cmbRol = new ComboBox()
            {
                Location = new Point(150, 145),
                Size = new Size(220, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbRol.Items.AddRange(new object[]
            {
                "Administrador",
                "Vendedor",
                "Cajero"
            });

            chkActivo = new CheckBox()
            {
                Location = new Point(150, 190),
                Text = "Usuario activo",
                ForeColor = grisTexto,
                AutoSize = true
            };

            btnGuardar = CrearBoton("Guardar", 420, 65);
            btnGuardar.Click += BtnGuardar_Click;

            btnEditar = CrearBoton("Editar", 420, 110);
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = CrearBoton("Eliminar", 420, 155);
            btnEliminar.Click += BtnEliminar_Click;

            dgvUsuarios = new DataGridView()
            {
                Location = new Point(40, 240),
                Size = new Size(800, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = azulPrincipal;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);

            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("Usuario", "Usuario");
            dgvUsuarios.Columns.Add("Clave", "Clave");
            dgvUsuarios.Columns.Add("Rol", "Rol");
            dgvUsuarios.Columns.Add("Activo", "Activo");

            dgvUsuarios.CellClick += DgvUsuarios_CellClick;

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblUsuario, lblClave, lblRol, lblEstado,
                txtUsuario, txtClave, cmbRol, chkActivo,
                btnGuardar, btnEditar, btnEliminar,
                dgvUsuarios
            });
        }

        /// <summary>
        /// Carga usuarios predeterminados en la grilla.
        /// </summary>
        private void CargarUsuariosIniciales()
        {
            dgvUsuarios.Rows.Add("1", "admin", "1234", "Administrador", "Sí");
            dgvUsuarios.Rows.Add("2", "vendedor1", "1234", "Vendedor", "Sí");
            dgvUsuarios.Rows.Add("3", "cajero1", "1234", "Cajero", "No");
        }

        /// <summary>
        /// Guarda un nuevo usuario en la lista.
        /// </summary>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text) ||
                cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Complete todos los campos",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int nuevoId = dgvUsuarios.Rows.Count + 1;

            dgvUsuarios.Rows.Add(
                nuevoId.ToString(),
                txtUsuario.Text,
                txtClave.Text,
                cmbRol.SelectedItem.ToString(),
                chkActivo.Checked ? "Sí" : "No"
            );

            LimpiarCampos();
        }

        /// <summary>
        /// Carga los datos del usuario seleccionado en los campos.
        /// </summary>
        private void DgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                indiceSeleccionado = e.RowIndex;

                txtUsuario.Text = dgvUsuarios.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtClave.Text = dgvUsuarios.Rows[e.RowIndex].Cells[2].Value.ToString();
                cmbRol.SelectedItem = dgvUsuarios.Rows[e.RowIndex].Cells[3].Value.ToString();
                chkActivo.Checked = dgvUsuarios.Rows[e.RowIndex].Cells[4].Value.ToString() == "Sí";

                btnGuardar.Enabled = false;
            }
        }

        /// <summary>
        /// Actualiza los datos del usuario seleccionado.
        /// </summary>
        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (indiceSeleccionado >= 0)
            {
                dgvUsuarios.Rows[indiceSeleccionado].Cells[1].Value = txtUsuario.Text;
                dgvUsuarios.Rows[indiceSeleccionado].Cells[2].Value = txtClave.Text;
                dgvUsuarios.Rows[indiceSeleccionado].Cells[3].Value = cmbRol.SelectedItem.ToString();
                dgvUsuarios.Rows[indiceSeleccionado].Cells[4].Value = chkActivo.Checked ? "Sí" : "No";

                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para editar",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Elimina el usuario seleccionado de la lista.
        /// </summary>
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (indiceSeleccionado >= 0)
            {
                dgvUsuarios.Rows.RemoveAt(indiceSeleccionado);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para eliminar",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Limpia los campos del formulario y restablece el estado.
        /// </summary>
        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtClave.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = false;
            indiceSeleccionado = -1;
            btnGuardar.Enabled = true;
        }

        /// <summary>
        /// Crea una etiqueta con estilo estándar.
        /// </summary>
        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label()
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = grisTexto
            };
        }

        /// <summary>
        /// Crea un TextBox con formato estándar.
        /// </summary>
        private TextBox CrearTextBox(int x, int y)
        {
            return new TextBox()
            {
                Location = new Point(x, y),
                Size = new Size(220, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        /// <summary>
        /// Crea un botón con el estilo visual del sistema.
        /// </summary>
        private Button CrearBoton(string texto, int x, int y)
        {
            Button btn = new Button()
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(140, 36),
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
    }
}

