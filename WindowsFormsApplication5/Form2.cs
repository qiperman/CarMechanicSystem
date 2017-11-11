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
    public partial class mechanic_form : Form
    {
        //----------Объявление переменных----------//

        OleDbCommand dbcom; // переменная команды
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
        EnterUser user; // объект класса enteruser
        //----------Объявление переменных----------//


        //----------Конструктор формы механика----------//

            public mechanic_form(int usr_id)
            {
                InitializeComponent();//инициализирует компоненты
                con.Open(); // открывает подключение 
                string query = "Select * From Employees where employeeID = " + usr_id; // строка запроса
                dbcom = new OleDbCommand(query); // добавляем запрос 
                dbcom.Connection = con; // добавляем подключение 
                OleDbDataReader reader = dbcom.ExecuteReader(); // выполням команду
                reader.Read(); // читаем 
                //заполняем экзепляр класса
                user = new EnterUser(usr_id, "mechanic", reader["surname"].ToString(), reader["first_name"].ToString(), reader["second_name"].ToString(), reader["photo"].ToString());
                con.Close(); // закрываем подключение
            }

        //----------Конструктор формы механика----------//

        //-------Кнопка выхода из программы-------//
                    
            private void mechanic_form_FormClosing(object sender, FormClosingEventArgs e)
            {
                Environment.Exit(0); // выход из программы
            }

        //-------Кнопка выхода из программы-------//



        //-------Количество заказов-------//

            int num_orders(int id, string status)
            {
                con.Open();// открывает подключение 
                string query = "Select Count(*) From Orders where mechanicId = " + id + " and status = '" + status + "'";// строка запроса
                dbcom = new OleDbCommand(query);// добавляем запрос 
                dbcom.Connection = con;// добавляем подключение 
                OleDbDataReader reader = dbcom.ExecuteReader();// выполням команду
                reader.Read();// читаем 
                int num = Convert.ToInt32(reader[0]); //получаем количество строк
                con.Close(); // закрываем подключение
                return num; //возвращаем значние
                
            }

        //-------Количество заказов-------//

        //-------Загрузка формы-------//
          
            private void mechanic_form_Load(object sender, EventArgs e)
            {
                update();
            }


        //------Загрузка формы--------//

            void update() {
                label5.Text = user.Name + " " + user.Second_name; // заполняем имя
                label2.Text = num_orders(user.Id, "ready").ToString(); //заполняем количество выполненных заказов
                label3.Text = num_orders(user.Id, "execute").ToString(); //заполняем количество которые выполняются заказов
                label10.Text = num_orders(-1, "accepted").ToString();//заполняем количество свободных заказов
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\photos\\" + user.Photo);//заполняем фото
            }


        //----Нажатие на свободные заказы---//

            private void label8_Click(object sender, EventArgs e)
            {
                freeorders(); //вызываем метод freeorders
            }

            private void panel3_Click(object sender, EventArgs e)
            {
                freeorders();//вызываем метод freeorders
            }

            private void label10_Click(object sender, EventArgs e)
            {
                freeorders();//вызываем метод freeorders
            }

            void freeorders() {
                //строка запроса
                string query = "Select Orders.orderId,Services.name,Cars.brand,Cars.model,Cars.date,price,problemDescription From ((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID) LEFT JOIN Services ON Orders.serviceId = Services.serviceID) where status='accepted' and mechanicId = -1";
                //массив заголовков
                string[] header_name = {"Id заказа","Название Сервиса","Марка авто", "Модель", "Год выпуска","Стоимость ремонта", "Описание проблемы"};
                openwindow(query, header_name, "freeorders"); //вызываем метод openwindow
            }

        //----Нажатие на свободные заказы---//


        //----Открытие формы---//
           
            void openwindow(string query,string[] header_name,string typewindow) {
                Form3 f = new Form3(con,user, this, query, header_name, typewindow); //новый экземпляр класса form3 
                f.Show(); // показываем форму
                this.Hide();//Эту скрываем
            }

        //----Открытие формы---//


       //----Активация формы---//

            private void mechanic_form_Activated(object sender, EventArgs e)
            {
                update(); //вызываем метод update
            }

        //----Активация формы---//

       //----Нажатие на ожидающие заказы---//

            private void label6_Click(object sender, EventArgs e)
            {
                executeorders();//вызываем метод executeorders
            }

            private void label3_Click(object sender, EventArgs e)
            {
                executeorders();//вызываем метод executeorders
            }

            private void panel2_Click(object sender, EventArgs e)
            {
                executeorders();//вызываем метод executeorders
            }

            void executeorders()
            {
                //строка запроса
                string query = "Select Orders.orderId,Services.name,Cars.brand,Cars.model,Cars.date,price,problemDescription From ((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID) LEFT JOIN Services ON Orders.serviceId = Services.serviceID) where status='execute' and mechanicId = "+user.Id;
                //строка заголовков
                string[] header_name = { "Id заказа", "Название Сервиса", "Марка авто", "Модель", "Год выпуска", "Стоимость ремонта", "Описание проблемы" };
                openwindow(query, header_name, "executeorders");//вызываем метод openwindow
            }


        //----Нажатие на ожидающие заказы---//

            private void label4_Click(object sender, EventArgs e)
            {
                readyorders();//вызываем метод readyorders
            }

            private void panel1_Click(object sender, EventArgs e)
            {
                readyorders();//вызываем метод readyorders
            }

            private void label2_Click(object sender, EventArgs e)
            {
                readyorders();//вызываем метод readyorders
            }

            void readyorders()//вызываем метод readyorders
            {
                //строка запроса
                string query = "Select Orders.orderId,Services.name,Cars.brand,Cars.model,Cars.date,price,problemDescription From ((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID) LEFT JOIN Services ON Orders.serviceId = Services.serviceID) where status='ready' and mechanicId = " + user.Id;
                //строка заголовков
                string[] header_name = { "Id заказа", "Название Сервиса", "Марка авто", "Модель", "Год выпуска", "Стоимость ремонта", "Описание проблемы" };
                openwindow(query, header_name, "readyorders");//вызываем метод openwindow
            }


        //----Нажатие на завершенные заказы---//
        

    }
}
