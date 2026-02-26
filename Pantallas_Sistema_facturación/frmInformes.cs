using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturación
{
    /// <summary>
    /// Formulario de generación de informes del sistema.
    /// </summary>
    public class frmInformes : Form
    {
        #region Controles

        private Label lblTipoInforme;
        private Label lblFechaInicio;
        private Label lblFechaFin;
        private Label lblResumen;

        private ComboBox cmbTipoInforme;
        private Button btnGenerar;
        private Button btnSalir;

        private DataGridView dgvInforme;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;

        private RadioButton rbPantalla;
        private RadioButton rbPdf;
        private RadioButton rbExcel;

        private Panel panelFiltros;
        private Panel panelResultados;

        #endregion

        #region Constructor

        public frmInformes()
        {
            InicializarComponentes();
        }

        #endregion

        #region Inicialización UI

        private void InicializarComponentes()
        {
            Text = "Informes del Sistema";
            ClientSize = new Size(980, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(232, 241, 255);

            Label lblTitulo = new Label
            {
                Text = "Centro de Informes",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 95, 180),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            Controls.Add(lblTitulo);

            panelFiltros = CrearCard(new Size(900, 110), new Point(30, 60));

            lblTipoInforme = CrearLabel("Tipo de informe", 20, 20);
            cmbTipoInforme = CrearComboBox(20, 45, 220);
            cmbTipoInforme.Items.AddRange(new object[]
            {
                "Seleccione informe",
                "Ventas",
                "Clientes",
                "Productos",
                "Empleados"
            });
            cmbTipoInforme.SelectedIndex = 0;

            lblFechaInicio = CrearLabel("Fecha inicio", 270, 20);
            dtpInicio = new DateTimePicker
            {
                Location = new Point(270, 45),
                Format = DateTimePickerFormat.Short,
                Width = 150
            };

            lblFechaFin = CrearLabel("Fecha fin", 450, 20);
            dtpFin = new DateTimePicker
            {
                Location = new Point(450, 45),
                Format = DateTimePickerFormat.Short,
                Width = 150
            };

            Panel panelSalida = new Panel
            {
                Location = new Point(630, 20),
                Size = new Size(240, 70)
            };

            rbPantalla = CrearRadio("Pantalla", 0, 0, true);
            rbPdf = CrearRadio("PDF", 0, 25, false);
            rbExcel = CrearRadio("Excel", 110, 25, false);

            panelSalida.Controls.AddRange(new Control[]
            {
                rbPantalla, rbPdf, rbExcel
            });

            panelFiltros.Controls.AddRange(new Control[]
            {
                lblTipoInforme, cmbTipoInforme,
                lblFechaInicio, dtpInicio,
                lblFechaFin, dtpFin,
                panelSalida
            });

            Controls.Add(panelFiltros);

            panelResultados = CrearCard(new Size(900, 360), new Point(30, 190));

            dgvInforme = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(860, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            lblResumen = new Label
            {
                Location = new Point(20, 295),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 95, 180)
            };

            panelResultados.Controls.Add(dgvInforme);
            panelResultados.Controls.Add(lblResumen);

            Controls.Add(panelResultados);

            btnGenerar = CrearBoton(
                "Generar informe",
                new Point(660, 585),
                Color.FromArgb(86, 128, 255),
                Color.White);

            btnSalir = CrearBoton(
                "Salir",
                new Point(820, 585),
                Color.FromArgb(210, 224, 255),
                Color.FromArgb(45, 95, 180));

            btnGenerar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            btnGenerar.Click += BtnGenerar_Click;
            btnSalir.Click += (s, e) => Close();

            Controls.Add(btnGenerar);
            Controls.Add(btnSalir);
        }

        #endregion

        #region Generación de informe

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            if (!ValidarFiltros())
                return;

            PrepararGrid();

            // Datos simulados (no se modifican)
            dgvInforme.Rows.Add(
                DateTime.Today.AddDays(-1).ToShortDateString(),
                "Registro 1",
                "100.000");

            dgvInforme.Rows.Add(
                DateTime.Today.ToShortDateString(),
                "Registro 2",
                "250.000");

            lblResumen.Text =
                $"Informe generado del {dtpInicio.Value.ToShortDateString()} al {dtpFin.Value.ToShortDateString()}";

            if (rbPdf.Checked)
                MessageBox.Show("Aquí se generaría el PDF 📄");

            if (rbExcel.Checked)
                MessageBox.Show("Aquí se exportaría a Excel 📊");
        }

        #endregion

        #region Validaciones

        private bool ValidarFiltros()
        {
            if (cmbTipoInforme.SelectedIndex == 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un tipo de informe",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (dtpInicio.Value.Date > dtpFin.Value.Date)
            {
                MessageBox.Show(
                    "La fecha inicio no puede ser mayor que la fecha fin",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void PrepararGrid()
        {
            dgvInforme.Columns.Clear();
            dgvInforme.Rows.Clear();
            lblResumen.Text = string.Empty;

            dgvInforme.Columns.Add("Fecha", "Fecha");
            dgvInforme.Columns.Add("Detalle", "Detalle");
            dgvInforme.Columns.Add("Valor", "Valor");
        }

        #endregion

        #region Controles reutilizables

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
                int r = 28;
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

        private Label CrearLabel(string text, int x, int y) =>
            new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true
            };

        private ComboBox CrearComboBox(int x, int y, int width) =>
            new ComboBox
            {
                Location = new Point(x, y),
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

        private RadioButton CrearRadio(string text, int x, int y, bool check) =>
            new RadioButton
            {
                Text = text,
                Location = new Point(x, y),
                Checked = check,
                AutoSize = true
            };

        private Button CrearBoton(string texto, Point location, Color back, Color fore) =>
            new Button
            {
                Text = texto,
                Size = new Size(140, 42),
                Location = location,
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

        #endregion
    }
}

