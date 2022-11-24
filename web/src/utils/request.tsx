import { extend } from 'umi-request';
const baseUrl = 'http://localhost:5293';

const errorHandler = (error: any) => {
};

const request = extend({
  errorHandler, // 默认错误处理
  prefix: baseUrl,
  credentials: 'include', // 默认请求是否带上cookie
});

export default request;
