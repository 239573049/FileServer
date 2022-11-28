import { Component, ReactNode } from "react";
import { Modal, Input, message, Radio } from 'antd';
import { FilesListDto, FileType } from "@/module/filesListDto";
import routeMappingApi from "@/apis/routeMappingApi";

interface IProps {
    isModalOpen: boolean,
    onCancel: any,
    info: FilesListDto,
    load: boolean
}



class CreateRouteMapping extends Component<IProps>{

    state = {
        input: {
            fullName: '',
            route: '',
            path: "",
            type: FileType.File,
            visitor: false,
        },
        open: true
    }

    constructor(props: IProps) {
        super(props)
        var { input } = this.state;
        input.fullName = props.info.fullName!;
        this.getRoute(true, props.info.fullName!)
    }


    shouldComponentUpdate(nextProps: Readonly<IProps>, nextState: Readonly<any>, nextContext: any): boolean {
        var { open } = nextState;
        console.log(open);
        console.log(nextProps.isModalOpen);

        if (open === true) {
            if (nextProps.isModalOpen) {
                this.getRoute(false, nextProps.info.fullName!)
            }
        }
        return true;
    }

    getRoute(update: boolean, fullName: string) {
        var { input } = this.state
        routeMappingApi.get(fullName ?? "")
            .then(res => {
                if (res != undefined) {
                    if (res.path !== "") {
                        input.route = res.route;
                        input.type = res.type;
                        input.visitor = res.visitor;
                        this.setState({
                            input,
                            open: false
                        })
                    }
                }
            })
    }

    handleOk() {
        var { info } = this.props;
        var { input } = this.state;
        if (input.route.startsWith("/") === false) {
            message.error("路由开头必须使用“/”")
            return
        }

        input.path = info.fullName!;
        input.type = info.type!;

        routeMappingApi.create(this.state.input)
            .then(res => {
                if (res != undefined) {

                }
            });
    }

    handleCancel() {
        this.props.onCancel(false)
    }
    render(): ReactNode {
        var { input } = this.state
        return (
            <Modal title="新建路由映射配置" open={this.props.isModalOpen} onOk={() => this.handleOk()} onCancel={() => this.handleCancel()} >
                <Input value={input.route} onChange={(e) => {
                    input.route = e.target.value
                    this.setState({
                        input
                    })
                }} placeholder="请输入路由地址;示例“/r/s”" />

                <Radio.Group onChange={(e) => {
                    input.visitor = e.target.value
                    this.setState({
                        input
                    })
                }} value={input.visitor}>
                    <Radio value={true}>允许游客访问</Radio>
                    <Radio value={false}>不允许游客访问</Radio>
                </Radio.Group>
            </Modal>)
    }
}

export default CreateRouteMapping