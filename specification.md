# 產品管理系統規格書

## 1. 系統概述
本系統為一個產品管理系統，採用前後端分離架構，後端使用 ASP.NET Core 提供 RESTful API，前端使用 React 開發單頁應用程式（SPA）。系統實作基本的 CRUD 操作，並支援多使用者同時操作。

## 2. 開發環境與技術要求
### 2.1 開發環境
- 後端開發工具：Visual Studio 2022 Community
- 程式語言：
  - 後端：C# 12
- 框架版本：
  - 後端：.NET 8
- 版本控制：Git + GitHub + Sourcetree

### 2.2 技術架構
#### 2.2.1 後端架構
- 採用三層式架構（Three-Tier Architecture）
  - **API 層**：ASP.NET Core Web API
  - **業務邏輯層（Business Logic Layer）**：處理業務邏輯與資料驗證
  - **資料存取層（Data Access Layer）**：Entity Framework Core
- 使用依賴注入（Dependency Injection）框架
- 實作 RESTful API 風格
- 實作 JWT 認證機制

### 2.3 資料庫
- 資料庫選項：
  - SQL Server on Linux (Docker)
- 資料存取方式：
  - Entity Framework Core

## 3. API 設計
### 3.1 RESTful API 端點
#### 3.1.1 產品管理 API
```
GET    /api/products          # 取得所有產品
GET    /api/products/{id}     # 取得單一產品
POST   /api/products          # 新增產品
PUT    /api/products/{id}     # 更新產品
DELETE /api/products/{id}     # 刪除產品
```

#### 3.1.2 API 回應格式
```json
{
  "success": true,
  "data": {
    // 回應資料
  },
  "message": "操作成功",
  "errors": null
}
```

### 3.2 API 文件
- 使用 Swagger/OpenAPI 產生 API 文件
- 提供 API 測試介面
- 詳細的 API 參數說明

## 4. 前端功能需求
### 4.1 產品管理
#### 4.1.1 產品列表頁面
- 顯示產品列表
- 支援分頁功能
- 支援排序功能
- 支援搜尋功能

#### 4.1.2 產品詳情頁面
- 顯示產品詳細資訊
- 支援編輯功能
- 支援刪除功能

#### 4.1.3 新增產品頁面
- 產品資訊輸入表單
- 表單驗證
- 提交功能

## 5. 資料庫設計
### 5.1 產品表（Products）
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| Id | INT | 主鍵，自動遞增 |
| Name | NVARCHAR(100) | 產品名稱 |
| Description | NVARCHAR(500) | 產品描述 |
| Price | DECIMAL(18,2) | 產品價格 |
| Stock | INT | 庫存數量 |
| CreateTime | DATETIME | 建立時間 |
| UpdateTime | DATETIME | 更新時間 |
| IsActive | BIT | 是否啟用 |

## 6. 專案結構
```
ProductHub/
├── ProductHub.Server/           # 後端 API 專案
├── ProductHub.Business/      # 業務邏輯層
├── ProductHub.Data/          # 資料存取層
├── ProductHub.Common/        # 共用元件
├── ProductHub.Tests/         # 單元測試專案
└── ProductHub.client/           # React 前端專案
    ├── src/
    │   ├── components/      # React 元件
    │   ├── pages/          # 頁面元件
    │   ├── services/       # API 服務
    │   ├── store/          # Redux store
    │   └── utils/          # 工具函數
    └── public/             # 靜態資源
```

## 7. 開發規範
### 7.1 版本控制
- 使用 Git 進行版本控制
- 使用 Sourcetree 進行視覺化版本控制
- 分支策略：
  - main：主分支，保持穩定
  - develop：開發分支
  - feature/*：功能分支
  - hotfix/*：緊急修復分支

### 7.2 程式碼規範
- 後端：
  - 遵循 C# 程式碼規範
  - 使用 async/await 處理非同步操作
  - 實作介面優先原則
  - 使用 SOLID 原則

### 7.3 單元測試
- 後端測試：
  - 使用 xUnit 進行單元測試
  - 測試覆蓋率要求：至少 70%

## 8. 安全性要求
- 實作 CORS 政策
- 所有輸入資料必須進行驗證
- 防止 SQL 注入攻擊
- 資料庫連線字串必須加密
- 實作適當的錯誤處理機制

## 9. 效能要求
- API 回應時間不超過 1 秒
- 前端頁面載入時間不超過 3 秒
- 實作適當的快取機制
- 實作資料分頁機制