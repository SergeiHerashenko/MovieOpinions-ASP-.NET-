import './DetailedFilmForm.css';

import { Link } from 'react-router-dom';

const DetailedFilmForm = ({ film }) => {
    return (
        <section className='film'>
            <figure className='film__poster'>
                <img
                    className='film_image'
                    src={film.imageFilm}
                    alt={`Постер до фільму "${film.nameFilm}"`}
                >
                </img>
            </figure>
            <div className='film__info'>
                <div className='film__details'>
                    <span>{film.nameFilm}</span>
                </div>
                <div className='film__details'>
                    <span className="film__label">Рік фільму:</span>
                    <span className="film__value">
                        <Link to='/films'>
                            {film.yearFilm}
                        </Link>
                    </span>
                </div>
                <div className="film__details">
                    <span className="film__label">Жанр:</span>
                    <span className="film__value">
                        {film.genreFilm.map((g, index) => (
                            <Link key={g.idGenre} to={`/genres/${g.idGenre}`}>
                                {g.nameGenre}{index < film.genreFilm.length - 1 ? ', ' : ''}
                            </Link>
                        ))}
                    </span>
                </div>

                <div className="film__details">
                    <span className="film__label">Режисер:</span>
                    <span className="film__value">{film.directorFilm}</span>
                </div>

                <div className="film__details">
                    <span className="film__label">Актори:</span>
                    <span className="film__value">
                        {film.actorFilm.map((a, index) => (
                            <Link key={a.idActor} to={`/actors/${a.idActor}`}>
                                {a.nameActor}{index < film.actorFilm.length - 1 ? ', ' : ''}
                            </Link>
                        ))}
                    </span>
                </div>

                <div className="film__details">
                    <span className="film__label">Країна:</span>
                    <span className="film__value">
                        {film.countryFilm.map((c, index) => (
                            <Link key={c.idCountry}>
                                {c.nameCountry}{index < film.countryFilm.length - 1 ? ', ' : ''}
                            </Link>
                        ))}
                    </span>
                </div>

                <div className="film__details">
                    <span className="film__label">Опис:</span>
                    <p className="film__description">{film.descriptionFilm}</p>
                </div>
            </div>
        </section>
    );
};

export default DetailedFilmForm;
