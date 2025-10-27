import './MainLayout.css';

import Header from '../components/organisms/header/Header';

function MainLayout({ children, user, onLogout }) {
    return(
        <div className='layout'>
            <Header></Header>
            <main className='layout__content'>
                {children}
            </main>
        </div>
    );
}

export default MainLayout;