import { HomeOutlined, UserOutlined } from '@ant-design/icons';

const routes = {
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
          icon: <HomeOutlined />,
        },
        {
          name: '文件管理',
          path: '/file',
          icon: <UserOutlined />,
          component: '@/pages/file/index',
        },
        {
          name: '访问记录',
          path: '/visit',
          icon: <UserOutlined />,
          component: '@/pages/visit/index',
        }
      ],
    },
  ],
};
export default routes;
