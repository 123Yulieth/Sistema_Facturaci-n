using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    public class frmRolEmpleados : Form
    {
        #region Controles

        private DataGridView dgvRoles;
        private Label lblTitulo;
        private Label lblNombreRol;
        private Label lblModo;
        private TextBox txtNombreRol;
        private Button btnGuardar;
        private Button btnEliminar;

        #endregion

        #region Variables privadas

        private int indiceSeleccionado = -1;

        #endregion

        #region Colores

        private readonly Color fondo = Color.FromArgb(245, 248, 255);
        private readonly Color azulTitulo = Color.FromArgb(102, 153, 255);
        private readonly Color azulBoton = Color.FromArgb(95, 135, 255);
        private readonly Color azulHover = Color.FromArgb(130, 165, 255);

        #endregion

        #region Constructor

        public frmRolEmpleados()
        {
            InicializarFormulario();
            InicializarControles();
            CargarRolesPredeterminados();
        }

        #endregion

        #region Inicialización del formulario

        private void InicializarFormulario()
        {
            Text = "Gestión de Roles de Empleados";
            Size = new Size(650, 520);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = fondo;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);
        }

        #endregion

        #region Inicialización de controles

        private void InicializarControles()
        {
            lblTitulo = new Label
            {
                Text = "Roles de Empleados",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = azulTitulo,
                AutoSize = true,
                Location = new Point(30, 20)
            };

            lblModo = new Label
            {
                Text = "Modo: Crear nuevo rol",
                Location = new Point(40, 60),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            lblNombreRol = new Label
            {
                Text = "Nombre del Rol:",
                Location = new Point(40, 95),
                AutoSize = true
            };

            txtNombreRol = new TextBox
            {
                Location = new Point(180, 90),
                Size = new Size(260, 28)
            };

            btnGuardar = CrearBoton("Guardar", new Point(460, 88));
            btnGuardar.Click += BtnGuardar_Click;

            dgvRoles = new DataGridView
            {
                Location = new Point(40, 140),
                Size = new Size(550, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            dgvRoles.Columns.Add("Id", "ID");
            dgvRoles.Columns.Add("NombreRol", "Nombre del Rol");

            dgvRoles.CellClick += DgvRoles_CellClick;

            btnEliminar = CrearBoton("Eliminar", new Point(260, 410));
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[]
            {
                lblTitulo,
                lblModo,
                lblNombreRol,
                txtNombreRol,
                btnGuardar,
                dgvRoles,
                btnEliminar
            });
        }

        #endregion

        #region Datos iniciales

        private void CargarRolesPredeterminados()
        {
            dgvRoles.Rows.Add("1", "Administrador");
            dgvRoles.Rows.Add("2", "Vendedor");
            dgvRoles.Rows.Add("3", "Cajero");
            dgvRoles.Rows.Add("4", "Supervisor");
        }

        #endregion

        #region Eventos

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreRol.Text))
            {
                MessageBox.Show("Ingrese el nombre del rol");
                return;
            }

            if (indiceSeleccionado == -1)
            {
                AgregarRol();
            }
            else
            {
                ActualizarRol();
            }

            LimpiarFormulario();
        }

        private void DgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            indiceSeleccionado = e.RowIndex;
            txtNombreRol.Text = dgvRoles.Rows[e.RowIndex].Cells["NombreRol"].Value.ToString();
            lblModo.Text = "Modo: Editando rol";
            btnGuardar.Text = "Actualizar";
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (indiceSeleccionado < 0)
            {
                MessageBox.Show("Seleccione un rol para eliminar");
                return;
            }

            dgvRoles.Rows.RemoveAt(indiceSeleccionado);
            LimpiarFormulario();
        }

        #endregion

        #region Lógica de roles

        private void AgregarRol()
        {
            int nuevoId = dgvRoles.Rows.Count + 1;
            dgvRoles.Rows.Add(nuevoId.ToString(), txtNombreRol.Text);
        }

        private void ActualizarRol()
        {
            dgvRoles.Rows[indiceSeleccionado].Cells["NombreRol"].Value = txtNombreRol.Text;
        }

        private void LimpiarFormulario()
        {
            txtNombreRol.Clear();
            indiceSeleccionado = -1;
            btnGuardar.Text = "Guardar";
            lblModo.Text = "Modo: Crear nuevo rol";
            txtNombreRol.Focus();
        }

        #endregion

        #region Helpers

        private Button CrearBoton(string texto, Point location)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = location,
                Size = new Size(130, 40),
                BackColor = azulBoton,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = azulHover;
            btn.MouseLeave += (s, e) => btn.BackColor = azulBoton;

            return btn;
        }

        #endregion
    }
}
