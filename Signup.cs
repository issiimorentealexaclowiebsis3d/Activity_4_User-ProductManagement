
using System;
using System.Data.OleDb;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace morente_sample
{
    public partial class Signup : Form
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fullName = textBox2.Text.Trim();
            string username = textBox1.Text.Trim();
            string password = textBox3.Text;
            string rePassword = textBox4.Text;


            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(rePassword))
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Registration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (password != rePassword)
            {
                MessageBox.Show(
                    "Passwords do not match.",
                    "Registration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                Database.EnsureUsersTable();

                using OleDbConnection connection = Database.GetConnection();
                connection.Open();

                using OleDbCommand checkCommand = new(
                    "SELECT COUNT(*) FROM Users WHERE Username = ?",
                    connection);
                checkCommand.Parameters.AddWithValue("@Username", username);

                if (Convert.ToInt32(checkCommand.ExecuteScalar()) > 0)
                {
                    MessageBox.Show(
                        "Username already exists.",
                        "Registration",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using OleDbCommand insertCommand = new(
                    "INSERT INTO Users (FullName, Username, PasswordHash) VALUES (?, ?, ?)",
                    connection);
                insertCommand.Parameters.AddWithValue("@FullName", fullName);
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@PasswordHash", Database.HashPassword(password));
                insertCommand.ExecuteNonQuery();

                MessageBox.Show(
                    "Registration successful!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                textBox2.Clear();
                textBox1.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Registration failed: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            login loginForm = new login();

            loginForm.Show();

            this.Hide();
        }
    }
}

