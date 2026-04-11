import React from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';

const Navbar = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const userData = localStorage.getItem('user');
    const user = userData ? JSON.parse(userData) : null;

    // Функция выхода
    const handleLogout = () => {
        localStorage.removeItem('user'); 
        navigate('/login');              
    };

    if (location.pathname === '/login') return null;

    return (
        <nav style={styles.nav}>
            <div style={styles.logo}>📦 Складской Учёт</div>
            
            <div style={styles.links}>
                <Link to="/products" style={styles.link}>Товары</Link>
                <Link to="/warehouses" style={styles.link}>Склады</Link>
                <Link to="/transactions" style={styles.link}>История</Link>
                <Link to="/inventory" style={styles.link}>Остатки</Link>
                
                {user && (
                    <div style={styles.userSection}>
                        <span style={styles.userName}>👤 {user.FullName}</span>
                        <button onClick={handleLogout} style={styles.logoutButton}>
                            Выйти
                        </button>
                    </div>
                )}
            </div>
        </nav>
    );
};

const styles = {
    nav: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '0 30px',
        backgroundColor: '#0078d4',
        color: 'white',
        height: '60px',
        marginBottom: '20px'
    },
    logo: { fontSize: '1.5rem', fontWeight: 'bold' },
    links: { display: 'flex', alignItems: 'center', gap: '20px' },
    link: { color: 'white', textDecoration: 'none', fontSize: '1.1rem' },
    userSection: {
        display: 'flex',
        alignItems: 'center',
        gap: '15px',
        borderLeft: '1px solid rgba(255,255,255,0.3)',
        paddingLeft: '20px',
        marginLeft: '10px'
    },
    userName: { fontSize: '0.95rem'},
    logoutButton: {
        backgroundColor: '#ffffff', 
        color: 'black',
        border: 'none',
        padding: '6px 12px',
        borderRadius: '4px',
        cursor: 'pointer',
        fontWeight: 'bold',
        fontSize: '0.85rem'
    }
};

export default Navbar;