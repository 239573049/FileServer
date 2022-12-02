import { GetStatisticsInput } from '@/module/input/getStatisticsInput';
import { PieInput } from '@/module/input/pieInput';
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

    /**
     * 获取访问列表
     * @param input 
     * @returns 
     */
    getList(input: GetStatisticsInput) {
        return request.get(api + '/list', {
            params: input
        })
    }
}
export default new statisticsApi();
