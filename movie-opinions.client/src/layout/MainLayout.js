import Header from '../components/header/Header.js';
import Footer from '../components/footer/Footer.js';
import '../app/App.css';

export default function MainLayout({ children }) {
    return (
        <div className="layout">
            <Header />
            <main className="layout__content">
                {children}
            </main>
            <Footer />
        </div>
    );
}
