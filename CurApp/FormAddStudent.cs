using System;
using System.Data;
using System.Windows.Forms;

namespace CurApp
{
    public partial class FormAddStudent : Form
    {
        DataSet dataSet;
        string id;
        public FormAddStudent(DataSet ds, string idgr)
        {
            dataSet = ds;
            InitializeComponent();
            id = idgr;
            buttonAdd.Click += (sender, args)=>AddStudent();
            textBoxMobileNumber.KeyPress += (sender, args) => CheckNumber(args);
        }



        public string SName { get { return textBoxName.Text.Trim(); } }

        public string Surname { get { return textBoxSurname.Text.Trim(); } }
        public string Patronymic { get { return textBoxPatronymic.Text.Trim(); } }

        public string MobNumber { get { return textBoxMobileNumber.Text.Trim(); } }
        private void AddStudent()
        {
            if (SName != "" && Surname != "")
            {
                try
                {
                    HandleReq();

                    MessageBox.Show("Добавление выполнено!");
                    textBoxName.Clear();
                    textBoxPatronymic.Clear();
                    textBoxMobileNumber.Clear();
                    textBoxSurname.Clear();
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Заполните поля!");
        }
        public DataSet GetSet()
        {
            return dataSet;
        }
        
        private void HandleReq()
        {
            DataRow dataRow = dataSet.Tables[0].NewRow();
            dataRow["name"] = SName;
            dataRow["surname"] = Surname;
            dataRow["patronymic"] = Patronymic;
            dataRow["idGroup"] = id;
            dataRow["mobileNumber"] = MobNumber;
            dataSet.Tables[0].Rows.Add(dataRow);
        }


        private void CheckNumber(KeyPressEventArgs args)
        {
            char c = args.KeyChar;
            if ((c >= '0' && c <= '9'))
            {
                return;
            }

            if (c != '+' && c != '-' && c != '\b')
            {
                args.Handled = true;
            }
            
        }
    }
}
