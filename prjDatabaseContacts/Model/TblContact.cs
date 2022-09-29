using System;
using System.Collections.Generic;

#nullable disable

namespace prjDatabaseContacts.Model
{
    public partial class TblContact
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
    }
}
