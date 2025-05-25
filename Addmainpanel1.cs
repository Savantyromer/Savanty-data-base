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
    public partial class Addmainpanel1 : Form
    {
        DataBase dataBase = new DataBase();
        string myConnectionString = "Database=museum_of_nathistory;Data Source=127.0.0.1;User Id=root;Password=Rooter123";
        public Addmainpanel1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            DataTable idexcursion = new DataTable();
            MySqlConnection connection_idexcursion = new MySqlConnection(myConnectionString);
            {
                MySqlCommand command = new MySqlCommand("select idexcursion from excursion", connection_idexcursion);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(idexcursion);
            }
            comboBox1.DataSource = idexcursion;
            comboBox1.DisplayMember = "idexcursion ";
            comboBox1.ValueMember = "idexcursion";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();


            MySqlCommand cmd = new MySqlCommand("insert into guide (FIO, salary, `number`, idexcursion) values (@FIO, @salary, @number, @idexcursion);", dataBase.getConnection());
            cmd.Parameters.Add("@FIO", MySqlDbType.VarChar).Value = textBox_FIO.Text;
            cmd.Parameters.Add("@salary", MySqlDbType.VarChar).Value = textBox_salary.Text;
            cmd.Parameters.Add("@number", MySqlDbType.VarChar).Value = textBox_number.Text;
            cmd.Parameters.Add("@idexcursion", MySqlDbType.VarChar).Value = comboBox1.SelectedValue;

            if (textBox_FIO.Text == "" || textBox_number.Text == "" || textBox_salary.Text == "")
            {
                MessageBox.Show("1.Введите ФИО, зарплату, номер и ID экскурсии.\r\n" 
                    , "Несоответствие форме добавления записи");
            }
            else if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Экскурсовод успешно добавлен.", "Добавление экскурсовода...");

                dataBase.closeConnection();

            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении экскурсовода.", "Ошибка при добавлении...");
            }
            dataBase.closeConnection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox_FIO.Text = "";
            textBox_salary.Text = "";
            textBox_number.Text = "";
        }

        private void Addmainpanel1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
