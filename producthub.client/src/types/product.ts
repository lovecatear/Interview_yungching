export interface Product {
    id: string;
    name: string;
    description: string;
    price: number;
    isActive: boolean;
    createTime: string;
    updateTime: string;
}

export interface ProductQueryParameters {
    pageNumber: number;
    pageSize: number;
    searchTerm?: string;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
} 