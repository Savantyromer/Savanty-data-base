using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHM
{
    public partial class mainpanel1 : Form
    {
        DataBase dataBase = new DataBase();
        public static int id_ex;
        int selectedRow;

        public mainpanel1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void mainpanel1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("idguide", "ID");
            dataGridView1.Columns.Add("FIO", "ФИО");
            dataGridView1.Columns.Add("salary", "зарплата");
            dataGridView1.Columns.Add("number", "номер");
            dataGridView1.Columns.Add("idexcursion", "ID экскурсии");
            dataGridView1.Columns.Add("IsNew", String.Empty);
            dataGridView1.Columns[5].Visible = false;
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox10.Text = "";
        }
        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDouble(2), record.GetInt32(3), record.GetInt32(4), RowState.ModiviedNew);

        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            string querystring = $"SELECT * FROM guide;";
            MySqlCommand command = new MySqlCommand(querystring, dataBase.getConnection());
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

        private void closybut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
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
                textBox10.Text = row.Cells[4].Value.ToString();
            }
        }
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = "SELECT * FROM guide WHERE CONCAT(idguide, FIO, salary, `number`, idexcursion) LIKE '%" + textBox9.Text + "%'";
            MySqlCommand com = new MySqlCommand(searchString, dataBase.getConnection());

            dataBase.openConnection();

            MySqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
        }
        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
            }

        }
        private void Update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[5].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idguide = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"DELETE FROM guide WHERE idguide = {idguide}";

                    var command = new MySqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var idguide = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var FIO = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var salary = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var number = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    MySqlCommand command = new MySqlCommand("update guide set FIO = @FIO, salary = @salary, `number` = @number where idguide = @idguide", dataBase.getConnection());
                    command.Parameters.Add("@idguide", MySqlDbType.VarChar).Value = idguide;
                    command.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = FIO;
                    command.Parameters.Add("@salary", MySqlDbType.VarChar).Value = salary;
                    command.Parameters.Add("@number", MySqlDbType.VarChar).Value = number;

                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Addmainpanel1 addmainp = new Addmainpanel1();
            addmainp.Show();
        }
        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var idguide = textBox1.Text;
            var FIO = textBox2.Text;
            float salary;
            int number;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if ((int.TryParse(textBox4.Text, out number)) && (float.TryParse(textBox3.Text, out salary)))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(idguide, FIO, salary, number);
                    dataGridView1.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("id и цена должны иметь числовой формат!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            MySqlCommand command = new MySqlCommand("delete from guide where idguide = @idguide", dataBase.getConnection());
            command.Parameters.Add("@idguide", MySqlDbType.VarChar).Value = id_ex;
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно удалена.", "Удаление записи...");

            RefreshDataGrid(dataGridView1);

            dataBase.closeConnection();
            dataGridView1.ClearSelection();
        }
        
        public void Sumbody()
        {
            dataBase.openConnection();
            string query = "SELECT SUM(salary) FROM guide";
            using (MySqlCommand com = new MySqlCommand(query, dataBase.getConnection()))
            {
                object result = com.ExecuteScalar();
                label6.Text = " " + result.ToString();
            }
            dataBase.closeConnection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Sumbody();
        }
    }
}
