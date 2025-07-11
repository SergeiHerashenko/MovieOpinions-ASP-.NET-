import './Footer.css';
import Button from '../ui/button/Button.js';
import gitHabImage from '../../assets/Image/Icons/github_Icon.png';
import telegramImage from '../../assets/Image/Icons/telegram_Icon.png';

const Footer = () => {
    return(
        <footer className='footer'>
            <div className="footer__content">
            <div className="footer__contacts">
                <p>📧 Email: support@movieopinions.com</p>
                <p>📞 Тел: +38 050 123 4567</p>
            </div>
            <div className="footer__social">
                <Button className="footer__logo">
                    <img src={gitHabImage} className='footer__img' alt="Login" />
                </Button>
                <Button className="footer__logo">
                    <img src={telegramImage} className='footer__img' alt="Login" />
                </Button>
            </div>
            </div>
            <div className="footer__bottom">
                <p>&copy; 2025 MovieOpinions. Усі права захищено.</p>
            </div>
        </footer>
    );
};

export default Footer;