import { useLayoutEffect, useRef, useState, useEffect } from 'react';
import './HomePage.css';

const imageClasses = [
    "img--rotate-left",
    "",                 
    "img--rotate-right",
    "img--flip"  
];

const HomePage = () => {
    const containerRef = useRef(null);
    const measurementRef = useRef(null);
    const itemRef = useRef([]);

    const [images, setImages] = useState([]);
    const [visibleImage, setVisibleImage] = useState(0);

    // Оновлення заголовка сторінки
    useEffect(() => {
        document.title = "Movie Opinions - Головна";
    });

    // Завантаження зображень з API
    useEffect(() => {
        fetch("https://localhost:7230/api/home/image")
            .then(res => res.json())
            .then(data => setImages(data.data || []));
    }, []);

    // Визначення кількості видимих зображень
    useLayoutEffect(() => {
        const updateVisibleImage = () => {
            if (!containerRef.current || !measurementRef.current) return;

            if (window.innerWidth < 230) {
                setVisibleImage(0);
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
    }, [images]);

    return(
        <>
            {/* Прихований контейнер для вимірювання */}
            <div ref={measurementRef} style={{position: 'absolute', visibility: 'hidden', height: 0, overflow: 'hidden', whiteSpace: 'nowrap'}}>
                {images.map((image, i) => (
                    <img
                        key={image.id}
                        src={image.src}
                        alt={image.alt}
                        className={`home-main__img`}
                        ref={el => (itemRef.current[i] = el)}
                    />
                ))}
            </div>

            {/* Видимий контейнер для рендеру */}
            <main className="home-main" ref={containerRef}>
                <div className="home-main__images">
                    {images.slice(0, visibleImage).map((image, i) => (
                        <img
                            key={image.id}
                            src={image.src}
                            alt={image.alt}
                            className={`home-main__img ${imageClasses[i]}`}
                        />
                    ))}
                </div>
                <div className="home-main__text">
                    <h2>Відкрий світ кіно з нами!</h2>
                    <p>Огляди, рейтинги, обговорення — усе в одному місці 🎬</p>
                </div>
            </main>
        </>
    );
};

export default HomePage;