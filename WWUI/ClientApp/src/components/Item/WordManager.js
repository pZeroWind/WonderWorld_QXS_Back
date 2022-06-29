import { Button, Input, Modal, Popconfirm, Tag, message } from "antd";
import React from "react";
import wordApi from "../../api/wordApi";
import "../../css/Item/WordManager.scss"

export default class WordManager extends React.Component{
    state = {
        words: [],
        newWord: "",
        show: false
    }

    componentDidMount() {
        this.getWords()
    }
    
    //打开添加弹窗
    showADD() {
        this.setState({
            show: !this.state.show,
            newWord: ""
        })
    }

    //获取违禁词
    getWords() {
        wordApi.get(res => {
            res.data.forEach(p => {
                p.show = false
            })
            this.setState({
                words:res.data
            })
        })
    }

    //删除违禁词
    deleteWord(value) {
        wordApi.delete(value, () => {
            this.getWords()
            message.success("删除成功")
        })
    }

    //添加违禁词
    addWord() {
        wordApi.add({
            word:this.state.newWord
        }, () => {
            this.getWords()
            message.success("添加成功")
            this.showADD()
        })
    }

    //显示删除弹窗
    showDelete(id) {
        this.state.words.forEach(i => {
            if (i.id === id) {
                i.show = !i.show
            }
        })
        this.setState({
            words:this.state.words
        })
    }

    inputWord(e) {
        this.setState({
            newWord:e.target.value
        })
    }
    
    render() {
        return (
            <Modal
                visible={this.props.show}
                title="违禁词管理"
                footer={[
                    <Button
                        onClick={this.props.onCancel}
                        key="back">
                        关闭
                    </Button>,
                    <Button
                        type="primary"
                        onClick={this.showADD.bind(this)}
                        key="ok">
                        添加
                    </Button>
                ]}
                onCancel={this.props.onCancel}
            >
                <div className="WordList">
                    {this.state.words.map(p => (
                        <Popconfirm
                            visible={p.show}
                            key={p.id}
                            title="是否确认删除"
                            okText="是"
                            cancelText="否"
                            onCancel={this.showDelete.bind(this,p.id)}
                            onConfirm={this.deleteWord.bind(this,p.id)}
                        >
                            <Tag
                                visible={true}
                                closable
                                onClose={this.showDelete.bind(this,p.id)}
                                className="Words"
                            >
                                {p.word}
                            </Tag>
                        </Popconfirm>
                    ))}
                </div>
                <Modal
                    visible={this.state.show}
                    title="添加违禁词"
                    onCancel={this.showADD.bind(this)}
                    onOk={this.addWord.bind(this)}
                    okText="确定"
                    cancelText="返回"
                >
                    <Input placeholder="请输入想添加的违禁词" value={this.state.newWord} onInput={this.inputWord.bind(this)}/>
                </Modal>
            </Modal>
        )
    }
}