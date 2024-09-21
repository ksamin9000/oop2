using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace oop2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Insert button event handler
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-MEBL3FSE\\SQLEXPRESS;Initial Catalog=oop2;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    con.Open(); // Open the database connection
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ut (ID, Name) VALUES (@ID, @Name)", con))
                    {
                        if (ValidateInput()) // Check if inputs are valid
                        {
                            int id;
                            if (int.TryParse(textBox1.Text, out id)) // Validate ID is a number
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@Name", textBox2.Text); // textBox2 is for Name (nvarchar)

                                cmd.ExecuteNonQuery(); // Execute the SQL Insert command
                                MessageBox.Show("Record successfully inserted");

                                LoadData(); // Refresh DataGridView after insert
                            }
                            else
                            {
                                MessageBox.Show("Invalid ID. Please enter a valid number.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Delete button event handler
        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-MEBL3FSE\\SQLEXPRESS;Initial Catalog=oop2;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    con.Open(); // Open the database connection
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM ut WHERE ID = @ID", con))
                    {
                        if (ValidateDeleteInput()) // Check if ID input is valid
                        {
                            int id;
                            if (int.TryParse(textBox1.Text, out id)) // Validate that ID is a number
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the delete command

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record successfully deleted");
                                    LoadData(); // Refresh DataGridView after delete
                                }
                                else
                                {
                                    MessageBox.Show("No record found with the specified ID.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid ID. Please enter a valid number.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Update button event handler
        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-MEBL3FSE\\SQLEXPRESS;Initial Catalog=oop2;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    con.Open(); // Open the database connection
                    using (SqlCommand cmd = new SqlCommand("UPDATE ut SET Name = @Name WHERE ID = @ID", con))
                    {
                        if (ValidateInput()) // Check if inputs are valid
                        {
                            int id;
                            if (int.TryParse(textBox1.Text, out id)) // Validate that ID is a number
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@Name", textBox2.Text); // Update Name

                                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the update command

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record successfully updated");
                                    LoadData(); // Refresh DataGridView after update
                                }
                                else
                                {
                                    MessageBox.Show("No record found with the specified ID.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid ID. Please enter a valid number.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Search button event handler
        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-MEBL3FSE\\SQLEXPRESS;Initial Catalog=oop2;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    con.Open(); // Open the database connection
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM ut WHERE ID = @ID OR Name LIKE @Name", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", textBox1.Text); // Search by ID
                        cmd.Parameters.AddWithValue("@Name", "%" + textBox2.Text + "%"); // Search by Name (partial match)

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt); // Fill the DataTable with search results

                        dataGridView1.DataSource = dt; // Bind DataTable to DataGridView

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No records found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Method to load data into DataGridView
        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-MEBL3FSE\\SQLEXPRESS;Initial Catalog=oop2;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ut", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Method to validate input fields for insert/update operations
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter values for both ID and Name.");
                return false;
            }
            return true;
        }

        // Method to validate input fields for delete operation
        private bool ValidateDeleteInput()
        {
            if (string.IsNullOrEmpty(textBox1.Text)) // Ensure ID is not empty
            {
                MessageBox.Show("Please enter an ID to delete the record.");
                return false;
            }
            return true;
        }
    }
}
