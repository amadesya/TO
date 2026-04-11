export interface CategoryDTO {
    Id: number;
    Name: string;
    Description: string;
}

export interface CreateOrderRequest {
    OrderNumber: string;
    Marketplace: string;
    ProductId: number;
    WarehouseId: number;
    EmployeeId: number;
    Quantity: number;
}

export interface InventoryDTO {
    Id: number;
    ProductId: number;
    WarehouseId: number;
    Quantity: number
    ReservedQuantity: number
    WarehouseName: string

}

export interface MarketplaceOrderDTO {
    Id: number;
    OrderNumber: string;
    Marketplace: string;
    Status: string;
    OrderDate: Date;
}

export interface OrderStatusUpdateDTO {
    Status: string
}

export interface ProductDTO {
    Id: number;
    Name: string;
    CategoryId: number;
    CostPrice: number;
}

export interface RoleDTO {
    Name: string;
    Description: string
}

export interface TransactionCreateDTO {
    ProductId: number;
    FromWarehouseId: number;
    ToWarehouseId: number;
    EmployeeId: number;
    Quantity: number;
    TransactionType: string
}

export interface TransactionDTO {
    Id: number;
    ProductName: string;
    FromWarehouseName: string;
    ToWarehouseName: string;
    EmployeeName: string;
    Quantity: number;
    TransactionType: string;
    TransactionDate: Date;
}

export interface WarehouseDTO {
    Id: number;
    Name: string;
    Address: string
    WarehouseType: string
}
