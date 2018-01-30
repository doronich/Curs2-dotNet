using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace CurApp
{
    public partial class FormMain : Form
    {
        string connectionString = FormAuthorization.connectionString;//ConfigurationManager.ConnectionStrings["CurApp.Properties.Settings.dbCuratorConnectionString"].ConnectionString;
        DataSet ds;
        FormAuthorization formAuthorization;
        string sql = String.Format($"SELECT * FROM Students WHERE idGroup = '{IdGroup}';");

        public Curator Cur { get; set; }
        string filePath;
        public string resp { get; set; }
        public static string IdGroup { get; set; }
        static int id = 0;

        string fileName = "tempreport.rtf";

        string reqList = "!";
        public FormMain(string str, FormAuthorization form)
        {
            InitializeComponent();
            formAuthorization = form;
            IdGroup = str;
            ds = GetDataSet();
            FillDataGrid(ds.Tables[0]);
            buttonAddStudent.Click += (sender, args) => InvokeAdd();
            exitToolStripMenuItem.Click += (sender, args) => Application.Exit();

            dataGridViewStudentsList.AllowUserToResizeColumns = false;
            dataGridViewStudentsList.AllowUserToResizeRows = false;
            dataGridViewStudentsList.MultiSelect = false;

            buttonDeleteStudent.Click += (sender, args) => InvokeDelete();
            dataGridViewStudentsList.CellClick += (sender, args) => SelectFullRow(args);
            dataGridViewStudentsList.CellEnter += (sender, args) => SelectFullRow(args);
            labelTitle.Text = labelTitle.Text + GetGroupName();
            FillListTb();
            buttonAddPhoto.Click += (sender, args) => InvokeAddPhoto();
            buttonChangeStudent.Click += (sender, args) => ChangeCard();
            buttonSaveCard.Click += (sender, args) => SaveCard();
            buttonDeletePhoto.Click += (sender, args) => DeletePhoto();
            comboBox1.SelectedIndex = 0;
            tbSearchByWord.TextChanged += (sender, args) => SearchingByWord();
            buttonMakeReport.Click += (sender, args) => MakeReport();

            buttonOpenWord.Click += (sender, args) => OpenWithWord();
            this.Load += (sender, args) =>
            {
                buttonAddPhoto.Visible = false;
                buttonDeletePhoto.Visible = false;
                dtDOB.MaxDate = DateTime.Now.Date;
            };
        }
        
        private string GetGroupName()
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    string s = "";
                    SqlDataReader reader;
                    using (SqlCommand command = new SqlCommand("SELECT name FROM Groups WHERE id = " + IdGroup, conn))
                    {
                        reader = command.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            s = s + reader.GetString(0);
                        }
                        reader.Close();
                        s = s + " Куратор: ";
                        command.CommandText = "SELECT surname, name FROM Curators WHERE id = (SELECT idCurator FROM Groups WHERE id= " + IdGroup + ");";

                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            s = s + reader.GetString(0);
                            s =  s +" "+ reader.GetString(1);
                        }
                        reader.Close();
                    }
                    return s;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return "";
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        private void OpenWithWord()
        {
            
            try
            {
                richTextBox1.SaveFile(fileName);
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MakeReport()
        {
            if (reqList != "")
            {
                richTextBox1.LoadFile("recources/Report.rtf");
                richTextBox1.AppendText(reqList);
                richTextBox1.ReadOnly = false;
                richTextBox1.BackColor = Color.White;

                buttonOpenWord.Enabled = true;
            }

        }

        private void SearchingByWord()
        {
            try
            {
                string s = tbSearchByWord.Text;
                richTextBox1.Clear();
                DataRow[] res = null;

                if (s != "")
                {
                    if (comboBox1.Text == "Все поля")
                    {

                        res = ds.Tables[0].Select(
                            $"surname LIKE '*{s}*' OR name LIKE '*{s}*' OR patronymic LIKE '*{s}*' OR Citizenship LIKE '*{s}*' OR GraduetedEdInst LIKE '*{s}*' OR PasportData LIKE '*{s}*' OR " +
                            $"HomeAddressTel LIKE '*{s}*' OR PlaceOfResidence LIKE '*{s}*' OR HealthInfo LIKE '*{s}*' OR FatherInfo LIKE '*{s}*' OR MotherInfo LIKE '*{s}*' OR OtherFamilyInfo LIKE '*{s}*' OR " +
                            $"Social LIKE '*{s}*' OR Interests LIKE '*{s}*' OR OtherInfo LIKE '*{s}*' OR MobileNumber LIKE '*{s}*'");
                    }
                    else if (comboBox1.Text == "Имя")
                    {
                        string str = String.Format($"name LIKE '{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Фамилия")
                    {
                        string str = String.Format($"surname LIKE '{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Отчество")
                    {
                        string str = String.Format($"patronymic LIKE '{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Мобильный номер")
                    {
                        string str = String.Format($"mobileNumber LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Гражданство")
                    {
                        string str = String.Format($"Citizenship LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Паспортные данные")
                    {
                        string str = String.Format($"PasportData LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Домашний адрес и телефон")
                    {
                        string str = String.Format($"HomeAddressTel LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Место жительства")
                    {
                        string str = String.Format($"PlaceOfResidence LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Состояние здоровья")
                    {
                        string str = String.Format($"HealthInfo LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Информация об отце")
                    {
                        string str = String.Format($"FatherInfo LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Информация о матери")
                    {
                        string str = String.Format($"MotherInfo LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Информация о семье")
                    {
                        string str = String.Format($"OtherFamilyInfo LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Социальные и бытовые условия")
                    {
                        string str = String.Format($"Social LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Интересы")
                    {
                        string str = String.Format($"Interests LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }
                    else if (comboBox1.Text == "Другая информация")
                    {
                        string str = String.Format($"OtherInfo LIKE '*{s}*'");
                        res = ds.Tables[0].Select(str);
                    }

                    int k = 1;
                    reqList = "";
                    foreach (var item in res)
                    {

                        reqList += String.Format(k++ + ". " + item["surname"].ToString() + " " + item["name"].ToString() + " " + item["patronymic"].ToString() + "\n");

                    }
                    richTextBox1.AppendText(reqList);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeletePhoto()
        {
            pbPhoto.Image = Properties.Resources.no_image;
            filePath = "!";
        }

        private void ChangeCard()
        {
            if (dataGridViewStudentsList.SelectedCells.Count == 0) return;
            if (tbStSName.ReadOnly == true)
            {
                foreach (var item in _listTb)
                {
                    item.Key.ReadOnly = false;
                }
                dtDOB.Enabled = true;
                buttonSaveCard.Enabled = true;
                buttonAddPhoto.Visible = true;
                buttonDeletePhoto.Visible = true;
                buttonChangeStudent.Text = "Отмена";


            }
            else
            {
                foreach (var item in _listTb)
                {
                    item.Key.ReadOnly = true;
                }
                dtDOB.Enabled = false;
                buttonSaveCard.Enabled = false;
                buttonAddPhoto.Visible = false;
                buttonDeletePhoto.Visible = false;
                buttonChangeStudent.Text = "Редактировать";
                SelectFullRow(null);
            }


        }

        private void InvokeAddPhoto()
        {
            try
            {
                OpenFileDialog opDialog = new OpenFileDialog();
                opDialog.Filter = "PNG(*.png)|*.png|JPEG(*.jpg)|*.jpg|All files(*.*)|*.*";
                opDialog.Multiselect = false;
                if (opDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                filePath = opDialog.FileName;

                pbPhoto.ImageLocation = filePath;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetNumb()
        {
            List<DataRow> list = new List<DataRow>();
            try
            {
                DataRow[] dataRowmas = new DataRow[ds.Tables[0].Rows.Count];
                ds.Tables[0].Rows.CopyTo(dataRowmas, 0);
                foreach (DataRow item in dataRowmas)
                {
                    list.Add(item);
                }
                list.Sort((x, y) =>
                {
                    return String.Compare(x.ItemArray[3].ToString(), y.ItemArray[3].ToString());
                });

                int k = 0;
                foreach (DataRow item in list)
                {
                    item.BeginEdit();
                    item["number"] = ++k;
                    item.EndEdit();
                }
                dataGridViewStudentsList.Sort(dataGridViewStudentsList.Columns[3], ListSortDirection.Ascending);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectFullRow(DataGridViewCellEventArgs args)
        {
            //dataGridViewStudentsList.CurrentRow.Selected = true;
            //DataRow dr = ds.Tables[0].Rows[];
            try
            {

                int x = Convert.ToInt32(dataGridViewStudentsList.CurrentRow.Cells["number"].Value);

                DataTable dt = (DataTable)dataGridViewStudentsList.DataSource;
                FillCard(dt, x);

            }
            catch (Exception)
            {
            }



        }

        public void FillDataGrid(DataTable dt)
        {
            int n = 88;
            dataGridViewStudentsList.DataSource = dt;
            //dataGridViewStudentsList.AutoResizeColumns();
            dataGridViewStudentsList.RowHeadersWidth = 25;
            dataGridViewStudentsList.Columns[0].Visible = false;
            dataGridViewStudentsList.Columns[1].Visible = false;
            for (int i = 7; i < dataGridViewStudentsList.Columns.Count; i++)
            {
                dataGridViewStudentsList.Columns[i].Visible = false;
            }
            dataGridViewStudentsList.Columns["number"].HeaderText = "№ п/п";
            dataGridViewStudentsList.Columns["number"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewStudentsList.Columns["number"].Width = 30;
            dataGridViewStudentsList.Columns["surname"].HeaderCell.Value = "Фамилия";
            dataGridViewStudentsList.Columns["surname"].Width = n;
            dataGridViewStudentsList.Columns["name"].HeaderCell.Value = "Имя";
            dataGridViewStudentsList.Columns["name"].Width = n;
            dataGridViewStudentsList.Columns["patronymic"].HeaderText = "Отчество";
            dataGridViewStudentsList.Columns["patronymic"].Width = n+4;

            dataGridViewStudentsList.Columns["mobileNumber"].HeaderText = "Контактный телефон";
            dataGridViewStudentsList.Columns["mobileNumber"].Width = n + 18;
            dataGridViewStudentsList.ClearSelection();
            dataGridViewStudentsList.ReadOnly = true;
            dataGridViewStudentsList.Sort(dataGridViewStudentsList.Columns[3], ListSortDirection.Ascending);
        }

        private void changeUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            formAuthorization.Show();
            Hide();

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            Application.Exit();
        }

        public void InvokeAdd()
        {
            try
            {
                FormAddStudent formAddStudent = new FormAddStudent(ds, IdGroup);
                formAddStudent.ShowDialog();
                ds = formAddStudent.GetSet();
                SetNumb();
                InvokeSave();
                InvokeUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        public void InvokeSave()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);

                }
                //MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void InvokeDelete()
        {
            if (dataGridViewStudentsList.SelectedCells.Count >= 1)
            {
                DialogResult dialog =
                MessageBox.Show("Все данные о студенте будут удалены.\nВы уверены, что хотите удалить выделенные строки?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialog == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dataGridViewStudentsList.SelectedRows)
                    {
                        dataGridViewStudentsList.Rows.Remove(row);
                    }
                    
                    InvokeSave();
                    SetNumb();
                    InvokeSave();
                }
            }
            else MessageBox.Show("Выделите полностью хотя бы одну строку!");


        }

        private void InvokeUpdate()
        {
            FillDataGrid(GetStudents());
        }

        private DataSet GetDataSet()
        {
            DataSet dataSet = null;
            try
            {
                string commandString;
                commandString = String.Format($"SELECT * FROM Students WHERE idGroup = '{IdGroup}';");
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(commandString, sqlConnection))
                    {
                        dataSet = new DataSet();
                        adapter.Fill(dataSet);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dataSet;
        }

        #region Выборка списка студентов
        public DataTable GetStudents()
        {
            ds = GetDataSet();

            return ds.Tables[0];
        }
        #endregion

        Dictionary<TextBox, string> _listTb = new Dictionary<TextBox, string>();
        private void FillListTb()
        {
            _listTb.Add(tbStSName, "surname");
            _listTb.Add(tbStName, "name");
            _listTb.Add(tbNumber, "mobileNumber");
            _listTb.Add(tbStPatronymic, "patronymic");
            _listTb.Add(tbCitizenShip, "CitizenShip");
            _listTb.Add(tbGradEI, "GraduetedEdInst");
            _listTb.Add(tbPasport, "PasportData");
            _listTb.Add(tbHomeAddrTel, "HomeAddressTel");
            _listTb.Add(tbPlace, "PlaceOfResidence");
            _listTb.Add(tbHealth, "HealthInfo");
            _listTb.Add(tbFather, "FatherInfo");
            _listTb.Add(tbMother, "MotherInfo");
            _listTb.Add(tbFamily, "OtherFamilyInfo");
            _listTb.Add(tbSocial, "Social");
            _listTb.Add(tbInterests, "Interests");
            _listTb.Add(tbOtherInfo, "OtherInfo");
        }
        private void FillCard(DataTable dataTable, int x)
        {

            try
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataTable.Rows[i]["number"]) == x)
                    {
                        id = i;
                        break;
                    }
                }

                foreach (var item in _listTb)
                {
                    item.Key.Text = ds.Tables[0].Rows[id][item.Value].ToString();
                }

                if (ds.Tables[0].Rows[id]["Photo"] != null && ds.Tables[0].Rows[id]["Photo"].ToString() != "")
                {
                    pbPhoto.Image = Image.FromStream(PhotoFromDB());
                }
                else pbPhoto.Image = Properties.Resources.no_image;//Image.FromFile(filePath);
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }

            try
            {
                if (Convert.ToDateTime(dataTable.Rows[id]["DOB"].ToString()) != null)
                {
                    dtDOB.Value = Convert.ToDateTime(dataTable.Rows[id]["DOB"]);
                }
            }
            catch (Exception)
            {
                dtDOB.Value = DateTime.Parse("01.01.1990");
            }

        }

        private void SaveCard()
        {

            try
            {
                foreach (var item in _listTb)
                {
                    ds.Tables[0].Rows[id][item.Value] = item.Key.Text;
                }
                try
                {
                    if (filePath == "!")
                    {

                        Properties.Resources.no_image.Save("temp.png");
                        filePath = "temp.png";
                        ds.Tables[0].Rows[id]["Photo"] = ConvertPhoto();
                        File.Delete(filePath);
                    }
                    else
                    {
                        if (filePath != "")
                        {
                            ds.Tables[0].Rows[id]["Photo"] = ConvertPhoto();
                        }

                    }
                    SetNumb();

                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                }
                ds.Tables[0].Rows[id]["DOB"] = dtDOB.Value;
                InvokeSave();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                ChangeCard();
                filePath = "";
            }

        }

        private byte[] ConvertPhoto()
        {

            byte[] buf = File.ReadAllBytes(filePath);

            return buf;
        }

        private MemoryStream PhotoFromDB()
        {
            byte[] buf = (byte[])ds.Tables[0].Rows[id]["Photo"];
            return new MemoryStream(buf);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("Times New Roman", 14), Brushes.Black, new PointF(100,100));
        }

        private void опрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog();
        }

        private void tbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumber(e);
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
