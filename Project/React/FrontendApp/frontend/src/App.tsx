import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import ProductsPage from './pages/ProductsPage'; 
import WarehousesPage from './pages/WarehousesPage';
import Navbar from './components/Navbar';
import LoginPage from './pages/LoginPage';
import InventoryPage from './pages/InventoryPage';
import TransactionsPage from './pages/TransactionsPage';

function App() {
  return (
<Router>
      <Navbar />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/products" element={<ProductsPage />} />
        <Route path="/warehouses" element={<WarehousesPage />} />
        <Route path="/transactions" element={<TransactionsPage />} />
        <Route path="/inventory" element={<InventoryPage />} />
        <Route path="/" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;