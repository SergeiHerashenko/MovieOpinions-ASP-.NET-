import './LoginPage.css';
import Form from '../../components/ui/form/Form.js';
import Input from '../../components/ui/input/Input.js';
import Button from '../../components/ui/button/Button.js';
import ModalWindow from '../../components/modalWindows/ModalWindow.js';
import { Link } from 'react-router-dom';
import React, { useState } from 'react';

const LoginPage = ({ onLogin }) => {

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [modalMessage, setModalMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formData = new FormData(e.target);
        const loginUser = formData.get('loginUser');
        const passwordUser = formData.get('passwordUser');

        try{
            const response = await fetch('https://localhost:7230/api/account/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    LoginUser: loginUser,
                    PasswordUser: passwordUser,
                }),
            });

            const data = await response.json();

            if (!response.ok) {
                setModalMessage(data.message || 'Невірний логін або пароль');
                setIsModalOpen(true);
                return;
            }
            else{
                localStorage.setItem('jwtToken', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));
                onLogin(data.user, data.token);
            }

        } catch (error) {
            setModalMessage(`Отакої... що сталось з сервером. Код помилки ${error}`);
            setIsModalOpen(true);
        }
    };

    return(
        <>
            <Form onSubmit={handleSubmit}>
                <section className='form__header'>
                    <p>Вітаємо,</p>
                    <p>увійдіть в систему!</p>
                </section>
                <section className='form__fields'>
                    <div className='form__field'>
                        <Input id='loginUser' name='loginUser' required type="text" />
                        <label  className='form__label' htmlFor="loginUser">Логін</label>
                    </div>
                    <div className='form__field'>
                        <Input id='passwordUser' name='passwordUser' required type="password" />
                        <label  className='form__label' htmlFor="passwordUser">Пароль</label>
                    </div>
                </section>
                <section className='form__submit'>
                    <Button className='button--submit'  type="submit">
                        Увійти
                    </Button>
                </section>
                <section className='form__footer'>
                    <p>Досі не зареєстровані?</p>
                    <Link to='/register'>Зареєструватися</Link>
                </section>
            </Form>

            <ModalWindow
                isOpen={isModalOpen}
                message={modalMessage}
                onClose={() => setIsModalOpen(false)}
            />
        </>
    );
};

export default LoginPage;