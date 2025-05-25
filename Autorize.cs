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
    //Форма для окна авторизации, прошлую удалил из-за проблем с базой данных
    public partial class Autorize : Form
    {

        DataBase dataBase = new DataBase();

        public Autorize()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pasField.PasswordChar = '●';
            pictureBox3.Visible = false;
            pasField.MaxLength = 50;
            logField.MaxLength = 50;
        }

        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void closedButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void closedButton_MouseEnter(object sender, EventArgs e)
        {
            closedButton.ForeColor = Color.Olive;
        }
        private void closedButton_MouseLeave(object sender, EventArgs e)
        {
            closedButton.ForeColor = Color.LightYellow;
        }
        
        //Это кнопка войти
        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = logField.Text;
            var passUser = pasField.Text;

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();

            string queryString = $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passUser}'";

           MySqlCommand command = new MySqlCommand(queryString, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы вошли в аккаунт!!!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainpanel1 frm2 = new mainpanel1();
                this.Hide();
                frm2.ShowDialog();
                this.Show();
            }
            else
                MessageBox.Show("Такого аккаунта нет в базе данных!!!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void buttonofreg_Click(object sender, EventArgs e)
        {
            RegisteredForm frm_reg = new RegisteredForm();
            frm_reg.Show();
            this.Hide();
        }

        private void clearBut_Click(object sender, EventArgs e)
        {
            pasField.Text = "";
            logField.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pasField.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pasField.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = false;
        }
    }
}
