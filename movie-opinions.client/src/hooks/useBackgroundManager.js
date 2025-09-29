// useBackgroundManager.js
import { useEffect } from 'react';
import { useLocation } from 'react-router-dom';

export default function useBackgroundManager() {
    const location = useLocation();

    useEffect(() => {
        const body = document.body;

        body.classList.remove('DefaultImg', 'CastomImg');

        if (location.pathname.startsWith('/films/')) {
            body.classList.add('CastomImg'); 
        } else {
            body.classList.add('DefaultImg'); 
        }

        return () => {
            body.classList.remove('DefaultImg', 'CastomImg');
        };
    }, [location.pathname]);

    return null;
}
