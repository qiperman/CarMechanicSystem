using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;//работа с бд

namespace WindowsFormsApplication5
{
    public partial class Form5 : Form
    {

        //----Объявляем переменные----//

            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
            OleDbCommand dbcom; // переменная команды
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
            int driverId;

        //----Объявляем переменные----//

        //----------Конструктор формы ----------//

            public Form5()
            {
                InitializeComponent();//инициализируем компоненты
                fill();//вызываем метод fill
            }

        //----------Конструктор формы ----------//

        //----------Метод Fill----------//

            void fill() {
                con.Open();//открываем подключение
                string query = "SELECT MAX(driverId) FROM Drivers";//строка запроса
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем
                if (reader.HasRows)//если есть строка
                {
                    label1.Text = "Добавление водителя № " + ((int)reader[0] + 1).ToString(); // добавляем надпись 
                    driverId = (int)reader[0] + 1; // присваиваем переменной текущий id 
                }
                con.Close(); // закрываем подключение
            }

        //----------Метод Fill----------//

       //----------Конпка добавления----------//

            private void button1_Click(object sender, EventArgs e)
            {
                try
                {
                    con.Open(); //открываем подключение
                    //строка запроса
                    string sqlQuery = "INSERT INTO [Drivers]([driverId],[fio],[birthday],[registration],[pasport],[phone],[email]) VALUES (" + driverId + ",'" + textBox1.Text + "','" + maskedTextBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + maskedTextBox1.Text + "','" + textBox6.Text + "')";
                    OleDbCommand myCmd = new OleDbCommand(sqlQuery, con);//добавляем команду 

                    myCmd.ExecuteNonQuery();//выполняем
                    con.Close();//закрываем подключение 
                    this.Close();//закрываем форму

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);//показываем ошибку
                }
            }

        //----------Конпка добавления----------//
    }
}
