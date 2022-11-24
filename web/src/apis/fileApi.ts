import { GetListInput } from '@/module/getListInput';
import request from '@/utils/request';

const name = '/api/file';

class fileApi {
  /**
   * 获取文件列表
   */
  getList(input: GetListInput) {
    return request.get(name + '/list', {
      params: input
    });
  }

  /**
   * 获取文件内容
   */
  getFileContent(filePath: string) {
    return request.get(name + '/content?filePath=' + filePath);
  }
}
export default new fileApi();
