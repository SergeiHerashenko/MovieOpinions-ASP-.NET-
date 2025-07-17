using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.Movie
{
    public class Film
    {
        public int IdFilm { get; private set; }

        public string NameFilm { get; private set; }

        public int YearFilm { get; private set; }
    }
}
