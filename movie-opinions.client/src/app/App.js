import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { useEffect } from "react";

import MainLayout from '../layout/MainLayout.js';
import HomePage from '../pages/HomePage/HomePage.js';
import LoginPage from '../pages/LoginPage/LoginPage.js';

import './App.css';

function App() {
    useEffect(() => {
        fetch("https://localhost:7230/api/media/background/HomePage")
            .then(res => res.json())
            .then(response => {
                if (response.statusCode === 200) {
                    document.documentElement.style.setProperty(
                    '--background-image',
                    `url(${response.data.src})`
                    );
                }
            })
            .catch(err => console.error(err));
    }, []);

    return (
        <BrowserRouter>
            <Routes>
                <Route path='/' element={
                    <MainLayout >
                        <HomePage />
                    </MainLayout>
                }>
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;
