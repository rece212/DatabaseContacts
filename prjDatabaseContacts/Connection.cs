using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjDatabaseContacts
{
    class Connection
    {
        public const String conn = "Server=tcp:classdb.database.windows.net,1433;Initial Catalog=Contacts;Persist Security Info=False;User ID=adminadmin;Password=@Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
