import { extend } from 'umi-request';
import { message } from 'antd';
const baseUrl = 'http://localhost:5293';

const errorHandler = (error: any) => {
  if (error.response.status !== 200) {
    message.error(error.data.Message)
  }
};


const request = extend({
  errorHandler, // 默认错误处理
  prefix: baseUrl,
  credentials: 'include', // 默认请求是否带上cookie
});

//添加请求头

request.interceptors.request.use((url: any, options: any) => {

  const headers = {
    'Authorization': "Bearer " + window.localStorage.getItem("token")
  };
  return (
    {
      url: url,
      options: { ...options, headers: headers },
    }
  );

})



export default request;