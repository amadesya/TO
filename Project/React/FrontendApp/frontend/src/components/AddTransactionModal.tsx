import React, { useState } from 'react';
import apiClient from '../api/apiClient';
import { ProductDTO, WarehouseDTO } from '../types/types';

interface Props {
    products: ProductDTO[];
    warehouses: WarehouseDTO[];
    onClose: () => void;
    onSuccess: () => void;
}

const AddTransactionModal = ({ products, warehouses, onClose, onSuccess }: Props) => {
    const [productId, setProductId] = useState<number>(products[0]?.Id || 0);
    const [fromWarehouseId, setFromWarehouseId] = useState<number | null>(null);
    const [toWarehouseId, setToWarehouseId] = useState<number | null>(null);
    const [quantity, setQuantity] = useState<number>(1);
    const [type, setType] = useState('Приход');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        // Получаем ID текущего пользователя из localStorage
        const userData = localStorage.getItem('user');
        const user = userData ? JSON.parse(userData) : null;

        const data = {
            ProductId: productId,
            FromWarehouseId: type === 'Приход' ? null : fromWarehouseId,
            ToWarehouseId: type === 'Расход' ? null : toWarehouseId,
            EmployeeId: user?.Id, // Автоматически подставляем того, кто вошел
            Quantity: quantity,
            TransactionType: type
        };

        try {
            await apiClient.post('/Transaction', data);
            onSuccess();
            onClose();
        } catch (err) {
            console.error(err);
            alert("Ошибка при проведении операции. Проверьте остатки на складе.");
        }
    };

    return (
        <div style={styles.overlay}>
            <div style={styles.modal}>
                <h3>Новая операция</h3>
                <form onSubmit={handleSubmit} style={styles.form}>
                    
                    <label style={styles.label}>Тип операции:</label>
                    <select value={type} onChange={e => setType(e.target.value)} style={styles.input}>
                        <option value="Приход">Приход (на склад)</option>
                        <option value="Расход">Расход (со склада)</option>
                        <option value="Перемещение">Перемещение (между складами)</option>
                    </select>

                    <label style={styles.label}>Товар:</label>
                    <select value={productId} onChange={e => setProductId(Number(e.target.value))} style={styles.input}>
                        {products.map(p => <option key={p.Id} value={p.Id}>{p.Name}</option>)}
                    </select>

                    {type !== 'Приход' && (
                        <>
                            <label style={styles.label}>Откуда:</label>
                            <select onChange={e => setFromWarehouseId(Number(e.target.value))} style={styles.input} required>
                                <option value="">Выберите склад...</option>
                                {warehouses.map(w => <option key={w.Id} value={w.Id}>{w.Name}</option>)}
                            </select>
                        </>
                    )}

                    {type !== 'Расход' && (
                        <>
                            <label style={styles.label}>Куда:</label>
                            <select onChange={e => setToWarehouseId(Number(e.target.value))} style={styles.input} required>
                                <option value="">Выберите склад...</option>
                                {warehouses.map(w => <option key={w.Id} value={w.Id}>{w.Name}</option>)}
                            </select>
                        </>
                    )}

                    <label style={styles.label}>Количество:</label>
                    <input type="number" min="1" value={quantity} onChange={e => setQuantity(Number(e.target.value))} style={styles.input} />

                    <div style={styles.buttons}>
                        <button type="button" onClick={onClose} style={styles.cancelBtn}>Отмена</button>
                        <button type="submit" style={styles.submitBtn}>Провести</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

const styles = {
    overlay: { position: 'fixed' as const, top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000 },
    modal: { backgroundColor: 'white', padding: '25px', borderRadius: '8px', width: '400px', maxHeight: '90vh', overflowY: 'auto' as const },
    form: { display: 'flex', flexDirection: 'column' as const, gap: '10px' },
    label: { fontSize: '0.85rem', fontWeight: 'bold' as const, color: '#555' },
    input: { padding: '10px', borderRadius: '4px', border: '1px solid #ccc' },
    buttons: { display: 'flex', justifyContent: 'space-between', marginTop: '20px' },
    cancelBtn: { padding: '10px 20px', cursor: 'pointer', border: 'none', borderRadius: '4px' },
    submitBtn: { padding: '10px 20px', backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }
};

export default AddTransactionModal;