using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;//работа с бд
using System.IO;


namespace WindowsFormsApplication5
{
    public partial class addempl : Form
    {

        //----Объявляем переменные----//

        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
        OleDbCommand dbcom; // переменная команды
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        int emplId;

        //----Объявляем переменные----//

        //----------Конструктор формы ----------//
        public addempl()     
            {
                InitializeComponent();//инициализируем компоненты
                fill();//вызываем метод fill
            }
        //----------Конструктор формы ----------//

        //----------Метод Fill----------//

        void fill()
        {
            con.Open();//открываем подключение
            string query = "SELECT MAX(employeeID) FROM Employees";//строка запроса
            dbcom = new OleDbCommand(query);//добавляем запрос к команде
            dbcom.Connection = con;//добавляем подключение
            OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
            reader.Read();//читаем
            if (reader.HasRows)//если есть строка
            {
              emplId = (int)reader[0] + 1; // присваиваем переменной текущий id 
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
                string photo = "nophoto.jpg";
                if (label10.Text != "")
                    photo = label10.Text;
                string sqlQuery = "INSERT INTO [Employees]([employeeID],[surname],[first_name],[second_name],[job],[birth],[registration],[phone],[email],[photo]) VALUES (" + emplId + ",'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox5.Text + "','" + comboBox1.Text + "','" + maskedTextBox2.Text + "','" + textBox3.Text + "','" + maskedTextBox1.Text + "','" + textBox6.Text + "', '" + photo + "')";
                OleDbCommand myCmd = new OleDbCommand(sqlQuery, con);//добавляем команду 

                myCmd.ExecuteNonQuery();//выполняем
                con.Close();//закрываем подключение 
                this.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);//показываем ошибку
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // выход, если была нажата кнопка Отмена и прочие (кроме ОК)
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            // всё. имя файла теперь хранится в openFileDialog1.FileName
            label10.Text = Path.GetFileName(openFileDialog1.FileName);

            CopyFile(openFileDialog1.FileName, Application.StartupPath + "\\photos\\" + Path.GetFileName(openFileDialog1.FileName));

        }

        //----------Конпка добавления----------//


        void CopyFile(string sourcefn, string destinfn)
        {
            FileInfo fn = new FileInfo(sourcefn);
            fn.CopyTo(destinfn, true);
        }
    }
}
