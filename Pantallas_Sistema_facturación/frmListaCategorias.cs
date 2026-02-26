using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario para listar, buscar y administrar categorías.
    /// </summary>
    public partial class frmListarCategoria : Form
    {
        #region Controles

        private Panel panelSuperior;
        private Label lblTitulo;
        private TextBox txtBuscar;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private DataGridView dgvCategorias;

        #endregion

        #region Datos

        private readonly List<frmCategoriaProductos.Categoria> listaCategorias =
            new List<frmCategoriaProductos.Categoria>();

        private const string TextoPlaceholder = "Buscar categoría...";

        #endregion

        #region Constructor

        public frmListarCategoria()
        {
            InicializarComponentes();
            CargarDatosIniciales();
            CargarDatos();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Listado de Categorías";
            Size = new Size(800, 550);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(232, 241, 255);

            panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White
            };

            lblTitulo = new Label
            {
                Text = "Categorías Registradas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 95, 180),
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Text = TextoPlaceholder,
                ForeColor = Color.Gray,
                Size = new Size(250, 30),
                Location = new Point(20, 60)
            };

            txtBuscar.Enter += TxtBuscar_Enter;
            txtBuscar.Leave += TxtBuscar_Leave;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            btnNuevo = CrearBoton("Nuevo", new Point(300, 55), Color.FromArgb(86, 128, 255));
            btnEditar = CrearBoton("Editar", new Point(410, 55), Color.FromArgb(120, 170, 255));
            btnEliminar = CrearBoton("Eliminar", new Point(520, 55), Color.FromArgb(150, 190, 255));

            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            panelSuperior.Controls.AddRange(new Control[]
            {
                lblTitulo, txtBuscar, btnNuevo, btnEditar, btnEliminar
            });

            dgvCategorias = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            dgvCategorias.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(86, 128, 255);
            dgvCategorias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategorias.EnableHeadersVisualStyles = false;

            Controls.Add(dgvCategorias);
            Controls.Add(panelSuperior);
        }

        #endregion

        #region Datos

        private void CargarDatosIniciales()
        {
            listaCategorias.Add(new frmCategoriaProductos.Categoria
            {
                Nombre = "Electrónica",
                Descripcion = "Productos tecnológicos y dispositivos electrónicos"
            });

            listaCategorias.Add(new frmCategoriaProductos.Categoria
            {
                Nombre = "Ropa",
                Descripcion = "Prendas de vestir para hombre y mujer"
            });

            listaCategorias.Add(new frmCategoriaProductos.Categoria
            {
                Nombre = "Hogar",
                Descripcion = "Artículos para el hogar y decoración"
            });
        }

        private void CargarDatos(IEnumerable<frmCategoriaProductos.Categoria> datos = null)
        {
            dgvCategorias.DataSource = null;
            dgvCategorias.DataSource = datos == null
                ? listaCategorias
                : datos.ToList();
        }

        #endregion

        #region Eventos Botones

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            frmCategoriaProductos form = new frmCategoriaProductos();

            if (form.ShowDialog() == DialogResult.OK)
            {
                listaCategorias.Add(form.CategoriaGuardada);
                CargarDatos();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
            {
                MostrarAviso("Seleccione una categoría para editar.");
                return;
            }

            var categoria =
                (frmCategoriaProductos.Categoria)dgvCategorias.CurrentRow.DataBoundItem;

            frmCategoriaProductos form = new frmCategoriaProductos
            {
                EsEdicion = true,
                NombreCategoria = categoria.Nombre,
                DescripcionCategoria = categoria.Descripcion
            };

            if (form.ShowDialog() == DialogResult.OK)
            {
                categoria.Nombre = form.CategoriaGuardada.Nombre;
                categoria.Descripcion = form.CategoriaGuardada.Descripcion;
                CargarDatos();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
            {
                MostrarAviso("Seleccione una categoría para eliminar.");
                return;
            }

            var categoria =
                (frmCategoriaProductos.Categoria)dgvCategorias.CurrentRow.DataBoundItem;

            var confirmar = MessageBox.Show(
                "¿Está seguro que desea eliminar esta categoría?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                listaCategorias.Remove(categoria);
                CargarDatos();
            }
        }

        #endregion

        #region Búsqueda

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscar.Text == TextoPlaceholder)
                return;

            string filtro = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filtro))
            {
                CargarDatos();
                return;
            }

            var resultado = listaCategorias
                .Where(c => c.Nombre.ToLower().Contains(filtro))
                .ToList();

            CargarDatos(resultado);
        }

        private void TxtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == TextoPlaceholder)
            {
                txtBuscar.Text = string.Empty;
                txtBuscar.ForeColor = Color.Black;
            }
        }

        private void TxtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = TextoPlaceholder;
                txtBuscar.ForeColor = Color.Gray;
            }
        }

        #endregion

        #region Utilidades

        private Button CrearBoton(string texto, Point location, Color color)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(100, 35),
                Location = location,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void MostrarAviso(string mensaje)
        {
            MessageBox.Show(
                mensaje,
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion
    }
}
