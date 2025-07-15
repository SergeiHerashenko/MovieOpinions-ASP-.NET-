import Form from '../../components/ui/form/Form.js';
import Input from '../../components/ui/input/Input.js';
import Button from '../../components/ui/button/Button.js';
import ModalWindow from '../../components/modalWindows/ModalWindow.js';
import './RegistrationPage.css';
import { Link } from 'react-router-dom';
import React, { useState } from 'react';

const RegistrationPage = ({ onLogin }) => {

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [modalMessage, setModalMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formData = new FormData(e.target);
        const loginUser = formData.get('loginUser');
        const passwordUser = formData.get('passwordUser');
        const confirmPasswordUser = formData.get('confirmPasswordUser');

        try{
            const response = await fetch('https://localhost:7230/api/account/Registration', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    LoginUser: loginUser,
                    PasswordUser: passwordUser,
                    ConfirmPasswordUser: confirmPasswordUser,
                }),
            });

            const data = await response.json();

            if (!response.ok) {
                setModalMessage(data.message);
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
                    <p>зареєструйте свій аккаунт!</p>
                </section>
                <section className='form__fields'>
                    <div className='form__field'>
                        <Input id='loginUser' name='loginUser' required type="text" />
                        <label  className='form__label' htmlFor="loginUser">Введіть логін</label>
                    </div>
                    <div className='form__field'>
                        <Input id='PasswordUser' name='passwordUser' required type="password" />
                        <label  className='form__label' htmlFor="PasswordUser">Введіть пароль</label>
                    </div>
                    <div className='form__field'>
                        <Input id='confirmPasswordUser' name='confirmPasswordUser' required type="password" />
                        <label  className='form__label' htmlFor="confirmPasswordUser">Повторіть пароль</label>
                    </div>
                </section>
                <section className='form__submit'>
                    <Button className='button--submit'  type="submit">
                        Зараєструватися
                    </Button>
                </section>
                <section className='form__footer'>
                    <p>Вже маєте аккаун?</p>
                    <Link to='/login'>Увійдіть у свій аккаунт</Link>
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

export default RegistrationPage;