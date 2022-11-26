import { GetListInput } from '@/module/getListInput';
import { SaveFileContentInput } from '@/module/saveFileContentInput';
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

  /**
   * 保存文件
   * @param input 
   * @returns 
   */
  saveFileContent(input: SaveFileContentInput) {
    return request.post(name + '/save', {
      data: input
    })
  }
}
export default new fileApi();
