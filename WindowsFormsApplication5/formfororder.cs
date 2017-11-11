using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;// Для работы с базой

namespace WindowsFormsApplication5
{
    public partial class formfororder : Form
    {
        //----------Объявление переменных----------//
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
            DataSet dataSet = new DataSet();//новый экзепляр dataset
            OleDbConnection con;
            int order_id;
        //----------Объявление переменных----------//


        //----------Конструктор формы о заказе----------//

            public formfororder(OleDbConnection con,int order_id)
            {
                InitializeComponent();//открываем подключение 
                this.con = con;
                this.order_id = order_id;
            }

        //----------Конструктор формы о заказе----------//

        //-------Загрузка формы------//
           
            private void formfororder_Activated(object sender, EventArgs e)
            {
                OleDbCommand dbcom;//переменная команды
                con.Open();//открываем подключение 
                //строка запроса
                string query = "Select Orders.orderId,Drivers.fio,Drivers.phone,Cars.brand,Cars.model, Cars.color,Cars.regSign,Orders.datep,Orders.problemDescription From ((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID) LEFT JOIN Drivers ON Cars.driverId = Drivers.driverId) where orderId = " + order_id;
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем

                //----------Заполняем текст ----------//

                label1.Text = "Заказ №"+reader[0].ToString();
                label7.Text = reader[1].ToString();
                label8.Text = "Телефон: " + reader[2].ToString();
                label10.Text = reader[3].ToString() + reader[4].ToString();
                label11.Text = "Цвет: " + reader[5].ToString();
                label4.Text = "Номер: " + reader[6].ToString();
                label5.Text = "Номер: " + reader[7].ToString();
                richTextBox1.Text = reader[8].ToString();

                //----------Заполняем текст ----------//

                con.Close();//закрываем подключение
            }

        //-------Загрузка формы------//
        



    }
}
