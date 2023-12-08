using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace nwap3010
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlCommand cmdtedarik;

        string constr = "Data Source=.;Initial Catalog=NORTHWND;Integrated Security=True";
        private BindingSource bindingSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            try
            {
                using (con = new SqlConnection(constr))
                {
                    con.Open();
                    using (cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "INSERT INTO Products(ProductName, SupplierID, CategoryID, UnitPrice) " +
                                          "VALUES(@ProductName, @SupplierID, @CategoryID, @UnitPrice)";

                        cmd.Parameters.AddWithValue("@ProductName", txturunad.Text);
                        cmd.Parameters.AddWithValue("@SupplierID", cmbtedarik.SelectedValue);
                        cmd.Parameters.AddWithValue("@CategoryID", cmbkategori.SelectedValue);
                        cmd.Parameters.AddWithValue("@UnitPrice", nupbirimfiyat.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                tazele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluþtu: " + ex.Message);
            }
        }

        private void tazele()
        {
            using (con = new SqlConnection(constr))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM Products ORDER BY ProductID DESC";

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    bindingSource = new BindingSource();
                    bindingSource.DataSource = dt;

                    dataGridView1.DataSource = bindingSource;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (con = new SqlConnection(constr))
            {
                con.Open();
                con.Open();

                // Kategori bilgileri cmbkategori combosuna aktarýlýyor
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbkategori.ValueMember = "CategoryID";
                    cmbkategori.DisplayMember = "CategoryName";
                    cmbkategori.DataSource = dt;
                }

                // Tedarikçiler bilgileri cmbtedarik combosuna aktarýlýyor
                using (cmdtedarik = new SqlCommand())
                {
                    cmdtedarik.Connection = con;
                    cmdtedarik.CommandText = "SELECT SupplierID, CompanyName FROM Suppliers";
                    SqlDataAdapter da2 = new SqlDataAdapter(cmdtedarik);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    cmbtedarik.ValueMember = "SupplierID";
                    cmbtedarik.DisplayMember = "CompanyName";
                    cmbtedarik.DataSource = dt2;
                }

                con.Close();
            }
        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int productID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ProductID"].Value);

                    using (con = new SqlConnection(constr))
                    {
                        con.Open();
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = $"DELETE FROM Products WHERE ProductID = {productID}";
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tazele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediðiniz ürünü seçin.");
            }
        }

        private void btnguncel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int productID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ProductID"].Value);

                    using (con = new SqlConnection(constr))
                    {
                        con.Open();
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = $"UPDATE Products SET ProductName = @ProductName, " +
                                              $"SupplierID = @SupplierID, " +
                                              $"CategoryID = @CategoryID, " +
                                              $"UnitPrice = @UnitPrice " +
                                              $"WHERE ProductID = {productID}";

                            cmd.Parameters.AddWithValue("@ProductName", txturunad.Text);
                            cmd.Parameters.AddWithValue("@SupplierID", cmbtedarik.SelectedValue);
                            cmd.Parameters.AddWithValue("@CategoryID", cmbkategori.SelectedValue);
                            cmd.Parameters.AddWithValue("@UnitPrice", nupbirimfiyat.Value);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    tazele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediðiniz ürünü seçin.");
            }
        }

        private void btnbul_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txturunad.Text))
            {
                try
                {
                    using (con = new SqlConnection(constr))
                    {
                        con.Open();
                        using (cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = $"SELECT * FROM Products WHERE ProductName LIKE '%{txturunad.Text}%'";
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluþtu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir ürün adý girin.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
