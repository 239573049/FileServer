import request from '@/utils/request';

const api = '/api/directory';

class directoryApi {
    /**
     * 删除文件夹
     * @param path 
     * @returns 
     */
    delete(path: string) {
        return request.delete(api + "?path=" + path)
    }

    /**
     * 创建文件夹
     * @param path 
     * @param name 
     */
    create(path: string, name: string) {
        return request.post(api + "?path=" + path + "&name=" + name)
    }
}
export default new directoryApi();
