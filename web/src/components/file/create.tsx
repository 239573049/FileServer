import { Component, ReactNode } from "react";
import { Modal, Input, message, Mentions } from 'antd';
import { GetListInput } from "@/module/input/getListInput";
import fileApi from '../../apis/fileApi'
import Editor from "@monaco-editor/react";

interface IProps {
    isModalOpen: boolean,
    onCancel: any,
    input: GetListInput,
}



class CreateFile extends Component<IProps>{

    state = {
        language: 'text',
        input: {
            path: '',
            name: '',
            content: '',
        },
    }

    handleOk() {
        this.state.input.path = this.props.input.path
        fileApi.create(this.state.input)
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
        var { input, language } = this.state
        return (
            <Modal
                width="840px"
                title="新建文件"
                open={this.props.isModalOpen}
                onOk={() => this.handleOk()}
                onCancel={() => this.handleCancel()} >
                <Input value={input.name} onChange={(e) => {
                    input.name = e.target.value
                    if (input.name.endsWith(".md")) {
                        language = "markdown"
                    } else if (input.name.endsWith(".cs")) {
                        language = "csharp"
                    } else if (input.name.endsWith(".java")) {
                        language = "javascript"
                    } else if (input.name.endsWith(".js")) {
                        language = ""
                    } else if (input.name.endsWith(".scss")) {
                        language = "scss"
                    } else if (input.name.endsWith(".lua")) {
                        language = "lua"
                    } else if (input.name.endsWith(".json")) {
                        language = "json"
                    } else if (input.name.endsWith(".bat")) {
                        language = "bat"
                    } else if (input.name.endsWith("Dockerfile")) {
                        language = "Dockerfile"
                    } else if (input.name.endsWith(".go")) {
                        language = "go"
                    } else if (input.name.endsWith(".xml")) {
                        language = "xml"
                    } else if (input.name.endsWith(".yml")) {
                        language = "yml"
                    } else {
                        language = 'text'
                    }
                    console.log(language);

                    this.setState({
                        input,
                        language
                    })
                }} placeholder="请输入文件名称" />
                <Editor
                    height="600px"
                    width="800px"
                    language={language}
                    onChange={(value) => {
                        input.content = value ?? "";
                        this.setState({
                            input
                        })
                    }}
                    value={input.content}
                />
            </Modal>)
    }
}

export default CreateFile