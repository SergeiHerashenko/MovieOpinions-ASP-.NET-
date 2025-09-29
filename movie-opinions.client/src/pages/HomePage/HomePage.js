import './HomePage.css';

import imageOne from '../../assets/Image/HomePage/Image_One.png';
import imageTwo from '../../assets/Image/HomePage/Image_Two.png';
import imageThree from '../../assets/Image/HomePage/Image_Three.png';
import imageFour from '../../assets/Image/HomePage/Image_Four.png';

import { useLayoutEffect, useRef, useState, useEffect } from 'react';

const images = [
    {id: 1, src: imageOne, alt: "Movie1", className: "img--rotate-left"},
    {id: 2, src: imageTwo, alt: "Movie2", className: ""},
    {id: 3, src: imageThree, alt: "Movie3", className: "img--rotate-right"},
    {id: 4, src: imageFour, alt: "Movie4", className: "img--flip"},
];

const HomePage = () => {
    const containerRef = useRef(null);
    const measurementRef = useRef(null);
    const itemRef = useRef([]);

    useEffect(() => {
        document.title = "Movie Opinions - Головна";
    });

    const [visibleImage, setVisibleImage] = useState(images.length);

    useLayoutEffect(() => {
        const updateVisibleImage = () => {
            if (!containerRef.current || !measurementRef.current) return;

            if (window.innerWidth < 230) {
                setVisibleImage(0); // При дуже вузькому екрані — нічого не показуємо
                return;
            }

            const containerWidth = containerRef.current.offsetWidth;

            let totalWidth = 0;
            let fitCount = 0;

            for (let i = 0; i < images.length; i++) {
                const item = itemRef.current[i];
                if (!item) break;

                const itemWidth = item.offsetWidth;

                if (totalWidth + itemWidth < containerWidth - 200) {
                    totalWidth += itemWidth;
                    fitCount++;
                } else {
                    break;
                }
            }

            setVisibleImage(fitCount);
        };

        updateVisibleImage();

        const resizeObserver = new ResizeObserver(updateVisibleImage);
        resizeObserver.observe(containerRef.current);

        window.addEventListener('resize', updateVisibleImage);

        return () => {
            resizeObserver.disconnect();
            window.removeEventListener('resize', updateVisibleImage);
        };
    }, []);

    return (
        <>
            {/* Прихований контейнер для вимірювання */}
            <div ref={measurementRef} style={{position: 'absolute', visibility: 'hidden', height: 0, overflow: 'hidden', whiteSpace: 'nowrap'}}>
                {images.map((image, i) => (
                    <img
                        key={image.id}
                        src={image.src}
                        alt={image.alt}
                        className={`main__images__img ${image.className}`}
                        ref={el => (itemRef.current[i] = el)}
                    />
                ))}
            </div>

            {/* Видимий контейнер для рендеру */}
            <main className="main" ref={containerRef}>
                <div className="main__images">
                    {images.slice(0, visibleImage).map(image => (
                        <img
                            key={image.id}
                            src={image.src}
                            alt={image.alt}
                            className={`main__images__img ${image.className}`}
                        />
                    ))}
                </div>
                <div className="main__text">
                    <h2>Відкрий світ кіно з нами!</h2>
                    <p>Огляди, рейтинги, обговорення — усе в одному місці 🎬</p>
                </div>
            </main>
        </>
    );
};

export default HomePage;