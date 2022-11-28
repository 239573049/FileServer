import styles from './index.less';
import { Component, ReactNode } from 'react';
import { ProCard } from '@ant-design/pro-components';
import RcResizeObserver from 'rc-resize-observer';

class Index extends Component {
  state = {
    responsive: true
  }
  render(): ReactNode {
    var date = new Date();
    var { responsive } = this.state
    return <RcResizeObserver
      key="resize-observer"
      onResize={(offset) => {
        responsive = offset.width < 596
        this.setState({
          responsive
        })
      }}
    >
      <ProCard
        title="浏览分析"
        bordered
        headerBordered
        split={responsive ? 'horizontal' : 'vertical'}
      >
        <ProCard split="horizontal">
          <ProCard split="horizontal">
            <ProCard split={responsive ? 'horizontal' : 'vertical'}>
              <ProCard title="昨日全部流量">123</ProCard>
              <ProCard title="本月累计流量">234</ProCard>
              <ProCard title="今年累计流量">345</ProCard>
            </ProCard>
          </ProCard>
          <ProCard title="流量趋势">
            <div>

            </div>
            <div>图表</div>
            <div>图表</div>
            <div>图表</div>
            <div>图表</div>
          </ProCard>
        </ProCard>
      </ProCard>
    </RcResizeObserver>;
  }
}

export default Index;
