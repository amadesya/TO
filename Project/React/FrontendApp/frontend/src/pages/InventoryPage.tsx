import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';
import { InventoryDTO, ProductDTO, WarehouseDTO } from '../types/types';
import CreateOrderModal from '../components/CreateOrderModal'

const InventoryPage = () => {
    const [inventory, setInventory] = useState<InventoryDTO[]>([]);
    const [products, setProducts] = useState<ProductDTO[]>([]);
    const [warehouses, setWarehouses] = useState<WarehouseDTO[]>([]);
    const [isOrderModalOpen, setIsOrderModalOpen] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [invRes, prodRes, whRes] = await Promise.all([
                    apiClient.get<InventoryDTO[]>('/Inventory'),
                    apiClient.get<ProductDTO[]>('/Product'),
                    apiClient.get<WarehouseDTO[]>('/Warehouse')
                ]);

                setInventory(invRes.data);
                setProducts(prodRes.data);
                setWarehouses(whRes.data);
            } catch (err) {
                console.error("Ошибка при загрузке данных склада:", err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const getProductName = (productId: number) => {
        const product = products.find(p => p.Id === productId);
        return product ? product.Name : `Товар #${productId}`;
    };

    if (loading) return <div style={styles.container}>Загрузка остатков...</div>;

    return (
        <div style={styles.container}>
            <h1>Остатки на складах</h1>
            <button
                onClick={() => setIsOrderModalOpen(true)}
                style={{ padding: '10px 20px', backgroundColor: '#8a2be2', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
            >
                + Создать заказ
            </button>

            {inventory.length === 0 ? (
                <p>На складах пока нет товаров.</p>
            ) : (
                <table style={styles.table}>
                    <thead>
                        <tr style={styles.headerRow}>
                            <th style={styles.th}>Склад</th>
                            <th style={styles.th}>Товар</th>
                            <th style={styles.th}>Всего (шт.)</th>
                            <th style={styles.th}>В резерве</th>
                            <th style={styles.th}>Доступно к выдаче</th>
                        </tr>
                    </thead>
                    <tbody>
                        {inventory.map((item) => {
                            const available = item.Quantity - item.ReservedQuantity;

                            return (
                                <tr key={item.Id} style={styles.row}>
                                    <td style={styles.td}><strong>{item.WarehouseName}</strong></td>
                                    <td style={styles.td}>{getProductName(item.ProductId)}</td>
                                    <td style={styles.td}>{item.Quantity}</td>
                                    <td style={styles.td}>
                                        <span style={item.ReservedQuantity > 0 ? styles.reservedBadge : {}}>
                                            {item.ReservedQuantity}
                                        </span>
                                    </td>
                                    <td style={styles.td}>
                                        <span style={{
                                            ...styles.availableBadge,
                                            backgroundColor: available > 0 ? '#dff6dd' : '#fde7e9',
                                            color: available > 0 ? '#107c10' : '#a4262c'
                                        }}>
                                            {available} шт.
                                        </span>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            )}
            {isOrderModalOpen && (
                <CreateOrderModal 
                    products={products}
                    warehouses={warehouses}
                    onClose={() => setIsOrderModalOpen(false)}
                    onSuccess={() => window.location.reload()} 
                />
            )}
        </div>
    );
};

const styles = {
    container: { padding: '20px', fontFamily: 'Arial, sans-serif' },
    table: { width: '100%', borderCollapse: 'collapse' as const, marginTop: '20px', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' },
    headerRow: { backgroundColor: '#0078d4', color: 'white', textAlign: 'left' as const },
    th: { padding: '12px 15px', borderBottom: '2px solid #005a9e' },
    td: { padding: '12px 15px', borderBottom: '1px solid #ddd' },
    row: { transition: 'background-color 0.2s' },
    reservedBadge: { backgroundColor: '#fff4ce', color: '#8a6d3b', padding: '2px 6px', borderRadius: '4px', fontSize: '0.9em' },
    availableBadge: { padding: '4px 8px', borderRadius: '4px', fontWeight: 'bold' as const }
};

export default InventoryPage;