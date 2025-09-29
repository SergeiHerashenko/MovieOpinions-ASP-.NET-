import './FilmPage.css';

import { useEffect, useState } from 'react';

import FormFilm from '../../components/ui/formFilm/FormFilm.js';

const FilmPage = () => {
    
    const [films, setFilms] = useState([]);
    const [filter, setFilter] = useState([]);

    useEffect(() => {
        
        document.title = "Movie Opinions - Фільми";

        const fetchFilms = async () => {
            try {
                const [filmsResponse, genresResponse] = await Promise.all([
                    fetch("https://localhost:7230/api/Film/films"),
                    fetch("https://localhost:7230/api/Genre/genre")
                ]);

                if (!filmsResponse.ok || !genresResponse.ok) {
                    console.log("Помилка при отриманні даних");
                    return;
                }

                const filmsData = await filmsResponse.json();
                const genresData = await genresResponse.json();

                setFilms(filmsData.films);
                setFilter(genresData.genres);
            } catch (error) {
                console.error('Помилка:', error);
            }
        };

        fetchFilms();
    }, []);
    
    return(
        <div className='content'>
            <div className='content-filter'>
                <div className='content-filter__genre'>
                    <details open>
                        <summary>Фільтр за жанром</summary>
                        <ul>
                            {filter.map(filterGenre => (
                                <li key={filterGenre.idGenre}>
                                    <input type='checkbox'></input>
                                    {filterGenre.nameGenre}
                                </li>
                            ))}
                        </ul>
                    </details>
                </div>
                <div className='content-filter__Year'>
                    <details open>
                        <summary>Фільтр за роком</summary>
                        <ul>
                            {filter.map(filterGenre => (
                                <li key={filterGenre.idGenre}>
                                    <input type='checkbox'></input>
                                    {filterGenre.nameGenre}
                                </li>
                            ))}
                        </ul>
                    </details>
                </div>
                <div className='content-filter__Country'>
                    <details open>
                        <summary>Фільтр за країною</summary>
                        <ul>
                            {filter.map(filterGenre => (
                                <li key={filterGenre.idGenre}>
                                    <input type='checkbox'></input>
                                    {filterGenre.nameGenre}
                                </li>
                            ))}
                        </ul>
                    </details>
                </div>
            </div>
            <div className='movie-grid'>
                {films.map(film => (
                                <FormFilm
                                    key={film.idFilm}
                                    idFilm={film.idFilm}
                                    NameFilm={film.nameFilm}
                                    YearFilm={film.yearFilm}
                                    URLImageFilm={film.imageFilm}
                                />
                            ))}
            </div>
        </div>
    );
};

export default FilmPage;