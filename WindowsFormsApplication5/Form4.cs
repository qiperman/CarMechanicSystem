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


    public partial class Form4 : Form
    {
        //----Объявляем переменные----//

        EnterUser user; //переменная пользователя
        int orderId;//переменная id заказа
        OleDbConnection con;//переменная подключения

        //----Объявляем переменные----//

        //----------Конструктор формы ----------//
            public Form4(EnterUser user, int orderId,  OleDbConnection con,string typewindow)
            {
                InitializeComponent();//инициализируем компоненты
            
                //--Присваивам перменным значения--//

                    this.user = user;
                    this.orderId = orderId;
                    this.con = con;

                //--Присваивам перменным значения--//


                //в зависимости от типа окна показываем разные кнопки
                switch(typewindow){
                    case ("freeorders"):
                        button2.Visible = true;
                        button3.Visible = false;
                        button1.Visible = false;
                        break;

                    case ("executeorders"): 
                        button2.Visible = false;
                        button3.Visible = true;
                        button1.Visible = true;
                        break;

                    case ("readyorders"): 
                        button2.Visible = false;
                        button3.Visible = false;
                        button1.Visible = false;
                        break;
                    
                }

            }
       //----------Конструктор формы ----------//



       //----------Закрытие формы ----------//
            
            private void button1_Click(object sender, EventArgs e)
            {
                this.Close();//закрываем форму
            }

        //----------Конструктор формы ----------//


        //----------Нажатие взять заказ ----------//

            private void button2_Click(object sender, EventArgs e)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите взять этот заказ", "Подтверждние", MessageBoxButtons.YesNo);//окно подтверждения
                if (dialogResult == DialogResult.Yes)
                {
                    con.Open();//открытие подключение
                    string query = "UPDATE Orders SET mechanicId = " + user.Id + ", status = 'execute' WHERE orderId = " + orderId;//строка запроса
                    OleDbCommand command = new OleDbCommand(query, con);//добавляем комнанду 
                    command.ExecuteNonQuery();//выполняем команду
                    con.Close();//закрываем подключение

                    this.Close();//закрываем форму
                }
                else {
                    this.Close();//закрываем форму
                }
            }

        //----------Нажатие взять заказ ----------//


        //----------Загрузка формы ----------//

            private void Form4_Load(object sender, EventArgs e)
            {
                OleDbCommand dbcom;//переменная команды
                con.Open();//открываем подключение 
                //строка запроса
                string query = "Select Drivers.fio,Cars.brand,Cars.model,Cars.date,Orders.price,Services.name,Services.adress From (((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID) LEFT JOIN Drivers ON Cars.driverId = Drivers.driverId) LEFT JOIN Services ON Orders.serviceId = Services.serviceID) where orderId = " + orderId;
                dbcom = new OleDbCommand(query);//добавляем запрос к команде
                dbcom.Connection = con;//добавляем подключение
                OleDbDataReader reader = dbcom.ExecuteReader();//выполняем запрос
                reader.Read();//читаем

                //----------Заполняем текст ----------//

                    label8.Text = "Заказ № " + orderId;
                    label1.Text = "Водитель: " + reader[0].ToString();
                    label2.Text = "Автомобиль: " + reader[1].ToString() + " " + reader[2].ToString();
                    label3.Text = "Год выпуска: " + reader[3].ToString();
                    label4.Text = "Стоимость ремонта: " + reader[4].ToString()+" руб.";
                    label6.Text = reader[5].ToString();
                    label7.Text = reader[6].ToString();

                //----------Заполняем текст ----------//

                con.Close();//закрываем подключение
            }

        //----------Загрузка формы----------//


       //----------Нажатие на кнопку завершения заказа----------//

            private void button1_Click_1(object sender, EventArgs e)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите завершить этот заказ", "Подтверждние", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    con.Open();//открываем подключение 
                    string query = "UPDATE Orders SET  status = 'ready' WHERE orderId = " + orderId;//строка запроса
                    OleDbCommand command = new OleDbCommand(query, con);//добавляем команду
                    command.ExecuteNonQuery();//выполняем команду
                    con.Close();//закрываем подключение 

                    this.Close();//закрываем форму
                }
                else
                {
                    this.Close();//закрываем форму
                }
            }

        //----------Нажатие на кнопку завершения заказа----------//


       //----------Нажатие на кнопку отказа от заказа----------//

            private void button3_Click(object sender, EventArgs e)
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите отказаться от  этого заказа", "Подтверждние", MessageBoxButtons.YesNo);//окно подтверждения
                if (dialogResult == DialogResult.Yes)
                {
                    con.Open();//открываем опдключение
                    string query = "UPDATE Orders SET mechanicId = -1, status = 'accepted' WHERE orderId = " + orderId;//строка запроса
                    OleDbCommand command = new OleDbCommand(query, con);//добавляем команду
                    command.ExecuteNonQuery();//выполняем команду
                    con.Close();//закрываем подключение 

                    this.Close();//закрывваем форму
                }
                else
                {
                    this.Close();//закрываем форму
                }
            }

        //----------Нажатие на кнопку отказа от заказа----------//
    }
}
