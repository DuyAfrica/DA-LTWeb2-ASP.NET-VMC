using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCP.Models
{
    public class LoginVM
    {

        public string Username { get; set; }
        public string RawPW { get; set; }
        public bool Remember { get; set; }

    }
}