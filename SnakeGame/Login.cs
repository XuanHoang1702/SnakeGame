using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TRAN-XUAN-HOANG\\SQLEXPRESS;Initial Catalog=SnakeGame;Integrated Security=True";
            string checkUserQuery = "SELECT COUNT(1) FROM Game WHERE user_name = @UserName";
            string loginQuery = "SELECT COUNT(1) FROM Game WHERE user_name = @UserName AND pass_word = @Password";
            string bestScoreQuery = "SELECT best_score FROM Game WHERE user_name = @UserName";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        int userExists = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                        if (userExists == 0)
                        {
                            MessageBox.Show("Account does not exist.");
                            return;
                        }
                    }

                    using (SqlCommand loginCommand = new SqlCommand(loginQuery, connection))
                    {
                        loginCommand.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        loginCommand.Parameters.AddWithValue("@Password", txtPassWord.Text);
                        int isValidLogin = Convert.ToInt32(loginCommand.ExecuteScalar());

                        if (isValidLogin == 1)
                        {
                            int bestScore = 0;
                            using (SqlCommand scoreCommand = new SqlCommand(bestScoreQuery, connection))
                            {
                                scoreCommand.Parameters.AddWithValue("@UserName", txtUserName.Text);
                                object result = scoreCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    bestScore = Convert.ToInt32(result);
                                }
                            }
                            MessageBox.Show("Login successful!");

                            Form1 form1 = new Form1(txtUserName.Text, bestScore);
                            this.Hide();
                            form1.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
