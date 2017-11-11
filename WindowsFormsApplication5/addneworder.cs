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
    public partial class addneworder : Form
    {
        //----Объявляем переменные----//
        int orderId;//переменная id заказа
        OleDbConnection con;//переменная подключения
        string type = "add";
        OleDbCommand dbcom; // переменная команды
        OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
        //----Объявляем переменные----//

       //----------Конструктор формы ----------//
            
            public addneworder(OleDbConnection con,int orderId = -1)
            {
                InitializeComponent();//инициализируем компоненты

                //--Присваивам перменным значения--//

                this.orderId = orderId;
                this.con = con;

                //--Присваивам перменным значения--//

                if (orderId == -1)
                {
                    type = "add";
                    adding();
                }
                else {
                    type = "edit";
                    editing();
                }
            }

        //----------Конструктор формы ----------//

        //----------Метод заполнения----------//
            
            void adding() {
                DataSet cars = new DataSet();
                DataSet sto = new DataSet();
                con.Open();
                OleDbDataAdapter dbAdapter;

                string query = "SELECT MAX(orderId) FROM Orders";
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем
                if (reader.HasRows)
                {
                    label1.Text = "Добавление заказа № " + ((int)reader[0] + 1).ToString();
                    orderId = (int)reader[0] + 1;
                }

                cars.Clear();
                dbAdapter = new OleDbDataAdapter("SELECT * FROM Cars", con);
                dbAdapter.Fill(cars);
                comboBox1.DataSource = cars.Tables[0];
                comboBox1.ValueMember = "carID";
                comboBox1.DisplayMember = "regSign";

                sto.Clear();
                dbAdapter = new OleDbDataAdapter(@"SELECT * FROM Services", con);
                dbAdapter.Fill(sto);
                comboBox2.DataSource = sto.Tables[0];
                comboBox2.ValueMember = "serviceID";
                comboBox2.DisplayMember = "name";

                label9.Text = DateTime.Now.ToString("dd.MM.yyyy");
                con.Close();
            }

            void editing() {
                con.Open();
                DataSet cars = new DataSet();
                DataSet sto = new DataSet();
                OleDbDataAdapter dbAdapter;

                label1.Text = "Редактирование заказа № " + orderId;
                
                cars.Clear();
                dbAdapter = new OleDbDataAdapter("SELECT * FROM Cars where carID = (Select carId from Orders where orderId = " + orderId + ")", con);
                dbAdapter.Fill(cars);
                dbAdapter = new OleDbDataAdapter("SELECT * FROM Cars where NOT carID = (Select carId from Orders where orderId = " + orderId + ")", con);
                dbAdapter.Fill(cars);
                comboBox1.DataSource = cars.Tables[0];
                comboBox1.ValueMember = "carID";
                comboBox1.DisplayMember = "regSign";

                sto.Clear();
                dbAdapter = new OleDbDataAdapter(@"SELECT * FROM Services where serviceID = (Select serviceId from Orders where orderId = " + orderId + ")", con);
                dbAdapter.Fill(sto);
                dbAdapter = new OleDbDataAdapter(@"SELECT * FROM Services where NOT serviceID = (Select serviceId from Orders where orderId = " + orderId + ")", con);
                dbAdapter.Fill(sto);
                comboBox2.DataSource = sto.Tables[0];
                comboBox2.ValueMember = "serviceID";
                comboBox2.DisplayMember = "name";

                string query = "SELECT problemDescription, price FROM Orders where orderId = " + orderId;
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем

                richTextBox1.Text = reader[0].ToString();
                textBox1.Text = reader[1].ToString();

                con.Close();
            }



            private void button1_Click(object sender, EventArgs e)
            {
                try
                {
                    con.Open();
                    string sqlQuery = "";
                    if (type == "add")
                    {
                        sqlQuery = "INSERT INTO Orders (orderId,carId,serviceId,mechanicId,datep,price,problemDescription,status) VALUES (" + orderId + "," + comboBox1.SelectedValue.ToString() + "," + comboBox2.SelectedValue.ToString() + ", -1, '" + label9.Text + "'," + textBox1.Text + ",'" + richTextBox1.Text + "','accepted')";
                    }

                    else {
                        sqlQuery = "Update Orders SET carId=" + comboBox1.SelectedValue.ToString() + ",serviceId=" + comboBox2.SelectedValue.ToString() + ",price='" + textBox1.Text + "',problemDescription='" + richTextBox1.Text + "' where orderId = " + orderId ;
                    }
                    
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

            private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (Char.IsDigit(e.KeyChar) == true) return; // Если символ цифра, то возвращаемся из метода 
                e.Handled = true;
                return;
            }

            private void button2_Click(object sender, EventArgs e)
            {
                addnewcar addnewcar = new addnewcar();
                addnewcar.ShowDialog();
            }

            private void addneworder_Activated(object sender, EventArgs e)
            {
                if (type == "add")
                {
                    adding();
                }
                else {
                    editing();
                }
            }

        //----------Метод заполнения----------//
    }
}
