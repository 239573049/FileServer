import { AuthInput } from '@/module/input/authInput';
import request from '@/utils/request';

const api = '/api/auth';

class authApi {
    /**
     * 登录授权
     * @param input 
     */
    auth(input: AuthInput) {
        return request.post(api, {
            data: input
        })
    }

    /**
     * 获取当前登录人信息
     * @returns 
     */
    get() {
        return request.get("/api/user-info")
    }
}
export default new authApi();
