using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    public class EnterUser
    {
        int id;
        string type;
        string surname;
        string name;
        string second_name;
        string photo;



        public  EnterUser(int id, string type, string surname = "", string name = "", string second_name = "" ,string photo="")//, string registration="", string phone="", string email="")
        {
            this.id = id;
            this.type = type;
            this.surname = surname;
            this.name = name;
            this.second_name = second_name;
            this.photo = photo;

            /*this.registration = registration;
            this.phone = phone;
            this.email = email;
             * */
        }

        public int Id
        {
            get{return id;}
            set{id = value;}
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Second_name
        {
            get { return second_name; }
            set { second_name = value; }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }

    }
}
