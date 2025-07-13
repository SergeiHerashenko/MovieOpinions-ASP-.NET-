using MovieOpinions.server.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.User
{
    public class User
    {
        public Guid UserId { get; set; }

        public string LoginUser { get; set; }

        public string? EmailUser { get; set; }

        public Role Role { get; set; }

        public UserSecurity Security { get; set; }

        public UserProfile Profile { get; set; }
    }
}
