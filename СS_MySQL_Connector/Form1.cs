using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace СS_MySQL_Connector
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        MySqlConnection sqlConnection = new MySqlConnection();
        MySqlCommand sqlCommand = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        String sqlQuery;
        MySqlDataAdapter DtA = new MySqlDataAdapter();
        MySqlDataReader sqlRd;

        DataSet DS = new DataSet();
        String server = "127.0.0.1"; 
        String username = "margaret"; 
        String password = "margo"; 
        String database = "marharyta_prachuk"; 

        public Form1()
        {
            InitializeComponent();
        }
        private void uploadData()
        {
            sqlConnection.ConnectionString = "server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database;
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "SELECT * FROM marharyta_prachuk.employees";

            sqlRd = sqlCommand.ExecuteReader();
            sqlDt.Load(sqlRd);
            sqlRd.Close();
            sqlConnection.Close();
            dataGridView1.DataSource = sqlDt;
        }
  
        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult iExit;
            try 
            {
                iExit = MessageBox.Show(" Пiдтвердiть вихiд", "База даних працівників",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (iExit == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(Control c in panel4.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
                textBoxSearch.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                int height = dataGridView1.Height;
                dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 2;
                bitmap = new Bitmap(dataGridView1.Width, dataGridView1.Height);
                dataGridView1.DrawToBitmap(bitmap, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                printPreviewDialog1.ShowDialog();
                dataGridView1.Height = height;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uploadData();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            sqlConnection.ConnectionString = "server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database;

            try
            {
                sqlConnection.Open();
                sqlQuery = "insert into marharyta_prachuk.employees (id, full_name, telephone_number, position, hourly_rate, hire_date)" +
                    "values('" + textBoxID.Text + "' , '" + textBoxFullName.Text + "' , '" + textBoxTelNumber.Text + "' , '" +
                    textBoxPosition.Text + "' , '" + textBoxHourlyRate.Text + "' , '" + textBoxHireDate.Text + "')";

                sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
                sqlRd = sqlCommand.ExecuteReader();
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            uploadData();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            sqlConnection.ConnectionString = "server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database;
            sqlConnection.Open();

            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = "Update marharyta_prachuk.employees set id = @id, " +
                    "full_name = @full_name , telephone_number = @telephone_number, position = @position, hourly_rate = @hourly_rate, " +
                    "hire_date = @hire_date where id = @id";
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", textBoxID.Text);
                sqlCommand.Parameters.AddWithValue("@full_name", textBoxFullName.Text);
                sqlCommand.Parameters.AddWithValue("@telephone_number", textBoxTelNumber.Text);
                sqlCommand.Parameters.AddWithValue("@position", textBoxPosition.Text);
                sqlCommand.Parameters.AddWithValue("@hourly_rate", textBoxHourlyRate.Text);
                sqlCommand.Parameters.AddWithValue("@hire_date", textBoxHireDate.Text);

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                uploadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBoxID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBoxFullName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBoxTelNumber.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBoxPosition.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBoxHourlyRate.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBoxHireDate.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            sqlConnection.ConnectionString = "server=" + server + ";user id=" + username + ";password=" + password + ";database=" + database;
            sqlConnection.Open();

            try
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "delete from marharyta_prachuk.employees where id = @id";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
                sqlConnection.Close();
                foreach(DataGridViewRow item in this.dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index);
                }

                foreach (Control c in panel4.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
                textBoxSearch.Text = "";

                uploadData();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

   
        private void textBoxSearch_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;
                dv.RowFilter = string.Format("Employees like '%{0}%'", textBoxSearch.Text);
                dataGridView1.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
