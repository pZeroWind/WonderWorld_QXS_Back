import React,{Component} from "react";
import DataBox from "./DataBox";
import {Segmented, Spin} from "antd"
import analysisApi from "../../../api/analysisApi";
import EChartsReact from "echarts-for-react";
import { Tabs } from "antd";
const {TabPane} = Tabs

export default class TypeBox extends Component{

    constructor(props) {
        super(props)
        this.state = {
            type: [],
            option:this.setOptions([])
        }
    }

    componentDidMount() {
        analysisApi.typesdata(res => {
            this.setState({
                type:res
            })
            this.eachartsChange("全部")
        })
    }

    eachartsChange(value) {
        let data = []
        switch (value ){
            case "全部":
                this.state.type.forEach(i => {
                    data.push({
                        name: i.name,
                        value: i.allBook,
                        label: {
                            show: i.allBook !== 0
                        },
                        labelLine: {
                            show: i.allBook !== 0
                        }
                    })
                })
                break
            case "已发布":
                this.state.type.forEach(i => {
                    data.push({
                        name: i.name,
                        value: i.publishBook,
                        label: {
                            show: i.publishBook !== 0
                        },
                        labelLine: {
                            show: i.publishBook !== 0
                        }
                    })
                })
                break
            default:
                this.state.type.forEach(i => {
                    data.push({
                        name: i.name,
                        value: i.banBook,
                        label: {
                            show: i.banBook !== 0
                        },
                        labelLine: {
                            show: i.banBook !== 0
                        }
                    })
                })
                break
        }
        this.setState({
            option: this.setOptions(data)
        })
    }

    setOptions(data) {
        return({
            tooltip: {
                trigger: 'item'
            },
            legend: {
                top: '5%'
            },
            series: [
                {
                    name: '小说总数',
                    type: 'pie',
                    radius: ['30%', '60%'],
                    avoidLabelOverlap: false,
                    itemStyle: {
                        borderRadius: 10,
                        borderColor: '#fff',
                        borderWidth: 2
                    },
                    emphasis: {
                        label: {
                            show: true,
                            fontSize: '20',
                            fontWeight: 'bold'
                        }
                    },
                    data: data
                }
            ]
        })
    }


    render() {
        if (this.state.type.length === 0) {
            return (
                <Spin tip="加载中...">
                    <div className="Box" style={{height:"535px"}}>

                    </div>
                </Spin>
            )
        }
        else
        {
            return (
                <div className="Box">
                    <div className="TypeBox">
                        <Tabs defaultActiveKey='1' tabPosition='left'>
                        {
                            this.state.type.map(p => {
                                return (
                                    <TabPane tab={p.name} key={p.id}>
                                        <div className="Datum">
                                            <DataBox name="小说总数" data={p.allBook} />
                                            <DataBox name="已发布数" data={p.publishBook} />
                                            <DataBox name="作家总数" data={p.allWriter} />
                                            <DataBox name="上架总数" data={p.shelfBook} />
                                            <DataBox name="今日上架" data={p.todayShelf} />
                                        </div>
                                    </TabPane>
                                )
                            })
                        }
                        </Tabs>
                    </div>
                    <div className='TypeAllData'>
                        <EChartsReact option={this.state.option} style={{ width: "850px", height: "450px" }} ></EChartsReact>
                        <Segmented options={["全部", "已发布", "已封禁"]} onChange={this.eachartsChange.bind(this)} defaultValue="全部"/>
                    </div>
                </div>
            )
        }
    }
}