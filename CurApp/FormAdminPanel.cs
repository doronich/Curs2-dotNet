using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CurApp
{
    public partial class FormAdminPanel : Form
    {
        DataSet ds = new DataSet();
        string connectionString = FormAuthorization.connectionString;//System.Configuration.ConfigurationManager.ConnectionStrings["CurApp.Properties.Settings.dbCuratorConnectionString"].ConnectionString;
        string sql = "SELECT * FROM Users";
        FormAuthorization authorization = null;
        public FormAdminPanel(FormAuthorization formAuthorization)
        {
            InitializeComponent();
            authorization = formAuthorization;
            LoadDataGrid();
            buttonSave.Click += (sender, args) => SaveChanges();
            this.FormClosing += (sender, args) => authorization.Show();
            buttonEncr.Click += (sender, args) => Encrypt();
        }

        private void Encrypt()
        {
            if (textBoxPass.Text != "")
            {
                Model.BL.Model model = new Model.BL.Model(connectionString);
                textBoxPass.Text = model.Encrypting(textBoxPass.Text);
            }
        }

        private void SaveChanges()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDataGrid()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, con))
                    {
                        adapter.Fill(ds);
                    }
                }
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                dataGridView1.Columns["pwd"].Width = 314;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBoxPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if ((c >= '0' && c <= '9'))
            {
                return;
            }

            if ((c < 'A' || c > 'z') && c != '\b' && c != '.')
            {
                e.Handled = true;
            }
            if (c == ' ')
            {
                e.Handled = true;
            }
        }
    }
}
