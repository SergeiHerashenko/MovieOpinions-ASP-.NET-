import './FormFilm.css';

import { Link } from 'react-router-dom';

const FormFilm = ({ idFilm, NameFilm, YearFilm, URLImageFilm }) => {
    return(
        <Link to={`/films/${idFilm}`}>
            <article  className='movie-card'>
                <figure className='movie-card__img'>
                    <img src={URLImageFilm} alt='Постер до фільму'></img>
                </figure>
                <section className='movie-card__information'>
                    <div className='movie-card__title'>{NameFilm}</div>
                    <div className='movie-card__year'>{YearFilm}</div>
                </section>
            </article>
        </Link>
    );
};

export default FormFilm;