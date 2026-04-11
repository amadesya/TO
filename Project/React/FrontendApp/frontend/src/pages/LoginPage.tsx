import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../api/apiClient';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await apiClient.post('/Employee/login', { 
                email, 
                password 
            });
            // Сохраняем данные пользователя, чтобы Navbar их увидел
            localStorage.setItem('user', JSON.stringify(response.data));
            navigate('/products');
        } catch (err) {
            setError("Неверный логин или пароль");
        }
    };

    return (
        <div style={styles.container}>
            <form onSubmit={handleLogin} style={styles.card}>
                <h2>Вход в систему</h2>
                {error && <p style={{color: 'red'}}>{error}</p>}
                <input 
                    type="email" 
                    placeholder="Email" 
                    onChange={e => setEmail(e.target.value)} 
                    style={styles.input}
                    required 
                />
                <input 
                    type="password" 
                    placeholder="Пароль" 
                    onChange={e => setPassword(e.target.value)} 
                    style={styles.input}
                    required 
                />
                <button type="submit" style={styles.button}>Войти</button>
            </form>
        </div>
    );
};

const styles = {
    container: { display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' },
    card: { padding: '30px', boxShadow: '0 4px 8px rgba(0,0,0,0.1)', borderRadius: '8px', display: 'flex', flexDirection: 'column' as const, gap: '15px', width: '300px' },
    input: { padding: '10px', borderRadius: '4px', border: '1px solid #ccc' },
    button: { padding: '10px', backgroundColor: '#0078d4', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }
};

export default LoginPage;