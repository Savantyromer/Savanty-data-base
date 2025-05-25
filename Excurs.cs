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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModiviedNew,
        Deleted
    }
    public partial class Excurs : Form
    {
        DataBase dataBase = new DataBase();
        public static int id_ex;
        int selectedRow;
        public Excurs()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView2);
        }
        private void CreateColumns()
        {
            dataGridView2.Columns.Add("idexcursion", "ID");
            dataGridView2.Columns.Add("timeline", "Длительность");
            dataGridView2.Columns.Add("group", "Групповая");
            dataGridView2.Columns.Add("individual", "Индивидуальная");
            dataGridView2.Columns.Add("IsNew", String.Empty);
            dataGridView2.Columns[4].Visible = false;
        }

        private void ClearFields()
        {
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetInt32(2), record.GetInt32(3), RowState.ModiviedNew);
        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();
            string pups = $"select * from excursion";
            MySqlCommand command = new MySqlCommand(pups, dataBase.getConnection());
            dataBase.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            string i;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];
                i = row.Cells[0].Value.ToString();
                id_ex = Convert.ToInt32(i);

                textBox5.Text = row.Cells[0].Value.ToString();
                textBox6.Text = row.Cells[1].Value.ToString();
                textBox7.Text = row.Cells[2].Value.ToString();
                textBox8.Text = row.Cells[3].Value.ToString();

            }
        }
        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = "select * from excursion where concat (idexcursion, timeline, `group`, individual) like '%" + textBox11.Text + "%'";
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
            int index = dataGridView2.CurrentCell.RowIndex;

            dataGridView2.Rows[index].Visible = false;

            if (dataGridView2.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView2.Rows[index].Cells[4].Value = RowState.Deleted;
            }

        }
        private void Update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView2.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView2.Rows[index].Cells[4].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var idexcursion = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from excursion where idexcursion = {idexcursion}";

                    var command = new MySqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var idexcursion = dataGridView2.Rows[index].Cells[0].Value.ToString();
                    var timeline = dataGridView2.Rows[index].Cells[1].Value.ToString();
                    var group = dataGridView2.Rows[index].Cells[2].Value.ToString();
                    var individual = dataGridView2.Rows[index].Cells[3].Value.ToString();

                    MySqlCommand command = new MySqlCommand("update excursion set timeline = @timeline, `group` = @group, individual = @individual where idexcursion = @idexcursion", dataBase.getConnection());
                    command.Parameters.Add("@idexcursion", MySqlDbType.VarChar).Value = idexcursion;
                    command.Parameters.Add("@timeline", MySqlDbType.VarChar).Value = timeline;
                    command.Parameters.Add("@group", MySqlDbType.VarChar).Value = group;
                    command.Parameters.Add("@individual", MySqlDbType.VarChar).Value = individual;

                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView2);
            ClearFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddExcurs addex = new AddExcurs();
            addex.Show();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            MySqlCommand command = new MySqlCommand("delete from excursion where idexcursion = @idexcursion", dataBase.getConnection());
            command.Parameters.Add("@idexcursion", MySqlDbType.VarChar).Value = id_ex;
            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно удалена.", "Удаление записи...");

            RefreshDataGrid(dataGridView2);

            dataBase.closeConnection();
            dataGridView2.ClearSelection();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;

            var idexcursion = textBox5.Text;
            int timeline;
            int group;
            int individual;

            if (dataGridView2.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if ((int.TryParse(textBox6.Text, out timeline)) && (int.TryParse(textBox7.Text, out group)) && (int.TryParse(textBox8.Text, out individual)))
                {
                    dataGridView2.Rows[selectedRowIndex].SetValues(idexcursion, timeline, group, individual);
                    dataGridView2.Rows[selectedRowIndex].Cells[4].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("значение длительности должно иметь числовой формат!");
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        public void Sumbody()
        {
            dataBase.openConnection();
            string query = "SELECT MAX(timeline) FROM excursion";
            using (MySqlCommand com = new MySqlCommand(query, dataBase.getConnection()))
            {
                object result = com.ExecuteScalar();
                label2.Text = " " + result.ToString();
            }
            dataBase.closeConnection();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Sumbody();
        }

    }
}
