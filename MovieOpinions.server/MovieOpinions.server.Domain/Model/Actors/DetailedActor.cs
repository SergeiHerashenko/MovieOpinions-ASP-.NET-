using MovieOpinions.server.Domain.Model.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.Actors
{
    public class DetailedActor : Actor
    {
        public DateTime BirthdayActor { get; private set; }

        public IEnumerable<Film> FilmActor { get; private set; }

        public IEnumerable<Genre> GenreActor { get; private set; }

        public string CountryActor { get; private set; }

        public string URLImageActor { get; private set; }
    }
}
