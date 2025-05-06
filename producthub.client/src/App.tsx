import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Layout } from 'antd';
import ProductList from './pages/ProductList';

const { Header, Content } = Layout;

const App: React.FC = () => {
  return (
    <Router>
      <Layout>
        <Header style={{ 
          color: 'white', 
          fontSize: '20px',
          background: '#1890ff',
          display: 'flex',
          alignItems: 'center'
        }}>
          Product Hub
        </Header>
        <Content style={{ 
          padding: '24px',
          minHeight: 'calc(100vh - 64px)',
          background: '#f0f2f5'
        }}>
          <Routes>
            <Route path="/" element={<ProductList />} />
          </Routes>
        </Content>
      </Layout>
    </Router>
  );
};

export default App; 