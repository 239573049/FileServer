import { FilesListDto } from '@/module/filesListDto';
import { PagedResultDto } from '@/module/pagedResultDto';
import { Component, ReactNode } from 'react';
import fileApi from '../../apis/fileApi';

interface IProps {}

interface IState {
  data: PagedResultDto<FilesListDto>;
}
class File extends Component<IProps, IState> {
  state: Readonly<IState> = {
    data: {
      items: [],
      totalCount: 0,
    },
  };
  render(): ReactNode {
    var { data } = this.state;
    return <div>{data.items.map((x) => {})}</div>;
  }
}

export default File;
