import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';
import { WarehouseDTO } from '../types/types';

const WarehousesPage = () => {
    const [warehouses, setWarehouses] = useState<WarehouseDTO[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchWarehouses = async () => {
            try {
                const response = await apiClient.get<WarehouseDTO[]>('/Warehouse');
                setWarehouses(response.data);
                setError(null);
            } catch (err) {
                console.error("Ошибка при получении складов:", err);
                setError("Не удалось загрузить данные о складах.");
            } finally {
                setLoading(false);
            }
        };

        fetchWarehouses();
    }, []);

    if (loading) return <div style={styles.container}>Загрузка списка складов...</div>;
    if (error) return <div style={{ ...styles.container, color: 'red' }}>{error}</div>;

    return (
        <div style={styles.container}>
            <h1>Управление складами</h1>
            
            {warehouses.length === 0 ? (
                <p>Список складов пуст.</p>
            ) : (
                <table style={styles.table}>
                    <thead>
                        <tr style={styles.headerRow}>
                            <th style={styles.th}>Название</th>
                            <th style={styles.th}>Адрес</th>
                            <th style={styles.th}>Тип склада</th>
                        </tr>
                    </thead>
                    <tbody>
                        {warehouses.map((warehouse, index) => (
                            <tr key={index} style={index % 2 === 0 ? {} : styles.evenRow}>
                                <td style={styles.td}>{warehouse.Name}</td>
                                <td style={styles.td}>{warehouse.Address}</td>
                                <td style={styles.td}>
                                    <span style={styles.badge}>{warehouse.WarehouseType}</span>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};


const styles = {
    container: {
        padding: '20px',
        fontFamily: 'Arial, sans-serif'
    },
    table: {
        width: '100%',
        borderCollapse: 'collapse' as const,
        marginTop: '20px',
        boxShadow: '0 2px 5px rgba(0,0,0,0.1)'
    },
    headerRow: {
        backgroundColor: '#0078d4',
        color: 'white',
        textAlign: 'left' as const
    },
    th: {
        padding: '12px 15px',
        borderBottom: '1px solid #ddd'
    },
    td: {
        padding: '12px 15px',
        borderBottom: '1px solid #ddd'
    },
    evenRow: {
        backgroundColor: '#f9f9f9'
    },
    badge: {
        backgroundColor: '#e1dfdd',
        padding: '4px 8px',
        borderRadius: '4px',
        fontSize: '0.9em'
    }
};

export default WarehousesPage;