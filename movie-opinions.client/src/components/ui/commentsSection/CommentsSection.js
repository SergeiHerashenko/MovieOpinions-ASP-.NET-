import { useState } from 'react';

import './CommentsSection.css';

import DefaultAvatar from '../../../assets/Image/Icons/default_avatar.png';

const CommentsSection = ({ idComment, comment, wrapperClass }) => {
    const sectionClass = wrapperClass ? wrapperClass : 'comment';
    const [isExpanded, setIsExpanded] = useState(false);

    const toggleReplies = () => {
        setIsExpanded(prev => !prev);
    };

    return (
        <section id={idComment} className={`${sectionClass} ${isExpanded ? 'expanded' : ''}`}>
            <div className="comment__row">
                <div className='comment__left'>
                    <img className="comment__avatar" src={DefaultAvatar} alt="Avatar" />
                    <span className="user-name">
                        {comment.user.firstName}
                        <div className="tooltip">
                            <strong>{comment.user.firstName}</strong><br/>
                            {comment.user.lastName}<br/>
                            Додаткова інформація про користувача...
                        </div>
                    </span>
                </div>
                <div className="comment__center">
                    <main className='comment__text'>
                        {comment.textComment}
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
                    <time>{new Date(comment.createdAt).toLocaleDateString("uk-UA")}</time>
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

            {comment.replies.length > 0 && (
                <div className="count__answer" onClick={toggleReplies}>
                    {isExpanded ? "Сховати відповіді" : `Показати ${comment.replies.length} відповіді`}
                </div>
            )}

            <div className={`replies-wrapper ${isExpanded ? 'visible' : ''}`}>
                {comment.replies.map(reply => (
                    <CommentsSection
                        key={reply.idComment}
                        idComment={reply.idComment}
                        comment={reply}
                        wrapperClass="answer"
                    />
                ))}
            </div>
        </section>
    );
};

export default CommentsSection;
