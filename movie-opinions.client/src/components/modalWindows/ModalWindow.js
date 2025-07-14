import './ModalWindow.css';
import Button from '../ui/button/Button';

const ModalWindow = ({ isOpen, message, onClose }) => {
    if (!isOpen) return null;

    return (
        <div className="modal-overlay">
            <div className="modal">
                <p>{message}</p>
                <Button className='button--close' onClick={onClose}>Закрити</Button>
            </div>
        </div>
    );
};

export default ModalWindow;
