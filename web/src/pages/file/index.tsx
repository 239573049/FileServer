import { FilesListDto, FileType } from '@/module/filesListDto';
import { PagedResultDto } from '@/module/pagedResultDto';
import { Component, ReactNode } from 'react';
import fileApi from '../../apis/fileApi';
import directoryApi from '../../apis/directoryApi'
import { Input, List, Upload, message } from 'antd';
import './index.less'
import { Modal, Button, Tooltip } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';

import Editor from "@monaco-editor/react";

import {
  FolderOpenOutlined,
  FileOutlined
} from '@ant-design/icons';
import { GetListInput } from '@/module/getListInput';
import { SaveFileContentInput } from '@/module/saveFileContentInput';
import { FileContentDto } from '@/module/fileContentDto';
import CreateDirectory from '@/components/directory/create';
import CreateFile from '@/components/file/create';
import CreateRouteMapping from '@/components/routeMapping/create';

const { Dragger } = Upload;

const { Search } = Input;

interface IProps { }

interface IState {
  fileshow: boolean,
  data: PagedResultDto<FilesListDto>;
  input: GetListInput,
  file: FilesListDto | null,
  fileContent: SaveFileContentInput,
  edit: {
    language: string
  }
  options: {
    selectOnLineNumbers: boolean
  },
  createDirectory: {
    open: boolean,
  },
  createFile: {
    open: boolean
  },
  createRoute: {
    open: boolean,
    info: FilesListDto
  }
}

class File extends Component<IProps, IState> {
  state: Readonly<IState> = {
    fileshow: false,
    data: {
      items: [],
      totalCount: 0,
    },
    input: {
      name: '',
      page: 1,
      path: "/",
      pageSize: 20
    },
    file: null,
    fileContent: {
      filePath: '',
      content: ''
    },
    options: {
      selectOnLineNumbers: true
    },
    edit: {
      language: ''
    },
    createDirectory: {
      open: false
    },
    createFile: {
      open: false
    },
    createRoute: {
      open: false,
      info: {
        type: FileType.Directory,
        name: null,
        length: 0,
        icon: null,
        updateTime: null,
        fileType: null,
        createdTime: null,
        fullName: null,
      }
    }
  };

  constructor(props: IProps) {
    super(props)
    this.getListData()
    document.oncontextmenu = function (e) {
      return false
    }
  }

  /**
   * 打开文件
   * @param item 
   */
  onOpenFile(item: FilesListDto) {
    var { edit, fileContent } = this.state;

    fileApi.getFileContent(item.fullName!)
      .then((res: FileContentDto) => {
        fileContent.content = res.content;
        edit.language = res.language
        fileContent.filePath = item.fullName!;
        this.setState({
          fileContent,
          edit
        })
      })
    this.setState({
      fileshow: true,
      file: item
    })
  }

  /**
   * 打开文件夹
   * @param item 
   */
  onOpenDirectory(item: FilesListDto) {
    if (item.type === FileType.Directory) {
      var { input } = this.state
      input.path = item.fullName ?? "/";
      this.setState({
        input
      })
      this.getListData();
    }
  }

  onDeleteFile(item: FilesListDto) {
    if (item.fullName) {
      fileApi.deleteFile(item.fullName)
        .then((res) => {
          if (res != undefined) {
            message.success("删除成功")
            this.getListData()
          }
        })
    }
  }

  /**
   * 删除文件夹
   * @param item 
   */
  deleteDirectory(item: FilesListDto) {
    if (item.fullName) {
      directoryApi.delete(item.fullName)
        .then((res) => {
          if (res != undefined) {
            message.success("删除成功")
            this.getListData()
          }
        })
    }
  }

  /**
   * 创建文件夹
   */
  createDirecotry() {
    var { createDirectory } = this.state;
    createDirectory.open = true;

    this.setState({
      createDirectory
    })
  }

  /**
   * 创建文件
   */
  createFile() {

    var { createFile } = this.state;
    createFile.open = true;

    this.setState({
      createFile
    })
  }

  extractDirectory(item: FilesListDto) {
    fileApi.extractToDirectory(this.state.input.path, item.name!)
      .then(res => {
        if (res != undefined) {
          message.success("解压成功")
          this.getListData()
        }
      })
  }

  /**
   * 弹出操作栏
   * @param item 
   * @returns 
   */
  feature(item: FilesListDto) {
    if (item.type === FileType.File) {

      if (item.name?.endsWith(".zip")) {

        return (
          <span>
            <div className='file-delete' onClick={() => this.onDeleteFile(item)}>
              删除
            </div>
            <div className='file-delete' onClick={() => this.setRoute(item)}>
              设置路由
            </div>
            <div className="file-button" onClick={() => this.extractDirectory(item)}>
              解压Zip
            </div>
          </span>)
      }

      return (
        <span>
          <div className='file-delete' onClick={() => this.onDeleteFile(item)}>
            删除
          </div>
          <div className='file-delete' onClick={() => this.setRoute(item)}>
            设置路由
          </div>
          <div className="file-button" onClick={() => this.onOpenFile(item)}>
            编辑
          </div>
        </span>)
    } else {
      return (
        <div>
          <div className='file-delete' onClick={() => this.deleteDirectory(item)}>
            删除
          </div>
          <div className='file-delete' onClick={() => this.setRoute(item)}>
            设置路由
          </div>
        </div>)
    }

  }

  setRoute(item: FilesListDto) {
    var { createRoute } = this.state
    createRoute.info = item;
    createRoute.open = true;
    this.setState({
      createRoute
    })
  }

  /**
   * 获取列表展示
   * @param item 
   * @returns 
   */
  getList(item: FilesListDto) {
    return (
      <Tooltip placement="topLeft" title={() => this.feature(item)} trigger='contextMenu'>
        <div id="box" className='fileList' onDoubleClick={() => {
          if (item.type === FileType.Directory) {
            this.onOpenDirectory(item)
          } else {
            this.onOpenFile(item)
          }
        }}>
          {item.type === FileType.Directory ? <FolderOpenOutlined /> : <FileOutlined />}
          <span className='fileName'>
            {item.name}
            <span className='create-time'>
              创建时间：{item.createdTime}
            </span>
          </span>
        </div>
      </Tooltip>
    )
  }

  getListData() {
    fileApi.getList(this.state.input)
      .then((res: any) => {
        if (res != undefined) {
          this.setState({
            data: res
          })
        }
      })
  }

  onDrop(value: any) {
    console.log(value);

  }

  beforeUpload(file: any[]) {
    console.log(file);

    return false;
  }

  saveFileContent() {
    var { fileContent } = this.state;
    fileApi.saveFileContent(fileContent)
      .then((res) => {
        if (res != undefined) {
          message.success('操作成功')
        }
      })
  }
  editorDidMount(editor: any, monaco: any) {
    console.log('editorDidMount', editor);
    editor.focus();
  }

  goBack() {
    var { input } = this.state
    var path = input.path.replaceAll('\\', '/')
    var paths = path.split('/')
    path = ''
    for (let i = 0; i < paths.length - 1; i++) {
      if (paths.length > 2 && i === paths.length - 2) {
        path += paths[i]
      } else {
        path += paths[i] + '/'
      }
    }
    input.path = path
    this.setState({
      input
    })

    this.getListData();
  }

  render(): ReactNode {
    var { data, input, fileshow, file, fileContent, createRoute, edit, createDirectory, createFile } = this.state;
    return (<div>
      <Dragger multiple {...this.props} beforeUpload={(file: any) => this.beforeUpload(file)} openFileDialogOnClick={false} className="dargg">
        <div style={{ marginBottom: "10px" }}>
          <Search onSearch={() => this.getListData()} style={{ width: "50%" }} value={input.path} onChange={(value) => {
            input.path = value.target.value
            this.setState({ input })
          }} enterButton />
          <Button onClick={() => this.goBack()} type="primary" disabled={input.path === '/'} icon={<ArrowLeftOutlined />} />
          <Button onClick={() => this.createDirecotry()} type="primary" style={{ float: "right", marginLeft: '5px' }}>
            添加文件夹
          </Button>
          <Button onClick={() => this.createFile()} type="primary" style={{ float: "right", marginLeft: '5px' }}>
            添加文件
          </Button>
        </div>
        <div>
          <List
            itemLayout="horizontal"
            dataSource={data.items}
            renderItem={(item: FilesListDto) => this.getList(item)}>
          </List>
        </div>
      </Dragger>
      <Modal
        title={file?.name}
        open={fileshow}
        width="900px"
        onCancel={() => {
          this.setState({
            fileshow: false
          })
        }}
        footer={[
          <div>
            <Button type="primary" onClick={() => this.saveFileContent()}>
              保存
            </Button>
            <Button type="primary" danger onClick={() => {
              this.setState({
                fileshow: false
              })
            }}>
              取消
            </Button>
          </div>
        ]}
      >
        <Editor
          height="600px"
          width="800px"
          language={edit.language}
          onChange={(value) => {
            fileContent.content = value ?? "";
            this.setState({
              fileContent
            })
          }}
          value={fileContent.content}
        />
      </Modal>
      <CreateDirectory input={input} isModalOpen={createDirectory.open} onCancel={(value: boolean) => {
        createDirectory.open = false
        if (value) {
          this.getListData()
        }
        this.setState({
          createDirectory
        })
      }} />
      <CreateFile input={input} isModalOpen={createFile.open} onCancel={(value: boolean) => {
        createFile.open = false
        if (value) {
          this.getListData()
        }
        this.setState({
          createFile
        })
      }} />
      <CreateRouteMapping info={createRoute.info} isModalOpen={createRoute.open} load={createRoute.open} onCancel={(value: boolean) => {
        createRoute.open = false;
        this.setState({ createRoute })
      }} />
    </div>);
  }
}

export default File;
