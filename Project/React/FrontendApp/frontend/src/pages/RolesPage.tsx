import React, { useEffect, useState } from 'react';
import apiClient from '../api/apiClient';
import { RoleDTO } from '../types/types';

const RolesPage = () => {
    const [roles, setRoles] = useState<RoleDTO[]>([]); 
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        // Делаем запрос к RoleController
        apiClient.get<RoleDTO[]>('/Role') 
            .then(response => {
                setRoles(response.data);
            })
            .catch(err => {
                console.error(err);
                setError("Не удалось загрузить данные");
            });
    }, []);

    return (
        <div style={{ padding: '20px' }}>
            <h1>Список ролей</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <ul>
                {roles.map(role => (
                    <li>{role.Name}</li>
                ))}
            </ul>
        </div>
    );
};

export default RolesPage;