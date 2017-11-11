using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; //Для работы с MD5
using System.Data.OleDb;// Для работы с базой


namespace WindowsFormsApplication5
{
    public partial class Authorization : Form
    {
        
        //----------Объявление переменных----------//
            MD5 md5Hash = MD5.Create(); //экзепляр класса md5 
            OleDbCommand dbcom;//переменная подключения
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb");//создаем новое подключение 

        //----------Объявление переменных----------//
        


        //----------Конструктор формы авторизации----------//
           
            public Authorization()
            {
                InitializeComponent();
                this.AcceptButton = this.login_btn;//Нажатие на enter
            }

        //----------Конструктор формы авторизации----------//

        //----------Кнопка выхода из программы ----------//

            private void exit_btn_Click(object sender, EventArgs e)
            {
                Environment.Exit(0); // выход из программы
            }

         //----------Кнопка выхода из программы ----------//
           
        
        //----------Кнопка авторизации ----------//
            
            private void login_btn_Click(object sender, EventArgs e)
            {
                if ((login.Text != "") || (pass.Text != ""))
                {
                    //хешируем наш пароль в md5
                    string hash = GetMd5Hash(md5Hash, pass.Text);
                    con.Open();
                    //Выбираем данные
                    string query = "Select * From Login where login = '" + login.Text + "'and password = '" + hash + "'";
                    dbcom = new OleDbCommand(query);
                    dbcom.Connection = con;
                    
                    OleDbDataReader reader = dbcom.ExecuteReader();
                    reader.Read();
                    //если нужная строка нашлась
                    if (reader.HasRows)
                    {
                        
                        int user_id = Convert.ToInt32(reader["epmloyeeID"]);
                        string type = reader["type"].ToString();
                        reader.Close();
                        con.Close();
                        //в зависимости от типа пользователя, открываем нужную форму
                        switch (type) {
                            case "administator"://если администратор
                                break;
                            case "manager"://менеджер
                                managerform managerform = new managerform(user_id);
                                this.Hide();
                                managerform.Show();
                                break;
                            case "mechanic"://механик
                                mechanic_form mechanic_form = new mechanic_form(user_id);                       
                                this.Hide();
                                mechanic_form.Show();
                                break;
                        }
                    reader.Close();
                    }
                    
                    else
                    {
                        //выводим сообщение о неправильности логина и пароля
                        MessageBox.Show("Вы ввели не правильный логин или пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        pass.Text = "";
                        con.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Введите пожайлуста все данные", "Заполнены не все данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        //----------Кнопка авторизации----------//







       //----------Метод работы с MD5----------//
       
        static string GetMd5Hash(MD5 md5Hash, string input)
            {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

       //----------Метод работы с MD5----------//



    }
}
