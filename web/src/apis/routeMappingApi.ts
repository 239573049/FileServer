import { CreateRouteMappingInput } from '@/module/input/createRouteMappingInput';
import request from '@/utils/request';

const api = '/api/route-mapping';

class routeMappingApi {

    /**
     * 创建路由映射配置
     * @param input 
     * @returns 
     */
    create(input: CreateRouteMappingInput) {
        return request.post(api, {
            data: input
        })
    }

    /**
     * 删除路由映射配置
     * @param route 
     * @returns 
     */
    delete(route: string) {
        return request.delete(api + '?route=' + route)
    }

    /**
     * 获取路由配置
     * @param path 
     * @returns 
     */
    get(path: string) {
        return request.get(api + "?path=" + path)
    }
}
export default new routeMappingApi();
