import './Header.css';

import Button from '../../atoms/button/Button.js';
import Logo from '../../atoms/logo/Logo.js';
import LinkList from '../../molecules/linkList/LinkList.js';

import { useState, useRef, useEffect } from 'react';

const links = [
    { id: 1, label: 'Головна', path: '/' },
    { id: 2, label: 'Фільми', path: '/films' },
    { id: 3, label: 'Новинки', path: '/' },
    { id: 4, label: 'Відгуки', path: '/' },
    { id: 5, label: 'Контакти', path: '/' }
];

const Header = ({ user, onLogout }) => {
    const [isOverflow, setIsOverflow] = useState(false);
    const [wrapperWidth, setWrapperWidth] = useState(0);
    const [icons, setIcons] = useState([]);
    const [openMenu, setOpenMenu] = useState(null);

    const wrapperRef = useRef();

    useEffect(() => {
        const handleResize = () => {
            const wrapperWidth = wrapperRef.current.offsetWidth;
            setWrapperWidth(wrapperWidth);
        }

        handleResize(); // перевірка при монтуванні
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    // Завантаження іконок з API
    useEffect(() => {
        fetch("https://localhost:7230/api/home/icon")
            .then(res => res.json())
            .then(data => setIcons(data.data || []));
    }, []);

    const handleLinksVisibilityChange = (hidden) => {
        setIsOverflow(hidden);
    };

    const openButtonsMenu = () => {
        setOpenMenu(prev => prev === 'buttons' ? null : 'buttons');
    };

    return(
        <header className='header'>
            <div className={`header__wrapper ${isOverflow ? 'header__wrapper-mobile' : ''}`}>
                <div ref={wrapperRef} className="header__logo-wrapper">
                    <Logo></Logo>
                </div>
                <div className="header__nav-wrapper">
                    <LinkList
                        links={links}
                        mobileButtonContent={'☰'}
                        onHiddenChange={handleLinksVisibilityChange}
                        openMenu={openMenu}
                        setOpenMenu={setOpenMenu}
                        {...(isOverflow ? { wrapperWidth: wrapperWidth } : {})} 
                    ></LinkList>
                </div>
            </div>
            <div className="header__buttons-wrapper">
                {!isOverflow ? (
                    <>
                        <Button className="button--login">Увійти</Button>
                        <Button className="button--register">Зареєструватися</Button>
                    </>
                ) : (
                    <>
                    <Button className="header__button-mobile" onClick={openButtonsMenu}>
                        <img className='header__button-mobile-img' src={icons.src} alt={icons.name} />
                    </Button>

                    {openMenu === 'buttons' && (
                        <div className="mobile-buttons-dropdown">
                            <Button className="button--login">Увійти</Button>
                            <Button className="button--register">Зареєструватися</Button>
                        </div>
                    )}
                </>
                )}
            </div>
        </header>
    );
};

export default Header;