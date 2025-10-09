using MovieOpinions.server.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct.Users;

namespace MovieOpinions.server.Domain.Model.Comments
{
    public class Comment
    {
        public int IdComment { get; set; }

        public Guid IdUser { get; set; }

        public UserProfile User { get; set; }

        public string TextComment { get; set; }

        public int IdFilm { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsEdited { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ParentCommentId { get; set; }

        public ICollection<Comment>? Replies { get; set; } 
    }
}
