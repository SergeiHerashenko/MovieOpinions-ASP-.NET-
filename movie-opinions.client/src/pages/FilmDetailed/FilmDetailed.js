import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

import './FilmDetailed.css'

import FormDetailedFilm from '../../components/ui/formDetailedFilm/FormDetailedFilm.js';

const FilmDetailed = () => {
    const { idFilm } = useParams();
    const [film, setFilm] = useState(null);

    useEffect(() => {
        const fetchFilm = async () => {
            try {
                const response = await fetch(`https://localhost:7230/api/Film/${idFilm}`);
                const data = await response.json();
                setFilm(data.film);
                
                document.title = data.film.nameFilm;
            } catch (error) {
                console.error('Помилка завантаження фільму:', error);
            }
        };

        fetchFilm();
    }, [idFilm]);
    
    if(!film){
        return(
            <div className='Error'>
                Ой! Або фільм утік з кадру, або наш сервер вирішив піти на перерву. Спробуй ще раз за мить!
            </div>
        );
    }

    return (
        <FormDetailedFilm film = {film} ></FormDetailedFilm>
    );
};

export default FilmDetailed;