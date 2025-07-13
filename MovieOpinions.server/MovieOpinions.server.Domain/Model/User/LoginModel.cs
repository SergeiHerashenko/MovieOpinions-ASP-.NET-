using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.User
{
    public class LoginModel
    {
        [Display(Name = "LoginUser")]
        public string LoginUser { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordUser")]
        public string PasswordUser { get; set; }
    }
}
