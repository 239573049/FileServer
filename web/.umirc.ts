import { defineConfig } from 'umi';
import { HomeOutlined, UserOutlined } from '@ant-design/icons';

export default defineConfig({
  nodeModulesTransform: {
    type: 'none',
  },
  routes: [
    {
      path: '/',
      flatMenu: true,
      component: '@/layouts/index',
      routes: [
        {
          name: '首页',
          path: '/',
          component: '@/pages/home/index',
        },
        {
          name: '文件管理',
          path: '/file',
          component: '@/pages/file/index',
        },
      ],
    },
  ],
  fastRefresh: {},
});
