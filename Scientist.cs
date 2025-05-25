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

namespace NHM
{
    public partial class Scientist : Form
    {
        DataBase dataBase = new DataBase();
        public static int id_ex;
        int selectedRow;

        public Scientist()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Scientist_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("idscientist", "ID");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("salary", "зарплата");
            dataGridView1.Columns.Add("degree", "степень");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[4].Visible = false;
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), RowState.ModiviedNew);

        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            string queryString = $"select * from scientist";
            MySqlCommand command = new MySqlCommand(queryString, dataBase.getConnection());
            dataBase.openConnection();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Restorer rest = new Restorer();
                Scientist scien = new Scientist();
                Excurs form1 = new Excurs();
                mainpanel1 pan_guide = new mainpanel1();
                this.Hide();
                pan_guide.ShowDialog();
                this.Show();
                rest.Close();
                form1.Close();
                scien.Close();
            }
            if (comboBox1.SelectedIndex == 1)
            {
                Restorer rest = new Restorer();
                Scientist scien = new Scientist();
                Excurs form1 = new Excurs();
                mainpanel1 pan_guide = new mainpanel1();
                this.Hide();
                form1.ShowDialog();
                this.Show();
                rest.Close();
                scien.Close();
                pan_guide.Close();
            }
            if (comboBox1.SelectedIndex == 2)
            {
                Restorer rest = new Restorer();
                Scientist scien = new Scientist();
                Excurs form1 = new Excurs();
                mainpanel1 pan_guide = new mainpanel1();
                this.Hide();
                scien.ShowDialog();
                this.Show();
                rest.Close();
                form1.Close();
                pan_guide.Close();
            }
            if (comboBox1.SelectedIndex == 3)
            {
                Restorer rest = new Restorer();
                Scientist scien = new Scientist();
                Excurs form1 = new Excurs();
                mainpanel1 pan_guide = new mainpanel1();
                this.Hide();
                rest.ShowDialog();
                this.Show();
                scien.Close();
                form1.Close();
                pan_guide.Close();
            }
        }

        private void closedbut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SumbodyNew()
        {
            dataBase.openConnection();
            string strcom = "SELECT SUM(salary) FROM scientist";
            using (MySqlCommand comm = new MySqlCommand(strcom, dataBase.getConnection()))
            {
                object res = comm.ExecuteScalar();
                label6.Text = " " + res.ToString();
            }
            dataBase.closeConnection();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SumbodyNew();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            string i;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                i = row.Cells[0].Value.ToString();
                id_ex = Convert.ToInt32(i);

                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from scientist where concat (idscientist, FIO, salary, degree) like '%" + textBox9.Text + "%'";
            MySqlCommand com = new MySqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            MySqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
            }

        }

        private void Update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idscientist = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from scientist where idscientist = {idscientist}";

                    var command = new MySqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var idscientist = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var FIO = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var salary = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var degree = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    MySqlCommand command = new MySqlCommand("update scientist set FIO = @FIO, salary = @salary, degree = @degree where idscientist = @idscientist", dataBase.getConnection());
                    command.Parameters.Add("@idscientist", MySqlDbType.VarChar).Value = idscientist;
                    command.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = FIO;
                    command.Parameters.Add("@salary", MySqlDbType.VarChar).Value = salary;
                    command.Parameters.Add("@degree", MySqlDbType.VarChar).Value = degree;

                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            MySqlCommand command = new MySqlCommand("delete from scientist where idscientist = @idscientist", dataBase.getConnection());
            command.Parameters.Add("@idscientist", MySqlDbType.VarChar).Value = id_ex;
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно удалена.", "Удаление записи...");

            RefreshDataGrid(dataGridView1);

            dataBase.closeConnection();
            dataGridView1.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var idscientist = textBox1.Text;
            var FIO = textBox2.Text;
            float salary;
            var degree = textBox4.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if ((float.TryParse(textBox3.Text, out salary)))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(idscientist, FIO, salary, degree);
                    dataGridView1.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Заработная плата должна иметь числовой формат!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddScientist addsci = new AddScientist();
            addsci.Show();
        }
    }
}
