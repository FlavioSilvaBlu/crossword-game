using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jogopalavrascruzadas
{
    public partial class Form2 : Form
    {
        private SqlConnection conn;

        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'dadosDataSet2.palavras'. Você pode movê-la ou removê-la conforme necessário.MessageBox.Show(@"Updated successfully");
            String cortar  = "bin\\Debug\\jogopalavrascruzadas.exe";
            String path_exe = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String path_db = path_exe.Replace(cortar, "dados\\dados.mdf");
            this.palavrasTableAdapter.Fill(this.dadosDataSet2.palavras);
            conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + path_db + 
                                     ";Integrated Security=True;Connect Timeout=30");
            conn.Open();

            listar();
        }


        private void listar()
        {
            SqlCommand sql = new SqlCommand("Select * from [dbo].[palavras] order by palavra", conn);
            SqlDataReader sqlr = sql.ExecuteReader();
            
            Boolean aceito = false;
            while (sqlr.Read())
            {
                aceito = true;            
                dataGridView1.Rows.Add(sqlr["palavra"], sqlr["dica"]);
            }
            sqlr.Close();
        }

        private void salvar() {

            SqlCommand command = new SqlCommand("Insert into  [dbo].[palavras] (id, palavra, dica) values (@id, @pal, @dica)", conn);
            command.Parameters.AddWithValue("@id",   dataGridView1.Rows.Count);
            command.Parameters.AddWithValue("@pal",  txtID.Text);
            command.Parameters.AddWithValue("@dica", txtName.Text);
            
            command.ExecuteNonQuery();
           
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.palavrasTableAdapter.Fill(this.dadosDataSet2.palavras);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private int index = 0;
        
        /// <summary>
        /// Use for clear data after save
        /// </summary>
        void ClearData()
        {
            txtID.Clear();
            txtName.Clear();            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(txtID.Text, txtName.Text);
            ClearData();
            MessageBox.Show(@"Saved successfully");
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dataGridView1[0, index].Value = txtID.Text;
            dataGridView1[1, index].Value = txtName.Text;
            
            ClearData();
            MessageBox.Show(@"Updated successfully");

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(index);
            ClearData();
            MessageBox.Show(@"Deleted successfully");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView dgv = sender as System.Windows.Forms.DataGridView;

            DataGridViewRow row = dgv.SelectedRows[0];

            txtID.Text = row.Cells[0].Value.ToString();
            txtName.Text = row.Cells[1].Value.ToString();
            

            index = row.Index;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            dataGridView1[0, index].Value = txtID.Text;
            dataGridView1[1, index].Value = txtName.Text;
            

            ClearData();
            MessageBox.Show(@"Updated successfully");
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(txtID.Text, txtName.Text);
            salvar();
            ClearData();
            MessageBox.Show(@"Saved successfully");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
