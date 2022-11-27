import {
    AlipayCircleOutlined,
    LockOutlined,
    TaobaoCircleOutlined,
    UserOutlined,
    WeiboCircleOutlined,
} from '@ant-design/icons';
import {
    LoginForm,
    ProFormCheckbox,
    ProFormText,
} from '@ant-design/pro-components';
import { Space, message } from 'antd';
import { CSSProperties, useState } from 'react';
import request from '../../utils/request'
import { history } from 'umi';
import authApi from '@/apis/authApi';

const iconStyles: CSSProperties = {
    marginLeft: '16px',
    color: 'rgba(0, 0, 0, 0.2)',
    fontSize: '24px',
    verticalAlign: 'middle',
    cursor: 'pointer',
};

var user = JSON.parse(window.localStorage.getItem('user') ?? "{}");

export default () => {
    if (user?.autoLogin) {
        console.log('自动登录');
        onSubmit(user);
    }

    function onSubmit(value: any) {
        authApi.auth(value)
            .then((res: any) => {
                if (res != undefined) {
                    window.localStorage.setItem("token", res)
                    message.info('登录成功，请稍后！');
                    setTimeout(() => {
                        history.push('/');
                    }, 2000);

                    return;
                }
            });
    }

    return (
        <div style={{ backgroundColor: 'white' }}>
            <LoginForm
                logo="https://github.githubassets.com/images/modules/logos_page/Octocat.png"
                title="文件管理系统"
                subTitle="更好的文件管理系统"
                onFinish={async (value: any) => {
                    onSubmit(value);
                }}
            >
                <ProFormText
                    name="username"
                    fieldProps={{
                        size: 'large',
                        prefix: <UserOutlined className={'prefixIcon'} />,
                    }}
                    placeholder={'用户名: admin'}
                    rules={[
                        {
                            required: true,
                            message: '请输入用户名!',
                        },
                    ]}
                />
                <ProFormText.Password
                    name="password"
                    fieldProps={{
                        size: 'large',
                        prefix: <LockOutlined className={'prefixIcon'} />,
                    }}
                    placeholder={'默认密码: Aa123456.'}
                    rules={[
                        {
                            required: true,
                            message: '请输入密码！',
                        },
                    ]}
                />
                <div
                    style={{
                        marginBottom: 24,
                    }}
                >
                    <ProFormCheckbox noStyle name="autoLogin">
                        自动登录
                    </ProFormCheckbox>
                    <a
                        style={{
                            float: 'right',
                        }}
                    >
                        忘记密码
                    </a>
                </div>
            </LoginForm>
        </div>
    );
};
