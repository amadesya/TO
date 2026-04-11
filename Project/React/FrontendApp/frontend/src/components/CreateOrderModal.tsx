import React, { useState } from 'react';
import apiClient from '../api/apiClient';
import { ProductDTO, WarehouseDTO } from '../types/types';

interface Props {
    products: ProductDTO[];
    warehouses: WarehouseDTO[];
    onClose: () => void;
    onSuccess: () => void;
}

const CreateOrderModal = ({ products, warehouses, onClose, onSuccess }: Props) => {
    const [orderNumber, setOrderNumber] = useState('');
    const [marketplace, setMarketplace] = useState('Ozon');
    const [productId, setProductId] = useState<number>(products[0]?.Id || 0);
    const [warehouseId, setWarehouseId] = useState<number>(warehouses[0]?.Id || 0);
    const [quantity, setQuantity] = useState<number>(1);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        const userData = localStorage.getItem('user');
        const user = userData ? JSON.parse(userData) : null;

        const data = {
            OrderNumber: orderNumber,
            Marketplace: marketplace,
            ProductId: productId,
            WarehouseId: warehouseId,
            EmployeeId: user?.Id,
            Quantity: quantity
        };

        try {
            await apiClient.post('/MarketplaceOrder', data);
            alert("Заказ успешно создан! Товар зарезервирован.");
            onSuccess();
            onClose();
        } catch (err: any) {
            console.error(err);
            // Если бекенд вернул 400 с текстом (например, про нехватку остатков)
            if (err.response && err.response.data) {
                alert(err.response.data);
            } else {
                alert("Ошибка при создании заказа.");
            }
        }
    };

    return (
        <div style={styles.overlay}>
            <div style={styles.modal}>
                <h3>Оформить заказ</h3>
                <form onSubmit={handleSubmit} style={styles.form}>
                    
                    <label style={styles.label}>Номер заказа (внутренний):</label>
                    <input value={orderNumber} onChange={e => setOrderNumber(e.target.value)} style={styles.input} required placeholder="Например: ORD-1001" />

                    <label style={styles.label}>Маркетплейс:</label>
                    <select value={marketplace} onChange={e => setMarketplace(e.target.value)} style={styles.input}>
                        <option value="Ozon">Ozon</option>
                        <option value="Wildberries">Wildberries</option>
                        <option value="Яндекс.Маркет">Яндекс.Маркет</option>
                        <option value="Прямая продажа">Прямая продажа</option>
                    </select>

                    <label style={styles.label}>Товар:</label>
                    <select value={productId} onChange={e => setProductId(Number(e.target.value))} style={styles.input}>
                        {products.map(p => <option key={p.Id} value={p.Id}>{p.Name}</option>)}
                    </select>

                    <label style={styles.label}>Склад списания:</label>
                    <select value={warehouseId} onChange={e => setWarehouseId(Number(e.target.value))} style={styles.input}>
                        {warehouses.map(w => <option key={w.Id} value={w.Id}>{w.Name}</option>)}
                    </select>

                    <label style={styles.label}>Количество:</label>
                    <input type="number" min="1" value={quantity} onChange={e => setQuantity(Number(e.target.value))} style={styles.input} />

                    <div style={styles.buttons}>
                        <button type="button" onClick={onClose} style={styles.cancelBtn}>Отмена</button>
                        <button type="submit" style={styles.submitBtn}>Создать заказ</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

const styles = {
    overlay: { position: 'fixed' as const, top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000 },
    modal: { backgroundColor: 'white', padding: '25px', borderRadius: '8px', width: '400px' },
    form: { display: 'flex', flexDirection: 'column' as const, gap: '10px' },
    label: { fontSize: '0.85rem', fontWeight: 'bold' as const, color: '#555' },
    input: { padding: '10px', borderRadius: '4px', border: '1px solid #ccc' },
    buttons: { display: 'flex', justifyContent: 'space-between', marginTop: '20px' },
    cancelBtn: { padding: '10px 20px', cursor: 'pointer', border: 'none', borderRadius: '4px' },
    submitBtn: { padding: '10px 20px', backgroundColor: '#8a2be2', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' } // Фиолетовая кнопка для заказов
};

export default CreateOrderModal;