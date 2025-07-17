import { BrowserRouter, Route, Routes } from 'react-router-dom';
import React, { useState } from 'react';

import HomePage from '../pages/HomePage/HomePage.js';
import LoginPage from '../pages/LoginPage/LoginPage.js';
import RegistrationPage from '../pages/RegistrationPage/RegistrationPage.js';
import FilmPage from '../pages/FilmPage/FilmPage.js';
import MainLayout from '../layout/MainLayout.js';

import './App.css';


function App() {

    const [user, setUser] = useState(() => {
        const storedUser = localStorage.getItem('user');
        return storedUser ? JSON.parse(storedUser) : null;
    });

    const handleLogin = (userData, token) => {
        localStorage.setItem('jwtToken', token);
        localStorage.setItem('user', JSON.stringify(userData));
        setUser(userData);
    };

    const handleLogout = () => {
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('user');
        setUser(null);
    };

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={
                    <MainLayout user={user} onLogout={handleLogout} >
                        <HomePage />
                    </MainLayout>
                } />
                <Route path='/login' element={
                    <MainLayout user={user} onLogout={handleLogout} >
                        <LoginPage onLogin={handleLogin} />
                    </MainLayout>
                } />
                <Route path='/register' element={
                    <MainLayout user={user} onLogout={handleLogout} >
                        <RegistrationPage onLogin={handleLogin} />
                    </MainLayout>
                } />
                <Route path='/films' element={
                    <MainLayout user={user} onLogout={handleLogout} >
                        <FilmPage onLogin={handleLogin} />
                    </MainLayout>
                } />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
