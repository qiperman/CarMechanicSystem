using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb; //работа с бд

namespace WindowsFormsApplication5
{
    public partial class Form3 : Form
    {

        //----Объевление перемнных ----//
            string query=""; //переменная запроса
            string typewindow = ""; //переменная типа окна
            string[] header_name;//массив заголовков
            EnterUser user;//экзпеляр класса для пользователя
            Form parentform;//родительское окно
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
            OleDbConnection con;//переменная подключеня 
            DataSet dataSet = new DataSet();//новый экзепляр dataset
        //----Объевление перемнных ----//


        public Form3(OleDbConnection con,EnterUser user, Form f, string q,string[] h,string t)
        {
            InitializeComponent();//инициализируем компоненты

            //--Присваиваем переменным зачения--//

                query = q; 
                header_name = h;
                parentform = f;
                typewindow = t;
                this.user = user;
                this.con = con;

            //--Присваиваем переменным зачения--//

                //в зависимости от типа окна показываем разный текст в label
                switch (typewindow)
                {
                    case ("freeorders"):
                        label1.Text = "Свободные заказы";
                        break;

                    case ("executeorders"):
                        label1.Text = "Выполняющиеся заказы";
                        break;

                    case ("readyorders"):
                        label1.Text = "Выполненные заказы";
                        break;

                }

        }
        //----------Конструктор формы ----------//


        //----------Заполнение таблицы ----------//
           
            private void filltable()
            {
                dataSet.Clear();
                con.Open();//открываем подключение 
                OleDbCommand command = new OleDbCommand(query, con);//выполням команду
                con.Close();//закрываем подключение
                adapter.SelectCommand = command;//адаптеру присваиваем команду
                adapter.Fill(dataSet);//заполням адаптер
                dataGridView1.DataSource = dataSet.Tables[0];//присваиваем datasourse
                adapter.Update(dataSet);//обновлям

                int i = 0;//переменная итраций
                foreach (string el in header_name)//цикл заполнения заголовков таблицы
                {
                    dataGridView1.Columns[i].HeaderText = el;
                    i++;
                }
                dataGridView1.ClearSelection();
          }
         //----------Заполнение таблицы ----------//


        //----------Загрузка формы ----------//
           
             private void Form3_Load(object sender, EventArgs e)
            {
                filltable();//вызываем метод filltable 
            }

        //----------Загрузка формы ----------//



        //-------Кнопка выхода из формы-------//
             private void Form3_FormClosing(object sender, FormClosingEventArgs e)
             {
                parentform.Show(); //показываем родительскую форму
             }

        //-------Кнопка выхода из формы-------//



        //-------Двойной щелчек по ячейки-------//
        
            private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value); //переменная id заказа
                Form4 order = new Form4(user, orderId, con, typewindow);//экзепляр form4
                order.ShowDialog();//показываем 
            }

            private void Form3_Activated(object sender, EventArgs e)
            {
                filltable();//заполняем таблицу
            }



        //-------Двойной щелчек по ячейки-------//
        
    }
}
