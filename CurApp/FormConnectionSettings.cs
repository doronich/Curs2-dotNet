using System;
using System.Windows.Forms;

namespace CurApp
{
    public partial class FormConnectionSettings : Form
    {
        public FormConnectionSettings(string conn)
        {
            InitializeComponent();
            ConnectionString = conn;

            textBoxConnect.Text = "Data Source=(local);Initial Catalog = dbCurator; Integrated Security = False; User ID = login; Password=***";
        }

        public string ConnectionString { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionString = "Data Source = " + textBoxDataSource.Text + ";Initial Catalog = " + textBoxInitial.Text + "; Integrated Security = " + textBoxInt.Text +
                "; User ID = " + textBoxUser.Text + "; Password = " + textBoxPass.Text;
            textBoxConnect.Text = ConnectionString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConnectionString = "Data Source=(local);Initial Catalog = dbCurator; Integrated Security = False; User ID = test; Password=123";
        }
    }
}
