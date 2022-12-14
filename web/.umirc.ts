import { defineConfig } from 'umi';

export default defineConfig({
  nodeModulesTransform: {
    type: 'none',
  },
  routes: [
    {
      name: '登录',
      path: '/login',
      component: '@/pages/user/login',
    },
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
        {
          name: '访问记录',
          path: '/visit',
          component: '@/pages/visit/index',
        },
      ],
    },
  ],
  fastRefresh: {},
});
