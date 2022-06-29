import { Button, Image, Modal, Tag } from "antd";
import React from "react";
import { baseUrl } from "../../../api/ApiConfig";
import { GetDate } from "../../../plugins/TimeTool";
import { ScrollAndChapter } from "./ScrollAndChapter";
import "../../../css/Item/BookItem.scss"

export class BookItem extends React.Component {
    state = {
        show: false,
        showScroll: false
    }

    showModal() {
        this.setState({
            show: !this.state.show
        })
    }

    showScrollModal() {
        this.setState({
            showScroll: !this.state.showScroll
        })
    }

    render() {

        return (
            <div className="BookItem">
                <div className="Item" onClick={this.showModal.bind(this)}>
                    <img src={baseUrl + this.props.data.cover} alt={this.props.data.title} />
                    <div className="DataList">
                        <div className="title">{this.props.data.title}</div>
                        <div className="tags">{this.props.data.tags.map(i => (
                            <Tag key={i.id}>{i.name}</Tag>
                        ))}</div>
                        <div className="type">
                            <strong>作者：</strong>
                            {this.props.data.nickName}
                        </div>
                        <div className="type">
                            <strong>分区：</strong>
                            {this.props.data.type}
                        </div>
                        <div className="update">
                            <strong>最近更新：</strong>
                            {GetDate(this.props.data.updateTime)}
                        </div>
                    </div>
                </div>
                <Modal
                    visible={this.state.show}
                    onCancel={this.showModal.bind(this)}
                    footer={[
                        <Button key="back" onClick={this.showModal.bind(this)}>返回</Button>,
                        <Button key="show" type="primary" onClick={this.showScrollModal.bind(this)}>查看章节</Button>,
                    ]}
                >
                    <div className="BookModal">
                        <Image src={baseUrl + this.props.data.cover} />
                        <div className="DataList">
                            <div className="id">{this.props.data.id}</div>
                            <div className="title">{this.props.data.title}</div>
                            <div className="tags">{this.props.data.tags.map(i => (
                                <Tag key={i.id}>{i.name}</Tag>
                            ))}</div>
                            <div className="type">
                                <strong>作者：</strong>
                                {this.props.data.nickName}
                            </div>
                            <div className="type">
                                <strong>分区：</strong>
                                {this.props.data.type}
                            </div>
                            <div className="update">
                                <strong>创建时间：</strong>
                                {GetDate(this.props.data.createTime)}
                            </div>
                            <div className="update">
                                <strong>最新章节：</strong>
                                {this.props.data.newChapter}
                            </div>
                            <div className="update">
                                <strong>最近更新：</strong>
                                {GetDate(this.props.data.updateTime)}
                            </div>
                            <div className="update">
                                <strong>总字数：</strong>
                                {this.props.data.totalWord}
                            </div>
                            <div className="update">
                                <strong>总点击量：</strong>
                                {this.props.data.clickNum}
                            </div>
                            <div className="intro">
                                <strong>简介：</strong>
                                {this.props.data.intro}
                            </div>
                        </div>
                    </div>
                    <Modal
                        width="1200px"
                        visible={this.state.showScroll}
                        onCancel={this.showScrollModal.bind(this)}
                        footer={[
                            <Button key="back" onClick={this.showScrollModal.bind(this)}>返回</Button>
                        ]}
                    >
                        <ScrollAndChapter id={this.props.data.id}></ScrollAndChapter>
                    </Modal>
                </Modal>
            </div>
        )
    }
}