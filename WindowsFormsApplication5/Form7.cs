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
    public partial class Form7 : Form
    {


        //----Объявляем переменные----//
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
        OleDbCommand dbcom; // переменная команды
        OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
        int employeeID;
        //----Объявляем переменные----//


        public Form7(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            fill();
        }

        void fill() {

            con.Open();
            string query = "SELECT * FROM Employees where employeeID = " + employeeID;
            dbcom = new OleDbCommand(query);//добавляем запрос к команде
            dbcom.Connection = con;//добавляем подключение
            OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
            reader.Read();//читаем

            label1.Text = reader["surname"].ToString();
            label2.Text = reader[2].ToString();
            label3.Text = reader[3].ToString();
            label4.Text = "Должность: "+reader[4].ToString();
            label8.Text = "День рождение: " + reader[5].ToString();
            label5.Text = "Прописка: " + reader[6].ToString();
            label6.Text = "Телефон: " + reader[7].ToString();
            label7.Text = "Email: " + reader[8].ToString();
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\photos\\" + reader[9].ToString());//заполняем фото
            con.Close();
        }
    }
}
