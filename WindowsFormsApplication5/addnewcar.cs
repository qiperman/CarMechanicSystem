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
    public partial class addnewcar : Form
    {
        //----Объявляем переменные----//
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение  
            OleDbCommand dbcom; // переменная команды
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
            int carId;
        //----Объявляем переменные----//

        //----------Конструктор формы ----------//

            public addnewcar()
            {
                InitializeComponent();//инициализируем компоненты
                fill();
            }


            void fill() {
                DataSet drivers = new DataSet();
                con.Open();
                OleDbDataAdapter dbAdapter;

                string query = "SELECT MAX(carId) FROM Cars";
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем
                if (reader.HasRows)
                {
                    label1.Text = "Добавление автомобиля № " + ((int)reader[0] + 1).ToString();
                    carId = (int)reader[0] + 1;
                }

                drivers.Clear();
                dbAdapter = new OleDbDataAdapter("SELECT * FROM Drivers", con);
                dbAdapter.Fill(drivers);
                comboBox1.DataSource = drivers.Tables[0];
                comboBox1.ValueMember = "driverID";
                comboBox1.DisplayMember = "fio";


            
                con.Close();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                try
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO [Cars]([carID],[driverId],[brand],[model],[regSign],[color],[date]) VALUES (" + carId + "," + comboBox1.SelectedValue.ToString() + ",'" + textBox1.Text + "','" + textBox2.Text + "','"+maskedTextBox1.Text+"','" + textBox4.Text + "','" + textBox5.Text + "')";
                    OleDbCommand myCmd = new OleDbCommand(sqlQuery, con);
                    
                    myCmd.ExecuteNonQuery();
                    con.Close();
                    this.Close();
               
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }

            private void button2_Click(object sender, EventArgs e)
            {
                Form5 f = new Form5();
                f.ShowDialog();
            }

            private void addnewcar_Activated(object sender, EventArgs e)
            {
                fill();
            }
        //----------Конструктор формы ----------//
    }
}
