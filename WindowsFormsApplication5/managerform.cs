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
    public partial class managerform : Form
    {
        //----------Объявление переменных----------//

            OleDbCommand dbcom; // переменная команды
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
            EnterUser user; // объект класса enteruser
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
            DataSet dataSet = new DataSet();//новый экзепляр dataset

        //----------Объявление переменных----------//

        //----------Конструктор формы Менеджра----------//

            public managerform(int usr_id)
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

        //----------Конструктор формы Менеджера----------//

        //-------Кнопка выхода из программы-------//
           
            private void managerform_FormClosing(object sender, FormClosingEventArgs e)
            {
                Environment.Exit(0); //закрываем программу
            }

        //-------Кнопка выхода из программы-------//

        //-------Обновление данных------//

            void update()
            {
                label1.Text = "Здравствуйте, "+user.Name + " " + user.Second_name; // заполняем имя
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\photos\\" + user.Photo);//заполняем фото

                dataSet.Clear();
                con.Open();//открываем подключение 
                string query = "SELECT Orders.orderId,Drivers.fio,Cars.brand, Cars.date,Employees.surname,Employees.first_name,price,datep,status FROM ((((Orders LEFT JOIN Cars ON Orders.carId = Cars.carID)LEFT JOIN Drivers ON Cars.driverId = Drivers.driverId )LEFT JOIN Services ON Orders.serviceId = Services.serviceID)Left JOIN Employees ON Orders.mechanicId = Employees.employeeId)";

                OleDbCommand command = new OleDbCommand(query, con);//выполням команду
                con.Close();//закрываем подключение
                adapter.SelectCommand = command;//адаптеру присваиваем команду
                adapter.Fill(dataSet);//заполням адаптер
                dataGridView1.DataSource = dataSet.Tables[0];//присваиваем datasourse
                adapter.Update(dataSet);//обновлям

                string[] header_name = { "Id заказа", "ФИО водителя", "Автомобиль", "Год выпуска", "Фамилия механика", "Имя механика", "Цена работы", "Дата принятия заказа", "Статус" };

                int i = 0;//переменная итраций

                int status_num = dataGridView1.ColumnCount - 1;
                dataGridView1.ClearSelection();
                foreach (string el in header_name)//цикл заполнения заголовков таблицы
                {
                    dataGridView1.Columns[i].HeaderText = el;
                    i++;
                }
                
            }

         //-------Обновление данных------//



            //-------Загрузка формы------//

                private void managerform_Activated(object sender, EventArgs e)
                {
                    update();
                }

            //-------Загрузка формы------//

            //-------Двойной щелчек по строке------//

                private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
                {
                    formfororder formfororder = new formfororder(con,Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
                    formfororder.ShowDialog();

                }

            //-------Двойной щелчек по строке------//


            //-------Удаление------//

                private void button2_Click(object sender, EventArgs e)
                {

                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Вы не выбрали строку", "Ошибки", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else
                    {
                        int order = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                        DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить этот заказ", "Подтверждние", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            con.Open();//открываем подключение 
                            string query = "Delete From Orders WHERE orderId = " + order;//строка запроса
                            OleDbCommand command = new OleDbCommand(query, con);//добавляем команду
                            command.ExecuteNonQuery();//выполняем команду
                            con.Close();//закрываем подключение 
                        
                        }
                        update();
                    }
                }

                private void button1_Click(object sender, EventArgs e)
                {
                    addneworder addneworder = new addneworder(con);
                    addneworder.ShowDialog();
                }

                private void button3_Click(object sender, EventArgs e)
                {
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Вы не выбрали строку", "Ошибки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        int order = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                        addneworder addneworder = new addneworder(con,order);
                        addneworder.ShowDialog();
                    }
                }

                private void button4_Click(object sender, EventArgs e)
                {
                    Form6 f = new Form6();
                    f.ShowDialog();
                }

            //-------Удаление------//
            
        

    }
}
