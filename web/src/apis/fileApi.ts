import { CreateFileInput } from '@/module/createFileInput';
import { GetListInput } from '@/module/getListInput';
import { SaveFileContentInput } from '@/module/saveFileContentInput';
import request from '@/utils/request';

const api = '/api/file';

class fileApi {
  /**
   * 获取文件列表
   */
  getList(input: GetListInput) {
    return request.get(api + '/list', {
      params: input
    });
  }

  /**
   * 获取文件内容
   */
  getFileContent(filePath: string) {
    return request.get(api + '/content?filePath=' + filePath);
  }

  /**
   * 保存文件
   * @param input 
   * @returns 
   */
  saveFileContent(input: SaveFileContentInput) {
    return request.post(api + '/save', {
      data: input
    })
  }

  /**
   * 删除文件
   * @param path 
   * @returns 
   */
  deleteFile(path: string) {
    return request.delete(api + "?path=" + path)
  }

  /**
   * 创建文件
   * @param input 
   * @returns 
   */
  create(input: CreateFileInput) {
    return request.post(api, {
      data: input
    })
  }

  /**
   * 解压文件
   * @param path 
   * @param name 
   * @returns 
   */
  extractToDirectory(path: string, name: string) {
    return request.post(api + '/extract-directory?path=' + path + "&name=" + name)
  }
}
export default new fileApi();
