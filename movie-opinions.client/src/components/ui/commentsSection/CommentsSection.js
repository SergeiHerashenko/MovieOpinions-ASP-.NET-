import './CommentsSection.css';

import DefaultAvatar from '../../../assets/Image/Icons/default_avatar.png';

const CommentsSection = ({ comment }) => {
    return (
        <section  className='comment'>
            <div className="comment__row"> 
                 <div className='comment__left'>
                    <img className="comment__avatar" src={DefaultAvatar} alt="Nin" />
                    <span>Сергій Геращенко</span>
                </div>
                <div className="comment__center">
                    <main className='comment__text'>
                        Фільм чудовий, мені дуже сподобався!
                    </main>
                    <footer className='comment__text-footer'>
                        <nav className='comment__text-footer-nav'>
                            <ul>
                                <li><a href='#'>Поскаржитися</a></li>
                                <li><a href='#'>Відповісти</a></li>
                                <li><a href='#'>Поділитися</a></li>
                            </ul>
                        </nav>
                    </footer>
                </div>
                <div className="comment__right">
                    <time>06 жовтня 2025</time>
                    <footer className='comment__right-footer'>
                        <nav className='comment__right-footer-nav'>
                            <ul>
                                <li><a href='#'>Змінити</a></li>
                                <li><a href='#'>Видалити</a></li>
                            </ul>
                        </nav>  
                    </footer>
                </div>
            </div>
            <div className="count__answer">
                Показати 4 відповіді
            </div>
        </section >
    );
};

export default CommentsSection;