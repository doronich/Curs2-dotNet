using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurApp
{
    public interface IView
    {
        void ShowForm();
        ApplicationContext CloseForm();
    }



    public partial class FormAuthorization : Form, IView
    {
        FormMain main;

        internal static string connectionString = "Data Source=(local);Initial Catalog = dbCurator; Integrated Security = False; User ID = test; Password=123";//ConfigurationManager.ConnectionStrings["CurApp.Properties.Settings.dbCuratorConnectionString"].ConnectionString;
        ApplicationContext _context;
        public FormAuthorization(ApplicationContext context)
        {
            _context = context;
            InitializeComponent();
            this.FormClosing += (sender, args) => CloseForm();
        }

        public void ShowForm()
        {
            _context.MainForm = this;
            Application.Run(_context);
        }

        public ApplicationContext CloseForm()
        {
            Application.Exit();
            return _context;
        }

        public bool IsConfirm { get; set; } = false;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormRegistartion formRegistartion = new FormRegistartion();

            formRegistartion.ShowDialog();

        }

        public string login { get { return textLogin.Text.Trim(); } }
        public string password { get { return textPwd.Text.Trim(); } }

        private async Task<string> LogIn()
        {
            bool flag = false;
            string resp = "#error";
            if (login == "" || password == "")
            {
                MessageBox.Show("Заполните все поля", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = false;
                return "#error";
            }

            Model.BL.Model requestHandler = new Model.BL.Model(connectionString);

            try
            {
                resp = await requestHandler.Authorization(login, password);
                if (resp == "#error")
                {
                    flag = false;
                }
                else flag = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Неполадки с подключением к серверу!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "#error";
            }
            if (!flag)
            {
                MessageBox.Show("Неверный логин или пароль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return resp;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            string resp = await LogIn();
            if (resp == "Admin")
            {
                new FormAdminPanel(this).Show();
                textLogin.Clear();
                textPwd.Clear();
                Hide();
            }
            else if (resp !="#error")
            {
                main = new FormMain(resp, this);
                main.Show();
                textLogin.Clear();
                textPwd.Clear();
                Hide();
            }
        }

        private async void textPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            await CheckInput(sender,e);
        }

        private async Task ShowToolTip(Control sender)
        {
            toolTip1.Show("Разрешено вводить только латинские буквы и цифры",sender);
            await Task.Delay(2000);
            toolTip1.Hide(sender);
        }

        private async Task CheckInput(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if ((c >= '0' && c <= '9'))
            {
                return;
            }

            if ((c < 'A' || c > 'z') && c != '\b' && c != '.')
            {
                e.Handled = true;
                await ShowToolTip(textLogin);
            }
            if (c == ' ')
            {
                e.Handled = true;
            }
        }

        private async void textLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            await CheckInput(sender, e);
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormConnectionSettings fcs = new FormConnectionSettings(connectionString);
            fcs.ShowDialog();
            connectionString = fcs.ConnectionString;
        }
    }
}
