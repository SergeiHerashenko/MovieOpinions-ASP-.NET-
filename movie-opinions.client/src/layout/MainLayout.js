import Header from '../components/header/Header.js';
import Footer from '../components/footer/Footer.js';
import '../app/App.css';

export default function MainLayout({ children, user, onLogout }) {
    return (
        <div className="layout">
            <Header user={user} onLogout={onLogout} />
            <main className="layout__content">
                {children}
            </main>
            <Footer />
        </div>
    );
}
