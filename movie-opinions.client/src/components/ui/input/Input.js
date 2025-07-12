import './Input.css';

const Input = ({ id, name, type, required}) => {
    return(
        <input 
            id={id}
            className='input'
            name={name}
            type={type}
            required={required} 
        />
    );
};

export default Input;