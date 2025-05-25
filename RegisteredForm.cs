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
using System.Data.SqlClient;

namespace NHM
{
    public partial class RegisteredForm : Form
    {
        DataBase dataBase = new DataBase();
        public RegisteredForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void RegisteredForm_Load(object sender, EventArgs e)
        {
            pasField2.PasswordChar = '●';
            pictureBox3.Visible = false;
        }

        //Кнопка регистрации
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkuser())
            {
                return;
            }

            var login = logField2.Text;
            var password = pasField2.Text;

            string querystring = $"insert into register(login_user, password_user) values('{login}', '{password}')";

            MySqlCommand command = new MySqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт создан!", "Успех!");
                Autorize frm_login = new Autorize();
                this.Hide();
                frm_login.ShowDialog();

            }
            else
            {
                MessageBox.Show("Аккаунт не был создан");
            }
            dataBase.closeConnection();
        }

        private Boolean checkuser()
        {
            var loginUser = logField2.Text;
            var passUser = pasField2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passUser}'";

            /*MySqlCommand command = new MySqlCommand(querystring, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);*/

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void closedButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Кнопка очистки строк с паролем и логином
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pasField2.Text = "";
            logField2.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pasField2.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pasField2.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = false;
        }
    }
}
