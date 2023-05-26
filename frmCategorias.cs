using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using CFACADECONN;
using CFACADESTRUC;
using Npgsql;

namespace CATEGORIAS
{
    public partial class frmCategorias : CControl
    {
        CEstructura ep = new CEstructura();
        string sTitulo;

        public frmCategorias(CEstructura pe)
        {
            InitializeComponent();
            ep = pe;
        }

        private void frmCategorias_Load(object sender, EventArgs e)
        {
            HabilitarTeclaEscape = true;
            HabilitarTeclasSalir = true;

            switch (ep.Opcion)
            {
                case 0:
                    lblTitulo.Text = "ALTA DE CATEGORIA";
                    sTitulo = "ALTA DE CATEGORIAS";
                    AgregarControl(txtNumeroCategoria, null, true, "El campo [ NUMERO DE LA CATEGORIA ] no puede estar vacío, favor de verificar...", false);
                    break;
                case 1:
                    lblTitulo.Text = "ACTUALIZAR CATEGORIA";
                    sTitulo = "ACTUALIZAR CATEGORIAS";
                    AgregarControl(txtNumeroCategoria, fMostrarInformacion, true, "El campo [ NUMERO DE LA CATEGORIA ] no puede estar vacío, favor de verificar...", false);
                    break;
                case 2:
                    lblTitulo.Text = "ELIMINAR CATEGORIA";
                    sTitulo = "ELIMINAR CATEGORIAS";
                    AgregarControl(txtNumeroCategoria, fMostrarInformacion, true, "El campo [ NUMERO DE LA CATEGORIA ] no puede estar vacío, favor de verificar...", false);
                    txtNombreCategoria.ReadOnly = true;
                    txtNombreCategoria.Enabled = false;
                    break;
                default:
                    break;
            }

            AgregarControl(txtNombreCategoria, null, true, "El campo [ NOMBRE DE LA CATEGORIA ] no puede estar vacío, favor de verificar...", false);

            AgregarControl(btnLimpiar, null, "", false);
            AgregarControl(btnGuardar, null, "", false);
            AgregarControl(btnSalir, null, "", false);

            fInicializa();
        }

        public bool fMostrarInformacion()
        {
            string sConsulta, sError = "";
            Int32 nIdentificador;
            bool valorRegresa = false;

            try
            {
                nIdentificador = Convert.ToInt32(txtNumeroCategoria.Text.ToString().Trim());
                sConsulta = string.Format("SELECT nombre FROM consultar_categoria({0}::SMALLINT)", nIdentificador);

                NpgsqlConnection conn = new NpgsqlConnection();
                if(CConeccion.conexionPostgre(ep, ref conn, ref sError))
                {
                    NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                    NpgsqlDataReader reader;
                    reader = com.ExecuteReader();

                    if (reader.Read())
                    {
                        txtNombreCategoria.Text = reader["nombre"].ToString().Trim();

                        valorRegresa = true;
                    }
                    else
                    {
                        MessageBox.Show("El [ NUMERO DE CATEGORIA ] proporcionado, no contiene información...Verifique!!!", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se presento un problema al mostrar la información: \n" + ex.Message.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return valorRegresa;
        }

        public void fInicializa()
        {
            fLimpiarInformacion();
            txtNumeroCategoria.Select();
        }

        public void fLimpiarInformacion()
        {
            try
            {
                fLimpiarInformacion(groupBox1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se presento un problema al limpiar la información: \n" + ex.Message.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void fLimpiarInformacion(GroupBox gb)
        {
            foreach (Control c in gb.Controls)
            {
                if (c is TextBox || c is RichTextBox)
                {
                    c.Text = "";
                }
                else if (c is ComboBox)
                {
                    var tmp = c as ComboBox;
                    tmp.DataSource = null;
                    tmp.Items.Clear();
                }
                else if (c is DataGridView)
                {
                    var tmp = c as DataGridView;
                    tmp.Rows.Clear();
                    tmp.Columns.Clear();
                }
                else if (c is CheckBox)
                {
                    var tmp = c as CheckBox;
                    tmp.Checked = false;
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (fGrabarInformacion())
            {
                switch (ep.Opcion)
                {
                    case 0:
                        MessageBox.Show("Información guardada correctamente.", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 1:
                        MessageBox.Show("La información se actualizo correctamente.", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 2:
                        MessageBox.Show("La categoria se elimino correctamente.", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        break;
                }

                
                fInicializa();
            }
        }

        public bool fGrabarInformacion()
        {
            string sNombreCategoria, sConsulta, sError = "", sDato;
            Int64 nNumCategoria = 0;
            bool valorRegresa = false;

            try
            {
                sNombreCategoria = txtNombreCategoria.Text.Trim();
                if (!string.IsNullOrEmpty(sNombreCategoria))
                {
                    sDato = txtNumeroCategoria.Text.Trim();
                    if (!string.IsNullOrEmpty(sDato))
                    {
                        nNumCategoria = Convert.ToInt32(sDato);
                    }

                    NpgsqlConnection conn = new NpgsqlConnection();
                    if (CConeccion.conexionPostgre(ep, ref conn, ref sError))
                    {
                        sConsulta = String.Format("SELECT insertar_categoria ({0}::SMALLINT, {1}::INTEGER, '{2}')", ep.Opcion, nNumCategoria, sNombreCategoria);
                        NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                        com.ExecuteNonQuery();
                        valorRegresa = true;
                    }
                    else
                    {
                        MessageBox.Show("Se presento un problema al guardar la información: \n" + sError.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        valorRegresa = false;
                    }

                    if(conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("El campo [ NOMBRE DE LA CATEGORIA ] no puede estar vacío, favor de verificar...", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se presento un problema al guardar la información: \n" + ex.Message.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                valorRegresa = false;
            }

            return valorRegresa;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            fLimpiarInformacion();
        }

        private void txtNumeroCategoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo se van a permitir numeros
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == (Char)Keys.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
