import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

import './FilmDetailed.css'

import FormDetailedFilm from '../../components/ui/formDetailedFilm/FormDetailedFilm.js';
import CommentsSection from '../../components/ui/commentsSection/CommentsSection.js';

const FilmDetailed = () => {
    const { idFilm } = useParams();
    const [film, setFilm] = useState(null);
    const [comment, setComments] = useState([]);

    useEffect(() => {
        const fetchFilm = async () => {
            try {
                const [filmResponse, commentsResponse] = await Promise.all([
                    fetch(`https://localhost:7230/api/Film/${idFilm}`),
                    fetch(`https://localhost:7230/api/Comment/${idFilm}`)
                ]);

                const filmData = await filmResponse.json();
                const commentsData = await commentsResponse.json();

                setFilm(filmData.film);
                setComments(Array.isArray(commentsData?.data) ? commentsData.data : []);

                document.title = filmData.film.nameFilm;
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
        <section>
            <FormDetailedFilm film = {film} ></FormDetailedFilm>
            {comment.map((comment) => (
                <CommentsSection 
                    idComment={comment.idComment}
                    key={comment.idComment} 
                    comment={comment}></CommentsSection>
            ))};
        </section>
    );
};

export default FilmDetailed;