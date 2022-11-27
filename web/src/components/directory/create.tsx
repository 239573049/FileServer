import { Component, ReactNode } from "react";
import { Modal, Input, message } from 'antd';
import { GetListInput } from "@/module/getListInput";
import directoryApi from '../../apis/directoryApi'

interface IProps {
    isModalOpen: boolean,
    onCancel: any,
    input: GetListInput
}



class CreateDirectory extends Component<IProps>{

    state = {
        value: ''
    }

    handleOk() {
        directoryApi.create(this.props.input.path, this.state.value)
            .then(res => {
                if (res != undefined) {
                    message.success("添加成功")
                    this.props.onCancel(true)
                } else {
                    this.props.onCancel(false)
                }
            })
    }

    handleCancel() {
        this.props.onCancel(false)
    }
    render(): ReactNode {
        var { value } = this.state
        return (
            <Modal title="新建文件夹" open={this.props.isModalOpen} onOk={() => this.handleOk()} onCancel={() => this.handleCancel()} >
                <Input value={value} onChange={(e) => {
                    value = e.target.value
                    this.setState({
                        value
                    })
                }} placeholder="请输入文件夹名称" />
            </Modal>)
    }
}

export default CreateDirectory