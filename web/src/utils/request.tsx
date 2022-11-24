import { extend } from 'umi-request';
import { message } from 'antd';
const baseUrl = 'http://localhost:5293';

const errorHandler = (error: any) => {
  const { response = {} } = error;
  if (response.status > 500) {
    message.error('服务器发生错误，请检查服务器。');
  }
};

const request = extend({
  errorHandler, // 默认错误处理
  prefix: baseUrl,
  credentials: 'include', // 默认请求是否带上cookie
});

export default request;
