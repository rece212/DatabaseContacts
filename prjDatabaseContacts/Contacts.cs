using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjDatabaseContacts
{
    class Contacts : IComparable
    {
        public int ID = 0;
        private String username="";
        private string firstName = "";
        private string surname = "";
        private string phonenumber = "";
        private string emailAddress = "";

        public string FirstName { get => firstName; set => firstName = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Phonenumber { get => phonenumber; set => phonenumber = value; }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public string Username { get => username; set => username = value; }
        
        public Contacts()
        {

        }
        public Contacts(int id,string firstName, string surname, string phonenumber, string emailAddress, string username)
        {
            this.ID = id;
            FirstName = firstName;
            Surname = surname;
            Phonenumber = phonenumber;
            EmailAddress = emailAddress;
            Username = username;
        }
        public int CompareTo(object obj)
        {
            return firstName.CompareTo(obj.ToString());
        }
        public override string ToString()
        {
            return firstName;
        }

    }
}
