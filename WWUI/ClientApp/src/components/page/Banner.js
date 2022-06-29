import React, { Component } from "react"
import { Carousel, Spin, Pagination, Button, Upload, Popconfirm, message, Empty } from "antd"
import "../../css/Page/Banner.scss"
import bookApi from "../../api/bookApi"
import { baseUrl } from "../../api/ApiConfig"
import Book from "../Item/Banner/Book"
import Axios from "../../plugins/Axios"
import { AreaBox } from "../Item/AreaBox"
import SearchBox from "../Item/SearchBox"

export class Banner extends Component{
    constructor(porps) {
        super(porps)
        this.state = {
            banner: null,
            books: {
                data:null
            },
            src: ""
        }
    }

    componentDidMount() {
        this.init()
    }


    //载入
    init() {
        this.setState({
            banner: null,
            books: {
                data:null
            },
            src:""
        }) 
        this.bannerGet()
        this.changePage(1)
    }

    //获取轮播图
    bannerGet() {
        bookApi.banner(res => {
            this.setState({
                banner:res
            })
        })
    }

    //查询书籍
    changePage(page) {
        this.setState({
            books:{data:null}
        })
        bookApi.getList({size:8,page:page,src:this.state.src}, data => {
            this.setState({
                books:data
            })
        })
        
    }

    //搜索
    search() {
        this.changePage(1)
    }

    //确认搜索框内容
    changeSrc(e) {
        this.setState({
            src:e.target.value
        })
    }
    
    //获取最新书籍
    getNewBook() {
        bookApi.getList({
            mode: 2,
            size:3
        }, res => {
            this.setBanner(res)
        })
    }

    //获取热门书籍
    getHotBook() {
        bookApi.getList({
            mode: 1,
            size:3
        }, res => {
            this.setBanner(res)
        })
    }

    //载入轮播图列表
    setBanner(res) {
        let data = []
            for (let i in res.data) {
                data.push({
                    bookId: res.data[i].id,
                    title: res.data[i].title,
                    imgUrl: null,
                    id:i
                })
            }
            this.setState({
                banner:data
            })
    }

    deleteThisBook(id) {
        this.setState({
            banner:this.state.banner.filter(i=>i.bookId !== id)
        })
    }

    //修改书籍是否被选中
    changeBook(id) {
        let bannerIds = this.getBannerIds()
        if (bannerIds.indexOf(id) !== -1) {
            this.setState({
                banner:this.state.banner.filter(i=>i.bookId !== id)
            })
        } else {
            let book = this.state.books.data.filter(i => i.id === id)
            if (book.length > 0) {
                this.state.banner.push({
                    id: this.state.banner.length,
                    bookId: book[0].id,
                    title: book[0].title,
                    imgUrl: null,
                })
                this.setState({
                    banner:this.state.banner
                })
            }
        }
    }

    //获取轮播图的所有已选定书籍id
    getBannerIds() {
        let bannerIds = []
        this.state.banner.forEach(p => {
            bannerIds.push(p.bookId)
        })
        return bannerIds
    }

    //图片上传回调函数
	uploaded(info,id) {
		if (info.file.status === 'done') {
            this.state.banner.forEach(i => {
                if (i.id === id) {
                    i.imgUrl = info.file.response.data[0]
                }
            })
            this.setState({
                banner:this.state.banner
            })
		} else if (info.file.status === 'error') {
			message.error('上传失败');
		}
    }
    
    //保存修改后的轮播图
    saveBanner() {
        if (this.state.banner.filter(i => !i.imgUrl).length > 0) {
            message.warning("还有轮播图未设置图片")
        } else {
            bookApi.changeBanner({
                data:this.state.banner
            }, res => {
                if (typeof(res)!="string") {
                    this.setState({
                        banner:res
                    })
                    message.success("保存成功")
                }
            })
        }
    }

    render() {
        if (this.state.banner === null) {
            return (
                <Spin tip="加载中...">
                    <AreaBox></AreaBox>
                </Spin>
            )
        } else {
            let Books
            if (this.state.books.data !== null) {
                let bannerIds = this.getBannerIds()
                var data
                if (this.state.books.data.length === 0) {
                    data = (
                        <div className="BookList" style={{justifyContent: "center",alignItems: "center"}}>
                            <Empty description={
                                <span>没有找到相关的书籍</span>
                            } />
                        </div>
                    )
                } else {
                    data = (
                        <div className="BooksList">
                            {this.state.books.data.map(p => (
                                <div onClick={this.changeBook.bind(this,p.id)} key={p.id}>
                                    <Book bool={bannerIds.indexOf(p.id)!==-1} data={p}/>
                                </div>
                            ))}
                        </div>
                    )
                }
                Books = (
                    <div className="Books">
                        <SearchBox placeholder="输入文章标题或文章ID搜索" onSearch={this.search.bind(this)} onChange={this.changeSrc.bind(this)} defaultValue={this.state.src}/>
                        {data}
                        <Pagination onChange={this.changePage} current={this.state.books.page} defaultPageSize={this.state.books.size} showSizeChanger={false} total={this.state.books.total}></Pagination>
                    </div>
                )
            } else {
                Books = (
                    <Spin tip="加载中...">
                        <div className="Books">
                            <SearchBox placeholder="输入文章标题或文章ID搜索" onSearch={this.search.bind(this)} onChange={this.changeSrc.bind(this)} defaultValue={this.state.src} />
                            <div className="BookList"></div>
                            <Pagination onChange={this.changePage} current={this.state.books.page} defaultPageSize={this.state.books.size} showSizeChanger={false} total={this.state.books.total}></Pagination>
                        </div>
                    </Spin>
                )
            }
            return (
                <AreaBox className="BannerSet">
                    {Books}
                    <div className="Show">
                        <Carousel autoplay={true} className="Banners">
                            {this.state.banner.map(p => (
                                <div className="BannersItem" key={p.id}>
                                    {p.imgUrl ? <img src={baseUrl + p.imgUrl} alt="轮播图" /> :
                                        <img src={require("../../img/icon/image-add-fill.svg").default} alt="轮播图" />} 
                                </div>
                            ))}
                        </Carousel>
                        <div className="SelectBox">
                            <div className="btns">
                                <Button onClick={this.getHotBook.bind(this)}>选取热门</Button>
                                <Button onClick={this.getNewBook.bind(this)}>选取最新</Button>
                                <Button onClick={this.bannerGet.bind(this)}>取消变更</Button>
                                <Button onClick={this.saveBanner.bind(this)}>保存所选</Button>
                            </div>
                            <div className="list">
                                {this.state.banner.map(p => (                                    
                                    <div className="listBox" key={p.id}>
                                            <Popconfirm
                                                title="是否从轮播图中移除所选"
                                                okText="是"
                                                cancelText="否"
                                                onConfirm={this.deleteThisBook.bind(this,p.bookId) }
                                            >
                                                <div className="listData">
                                                    <p className="id">{p.bookId}</p>
                                                    <p className="title">{p.title}</p>
                                                </div> 
                                            </Popconfirm>
                                        <div>
                                            <Upload {...Axios.upload} showUploadList={false} onChange={(info)=>this.uploaded(info,p.id)}>
                                                {p.imgUrl ? <img src={baseUrl + p.imgUrl} style={{ width: '125px', height: "75px" }} alt="海报" /> :
                                                 <img src={require("../../img/icon/image-add-fill.svg").default} style={{ width: '125px', height: "75px" }} alt="轮播图" />} 
                                            </Upload>
                                        </div>
                                    </div>
                                    
                                ))}
                            </div>
                        </div>
                    </div>
                </AreaBox>
            )
        }
        
    }
}