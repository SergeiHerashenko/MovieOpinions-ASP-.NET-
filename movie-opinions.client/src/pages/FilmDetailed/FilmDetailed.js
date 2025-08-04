import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

const FilmDetailed = () => {
    const { idFilm } = useParams();
    const [film, setFilm] = useState(null);

    useEffect(() => {
        const fetchFilm = async () => {
            try {
                const response = await fetch(`https://localhost:7230/api/Film/${idFilm}`);
                const data = await response.json();
                setFilm(data);
            } catch (error) {
                console.error('Помилка завантаження фільму:', error);
            }
        };

        fetchFilm();
    }, [idFilm]);

    if (!film) {
        return <div>Завантаження...</div>;
    }
    
    return (
        <div className='film-details'>
            <h2>{film.nameFilm}</h2>
            <img src={film.imageFilm} alt='Постер' />
            <p><strong>Рік:</strong> {film.yearFilm}</p>
            <p><strong>Опис:</strong> {film.descriptionFilm}</p>
            {/* додай інші поля */}
        </div>
    );
};

export default FilmDetailed;