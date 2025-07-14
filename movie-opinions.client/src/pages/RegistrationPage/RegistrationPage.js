import Form from '../../components/ui/form/Form.js';
import Input from '../../components/ui/input/Input.js';
import Button from '../../components/ui/button/Button.js';
import './RegistrationPage.css';
import { Link } from 'react-router-dom';

const RegistrationPage = () => {
    return(
        <Form>
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
                    <Input id='confirmPasswordUser' name='passwordUser' required type="password" />
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
    );
};

export default RegistrationPage;