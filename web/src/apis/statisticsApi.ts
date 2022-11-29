import { PieInput } from '@/module/pieInput';
import request from '@/utils/request';

const api = '/api/statistics';

class statisticsApi {
    /**
     * 获取统计数量
     * @returns 
     */
    getStatistics() {
        return request.get(api + "/statistics")
    }

    /**
     * 获取饼图数据
     * @param type 
     * @returns 
     */
    getpie(type: PieInput) {
        return request.get(api + "/pie", { params: type })
    }
}
export default new statisticsApi();
