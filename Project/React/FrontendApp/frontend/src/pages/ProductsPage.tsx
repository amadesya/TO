import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';
import { ProductDTO, CategoryDTO } from '../types/types';
import { useNavigate } from 'react-router-dom';
import AddProductModal from '../components/AddProductModal';
import EditProductModal from '../components/EditProductModal';

const ProductsPage = () => {
    const [products, setProducts] = useState<ProductDTO[]>([]);
    const [categories, setCategories] = useState<CategoryDTO[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [editingProduct, setEditingProduct] = useState<ProductDTO | null>(null);
    const navigate = useNavigate();

    const fetchData = async () => {
        try {
            const [prodRes, catRes] = await Promise.all([
                apiClient.get<ProductDTO[]>('/Product'),
                apiClient.get<CategoryDTO[]>('/Category')
            ]);
            setProducts(prodRes.data);
            setCategories(catRes.data);
        } catch (err) { console.error(err); }
        finally { setLoading(false); }
    };

    const getCategoryName = (categoryId: number) => {
        const category = categories.find(c => c.Id === categoryId);
        return category ? category.Name : `ID: ${categoryId}`;
    };

    const handleDelete = async (id: number) => {
        if (window.confirm("Вы уверены, что хотите удалить этот товар?")) {
            try {
                await apiClient.delete(`/Product/${id}`);
                fetchData();
            } catch (err) {
                alert("Ошибка при удалении");
            }
        }
    };

    useEffect(() => {
        const user = localStorage.getItem('user');
        if (!user) {
            navigate('/login');
        }
    }, [navigate]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [prodRes, catRes] = await Promise.all([
                    apiClient.get<ProductDTO[]>('/Product'),
                    apiClient.get<CategoryDTO[]>('/Category')
                ]);

                setProducts(prodRes.data);
                setCategories(catRes.data);
                setError(null);
            } catch (err) {
                console.error("Ошибка при загрузке данных:", err);
                setError("Не удалось загрузить данные. Проверьте подключение к серверу.");
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    useEffect(() => { fetchData(); }, []);

    if (loading) return <div style={styles.container}>Загрузка списка товаров...</div>;
    if (error) return <div style={{ ...styles.container, color: 'red' }}>{error}</div>;

    return (
        <div style={styles.container}>
            <h1>Складской учёт: Товары</h1>
            <button
                onClick={() => setIsModalOpen(true)}
                style={{ padding: '10px 20px', backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
            >
                + Добавить товар
            </button>

            {products.length === 0 ? (
                <p>Список товаров пуст.</p>
            ) : (
                <table style={styles.table}>
                    <thead>
                        <tr style={styles.headerRow}>
                            <th style={styles.th}>Наименование</th>
                            <th style={styles.th}>Категория</th>
                            <th style={styles.th}>Себестоимость</th>
                            <th style={styles.th}>Действия</th>
                        </tr>
                    </thead>
                    <tbody>
                        {products.map((product, index) => (
                            <tr key={index} style={index % 2 === 0 ? {} : styles.evenRow}>
                                <td style={styles.td}>{product.Name}</td>
                                <td style={styles.td}>
                                    <span style={styles.badge}>
                                        {getCategoryName(product.CategoryId)}
                                    </span>
                                </td>
                                <td style={styles.td}>{product.CostPrice} ₽</td>
                                <td style={styles.td}>
                                    <button onClick={() => setEditingProduct(product)} style={styles.editBtn}>✏️</button>
                                    <button onClick={() => handleDelete(product.Id)} style={styles.deleteBtn}>❌</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
            {isModalOpen && (
                <AddProductModal
                    categories={categories}
                    onClose={() => setIsModalOpen(false)}
                    onSuccess={fetchData}
                />
            )}
            {editingProduct && (
                <EditProductModal
                    product={editingProduct}
                    categories={categories}
                    onClose={() => setEditingProduct(null)}
                    onSuccess={fetchData}
                />
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
    },
    deleteBtn: {
        backgroundColor: 'transparent',
        border: 'none',
        cursor: 'pointer',
        fontSize: '1.2rem',
        padding: '5px 10px',
        borderRadius: '4px',
        transition: 'background-color 0.2s',
    },
    editBtn: {
        backgroundColor: 'transparent',
        border: 'none',
        cursor: 'pointer',
        fontSize: '1.1rem',
        marginRight: '10px'
    }
};

export default ProductsPage;