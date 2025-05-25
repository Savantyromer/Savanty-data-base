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
    public partial class AddExcurs : Form
    {
        DataBase dataBase = new DataBase();
        public AddExcurs()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            int timeline;
            string group;
            string individual;

            MySqlCommand cmd = new MySqlCommand("insert into excursion (timeline, `group`, individual) values (@timeline, @group, @individual);", dataBase.getConnection());
            cmd.Parameters.Add("@timeline", MySqlDbType.VarChar).Value = textBox_duration.Text;
            cmd.Parameters.Add("@group", MySqlDbType.VarChar).Value = textBox_group.Text;
            cmd.Parameters.Add("@individual", MySqlDbType.VarChar).Value = textBox_individual.Text;

            if (textBox_duration.Text == "" || textBox_group.Text == "" || textBox_individual.Text == "")
            {
                    MessageBox.Show("1.Введите длительность экскурсии.\r\n" +
                        "2.Обозначьте вид экскурсии (групповая/индивидуальная).\r\n", "Несоответствие форме добавления записи");
            }
            else if (cmd.ExecuteNonQuery() == 1)
            {
                    MessageBox.Show("Экскурсия успешно добавлена.", "Добавление экскурсии...");

                    dataBase.closeConnection();

            }
            else
            {
                    MessageBox.Show("Произошла ошибка при добавлении экскурсии.", "Ошибка при добавлении...");
            }
                dataBase.closeConnection();
        }

        private void AddExcurs_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox_duration.Text = "";
            textBox_group.Text = "";
            textBox_individual.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
