import { Component, ReactNode } from "react";
import { Modal, Input, message, Radio } from 'antd';
import { FilesListDto, FileType } from "@/module/filesListDto";
import routeMappingApi from "@/apis/routeMappingApi";
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';

class CreateRouteMapping extends Component {

    state = {
        input: {
            fullName: '',
            route: '',
            path: "",
            type: FileType.File,
            password: '',
        },
        show: false
    }

    update(info: FilesListDto) {
        this.getRoute(info)
        this.setState({
            show: true
        })
    }

    getRoute(info: FilesListDto) {
        var { input } = this.state
        if (info) {
            routeMappingApi.get(info.fullName ?? "")
                .then(res => {
                    if (res != undefined) {
                        input.path = info.fullName!;
                        input.type = info.type;
                        if (res.path !== "") {
                            input.route = res.route;
                            input.type = res.type;
                            input.password = res.password;
                            this.setState({
                                input
                            })
                        }
                    }
                })
        }
    }

    handleOk() {
        var { input } = this.state;
        if (input?.route?.startsWith("/") === false) {
            message.error("路由开头必须使用“/”")
            return
        }

        routeMappingApi.create(this.state.input)
            .then(res => {
                if (res != undefined) {
                    this.setState({
                        show: false
                    })
                }
            });
    }

    handleCancel() {
        this.setState({
            show: false
        })
    }
    render(): ReactNode {
        var { input, show } = this.state
        return (
            <Modal title="新建路由映射配置" open={show} onOk={() => this.handleOk()} onCancel={() => this.handleCancel()} >
                访问路由：<Input value={input.route} onChange={(e) => {
                    input.route = e.target.value
                    this.setState({
                        input
                    })
                }} placeholder="请输入路由地址;示例“/r/s”" />
                访问密码： <Input.Password value={input.password}
                    iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                    onChange={(e) => {
                        input.password = e.target.value
                        this.setState({
                            input
                        })
                    }} placeholder="服务密码为空则不设置" />
            </Modal>)
    }
}

export default CreateRouteMapping