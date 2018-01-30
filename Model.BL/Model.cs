using CurApp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model.BL
{
    public interface IService
    {
        Task<string> Authorization(string login, string pwd);
        Task<bool> Registration(Curator curator);
        Task<bool> CheckGroup(string str);
        Task<bool> CheckLogin(string strLogin);
    }

    public class Model : IService
    {
        string connectionString;
        SqlConnection sqlConnection;
        public Model(string conn)
        {
            connectionString = conn;
        }

        #region Шифрование пароля
        public string Encrypting(string pwd)
        {
            MD5 mD5 = new MD5CryptoServiceProvider();
            byte[] checksum = mD5.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            return BitConverter.ToString(checksum).Replace("-", String.Empty); ;
        }
        #endregion

        #region Получение Id
        private async Task<string> GetGroupId(SqlConnection connection, Curator curator)
        {
            SqlDataReader reader = null;
            string resp = String.Empty;
            string commandString;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                commandString = String.Format($"SELECT id FROM Groups WHERE name='{curator.Group}'");
                command.CommandText = commandString;
                reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    resp = reader.GetInt32(0).ToString();
                }
            }
            reader.Close();
            return resp;
        }

        private async Task<int> GetUserId(SqlConnection connection, Curator curator)
        {
            SqlDataReader reader = null;
            int resp = -1;
            string commandString;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                commandString = String.Format($"SELECT id FROM Users WHERE login='{curator.Login}'");
                command.CommandText = commandString;
                reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    resp = reader.GetInt32(0);
                }
            }
            reader.Close();
            return resp;

        }
        #endregion

        #region Авторизация
        public async Task<string> Authorization(string login, string pwd)
        {
            string resp = String.Empty;
            SqlDataReader reader;
            bool IsAdmin = false;
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand command =
                    new SqlCommand("SELECT * FROM Users WHERE login=@login AND pwd=@pwd", sqlConnection))
                {
                    command.Parameters.AddWithValue("login", Convert.ToString(login));
                    command.Parameters.AddWithValue("pwd", Convert.ToString(Encrypting(pwd)));
                    reader = await command.ExecuteReaderAsync();
                }
                
                while (await reader.ReadAsync())
                {
                    resp = reader.GetInt32(0).ToString();
                    IsAdmin = reader.GetBoolean(3);
                }
                reader.Close();
                if (resp != "")
                {
                    using (SqlCommand command =
                            new SqlCommand($"SELECT id FROM Groups WHERE idCurator={resp} ", sqlConnection))
                    {
                        reader = await command.ExecuteReaderAsync();
                        
                    }

                    while (await reader.ReadAsync())
                    {
                        resp = reader.GetInt32(0).ToString();
                    }
                }
            }
            reader.Close();
            if (IsAdmin)
            {
                return "Admin";
                
            }
            else if (resp != "")
            {
                return resp;
            }
            else
            {
                return "#error";
            }
        }
        #endregion

        #region Регистрация
        public async Task<bool> Registration(Curator curator)
        {
            string resp = String.Empty;

            string commandString;
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                commandString = "sp_InsertUser";

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                using (SqlCommand command = new SqlCommand(commandString, sqlConnection))
                {
                    command.Transaction = transaction;
                    command.Connection = sqlConnection;

                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter param1 = new SqlParameter
                        {
                            ParameterName = "@login",
                            Value = curator.Login
                        };

                        SqlParameter param2 = new SqlParameter
                        {
                            ParameterName = "@pwd",
                            Value = Encrypting(curator.Password)
                        };

                        command.Parameters.Add(param1);
                        command.Parameters.Add(param2);

                        var idUser = await command.ExecuteScalarAsync();
                        command.Parameters.Clear();
                        ////
                        command.CommandType = CommandType.Text;
                        commandString = String.Format($"INSERT INTO Curators (id,name, surname) VALUES ('{idUser}','{curator.Name}', '{curator.Surname}')");
                        command.CommandText = commandString;
                        await command.ExecuteNonQueryAsync();
                        ////
                        commandString = String.Format($"INSERT INTO Groups (name, idCurator) VALUES ('{curator.Group}', '{idUser}'); ");
                        command.CommandText = commandString;
                        await command.ExecuteNonQueryAsync();
                        ////

                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }

                }
                //SqlTransaction transaction = sqlConnection.BeginTransaction();
                //using (SqlCommand command = sqlConnection.CreateCommand())
                //{
                //    command.Transaction = transaction;
                //    command.Connection = sqlConnection;
                //    try
                //    {
                //        commandString = String.Format($"INSERT INTO Users (login, pwd) VALUES ('{curator.Login}', '{Encrypting(curator.Password)}')");
                //        command.CommandText = commandString;
                //        x = await command.ExecuteNonQueryAsync();

                //        commandString = String.Format($"INSERT INTO Curators (id,name, surname) VALUES ('{await GetUserId(sqlConnection, curator)}','{curator.Name}', '{curator.Surname}')");
                //        command.CommandText = commandString;
                //        x += await command.ExecuteNonQueryAsync();

                //        commandString = String.Format($"INSERT INTO Groups (name, idCurator) VALUES ('{curator.Group}', '{await GetUserId(sqlConnection, curator)}'); ");
                //        command.CommandText = commandString;
                //        x += await command.ExecuteNonQueryAsync();

                //        commandString = String.Format($"INSERT INTO Students (idGroup) VALUES ('{await GetGroupId(sqlConnection, curator)}'); ");
                //        command.CommandText = commandString;
                //        x += await command.ExecuteNonQueryAsync();

                //        transaction.Commit();
                //    }
                //    catch (Exception)
                //    {
                //        transaction.Rollback();
                //        return false;
                //    }

                //}
                return true;
            }


        }
        #endregion

        #region Проверка на содержание
        public async Task<bool> CheckGroup(string groupStr)
        {
            string resp = null;
            SqlDataReader reader;
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand command =
                    new SqlCommand("SELECT name FROM Groups WHERE name=@name", sqlConnection))
                {
                    command.Parameters.AddWithValue("name", Convert.ToString(groupStr));
                    reader = await command.ExecuteReaderAsync();
                }

                while (await reader.ReadAsync())
                {
                    resp = reader.GetString(0);
                }

            }
            reader.Close();
            if (resp == groupStr)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> CheckLogin(string strLogin)
        {
            string resp = null;
            SqlDataReader reader;
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (SqlCommand command =
                    new SqlCommand("SELECT login FROM Users WHERE login=@login", sqlConnection))
                {
                    command.Parameters.AddWithValue("login", Convert.ToString(strLogin));
                    reader = await command.ExecuteReaderAsync();
                }


                while (await reader.ReadAsync())
                {
                    resp = reader.GetString(0);
                }

            }
            reader.Close();
            if (resp == strLogin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


    }
}
