import styles from './index.less';
import { Component, ReactNode } from 'react';
import { ProCard } from '@ant-design/pro-components';
import RcResizeObserver from 'rc-resize-observer';
import statisticsApi from '@/apis/statisticsApi';
import { StatisticsDto } from '@/module/statisticsDto';
import { Pie } from '@ant-design/plots';

interface IProps {

}

interface IState {
  responsive: boolean,
  statistics: StatisticsDto | null,
  todayConfig: any,
  YesterdayConfig: any,
  MonthConfig: any,
  TotalConfig: any
}

class Index extends Component<IProps, IState> {
  state: Readonly<IState> = {
    responsive: false,
    statistics: null,
    todayConfig: {
      appendPadding: 10,
      data: [],
      angleField: 'value',
      colorField: 'type',
      radius: 0.75,
      label: {
        type: 'spider',
        labelHeight: 28,
        content: '{name}\n{percentage}',
      },
      interactions: [
        {
          type: 'element-selected',
        },
        {
          type: 'element-active',
        },
      ],
    },
    YesterdayConfig: {
      appendPadding: 10,
      data: [{
        type: '分类一',
        value: 27,
      },
      {
        type: '分类二',
        value: 25,
      },
      {
        type: '分类三',
        value: 18,
      },
      {
        type: '分类四',
        value: 15,
      },
      {
        type: '分类五',
        value: 10,
      },
      {
        type: '其他',
        value: 5,
      }],
      angleField: 'value',
      colorField: 'type',
      radius: 0.75,
      label: {
        type: 'spider',
        labelHeight: 28,
        content: '{name}\n{percentage}',
      },
      interactions: [
        {
          type: 'element-selected',
        },
        {
          type: 'element-active',
        },
      ],
    },
    MonthConfig: {
      appendPadding: 10,
      data: [{
        type: '分类一',
        value: 27,
      },
      {
        type: '分类二',
        value: 25,
      },
      {
        type: '分类三',
        value: 18,
      },
      {
        type: '分类四',
        value: 15,
      },
      {
        type: '分类五',
        value: 10,
      },
      {
        type: '其他',
        value: 5,
      }],
      angleField: 'value',
      colorField: 'type',
      radius: 0.75,
      label: {
        type: 'spider',
        labelHeight: 28,
        content: '{name}\n{percentage}',
      },
      interactions: [
        {
          type: 'element-selected',
        },
        {
          type: 'element-active',
        },
      ],
    },
    TotalConfig: {
      appendPadding: 10,
      data: [{
        type: '分类一',
        value: 27,
      },
      {
        type: '分类二',
        value: 25,
      },
      {
        type: '分类三',
        value: 18,
      },
      {
        type: '分类四',
        value: 15,
      },
      {
        type: '分类五',
        value: 10,
      },
      {
        type: '其他',
        value: 5,
      }],
      angleField: 'value',
      colorField: 'type',
      radius: 0.75,
      label: {
        type: 'spider',
        labelHeight: 28,
        content: '{name}\n{percentage}',
      },
      interactions: [
        {
          type: 'element-selected',
        },
        {
          type: 'element-active',
        },
      ],
    }
  }

  getStatistics() {
    statisticsApi.getStatistics()
      .then(res => {
        this.setState({
          statistics: res
        })
      })
  }

  getpie() {
    var { todayConfig, YesterdayConfig, MonthConfig, TotalConfig } = this.state;
    var input = {
      type: 0
    }
    statisticsApi.getpie(input)
      .then(res => {
        todayConfig.data = res;
        this.setState({
          todayConfig
        })
      })

    input.type = 1;
    statisticsApi.getpie(input)
      .then(res => {
        YesterdayConfig.data = res;
        this.setState({
          YesterdayConfig
        })
      })


    input.type = 2;
    statisticsApi.getpie(input)
      .then(res => {
        MonthConfig.data = res;
        this.setState({
          MonthConfig
        })
      })

    input.type = 3;
    statisticsApi.getpie(input)
      .then(res => {
        TotalConfig.data = res;
        this.setState({
          TotalConfig
        })
      })
  }

  constructor(props: IProps) {
    super(props)
    this.getpie()
    this.getStatistics()
  }

  render(): ReactNode {
    var { responsive, statistics, todayConfig, YesterdayConfig, MonthConfig, TotalConfig } = this.state
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
              <ProCard title="昨天访问量">{statistics?.yesterday}</ProCard>
              <ProCard title="上星期访问量">{statistics?.lastWeek}</ProCard>
              <ProCard title="访问总量">{statistics?.total}</ProCard>
            </ProCard>
          </ProCard>
          <ProCard
            split={responsive ? 'horizontal' : 'vertical'}
            bordered
            headerBordered
          >
            <ProCard title="今日访问统计" colSpan="40%">
              <Pie {...todayConfig} />
            </ProCard>
            <ProCard title="昨天访问统计" colSpan="40%">
              <Pie {...YesterdayConfig} />
            </ProCard>
          </ProCard>

          <ProCard
            split={responsive ? 'horizontal' : 'vertical'}
            bordered
            headerBordered
          >
            <ProCard title="本月统计" colSpan="40%">
              <Pie {...MonthConfig} />
            </ProCard>
            <ProCard title="统计总量" colSpan="40%">
              <Pie {...TotalConfig} />
            </ProCard>
          </ProCard>
        </ProCard>
      </ProCard>
    </RcResizeObserver>;
  }
}

export default Index;
