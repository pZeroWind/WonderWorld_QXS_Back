import React, { Component } from "react"
import bookApi from "../../api/bookApi"
import { AreaBox } from "../Item/AreaBox"
import { BookItem } from "../Item/Library/BookItem"
import SearchBox from "../Item/SearchBox"
import "../../css/Page/Library.scss"
import { SelectList } from "../Item/Library/SelectList"
import { Spin, Pagination, Button, Menu, Dropdown, Empty, message } from "antd"
import WordManager from "../Item/WordManager"
export class Library extends Component {

    state = {
        showWord: false,
        data: null,
        page: 1,
        total: 0,
        count: 0,
        size: 6,
        mode: 1,
        src: "",
        maxWord: 0,
        minWord: 0,
        wordId: 1,
        srced: "",
        order: [
            {
                id: 1,
                name: "综合排序"
            },
            {
                id: 2,
                name: "最近更新"
            },
            {
                id: 3,
                name: "最新上架"
            },
            {
                id: 4,
                name: "总点击数"
            },
            {
                id: 5,
                name: "总月票数"
            },
            {
                id: 6,
                name: "总刀片数"
            },
            {
                id: 7,
                name: "总字数"
            }
        ],
        isPublish: 2,
        isPublishList: [],
        type: 0,
        typeList: [],
        wordList: [
            {
                id: 1,
                name: "无限制",
                data: {
                    maxWord: 0,
                    minWord: 0
                }
            },
            {
                id: 2,
                name: "30万以下",
                data: {
                    maxWord: 300000,
                    minWord: 0
                }
            },
            {
                id: 3,
                name: "30万-50万",
                data: {
                    maxWord: 500000,
                    minWord: 300000
                }
            },
            {
                id: 4,
                name: "50万-100万",
                data: {
                    maxWord: 1000000,
                    minWord: 500000
                }
            },
            {
                id: 5,
                name: "100万-200万",
                data: {
                    maxWord: 2000000,
                    minWord: 1000000
                }
            },
            {
                id: 6,
                name: "200万以上",
                data: {
                    maxWord: 0,
                    minWord: 2000000
                }
            }
        ]
    }

    componentDidMount() {
        this.getList()
        bookApi.state(res => {
            this.setState({
                isPublishList: res
            })
        })
        bookApi.type(res => {
            res.unshift({
                id: 0,
                name: "全部"
            })
            this.setState({
                typeList: res
            })
        })
    }

    //获取书籍
    getList() {
        this.setState({
            data: null
        }, () => {
            bookApi.getList({
                page: this.state.page,
                size: this.state.size,
                mode: this.state.mode,
                src: this.state.srced,
                isPublish: this.state.isPublish,
                type: this.state.type,
                minWord: this.state.minWord,
                maxWord: this.state.maxWord
            }, res => {
                this.setState(res)
            })
        })
    }

    //搜索
    search() {
        this.setState({
            srced: this.state.src
        }, () => {
            this.getList()
        })
    }

    //保存输入内容
    changeSrc(e) {
        this.setState({
            src: e.target.value
        })
    }

    //保存排序模式
    setMode(value) {
        this.setState({
            mode: value
        }, () => {
            this.getList()
        })
    }

    //保存当前分区
    setType(value) {
        this.setState({
            type: value
        }, () => {
            this.getList()
        })

    }

    //保存当前状态
    setIsPublish(value) {
        this.setState({
            isPublish: value
        }, () => {
            this.getList()
        })

    }

    //保存当前字数区间
    setWord(id) {
        let data = {
            minWord: 0,
            maxWord: 0,
            wordId: id
        }
        for (let p of this.state.wordList) {
            if (p.id === id) {
                data = p.data
                data.wordId = id
                break
            }
        }
        this.setState(data, () => {
            this.getList()
        })
    }

    //翻页
    changePage(page) {
        this.setState({
            page: page
        }, () => {
            this.getList()
        })
    }

    // //封禁
    // banBook(id) {
    //     const hide = message.loading('正在操作..', 0);
    //     bookApi.ban(id, () => {
    //         setTimeout(hide)
    //         this.getList()
    //     })
    // }

    render() {
        const menu = (
            <Menu items={[
                {
                    label: (
                        <div className="ControlItem" onClick={() => { this.setState({ showWord: true }) }}>违禁词管理</div>
                    )
                },
                {
                    label: (
                        <div className="ControlItem">标签管理</div>
                    )
                }
            ]}></Menu>
        )
        var Search = (
            <div className="SearchBox">
                <SearchBox
                    onSearch={this.search.bind(this)}
                    placeholder="输入文章标题或文章ID搜索"
                    onChange={this.changeSrc.bind(this)}
                    defaultValue={this.state.src}
                />
                <Dropdown
                    overlay={menu}
                    trigger={['click']}
                >
                    <Button className="More">更多操作</Button>
                </Dropdown>
            </div>

        )
        var Selects = (
            <div className="SelectBox">
                <SelectList
                    title="选中分区"
                    data={this.state.typeList}
                    value={this.state.type}
                    onChange={this.setType.bind(this)}
                />
                <SelectList
                    title="排序方式"
                    data={this.state.order}
                    value={this.state.mode}
                    onChange={this.setMode.bind(this)}
                />
                <SelectList
                    title="发布状态"
                    data={this.state.isPublishList}
                    value={this.state.isPublish}
                    onChange={this.setIsPublish.bind(this)}
                />
                <SelectList
                    title="字数限制"
                    data={this.state.wordList}
                    value={this.state.wordId}
                    onChange={this.setWord.bind(this)}
                />
            </div>
        )
        if (this.state.typeList.length === 0 || this.state.isPublishList.length === 0 || this.state.data === null) {
            return (
                <Spin tip="载入中">
                    <AreaBox className="LibraryBox">
                        {Search}
                        {Selects}
                    </AreaBox>
                </Spin>
            )
        }
        var data
        if (this.state.data.length === 0) {
            data = (
                <div className="BookList" style={{ justifyContent: "center", alignItems: "center" }}>
                    <Empty description={
                        <span>没有找到相关的书籍</span>
                    } />
                </div>
            )
        } else {
            data = (
                <div className="BookList">
                    {this.state.data.map(p => (
                        <BookItem key={p.id} data={p} />
                    ))}
                </div>
            )
        }
        return (
            <AreaBox className="LibraryBox">
                {Search}
                {Selects}
                {data}
                <div className="pageControl">
                    <Pagination onChange={this.changePage.bind(this)} current={this.state.page} defaultPageSize={this.state.size} showSizeChanger={false} total={this.state.total}></Pagination>
                </div>
                <WordManager show={this.state.showWord} onCancel={() => { this.setState({ showWord: false }) }} />
            </AreaBox>
        )
    }
}