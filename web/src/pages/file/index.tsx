import { FilesListDto, FileType } from '@/module/filesListDto';
import { PagedResultDto } from '@/module/pagedResultDto';
import { Component, ReactNode } from 'react';
import fileApi from '../../apis/fileApi';
import { Input, List, Upload, message } from 'antd';
import './index.less'
import { InboxOutlined } from '@ant-design/icons';
import { UploadProps, Button } from 'antd';

const { Dragger } = Upload;

const { Search } = Input;
import {
  FolderOpenOutlined,
  FileOutlined
} from '@ant-design/icons';
import { GetListInput } from '@/module/getListInput';
interface IProps { }

interface IState {
  data: PagedResultDto<FilesListDto>;
  input: GetListInput
}

class File extends Component<IProps, IState> {
  state: Readonly<IState> = {
    data: {
      items: [],
      totalCount: 0,
    },
    input: {
      name: '',
      page: 1,
      path: "/",
      pageSize: 20
    }
  };

  constructor(props: IProps) {
    super(props)
    this.getListData()
  }

  getList(item: FilesListDto) {
    return (
      <div id="box" className='fileList' onClick={() => this.onfileClick(item)}>
        {item.type === FileType.Directory ? <FolderOpenOutlined /> : <FileOutlined />}
        <span className='fileName'>
          {item.name}
        </span>
        <span>
          {item.type === FileType.Directory ?
            <Button>
              下载
            </Button> : <div>
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

  render(): ReactNode {
    var { data, input } = this.state;
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
    </div>);
  }
}

export default File;
