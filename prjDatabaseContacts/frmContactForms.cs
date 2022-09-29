using prjDatabaseContacts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjDatabaseContacts
{
    public partial class frmContactForms : Form
    {
        static String UserLogin = "";
        ArrayList arrContacts = new ArrayList();
        Contacts SelectedContactobj;
        ContactsContext db =new ContactsContext();
        public frmContactForms(string text)
        {
            InitializeComponent();
            UserLogin = text;
        }

        private void frmContactForms_Load(object sender, EventArgs e)
        {
            Reboot();
        }
        public void Reboot()
        {
            arrContacts = new ArrayList();

            List<TblContact> temp= db.TblContacts.Where(ur => ur.Username.Equals(UserLogin)).ToList();
            foreach (var item in temp)
            {

                Contacts temps = new Contacts(item.PersonId,item.FirstName,item.LastName, item.PhoneNumber, item.EmailAddress, item.Username);
                arrContacts.Add(temps);
            }
            /*
            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM tblContacts where Username = '" + UserLogin + "';";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                Contacts temp = new Contacts(reader.GetInt32(0), reader.GetString(1),
                                reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                                arrContacts.Add(temp);

                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }*/
            refreshUI();
        }
        private void refreshUI()
        {
            txtEmail.Clear();
            txtName.Clear();
            txtPhoneNumber.Clear();
            txtSurname.Clear();

            arrContacts.Sort();
            lvOutput.Items.Clear();
            foreach( Contacts contact in arrContacts)
            {
                lvOutput.Items.Add(contact.FirstName + " " + contact.Surname);
            }

            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            richOutput.Clear();
            btnAdd.Enabled = true;

            btnDelete.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length <= 0 || txtName.Text.Length <= 0 || txtPhoneNumber.Text.Length <= 0 || txtSurname.Text.Length <= 0)
            {
                MessageBox.Show("Please enter all the fields, make sure none are empty");
            }
            else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(Connection.conn))
                    {
                        SqlCommand command = new
                            SqlCommand("INSERT INTO tblContacts " +
                                       "VALUES(@Name, @Surname, @Phonenumber,@Email,@UserName) ;" +
                                       "Select SCOPE_IDENTITY();", connection);
                        command.Parameters.AddWithValue("@Name", txtName.Text);
                        command.Parameters.AddWithValue("@Surname", txtSurname.Text);
                        command.Parameters.AddWithValue("@Phonenumber", txtPhoneNumber.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@UserName", UserLogin);
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = command;

                        int id = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                        MessageBox.Show("User has been added to the database " + id);
                        adapter.Dispose();
                        Contacts temp = new Contacts(id, txtName.Text, txtSurname.Text, txtPhoneNumber.Text, txtEmail.Text, UserLogin);
                        arrContacts.Add(temp);
                        refreshUI();

                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error Connecting to the Database", "Connection Error");
                }
            }
        }

        private void lvOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOutput.SelectedItem != null)
            {
                String selectedName = lvOutput.SelectedItem.ToString();
                foreach(Contacts contact in arrContacts )
                {
                    if ((contact.FirstName+" "+contact.Surname).Equals(selectedName))
                    {
                        SelectedContactobj=contact;
                        break;  
                    }
                }
                richOutput.Clear();
                richOutput.Text += "Details \n";
                richOutput.Text += "ID  :"+ SelectedContactobj.ID + "\n";
                richOutput.Text += "Name  :" + SelectedContactobj.FirstName + "\n";
                richOutput.Text += "Surname  :" + SelectedContactobj.Surname + "\n";
                richOutput.Text += "Cell  :" + SelectedContactobj.Phonenumber + "\n";
                richOutput.Text += "Email  :" + SelectedContactobj.EmailAddress + "\n";
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtName.Text = SelectedContactobj.FirstName;
            txtSurname.Text = SelectedContactobj.Surname;
            txtEmail.Text = SelectedContactobj.EmailAddress;
            txtPhoneNumber.Text = SelectedContactobj.Phonenumber;
            btnSave.Enabled=true;
            btnEdit.Enabled = false;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ( txtEmail.Text.Length <= 0 || txtName.Text.Length <= 0 || txtPhoneNumber.Text.Length <= 0 || txtSurname.Text.Length <= 0)
            {
                MessageBox.Show("Please enter all the fields, make sure none are empty");
            }
            else
            {
                SelectedContactobj.FirstName= txtName.Text;
                SelectedContactobj.Surname= txtSurname.Text;
                SelectedContactobj.EmailAddress= txtEmail.Text;
                SelectedContactobj.Phonenumber= txtPhoneNumber.Text;

    

                TblContact c = (from x in db.TblContacts
                              where x.PersonId == SelectedContactobj.ID
                                select x).First();
                c.PhoneNumber = SelectedContactobj.Phonenumber;
                c.FirstName = SelectedContactobj.FirstName;
                c.LastName = SelectedContactobj.Surname;
                c.EmailAddress = SelectedContactobj.EmailAddress;
                db.SaveChanges();


                /*
                try
                {
                    using (SqlConnection connection = new SqlConnection(Connection.conn))
                    {
                        SqlCommand command = new
                            SqlCommand("UPDATE tblContacts " +
                                       "set Firstname  = @Name,LastName= @Surname,PhoneNumber= @Phonenumber,EmailAddress =@Email " +
                                       "where PersonID= @ID;", connection);
                        command.Parameters.AddWithValue("@Name", SelectedContactobj.FirstName);
                        command.Parameters.AddWithValue("@Surname", SelectedContactobj.Surname);
                        command.Parameters.AddWithValue("@Phonenumber", SelectedContactobj.Phonenumber);
                        command.Parameters.AddWithValue("@Email", SelectedContactobj.EmailAddress);
                        command.Parameters.AddWithValue("@ID", SelectedContactobj.ID);
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = command;

                        int id = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                        MessageBox.Show("User has been updated");
                        adapter.Dispose();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error Connecting to the Database " + ex.ToString(), "Connection Error");
                }*/
                Reboot();

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reboot();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            TblContact RemoveItem = db.TblContacts.Where(b => b.PersonId == SelectedContactobj.ID).First();
            db.TblContacts.Remove(RemoveItem);
            db.SaveChanges();

            /*

            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.conn))
                {
                    SqlCommand command = new
                        SqlCommand("DELETE FROM tblContacts WHERE PersonID = @ID;", connection);
                    command.Parameters.AddWithValue("@ID", SelectedContactobj.ID);
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = command;

                    int id = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    MessageBox.Show("User has been Deleted , Good luck");
                    adapter.Dispose();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Connecting to the Database " + ex.ToString(), "Connection Error");
            }*/
            Reboot();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        
    }
}
