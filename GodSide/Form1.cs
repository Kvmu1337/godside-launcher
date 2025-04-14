using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GodSide
{
    public partial class Form1 : Form
    {

        string DATABASEIP = "localhost";
        string DATABASEUSER = "root";
        string DATABASEPASS = "";
        string DATABASENAME = "godside";

        string externalip = new System.Net.WebClient().DownloadString("http://icanhazip.com");

        WebClient WebClient = new WebClient();
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            LoadSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.name != string.Empty)
            {
                textBox1.Text = Properties.Settings.Default.name;
                textBox2.Text = Properties.Settings.Default.password;
            }
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string connstring = $"server={DATABASEIP};userid={DATABASEUSER};password={DATABASEPASS};database={DATABASENAME}";
            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                try
                {
                    conn.Open();

                    // Query to retrieve user information
                    string query = "SELECT * FROM loginlauncher_users WHERE username = @username AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", textBox1.Text);
                    cmd.Parameters.AddWithValue("@password", textBox2.Text);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "loginlauncher_users");
                    DataTable dt = ds.Tables["loginlauncher_users"];

                   

                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        string dbIp = row["externalip"].ToString();
                        int isAllowed = Convert.ToInt32(row["IsAllowed"]);

                        // Verify if user is allowed and IP matches
                        //bool isIpCorrect = dbIp == externalip;
                        bool isUserAllowed = isAllowed == 1;

                        if (isUserAllowed)
                        {
                            Properties.Settings.Default.name = textBox1.Text;
                            Properties.Settings.Default.password = textBox2.Text;
                            Properties.Settings.Default.Save();

                            this.Opacity = 0;
                            this.Visible = false;
                            this.ShowInTaskbar = false;
                            this.ShowIcon = false;

                            Form5 f5 = new Form5(connstring);
                            f5.Show();
                        }

                        else
                        {
                            MessageBox.Show("You don't have a valid license or it has expired. Join our Discord for support.");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("The username or password is incorrect.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void LoadSettings()
        {
            string connstring = $"server={DATABASEIP};userid={DATABASEUSER};password={DATABASEPASS};database={DATABASENAME}";
            using (MySqlConnection conn = new MySqlConnection(connstring))
            {

                try
                {
                    conn.Open();
                    // Query to get the count of users from the 'loginlauncher_users' table
                    string countUsersQuery = "SELECT COUNT(*) FROM loginlauncher_users";
                    MySqlCommand countUsersCmd = new MySqlCommand(countUsersQuery, conn);

                    // Execute the query and fetch the result
                    object usersCountResult = countUsersCmd.ExecuteScalar();
                    int usersCount = Convert.ToInt32(usersCountResult);

                    // Update label11 with the users count
                    label11.Text = $"{usersCount}";
                    label11.ForeColor = System.Drawing.Color.SlateBlue; // Default color
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool mouseDown;
        private Point lastLocation;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void alphaBlendTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
            string url = "https://dsc.gg/godside";

            
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you need help, make a ticket on discord.");
        }

        private void label11_Click(object sender, EventArgs e)
        {
            
        }
    }
}
