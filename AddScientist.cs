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
    public partial class AddScientist : Form
    {
        DataBase dataBase = new DataBase();

        public AddScientist()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            dataBase.openConnection();

            var FIO = textBox2.Text;
            float salary;
            var degree = textBox4.Text;

            MySqlCommand cmd = new MySqlCommand("insert into scientist (FIO, salary, degree) values (@FIO, @salary, @degree);", dataBase.getConnection());
            cmd.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = textBox2.Text;
            cmd.Parameters.Add("@salary", MySqlDbType.VarChar).Value = textBox3.Text;
            cmd.Parameters.Add("@degree", MySqlDbType.VarChar).Value = textBox4.Text;

            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("1.Введите ФИО учёного.\r\n" +
                    "2.Введите размер заработной платы.\r\n" + "3.Введите научную степень.\r\n", "Несоответствие форме добавления записи");
            }
            else if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Учёный успешно добавлен.", "Добавление учёного...");

                dataBase.closeConnection();

            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении учёного.", "Ошибка при добавлении...");
            }
            dataBase.closeConnection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
