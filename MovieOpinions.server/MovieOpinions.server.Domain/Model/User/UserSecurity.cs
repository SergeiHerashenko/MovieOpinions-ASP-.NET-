using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.User
{
    public class UserSecurity
    {
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;

        public bool IsBlocked { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public bool IsEmailConfirmed { get; set; } = false;

        public DateTime? LastLoginDate { get; set; }
    }
}