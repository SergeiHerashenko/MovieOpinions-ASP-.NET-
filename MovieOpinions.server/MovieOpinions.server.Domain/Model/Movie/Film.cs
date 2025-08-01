using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.Movie
{
    public class Film
    {
        public int IdFilm { get; set; }

        public string NameFilm { get; set; }

        public int YearFilm { get; set; }

        public string ImageFilm { get; set; }
    }
}
