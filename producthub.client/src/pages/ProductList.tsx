import React, { useEffect, useState } from 'react';
import { Table, Button, Input, Space, Modal, Form, message, Card, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';
import productService from '../services/productService';
import { Product, ProductQueryParameters, PagedResult } from '../types/product';

const { Title } = Typography;

const ProductList: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(false);
    const [modalVisible, setModalVisible] = useState(false);
    const [editingProduct, setEditingProduct] = useState<Product | null>(null);
    const [form] = Form.useForm();
    const [searchTerm, setSearchTerm] = useState('');
    const [pagination, setPagination] = useState({
        current: 1,
        pageSize: 10,
        total: 0
    });
    const [sorter, setSorter] = useState<{ field: string; order: 'ascend' | 'descend' } | undefined>(undefined);

    const fetchProducts = async (params: ProductQueryParameters) => {
        try {
            setLoading(true);
            const data = await productService.getPaged(params);
            setProducts(data.items);
            setPagination({
                ...pagination,
                total: data.totalCount
            });
        } catch (error) {
            message.error('Failed to fetch products');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchProducts({
            pageNumber: pagination.current,
            pageSize: pagination.pageSize,
            searchTerm: searchTerm,
            sortBy: sorter?.field,
            sortOrder: sorter?.order === 'ascend' ? 'asc' : sorter?.order === 'descend' ? 'desc' : undefined
        });
    }, [pagination.current, pagination.pageSize, searchTerm, sorter]);

    const handleCreate = () => {
        setEditingProduct(null);
        form.resetFields();
        setModalVisible(true);
    };

    const handleEdit = (record: Product) => {
        setEditingProduct(record);
        form.setFieldsValue(record);
        setModalVisible(true);
    };

    const handleDelete = async (id: string) => {
        try {
            await productService.delete(id);
            message.success('Product deleted successfully');
            fetchProducts({
                pageNumber: pagination.current,
                pageSize: pagination.pageSize,
                searchTerm: searchTerm,
                sortBy: sorter?.field,
                sortOrder: sorter?.order === 'ascend' ? 'asc' : 'desc'
            });
        } catch (error) {
            message.error('Failed to delete product');
        }
    };

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            if (editingProduct) {
                await productService.update(editingProduct.id, { ...editingProduct, ...values });
                message.success('Product updated successfully');
            } else {
                await productService.create(values);
                message.success('Product created successfully');
            }
            setModalVisible(false);
            fetchProducts({
                pageNumber: pagination.current,
                pageSize: pagination.pageSize,
                searchTerm: searchTerm,
                sortBy: sorter?.field,
                sortOrder: sorter?.order === 'ascend' ? 'asc' : 'desc'
            });
        } catch (error) {
            message.error('Failed to save product');
        }
    };

    const handleTableChange = (pagination: any, filters: any, sorter: any) => {
        setPagination({
            ...pagination,
            current: pagination.current
        });
        setSorter(sorter);
    };

    const columns: ColumnsType<Product> = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
            sorter: true,
        },
        {
            title: 'Description',
            dataIndex: 'description',
            key: 'description',
        },
        {
            title: 'Price',
            dataIndex: 'price',
            key: 'price',
            render: (price: number) => `NT$ ${price.toLocaleString()}`,
            sorter: true,
        },
        {
            title: 'Status',
            dataIndex: 'isActive',
            key: 'isActive',
            render: (isActive: boolean) => (
                <span style={{ color: isActive ? 'green' : 'red' }}>
                    {isActive ? 'Active' : 'Inactive'}
                </span>
            ),
        },
        {
            title: 'Actions',
            key: 'actions',
            render: (_: unknown, record: Product) => (
                <Space>
                    <Button
                        type="primary"
                        icon={<EditOutlined />}
                        onClick={() => handleEdit(record)}
                    >
                        Edit
                    </Button>
                    <Button
                        danger
                        icon={<DeleteOutlined />}
                        onClick={() => handleDelete(record.id)}
                    >
                        Delete
                    </Button>
                </Space>
            ),
        },
    ];

    return (
        <Card style={{ margin: '24px' }}>
            <div style={{ marginBottom: '16px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Title level={2}>Product List</Title>
                <Space>
                    <Input
                        placeholder="Search products..."
                        prefix={<SearchOutlined />}
                        value={searchTerm}
                        onChange={e => setSearchTerm(e.target.value)}
                        style={{ width: 200 }}
                    />
                    <Button
                        type="primary"
                        icon={<PlusOutlined />}
                        onClick={handleCreate}
                        size="large"
                    >
                        Add Product
                    </Button>
                </Space>
            </div>

            <Table
                columns={columns}
                dataSource={products}
                loading={loading}
                rowKey="id"
                pagination={pagination}
                onChange={handleTableChange}
            />

            <Modal
                title={editingProduct ? 'Edit Product' : 'Create Product'}
                open={modalVisible}
                onOk={handleSubmit}
                onCancel={() => setModalVisible(false)}
                width={600}
            >
                <Form
                    form={form}
                    layout="vertical"
                >
                    <Form.Item
                        name="name"
                        label="Name"
                        rules={[{ required: true, message: 'Please input the product name!' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="description"
                        label="Description"
                        rules={[{ required: true, message: 'Please input the product description!' }]}
                    >
                        <Input.TextArea rows={4} />
                    </Form.Item>
                    <Form.Item
                        name="price"
                        label="Price"
                        rules={[{ required: true, message: 'Please input the product price!' }]}
                    >
                        <Input type="number" step="0.01" prefix="NT$" />
                    </Form.Item>
                    <Form.Item
                        name="isActive"
                        label="Status"
                        valuePropName="checked"
                    >
                        <Input type="checkbox" />
                    </Form.Item>
                </Form>
            </Modal>
        </Card>
    );
};

export default ProductList; 