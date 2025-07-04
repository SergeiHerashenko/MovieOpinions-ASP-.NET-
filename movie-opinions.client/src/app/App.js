import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HomePage from '../pages/HomePage/HomePage.js'
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
            </Routes>
        </BrowserRouter>
    );
}

export default App;
