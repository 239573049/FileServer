import { FilesListDto, FileType } from '@/module/filesListDto';
import { PagedResultDto } from '@/module/pagedResultDto';
import { Component, ReactNode } from 'react';
import fileApi from '../../apis/fileApi';
import directoryApi from '../../apis/directoryApi'
import { Input, List, Upload, message, Popconfirm, Modal, Button, Tooltip, Progress, Card, Tag } from 'antd';
import './index.less'
import { ArrowLeftOutlined, CheckCircleOutlined, SyncOutlined, CloseCircleOutlined } from '@ant-design/icons';
import { change } from '@/utils/util'
import * as signalR from "@microsoft/signalr";
import { MessagePackHubProtocol } from '@microsoft/signalr-protocol-msgpack';
import Editor from "@monaco-editor/react";


import {
  FolderOpenOutlined,
  FileOutlined
} from '@ant-design/icons';
import { GetListInput } from '@/module/input/getListInput';
import { SaveFileContentInput } from '@/module/input/saveFileContentInput';
import { FileContentDto } from '@/module/fileContentDto';
import CreateDirectory from '@/components/directory/create';
import CreateFile from '@/components/file/create';
import CreateRouteMapping from '@/components/routeMapping/create';
import React from 'react';
import { baseUrl } from '@/utils/request';
import { UploadModule } from '@/module/uploadModule';

var signalr = new signalR.HubConnectionBuilder()
  .withUrl(process.env.NODE_ENV === "development" ? baseUrl + "/uploading" : "/uploading", { accessTokenFactory: () => window.localStorage.getItem("token") ?? "" })
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Debug)
  .withHubProtocol(new MessagePackHubProtocol())
  .build();

signalr.start();


const { Dragger } = Upload;

const { Search } = Input;

interface IProps { }

interface IState {
  fileshow: boolean,
  data: PagedResultDto<FilesListDto>;
  rename: string,
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
    info: FilesListDto,
    CreateRouteComponent: any
  },
  UploadShow: boolean,
  uploadList: UploadModule[]
}

class File extends Component<IProps, IState> {
  state: Readonly<IState> = {
    rename: '',
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
      },
      CreateRouteComponent: null
    },
    UploadShow: false,
    uploadList: []
  };

  constructor(props: IProps) {
    super(props);

    this.getListData()
    this.state.createRoute.CreateRouteComponent = React.createRef();
    signalr.on('upload', (msg: UploadModule) => {
      var { uploadList, UploadShow } = this.state;
      // 更新上传进度
      for (let i = 0; i < uploadList.length; i++) {
        if (msg.fileName === uploadList[i].fileName) {
          if (msg.complete) {
            uploadList[i].uploadingProgress = uploadList[i].size;
          } else {
            uploadList[i].uploadingProgress = msg.uploadingProgress;
          }
          uploadList[i].state = msg.state;
          uploadList[i].complete = msg.complete;
        }

      }
      // 如果上传完成刷新列表
      if (msg.complete) {
        message.success(msg.fileName + "上传成功😘")
        this.getListData()
        this.setState({
          uploadList: [...uploadList]
        })
      } else if (msg.complete && msg.state === "BeDefeated") {
        message.error(msg.message)
        this.setState({
          uploadList: [...uploadList]
        })
      } else if (UploadShow) {
        this.setState({
          uploadList: [...uploadList]
        })
      }
    })
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

  deleteFile(item: FilesListDto) {
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

  /**
   * 解压zip压缩包
   * @param item 
   */
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
    return (
      <span>
        <div className='file-delete' onClick={() => {
          if (item.type === FileType.File) {
            this.deleteFile(item)
          } else {
            this.deleteDirectory(item)
          }
        }}>
          删除
        </div>
        <div className='file-delete' onClick={() => this.setRoute(item)}>
          设置路由
        </div>
        {this.getrename(item)}
        {item.name?.endsWith(".zip") ? <div className="file-button" onClick={() => this.extractDirectory(item)}>解压Zip</div> : ''}

        {item.type === FileType.File ? <div className="file-button" onClick={() => this.onOpenFile(item)}>编辑</div> : ''}
      </span>)
  }

  /**
   * 重命名dom
   * @param item 
   * @returns 
   */
  getrename(item: FilesListDto) {
    var { rename } = this.state;
    return <Popconfirm
      placement="rightTop"
      title={<Input placeholder="请输入新名称" value={rename} onChange={(e) => {
        rename = e.target.value;
        this.setState({ rename })
      }} />}
      onConfirm={() => this.renameOk(item)}
      onCancel={() => {
        this.setState({ rename: '' })
      }}
      okText="Yes"
      cancelText="No"
    >
      <div className='file-delete' >
        重命名
      </div>
    </Popconfirm>
  }

  /**
   * 请求重命名
   * @param item 
   */
  renameOk(item: FilesListDto) {
    var { rename } = this.state;
    directoryApi.rename(item.fullName!, rename, item.name!)
      .then(res => {
        if (res != undefined) {
          message.success("修改成功")
          this.getListData()
        }
      })

    this.setState({ rename: '' })
  }

  /**
   * 设置路由配置
   * @param item 
   */
  setRoute(item: FilesListDto) {
    var { createRoute } = this.state
    createRoute.info = item;
    createRoute.open = true;
    this.setState({
      createRoute
    })
    createRoute.CreateRouteComponent.current.update(createRoute.info);
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
            {item.type === FileType.File ?
              <span className='create-time'>
                文件大小：{change(item.length)}
              </span>
              : ""}
          </span>
        </div>
      </Tooltip>
    )
  }

  /**
   * 拉取列表数据
   */
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

  /**
   * 上传文件处理
   * @param file 
   * @returns 
   */
  beforeUpload(file: any) {
    var { uploadList } = this.state;
    const subject = new signalR.Subject<Int8Array>();
    var upload = {
      fileName: file.name,
      uploadingProgress: 0,
      complete: false,
      size: file.size,
      state: "BeingProcessed",
      message: '',
    };

    uploadList.push(upload)
    console.log('uploadList', uploadList);

    this.setState({
      uploadList: uploadList
    })

    signalr!.send("UploadStream", this.state.input.path, file.webkitRelativePath, file.name, subject)
      .then(() => {
        var fr = new FileReader();
        fr.readAsArrayBuffer(file);
        if (fr) {
          var len = file.size;
          var size = 0;
          fr.onload = function (x) {
            while (len > 0) {
              var buffer = fr.result?.slice(size, size + (1024 * 20)) as ArrayBuffer;
              size += (1024 * 20);
              len -= (1024 * 20);
              subject.next(new Int8Array(buffer))
            }
            console.log('complete');
            subject.complete();
          };
        }
      });

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
    var { data, input, fileshow, file, fileContent, createRoute, edit, createDirectory, createFile, UploadShow, uploadList } = this.state;
    return (<div>
      <Dragger directory multiple showUploadList={false} {...this.props} beforeUpload={(file: any) => this.beforeUpload(file)} openFileDialogOnClick={false} className="dargg">
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
          <Button onClick={() => {
            this.setState({
              UploadShow: true
            })
          }} type="primary" style={{ float: "right", marginLeft: '5px' }}>
            显示下载进度
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
      <CreateRouteMapping ref={createRoute.CreateRouteComponent} />

      <Modal title="上传列表" open={UploadShow} onOk={() => {
        this.setState({
          UploadShow: false
        })
      }}
        onCancel={() => {
          this.setState({
            UploadShow: false
          })
        }}>
        <div style={{
          overflow: "auto",
          height: "500px",
        }}>
          {uploadList.map((x) => {
            return (<Card title={x.fileName} style={{ width: '100%' }}>
              {x.state === "BeDefeated" ?
                <Tag icon={<CloseCircleOutlined />} color="error">
                  上传失败
                </Tag> : (x.state === "Complete" ? <Tag icon={<CheckCircleOutlined />} color="success">
                  上传完成
                </Tag> : <Tag icon={<SyncOutlined spin />} color="processing">
                  上传中
                </Tag>)}
              <Tag>{change(x.uploadingProgress) + "/" + change(x.size)}</Tag>
              <Progress
                strokeColor={{ '0%': '#108ee9', '100%': '#87d068' }}
                percent={parseInt(`${x.uploadingProgress / x.size * 100}`)}
              />
            </Card>)
          })}
        </div>

      </Modal>
    </div>);
  }
}

export default File;
