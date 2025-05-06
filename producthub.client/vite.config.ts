import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 50101,
    proxy: {
      '^/api/.*': {
        target: 'https://localhost:7080',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path
      }
    }
  }
}) 