import { Button, Pagination, Table } from "antd";
import React from "react";
import reportApi from "../../api/reportApi";
import { AreaBox } from "../Item/AreaBox";

export class Message extends React.Component {

    state = {
        col: [
            {
                title: "账号",
                dataIndex: "userId"
            },
            {
                title: "详情信息",
                dataIndex: "details",
            },
            {
                title: "章节ID",
                dataIndex: "chapterId",
            },
            {
                title: "操作",
                dataIndex: "state",
                render: (p) => (
                    p == 1 ?
                        <Button>处理</Button>
                        :
                        <Button>已完成</Button>
                )
            }
        ],
        dataList: [],
        page: 0,
        total: 0,
        size: 0
    }

    componentDidMount() {
        this.get(1)
    }

    get(page) {
        reportApi.get(page, res => {
            this.state.dataList = res.data
            this.setState({
                dataList: this.state.dataList,
                page: res.page,
                total: res.total,
                size: res.size
            })
        })
    }

    render() {
        return (
            <AreaBox className="message">
                <div style={{ width: "100%" }}>
                    <Table style={{ width: "100%" }} columns={this.state.col} dataSource={this.state.dataList} pagination={false}></Table>
                    <div className="pageControl">
                        <Pagination onChange={this.get.bind(this)} current={this.state.page} pageSize={this.state.size} showSizeChanger={false} total={this.state.total}></Pagination>
                    </div>
                </div>
            </AreaBox>
        )
    }
}