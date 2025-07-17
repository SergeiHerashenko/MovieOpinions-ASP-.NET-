import './FormFilm.css';

const FormFilm = ({ NameFilm, YearFilm, URLImageFilm }) => {
    return(
        <article  className='movie-card'>
            <figure className='movie-card__img'>
                <img src={URLImageFilm} alt='Постер до фільму'></img>
            </figure>
            <section className='movie-card__information'>
                {NameFilm},
                {YearFilm}
            </section>
        </article>
    );
};

export default FormFilm;