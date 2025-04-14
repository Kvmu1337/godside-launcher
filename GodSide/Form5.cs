using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Mysqlx.Cursor;



namespace GodSide
{
    public partial class Form5 : Form
    {
        private string connstring;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        public Form5(string connectionString)
        {
            InitializeComponent();
            connstring = connectionString;
            LoadSettings();
        }



        private void LoadSettings()
        {
            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                try
                {
                    conn.Open();

                    // Query to get both 'detection' and 'version' values from the 'settings' table
                    string query = "SELECT detection, version FROM settings LIMIT 1";  // Assuming there is only one row
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Execute the query and fetch the result
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fetch 'detection' value and update label22
                            string detectionValue = reader["detection"].ToString();
                            label20.Text = detectionValue;

                            if (detectionValue.Equals("UNDETECTED", StringComparison.OrdinalIgnoreCase))
                            {
                                label20.ForeColor = System.Drawing.Color.LightGreen;
                            }
                            else if (detectionValue.Equals("DETECTED", StringComparison.OrdinalIgnoreCase))
                            {
                                label20.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                label20.ForeColor = System.Drawing.Color.SlateBlue; // Default color
                            }

                            // Fetch 'version' value and update label22
                            string versionValue = reader["version"].ToString();
                            label22.Text = versionValue;

                            // Update label22 color based on version value (for example)
                            
                            label22.ForeColor = System.Drawing.Color.SlateBlue; // Default color
                     
                        }
                        else
                        {
                            MessageBox.Show("No settings found in the database.");
                        }
                    }

                    string countUsersQuery = "SELECT COUNT(*) FROM loginlauncher_users";
                    MySqlCommand countUsersCmd = new MySqlCommand(countUsersQuery, conn);

                    // Execute the query and fetch the result
                    object usersCountResult = countUsersCmd.ExecuteScalar();
                    int usersCount = Convert.ToInt32(usersCountResult);

                    // Update label11 with the users count
                    label14.Text = $"{usersCount}";
                    label14.ForeColor = System.Drawing.Color.SlateBlue; // Default color


                    // Query to get the 'username' and 'validLicense' from the 'loginlauncher_users' table
                    string usersQuery = "SELECT username, validLicense, rank FROM loginlauncher_users LIMIT 1";  // Modify the condition as needed
                    MySqlCommand usersCmd = new MySqlCommand(usersQuery, conn);

                    // Execute the query and fetch the result
                    using (MySqlDataReader usersReader = usersCmd.ExecuteReader())
                    {
                        if (usersReader.Read())
                        {
                            // Fetch 'username' value and update label8
                            string usernameValue = usersReader["username"].ToString();
                            label8.Text = usernameValue;
                            label8.ForeColor = System.Drawing.Color.SlateBlue; // Default color


                            DateTime validLicenseValue = Convert.ToDateTime(usersReader["validLicense"]);
                            DateTime currentDate = DateTime.Now;

                            // Calculate the difference in days
                            TimeSpan difference = validLicenseValue - currentDate;
                            int totalDaysLeft = (int)difference.TotalDays;

                            // Calculate the difference in months and days
                            int monthsLeft = ((validLicenseValue.Year - currentDate.Year) * 12) + validLicenseValue.Month - currentDate.Month;
                            int daysLeft = validLicenseValue.Day - currentDate.Day;

                            if (daysLeft < 0)
                            {
                                monthsLeft--;
                                DateTime previousMonth = validLicenseValue.AddMonths(-1);
                                daysLeft += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                            }

                            if (monthsLeft >= 1)
                            {
                                label9.Text = $"{monthsLeft} months left";
                                label9.ForeColor = System.Drawing.Color.DarkGray;
                                label4.Text = "active";
                            }
                            else if (totalDaysLeft > 5)
                            {
                                label9.Text = $"{totalDaysLeft} days left";
                                label9.ForeColor = System.Drawing.Color.DarkGray;
                                label4.Text = "active";
                            }
       
                            else
                            {
                                label9.Text = "Expired";
                                label9.ForeColor = System.Drawing.Color.Red;
                                label4.Text = "";
                            }


                            string rankValue = usersReader["rank"].ToString();
                            label6.Text = rankValue;

                            if (rankValue == "Developer")
                            {
                                label6.ForeColor = System.Drawing.Color.SlateBlue; 
                            }
                            else if (rankValue == "V.I.P")
                            {
                                label6.ForeColor = System.Drawing.Color.Gold;
                            }
                            else if (rankValue == "Member")
                            {
                                label6.ForeColor = System.Drawing.Color.LightGreen;
                            }
                            else
                            {
                                label6.ForeColor = System.Drawing.Color.DarkGray;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No user found in the database.");
                        }
                      }

                    // Query to get the count of admin users from the 'loginlauncher_users' table
                    string countAdminsQuery = "SELECT COUNT(*) FROM loginlauncher_users WHERE IsAdmin = TRUE";
                    MySqlCommand countAdminsCmd = new MySqlCommand(countAdminsQuery, conn);

                    // Execute the query and fetch the result
                    object adminsCountResult = countAdminsCmd.ExecuteScalar();
                    int adminsCount = Convert.ToInt32(adminsCountResult);

                    // Update label12 with the admin users count
                    label12.Text = $"{adminsCount}";
                    label12.ForeColor = System.Drawing.Color.SlateBlue; // Default color
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }
        public class WebClientWithTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wc = base.GetWebRequest(address);
                wc.Timeout = 5000;
                return wc;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Set the DLL path and process name
            string dllPath = @"D:\Downloads\Caesar.dll"; // Update this path to where your DLL is located
            string procName = "hl.exe"; // Replace with the actual process name

            // Check if the DLL file exists
            if (!System.IO.File.Exists(dllPath))
            {
                MessageBox.Show("DLL file not found. Please check the path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

         

            // Attempt to inject the DLL
            bool result = NativeMethods.injectDll(dllPath, procName);

            if (result)
            {
                MessageBox.Show("DLL injected successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();  // Exit the application if DLL injection was successful
            }
            else
            {
                MessageBox.Show("DLL injection failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click_1(object sender, EventArgs e)
        {

        }

        private void label15_Click_1(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click_2(object sender, EventArgs e)
        {

        }
    }
}
