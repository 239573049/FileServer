import { ProLayout } from '@ant-design/pro-components';
import { Component, ReactNode } from 'react';
import { Avatar, Popover } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import { Link } from 'umi';
import styles from './index.less';
import menu from './menu';
export default class App extends Component {
  state = {
    pathname: '',
  };

  constructor(props: any) {
    super(props);
  }

  render(): ReactNode {
    var { pathname } = this.state;
    return (
      <div
        style={{
          height: '100vh',
        }}
      >
        <ProLayout
          route={menu}
          location={{
            pathname,
          }}
          navTheme="light"
          fixSiderbar
          onMenuHeaderClick={(e) => console.log(e)}
          title="文件管理系统"
          logo="https://gw.alipayobjects.com/mdn/rms_b5fcc5/afts/img/A*1NHAQYduQiQAAAAAAAAAAABkARQnAQ"
          menuHeaderRender={(logo: any, title: any) => (
            <div
              id="customize_menu_header"
              onClick={() => {
                window.open('https://cn.bing.com/');
              }}
            >
              {logo}
              {title}
            </div>
          )}
          menuItemRender={(item: any, dom: any) => (
            <a
              onClick={() => {
                pathname = item.path || '/';

                this.setState({
                  pathname,
                });
              }}
            >
              <Link to={item.path ?? '/'}>{dom}</Link>
            </a>
          )}
          headerRender={() => (
            <div className={styles.header}>
              <Popover
                placement="bottomRight"
                content={() => (
                  <div style={{ width: '100%' }}>
                    <div className={styles.popover}>个人资料</div>
                    <div className={styles.popover}>退出登录</div>
                  </div>
                )}
                trigger="click"
              >
                <Avatar
                  shape="square"
                  size="large"
                  src=""
                  icon={<UserOutlined />}
                />
              </Popover>
            </div>
          )}
        >
          {this.props.children}
        </ProLayout>
      </div>
    );
  }
}
