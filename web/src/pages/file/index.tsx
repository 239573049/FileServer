import { FilesListDto, FileType } from '@/module/filesListDto';
import { PagedResultDto } from '@/module/pagedResultDto';
import { Component, ReactNode } from 'react';
import fileApi from '../../apis/fileApi';
import { Input, List, Upload, message } from 'antd';
import './index.less'
import { Modal, Button } from 'antd';

const { Dragger } = Upload;

const { Search, TextArea } = Input;
import {
  FolderOpenOutlined,
  FileOutlined
} from '@ant-design/icons';
import { GetListInput } from '@/module/getListInput';
import { SaveFileContentInput } from '@/module/saveFileContentInput';
interface IProps { }

interface IState {
  fileshow: boolean,
  data: PagedResultDto<FilesListDto>;
  input: GetListInput,
  file: FilesListDto | null,
  fileContent: SaveFileContentInput
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
    }
  };

  constructor(props: IProps) {
    super(props)
    this.getListData()
  }

  onOpenClick(item: FilesListDto) {
    var { fileContent } = this.state;

    fileApi.getFileContent(item.fullName!)
      .then((res) => {
        fileContent.content = res;
        fileContent.filePath = item.fullName!;
        this.setState({
          fileContent
        })
      })
    this.setState({
      fileshow: true,
      file: item
    })
  }

  getList(item: FilesListDto) {
    return (
      <div id="box" className='fileList' onClick={() => this.onfileClick(item)}>
        {item.type === FileType.Directory ? <FolderOpenOutlined /> : <FileOutlined />}
        <span className='fileName'>
          {item.name}
        </span>
        <span>
          {item.type === FileType.File ?
            <span className="file-button" onClick={() => this.onOpenClick(item)}>
              打开
            </span>
            :
            <div>

            </div>}
        </span>
      </div>)
  }

  getListData() {
    fileApi.getList(this.state.input)
      .then((res: any) => {
        this.setState({
          data: res
        })
      })
  }

  onfileClick(item: FilesListDto) {
    if (item.type === FileType.Directory) {
      var { input } = this.state;
      input.path = item.fullName!;
      this.setState({ input })
      this.getListData();
    } else {

    }
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
        if (res) {
          message.success('操作成功')
        }
      })
  }

  render(): ReactNode {
    var { data, input, fileshow, file, fileContent } = this.state;
    return (<div>
      <Dragger multiple {...this.props} beforeUpload={(file: any) => this.beforeUpload(file)} openFileDialogOnClick={false} className="dargg">
        <div style={{ marginBottom: "10px" }}>
          <Search onSearch={() => this.getListData()} style={{ width: "70%" }} value={input.path} onChange={(value) => {
            input.path = value.target.value
            this.setState({ input })
          }} enterButton />
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
        <TextArea
          value={fileContent.content}
          onChange={(e) => {
            fileContent.content = e.target.value;
            this.setState({
              fileContent
            })
          }}
          autoSize={{ minRows: 20, maxRows: 27, }}
        />
      </Modal>
    </div>);
  }
}

export default File;
