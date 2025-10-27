import './Button.css';

function Button({ title, onClick, className, children }) {
    return (
        <button className={`button ${className || ''}`} onClick={onClick}>
            { children || title}
        </button>
    );
}

export default Button;