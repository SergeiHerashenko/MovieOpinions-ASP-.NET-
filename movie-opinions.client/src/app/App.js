import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage.js';
import LoginPage from '../pages/LoginPage/LoginPage.js';
import './App.css';
import MainLayout from '../layout/MainLayout.js';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={
                    <MainLayout>
                        <HomePage />
                    </MainLayout>
                } />
                <Route path='/login' element={
                    <MainLayout>
                        <LoginPage />
                    </MainLayout>
                } />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
