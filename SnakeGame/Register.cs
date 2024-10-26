using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TRAN-XUAN-HOANG\\SQLEXPRESS;Initial Catalog=SnakeGame;Integrated Security=True";
            string query = "INSERT INTO Game (user_name, pass_word) VALUES (@UserName, @Password)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", txtUserName.Text);
                    command.Parameters.AddWithValue("@Password", txtPassword.Text);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Register Success!");
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Register fail! Please try agian");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
