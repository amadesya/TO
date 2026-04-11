import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';
import { TransactionDTO } from '../types/types';
import AddTransactionModal from '../components/AddTransactionModal';
import { ProductDTO, WarehouseDTO } from '../types/types';

const TransactionsPage = () => {
    const [transactions, setTransactions] = useState<TransactionDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const [products, setProducts] = useState<ProductDTO[]>([]);
    const [warehouses, setWarehouses] = useState<WarehouseDTO[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const fetchData = async () => {
        setLoading(true);
        try {
            const [transRes, prodRes, whRes] = await Promise.all([
                apiClient.get<TransactionDTO[]>('/Transaction'),
                apiClient.get<ProductDTO[]>('/Product'),
                apiClient.get<WarehouseDTO[]>('/Warehouse')
            ]);
            setTransactions(transRes.data);
            setProducts(prodRes.data);
            setWarehouses(whRes.data);
        } catch (err) { console.error(err); }
        finally { setLoading(false); }
    };

    useEffect(() => { fetchData(); }, []);

    useEffect(() => {
        apiClient.get<TransactionDTO[]>('/Transaction')
            .then(res => setTransactions(res.data))
            .catch(err => console.error(err))
            .finally(() => setLoading(false));
    }, []);

    // Функция для красивой даты
    const formatDate = (dateStr: string | Date) => {
        return new Date(dateStr).toLocaleString('ru-RU');
    };

    if (loading) return <div style={styles.container}>Загрузка истории операций...</div>;

    return (
        <div style={styles.container}>
            <h1>История движений товаров</h1>
            <button
                onClick={() => setIsModalOpen(true)}
                style={{ padding: '10px 20px', backgroundColor: '#0078d4', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
            >
                + Создать операцию
            </button>
            <table style={styles.table}>
                <thead>
                    <tr style={styles.headerRow}>
                        <th style={styles.th}>Дата</th>
                        <th style={styles.th}>Товар</th>
                        <th style={styles.th}>Тип</th>
                        <th style={styles.th}>Кол-во</th>
                        <th style={styles.th}>Откуда</th>
                        <th style={styles.th}>Куда</th>
                        <th style={styles.th}>Сотрудник</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions.map((t) => (
                        <tr key={t.Id} style={styles.row}>
                            <td style={styles.td}>{formatDate(t.TransactionDate)}</td>
                            <td style={styles.td}><strong>{t.ProductName}</strong></td>
                            <td style={styles.td}>
                                <span style={{
                                    ...styles.badge,
                                    backgroundColor: t.TransactionType === 'Приход' ? '#dff6dd' : '#fde7e9',
                                    color: t.TransactionType === 'Приход' ? '#107c10' : '#a4262c'
                                }}>
                                    {t.TransactionType}
                                </span>
                            </td>
                            <td style={styles.td}>{t.Quantity} шт.</td>
                            <td style={styles.td}>{t.FromWarehouseName || '—'}</td>
                            <td style={styles.td}>{t.ToWarehouseName || '—'}</td>
                            <td style={styles.td}>{t.EmployeeName}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {isModalOpen && (
                <AddTransactionModal
                    products={products}
                    warehouses={warehouses}
                    onClose={() => setIsModalOpen(false)}
                    onSuccess={fetchData}
                />
            )}
        </div>
    );
};

const styles = {
    container: { padding: '20px', fontFamily: 'Arial, sans-serif' },
    table: { width: '100%', borderCollapse: 'collapse' as const, marginTop: '20px', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' },
    headerRow: { backgroundColor: '#0078d4', color: 'white', textAlign: 'left' as const },
    th: { padding: '12px 15px' },
    td: { padding: '12px 15px', borderBottom: '1px solid #ddd' },
    row: { transition: 'background-color 0.2s' },
    badge: { padding: '4px 8px', borderRadius: '4px', fontSize: '0.85em', fontWeight: 'bold' as const }
};

export default TransactionsPage;