import statisticsApi from "@/apis/statisticsApi";
import { GetStatisticsDto } from "@/module/dto/getStatisticsDto";
import { GetStatisticsInput } from "@/module/input/getStatisticsInput";
import { PagedResultDto } from "@/module/pagedResultDto";
import { Table, Input, Tag, Pagination } from 'antd';
import { Component, ReactNode } from "react";

var { Search } = Input

interface IProps {

}

interface IState {
    input: GetStatisticsInput,
    data: PagedResultDto<GetStatisticsDto>
}

const columns = [
    {
        title: '响应耗时（ms）',
        dataIndex: 'responseTime',
        key: 'responseTime',
    },
    {
        title: '状态码',
        dataIndex: 'code',
        key: 'code'
    },
    {
        title: '是否访问成功',
        dataIndex: 'succeed',
        key: 'succeed',
        render: (value: boolean) => value ? <Tag color="magenta">成功</Tag> : <Tag color="magenta">失败</Tag>

    },
    {
        title: '访问路由',
        dataIndex: 'path',
        key: 'path',
    },
    {
        title: '访问时间',
        dataIndex: 'createdTime',
        key: 'createdTime',
    },
    {
        title: '访问参数',
        dataIndex: 'query',
        key: 'query',
    },
];


class Visit extends Component<IProps, IState> {

    state: Readonly<IState> = {
        input: {
            keywords: '',
            page: 1,
            pageSize: 20
        },
        data: {
            items: [],
            totalCount: 0
        }
    }
    constructor(props: IProps) {
        super(props)
        this.getList()
    }

    getList() {
        var { input, data } = this.state;
        statisticsApi.getList(input)
            .then(res => {
                data = res;
                this.setState({
                    data
                })
            })
    }
    render(): ReactNode {
        var { input, data } = this.state
        return (<div>
            <div style={{ marginBottom: "10px" }}>
                <Search onSearch={() => this.getList()} style={{ width: "50%" }} value={input.keywords} onChange={(value: any) => {
                    input.keywords = value.target.value
                    this.setState({ input })
                }} enterButton />
            </div>
            <div >
                <Table scroll={{ x: 1500, y: 900 }} pagination={false} dataSource={data.items} columns={columns} />
                <Pagination defaultCurrent={input.page} total={data.totalCount} onChange={(page, pageSize) => {
                    input.page = page;
                    input.pageSize = pageSize;
                    this.setState({
                        input
                    })
                    this.getList()
                }} />
            </div>
        </div>)
    }
}

export default Visit