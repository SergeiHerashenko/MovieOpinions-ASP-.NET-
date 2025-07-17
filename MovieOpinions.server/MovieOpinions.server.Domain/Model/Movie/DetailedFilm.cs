using MovieOpinions.server.Domain.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Model.Movie
{
    public class DetailedFilm : Film
    {
        public string DescriptionFilm { get; private set; }

        public IEnumerable<Actor> ActorFilm { get; private set; }

        public IEnumerable<Genre> GenreFilm { get; private set; }

        public IEnumerable<Country> CountryFilm { get; private set; }

        public double RatingFilm { get; private set; }

        public string URLImageFilm { get; private set; }
    }
}
