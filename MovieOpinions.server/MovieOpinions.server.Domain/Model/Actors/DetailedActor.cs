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
        public DateTime BirthdayActor { get; set; }

        public IEnumerable<Film> FilmsActor { get; set; }

        public IEnumerable<Genre> GenresActor { get; set; }

        public string CountryActor { get; set; }

        public string URLImageActor { get; set; }
    }
}
