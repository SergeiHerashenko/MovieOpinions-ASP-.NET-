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
        public string DescriptionFilm { get; set; }

        public IEnumerable<Actor> ActorFilm { get; set; }

        public IEnumerable<Genre> GenreFilm { get; set; }

        public IEnumerable<Country> CountryFilm { get; set; }

        public string DirectorFilm { get; set; }

        public double RatingFilm { get; set; }
    }
}
