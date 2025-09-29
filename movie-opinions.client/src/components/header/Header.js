import './Header.css';
import '../../style/Variables.css';

import NavLinks from '../header/NavLinks.js'
import Logo from "./Logo.js";
import Button from '../ui/button/Button.js';
import loginIcon from '../../assets/Image/Login_icon.png';

import { useNavigate } from "react-router-dom";
import { useState } from "react";

const Header = ({ user, onLogout }) => {

    const navigate = useNavigate();

    const [menuOpen, setMenuOpen] = useState(false);
    const [loginOpen, setLoginOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen((prev) => !prev);
        setLoginOpen(false);
    };

    const toggleLogin = () => {
        setLoginOpen((prev) => !prev);
        setMenuOpen(false);
    };

    return (
        <header className="header">

            <Button className="header__menu-mobile" onClick={toggleMenu}>
                ☰
            </Button>
            <div className="header__logo-wrapper">
                <Logo />
            </div>
            <div className="header__nav-wrapper">
                <NavLinks menuOpen={menuOpen} />
            </div>

            <div className="header__buttons-wrapper">
                <nav className={`header__auth ${loginOpen ? "open" : ""}`}>
                    {user ? (
                        <>
                            <span className="header__username">Вітаю, {user.loginUser}!</span>
                            <Button className="button--logout" onClick={onLogout}>Вийти</Button>
                        </>
                    ) : (
                        <>
                            <Button className="button--login" onClick={() => navigate("/login")}>Увійти</Button>
                            <Button className="button--register" onClick={() => navigate("/register")}>Зареєструватися</Button>
                        </>
                    )}
                </nav>
            </div>
            
            <Button className="header__menu-Login-mobile" onClick={toggleLogin}>
                <img src={loginIcon} className='header__img' alt="Увійти" />
            </Button>
        </header>
    );
};

export default Header;