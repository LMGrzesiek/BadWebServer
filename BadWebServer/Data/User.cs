using System;
using System.Collections.Generic;
using System.Text;

namespace GoodServer.Data
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
    }

}
