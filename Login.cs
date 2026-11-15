
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace morente_sample
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Enter your username and password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Database.EnsureUsersTable();

                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand command = new(
                    "SELECT COUNT(*) FROM Users WHERE Username = ? AND PasswordHash = ?",
                    connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", Database.HashPassword(password));

                if (Convert.ToInt32(command.ExecuteScalar()) == 1)
                {
                    MessageBox.Show("Login Successful!");

                    inventory dashboard = new inventory();
                    dashboard.Show();

                    this.Hide();
                    return;
                }

                MessageBox.Show(
                    "Invalid username or password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login failed: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            Signup signupForm = new Signup();
            signupForm.Show();
            this.Hide();
        }
    }
}
