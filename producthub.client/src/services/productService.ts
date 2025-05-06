import axios from 'axios';
import { Product, ProductQueryParameters, PagedResult } from '../types/product';

const API_BASE_URL = 'https://localhost:7080/api';

// 創建 axios 實例並配置
const axiosInstance = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: false,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
});

// 添加請求攔截器
axiosInstance.interceptors.request.use(
    (config) => {
        console.log('Request:', config);
        return config;
    },
    (error) => {
        console.error('Request Error:', error);
        return Promise.reject(error);
    }
);

// 添加響應攔截器
axiosInstance.interceptors.response.use(
    (response) => {
        console.log('Response:', response);
        return response;
    },
    (error) => {
        console.error('Response Error:', error);
        return Promise.reject(error);
    }
);

const productService = {
    async getAll(): Promise<Product[]> {
        const response = await axiosInstance.get('/Products');
        return response.data;
    },

    async getPaged(params: ProductQueryParameters): Promise<PagedResult<Product>> {
        const response = await axiosInstance.get('/Products/paged', { 
            params: {
                PageNumber: params.pageNumber,
                PageSize: params.pageSize,
                SearchTerm: params.searchTerm || '',
                SortBy: params.sortBy || 'Name',
                SortOrder: params.sortOrder || 'asc'
            }
        });
        return response.data;
    },

    async getById(id: string): Promise<Product> {
        const response = await axiosInstance.get(`/Products/${id}`);
        return response.data;
    },

    async create(product: Omit<Product, 'id' | 'createTime' | 'updateTime'>): Promise<Product> {
        const response = await axiosInstance.post('/Products', product);
        return response.data;
    },

    async update(id: string, product: Product): Promise<void> {
        await axiosInstance.put(`/Products/${id}`, product);
    },

    async delete(id: string): Promise<void> {
        await axiosInstance.delete(`/Products/${id}`);
    }
};

export default productService; 