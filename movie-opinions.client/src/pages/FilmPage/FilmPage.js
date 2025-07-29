import './FilmPage.css';
import { useEffect, useState } from 'react';
import FormFilm from '../../components/ui/formFilm/FormFilm.js';

const FilmPage = () => {
    
    const [films, setFilms] = useState([]);

    useEffect(() => {
        const fetchFilms = async () => {
            try {
                const response = await fetch("https://localhost:7230/api/Film/films", {
                    method: "GET"
                });

                if(!response.ok) {
                    console.log("Помилка")
                }

                const data = await response.json();
                setFilms(data);

                console.log(data);
            } catch (error) {
                console.error('Помилка:', error);
            }
        };

        fetchFilms();
    }, []);
    
    return(
        <p>Сторінка фільмів</p>
    );
};

export default FilmPage;