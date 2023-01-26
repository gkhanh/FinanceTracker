using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Finance_Tracking_Web_Application.Models
{
    public class UserLoginInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
