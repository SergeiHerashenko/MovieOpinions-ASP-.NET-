import './LinkList.css';
import { useEffect, useRef, useState } from 'react';
import { Link } from 'react-router-dom';

import Button from '../../atoms/button/Button';

const LinkList = ({ links, mobileButtonContent, onHiddenChange, wrapperWidth, openMenu, setOpenMenu }) => {
    const containerRef = useRef();
    const itemsRef = useRef([]);

    const [containerWidth, setContainerWidth] = useState(0);
    const [totalLinksWidth, setTotalLinksWidth] = useState(0);
    const [isHidden, setIsHidden] = useState(false);
    const [localOpen, setLocalOpen] = useState(false);

    // Оновлюємо ширину контейнера
    const updateContainerWidth = () => {
        if (containerRef.current) {
            setContainerWidth(containerRef.current.clientWidth);
        }
    };

    // Розрахунок сумарної ширини лінків
    const calculateTotalLinksWidth = () => {
        const totalWidth = itemsRef.current.reduce((sum, el, index) => {
            if (!el) return sum;
            return sum + el.offsetWidth + (index < itemsRef.current.length - 1 ? 10 : 0);
        }, 0);
        setTotalLinksWidth(totalWidth);
    };

    useEffect(() => {
        updateContainerWidth();
        calculateTotalLinksWidth();
        window.addEventListener('resize', updateContainerWidth);

        return () => window.removeEventListener('resize', updateContainerWidth);
    }, [links]);

    // Скидаємо посилання перед рендером
    itemsRef.current = [];
    
    // Визначаємо, чи лінки ховаються
    useEffect(() => {
        const hidden = containerWidth < totalLinksWidth;
        setIsHidden(hidden);
        if((totalLinksWidth + 200 ) < wrapperWidth) {
            setContainerWidth(wrapperWidth)
        }
        if (onHiddenChange) onHiddenChange(hidden);
    }, [containerWidth, totalLinksWidth, onHiddenChange, wrapperWidth]);

    const isControlled = openMenu !== undefined && setOpenMenu !== undefined;
    const isOpen = isControlled ? openMenu === 'links' : localOpen;

    const openDropDownMenu = () => {
        if (isControlled) {
            setOpenMenu(prev => (prev === 'links' ? null : 'links'));
        } else {
            setLocalOpen(prev => !prev);
        }
    };

    return (
        <div ref={containerRef} className="menu-container">
            {containerWidth >= totalLinksWidth ? (
                <ul className="menu">
                    {links.map((link, index) => (
                        <li key={link.id} ref={(el) => (itemsRef.current[index] = el)}>
                            <Link to={link.path}>{link.label}</Link>
                        </li>
                    ))}
                </ul>
            ) : (
                <>
                    <Button className="menu-mobile" onClick={openDropDownMenu}>
                        {mobileButtonContent}
                    </Button>

                    {isOpen && (
                        <div className="mobile-dropdown">
                            <ul>
                                {links.map(link => (
                                    <li key={link.id}>
                                        <Link to={link.path} onClick={() => setOpenMenu(false)}>
                                            {link.label}
                                        </Link>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default LinkList;
