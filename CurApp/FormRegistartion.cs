using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurApp
{
    public partial class FormRegistartion : Form
    {
        
        public FormRegistartion()
        {
            InitializeComponent();
            
        }
        string connectionString = FormAuthorization.connectionString;//ConfigurationManager.ConnectionStrings["CurApp.Properties.Settings.dbCuratorConnectionString"].ConnectionString;
        List<TextBox> listTextbox = new List<TextBox>();
        
        private void FormRegistartion_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FormRegistartion_Load(object sender, EventArgs e)
        {
            listTextbox.Add(textLogin);
            listTextbox.Add(textName);
            listTextbox.Add(textSurname);
            listTextbox.Add(textGroup);
            listTextbox.Add(textPwd);
            listTextbox.Add(textPwdRepeat);
        }

        private string login { get { return textLogin.Text.Trim(); } }

        private string name { get { return textName.Text.Trim(); } }
        private string surname { get { return textSurname.Text.Trim(); } }
        private string group { get { return textGroup.Text.Trim(); } }

        private string password { get { return textPwd.Text.Trim(); } }

        private async Task Registrate()
        {
            bool flag = true;

            foreach (var item in listTextbox)
            {
                if (item.Text.Trim() == "")
                {
                    MessageBox.Show("Заполните все поля", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    flag = false;
                    return;
                }
            }
            if (password.Length<5)
            {
                MessageBox.Show("Пароль должен содежрать не менее 5 символов", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textPwd.Text != textPwdRepeat.Text)
            {
                MessageBox.Show("Ошибка в повторении пароля", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = false;
            }
            Model.BL.Model requestHandler = new Model.BL.Model(connectionString);
            try
            {

                bool resp = await requestHandler.CheckLogin(login);
                if (!resp)
                {
                    flag = false;
                    MessageBox.Show("Логин не доступен", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                resp = await requestHandler.CheckGroup(group);
                if (!resp)
                {
                    flag = false;
                    MessageBox.Show("Введенная вами группа уже занята", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Неполадки с подключением к серверу!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = false;
            }
            bool response = false;
            if (flag)
            {
                try
                {
                    Curator curator = new Curator();
                    curator.Name = name;
                    curator.Surname = surname;
                    curator.Group = group;
                    curator.Login = login;
                    curator.Password = password;
                    response = await requestHandler.Registration(curator);
                }
                catch (Exception)
                {
                    MessageBox.Show("Неполадки с подключением к серверу!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    if (response)
                    {
                        MessageBox.Show("Вы успешно зарегистрированы!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка регистрации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Registrate();
        }

        private async void textLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            await CheckInput(sender, e);
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

        private async void textPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            await CheckInput(sender, e);
        }

        private async void textPwdRepeat_KeyPress(object sender, KeyPressEventArgs e)
        {
            await CheckInput(sender, e);
        }

        private async Task ShowToolTip(Control sender)
        {
            toolTip1.Show("Разрешено вводить только латинские буквы и цифры", sender);
            await Task.Delay(2000);
            toolTip1.Hide(sender);
        }

    }
}
