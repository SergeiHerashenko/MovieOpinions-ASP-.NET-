import './Header.css';
import '../../style/Fonts.css';
import { useLayoutEffect, useRef, useState } from 'react';

const NavLink = ({menuOpen}) => {

    const links = [
        { id: 1, label: 'Головна' },
        { id: 2, label: 'Фільми' },
        { id: 3, label: 'Новинки' },
        { id: 4, label: 'Залишити відгук' },
        { id: 5, label: 'Контакти' }
    ];

    const conteinerRef = useRef(null);
    const measurenebtRef = useRef(null);
    const itemRef = useRef([]);

    const [visibleCount, setVisibleCount] = useState(links.length);
    const [isDropdownOpen, setDropdownOpen] = useState(false);

    useLayoutEffect (() => {
        const updateVisibleItems = () => {
            if(!conteinerRef.current || !measurenebtRef.current) return;

            // 🛑 Не виконувати логіку на мобільних екранах
            if (window.innerWidth < 730) {
                setVisibleCount(links.length); // Показати всі
                return;
            }

            // Доступна ширина для меню
            const  containerWidth = conteinerRef.current.offsetWidth;
            // Скільки вже зайнято ширини
            let totalWidth = 0;
            // Скільки пунктів меню влізе в цю ширину
            let fitCount = 0;

            for(let i = 0; i < links.length; i++){
                const item = itemRef.current[i];

                if(!item){
                    break;
                }

                const itemWidth = item.offsetWidth;

                if(totalWidth + itemWidth <= containerWidth - 50){
                    totalWidth += itemWidth;
                    fitCount++;
                } else{
                    break;
                }
            }

            setVisibleCount(fitCount);
        };

        updateVisibleItems();

        const resizeObserver = new ResizeObserver(updateVisibleItems);
        resizeObserver.observe(conteinerRef.current); 
    }, [links]);

    return (
        <>
            {/* Прихований блок для вимірювання розмірів пунктів */}
            <ul ref={measurenebtRef} className='measurement-menu'>
                {links.map((link, i) => (
                    <li key={link.id} ref={(el) => (itemRef.current[i] = el)}>
                        <a href='#'>{link.label}</a>
                    </li>
                ))}
            </ul>

            {/* реальний рендер видимого меню */}
            <nav className={`header__nav ${menuOpen ? "open" : ""}`} ref={conteinerRef}>
                <ul className='header__menu'>
                    {links.slice(0, visibleCount).map((link) => (
                        <li key={link.id}>
                            <a href='#'>{link.label}</a>
                        </li>
                    ))}

                    {visibleCount < links.length && (
                        <li className='header__more' onClick={ () => setDropdownOpen((prev) => !prev)}>
                            ...
                            {isDropdownOpen && (
                                <ul className='header__dropdown'>
                                    {links.slice(visibleCount).map((link) => (
                                        <li key={link.id}>
                                            <a href="#">{link.label}</a>
                                        </li>
                                    ))}
                                </ul>
                            )}
                        </li>
                    )}
                </ul>
            </nav>
        </>
    );
};

export default NavLink;