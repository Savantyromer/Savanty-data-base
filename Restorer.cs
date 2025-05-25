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
    public partial class Restorer : Form
    {
        DataBase dataBase = new DataBase();
        public static int id_ex;
        int selectedRow;

        public Restorer()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Restorer_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("idrestorer", "ID");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("salary", "зарплата");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[3].Visible = false;
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDouble(2), RowState.ModiviedNew);

        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            string queryString = $"select * from restorer";
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

        private void cloosed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Argsmeth();
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
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddRestorer addrest = new AddRestorer();
            addrest.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from restorer where concat (idrestorer, FIO, salary) like '%" + textBox9.Text + "%'";
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
                dataGridView1.Rows[index].Cells[3].Value = RowState.Deleted;
            }

        }

        private void Update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[3].Value;

                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idrestorer = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from restorer where idrestorer = {idrestorer}";

                    var command = new MySqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var idrestorer = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var FIO = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var salary = dataGridView1.Rows[index].Cells[2].Value.ToString();

                    MySqlCommand command = new MySqlCommand("update restorer set FIO = @FIO, salary = @salary where idrestorer = @idrestorer", dataBase.getConnection());
                    command.Parameters.Add("@idrestorer", MySqlDbType.VarChar).Value = idrestorer;
                    command.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = FIO;
                    command.Parameters.Add("@salary", MySqlDbType.VarChar).Value = salary;

                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            MySqlCommand command = new MySqlCommand("delete from restorer where idrestorer = @idrestorer", dataBase.getConnection());
            command.Parameters.Add("@idrestorer", MySqlDbType.VarChar).Value = id_ex;
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

            var idrestorer = textBox1.Text;
            var FIO = textBox2.Text;
            float salary;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if ((float.TryParse(textBox3.Text, out salary)))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(idrestorer, FIO, salary);
                    dataGridView1.Rows[selectedRowIndex].Cells[3].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Заработная плата должна иметь числовой формат!");
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();   
        }

        private void Argsmeth()
        {
            string query = "SELECT AVG(salary) FROM restorer";
            using (MySqlCommand com = new MySqlCommand(query, dataBase.getConnection()))
            {
                object result = com.ExecuteScalar();
                label6.Text = " " + result.ToString();
            }
        }

    }
}
