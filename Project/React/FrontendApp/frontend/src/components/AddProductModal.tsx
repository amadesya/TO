import React, { useState } from 'react';
import apiClient from '../api/apiClient';
import { CategoryDTO } from '../types/types';

interface Props {
    categories: CategoryDTO[];
    onClose: () => void;
    onSuccess: () => void; // Чтобы обновить таблицу после добавления
}

const AddProductModal = ({ categories, onClose, onSuccess }: Props) => {
    const [name, setName] = useState('');
    const [categoryId, setCategoryId] = useState<number>(categories[0]?.Id || 0);
    const [costPrice, setCostPrice] = useState<number>(0);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await apiClient.post('/Product', {
                Name: name,
                CategoryId: Number(categoryId),
                CostPrice: Number(costPrice)
            });
            onSuccess(); // Обновляем список
            onClose();   // Закрываем окно
        } catch (err) {
            console.error("Ошибка при создании товара:", err);
            alert("Не удалось добавить товар");
        }
    };

    return (
        <div style={styles.overlay}>
            <div style={styles.modal}>
                <h3>Добавить новый товар</h3>
                <form onSubmit={handleSubmit} style={styles.form}>
                    <input 
                        placeholder="Название товара" 
                        value={name} 
                        onChange={e => setName(e.target.value)} 
                        style={styles.input} required 
                    />
                    
                    <select 
                        value={categoryId} 
                        onChange={e => setCategoryId(Number(e.target.value))} 
                        style={styles.input}
                    >
                        {categories.map(cat => (
                            <option key={cat.Id} value={cat.Id}>{cat.Name}</option>
                        ))}
                    </select>

                    <input 
                        type="number" 
                        placeholder="Себестоимость" 
                        value={costPrice} 
                        onChange={e => setCostPrice(Number(e.target.value))} 
                        style={styles.input} required 
                    />

                    <div style={styles.buttons}>
                        <button type="button" onClick={onClose} style={styles.cancelBtn}>Отмена</button>
                        <button type="submit" style={styles.submitBtn}>Сохранить</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

const styles = {
    overlay: { position: 'fixed' as const, top: 0, left: 0, width: '100%', height: '100%', backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center' },
    modal: { backgroundColor: 'white', padding: '25px', borderRadius: '8px', width: '350px' },
    form: { display: 'flex', flexDirection: 'column' as const, gap: '15px' },
    input: { padding: '10px', borderRadius: '4px', border: '1px solid #ccc' },
    buttons: { display: 'flex', justifyContent: 'space-between', marginTop: '10px' },
    cancelBtn: { padding: '8px 15px', cursor: 'pointer', border: 'none', borderRadius: '4px' },
    submitBtn: { padding: '8px 15px', backgroundColor: '#0078d4', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }
};

export default AddProductModal;