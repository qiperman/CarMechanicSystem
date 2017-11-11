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
    public partial class Form6 : Form
    {

        //----Объявляем переменные----//
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\STO.accdb"); //создаем новое подключение 
            OleDbDataAdapter adapter = new OleDbDataAdapter();//перменная адаптера 
        //----Объявляем переменные----//

        //----Конструктор формы----//
            public Form6()
            {
                InitializeComponent();
                update();
            }
       //----Конструктор формы----//


       //----Акцивация кода формы----//

            private void Form6_Activated(object sender, EventArgs e)
            {
                update();
            }

       //----Акцивация кода формы----//

        void update() {
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            con.Open();//открываем подключение 
            string query = "SELECT employeeID, surname,first_name,second_name,job FROM Employees WHERE NOT employeeID = -1 AND NOT employeeID = 0";

            OleDbCommand command = new OleDbCommand(query, con);//выполням команду
            con.Close();//закрываем подключение
            adapter.SelectCommand = command;//адаптеру присваиваем команду
            adapter.Fill(dataSet);//заполням адаптер
            dataGridView1.DataSource = dataSet.Tables[0];//присваиваем datasourse
            adapter.Update(dataSet);//обновлям

            string[] header_name = { "Id сотрудника","Фамилия", "Имя", "Отчество", "Должность"};

            int i = 0;//переменная итраций

            int status_num = dataGridView1.ColumnCount - 1;
            dataGridView1.ClearSelection();
            foreach (string el in header_name)//цикл заполнения заголовков таблицы
            {
                dataGridView1.Columns[i].HeaderText = el;
                i++;
            }
        
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form7 f = new Form7(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addempl addempl = new addempl();
            addempl.ShowDialog();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            update();
        }
    }
}
