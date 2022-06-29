import { Button, Pagination, Popconfirm, message } from "antd";
import React from "react";
import chapterApi from "../../../api/chapterApi"
import "../../../css/Item/ScrollAndChapter.scss"

export class ScrollAndChapter extends React.Component {
    state = {
        scroll: [],
        chapter: [],
        page: 1,
        size: 5,
        total: 0,
        select: 0,
        id: 0,
        data: ""
    }

    componentDidMount() {
        chapterApi.getScroll(this.props.id, res => {
            this.setState({
                scroll: res.data,
                select: res.data[0].id
            }, () => {
                this.getChapter(res.data[0].id, 1)
            })
        })
    }

    getChapter(id) {
        chapterApi.getChapter(id, this.state.page, res => {
            this.setState({
                chapter: res.data,
                page: res.page,
                size: res.size,
                total: res.total
            })
        })
    }

    getData(id) {
        chapterApi.get(id, this.state.select, this.props.id, res => {
            this.setState({
                data: res.data.content
            })
        })
    }

    selectC(id) {
        this.setState({
            page: 1,
            select: id
        }, () => {
            this.getChapter(id)
        })
    }

    selectData(id) {
        this.setState({
            id: id
        }, () => {
            this.getData(id)
        })
    }

    ban() {
        chapterApi.ban(this.state.id, res => {
            if (res.code == 200) {
                this.selectC(this.state.select)
                this.setState({
                    data: ""
                })
                message.success("封禁成功")
            }
        })
    }

    render() {

        return (
            <div className="SAC">
                <div className="SAC-box">
                    {
                        this.state.scroll.map((p) => (
                            <div key={p.id} onClick={this.selectC.bind(this, p.id)} className={this.state.select == p.id ? 'SAC-box-select box' : "box"} >{p.name}</div>
                        ))
                    }
                </div>
                <div className="SAC-box">
                    <div style={{ flex: 1 }} >
                        {
                            this.state.chapter.map((p) => (
                                <div onClick={this.selectData.bind(this, p.id)} className={this.state.id == p.id ? 'SAC-box-select box' : "box"} key={p.id} >{p.title}</div>
                            ))
                        }
                    </div>
                    <div className="pageControl">
                        <Pagination onChange={this.getChapter.bind(this, this.state.select)} current={this.state.page} defaultPageSize={this.state.size} showSizeChanger={false} total={this.state.total}></Pagination>
                    </div>
                </div>
                <div className="SAC-box">
                    <div style={{ flex: 1 }} dangerouslySetInnerHTML={{ __html: this.state.data }}></div>
                    {
                        this.state.data != "" ? (
                            <Popconfirm
                                title="确定封禁吗？"
                                cancelText="否"
                                okText="是"
                                onConfirm={this.ban.bind(this)}
                            >
                                <Button>封禁文章</Button>
                            </Popconfirm>
                        ) : ""
                    }
                </div>
            </div >
        )
    }
}