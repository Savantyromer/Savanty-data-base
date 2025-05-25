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
    public partial class AddRestorer : Form
    {

        DataBase dataBase = new DataBase();
        public AddRestorer()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var FIO = textBox2.Text;
            float salary;

            MySqlCommand cmd = new MySqlCommand("insert into restorer (FIO, salary) values (@FIO, @salary);", dataBase.getConnection());
            cmd.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = textBox2.Text;
            cmd.Parameters.Add("@salary", MySqlDbType.VarChar).Value = textBox3.Text;

            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("1.Введите ФИО реставратора.\r\n" +
                    "2.Введите значение заработной платы.\r\n", "Несоответствие форме добавления записи");
            }
            else if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Реставратор успешно добавлен.", "Добавление реставратора...");
                dataBase.closeConnection();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении реставратора.", "Ошибка при добавлении...");
            }
            dataBase.closeConnection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
