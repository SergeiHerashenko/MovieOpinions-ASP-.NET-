import './LoginPage.css';
import Form from '../../components/ui/form/Form.js';
import Input from '../../components/ui/input/Input.js';
import Button from '../../components/ui/button/Button.js';
import { Link } from 'react-router-dom';

const LoginPage = () => {

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formData = new FormData(e.target);
        const loginUser = formData.get('loginUser');
        const passwordUser = formData.get('passwordUser');

        try{
            console.log(passwordUser)
            const response = await fetch('https://localhost:7284/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Login: loginUser,
                    Password: passwordUser,
                }),
            });

            if(!response.ok){
                console.log('Error');
            }

            const data = await response.json();
            console.log('Успішний логін:', data)
        } catch (error) {
            alert(error.message);
        }
    };

    return(
        <Form onSubmit={handleSubmit}>
            <section className='form__header'>
                <p>Вітаємо</p>
                <h2>Увійдіть в систему!</h2>
            </section>
            <section className='form__fields'>
                <div className='form__field'>
                    <Input id='loginUser' name='loginUser' required type="text" />
                    <label  className='form__label' htmlFor="loginUser">Логін</label>
                </div>
                <div className='form__field'>
                    <Input id='PasswordUser' name='passwordUser' required type="password" />
                    <label  className='form__label' htmlFor="PasswordUser">Пароль</label>
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
    );
};

export default LoginPage;