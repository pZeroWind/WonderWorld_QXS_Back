import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import UserApi from '../api/userApi';
import './NavMenu.css';

export class NavMenu extends Component {
    static displayName = NavMenu.name;
    constructor(props) {
        super(props);
        this.state = {
            //列表数据
            tab: [
                {
                    index: 0,
                    name: "数据分析",
                    url: "/analysis",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/donut-chart-fill2.svg").default,
                    icon_sel: require("../img/icon/donut-chart-fill1.svg").default
                },
                {
                    index: 1,
                    name: "书库管理",
                    url: "/library",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/book-read-fill2.svg").default,
                    icon_sel: require("../img/icon/book-read-fill1.svg").default
                },
                {
                    index: 2,
                    name: "轮播管理",
                    url: "/banner",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/image-fill2.svg").default,
                    icon_sel: require("../img/icon/image-fill1.svg").default
                },
                {
                    index: 3,
                    name: "用户管理",
                    url: "/users",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/user-fill2.svg").default,
                    icon_sel: require("../img/icon/user-fill1.svg").default
                },
                {
                    index: 4,
                    name: "消息中心",
                    url: "/message",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/feedback-fill2.svg").default,
                    icon_sel: require("../img/icon/feedback-fill1.svg").default
                },
                {
                    index: 5,
                    name: "权限管理",
                    url: "/permissions",
                    class: "router",
                    class_sel: "router_seled",
                    icon: require("../img/icon/shield-keyhole-fill2.svg").default,
                    icon_sel: require("../img/icon/shield-keyhole-fill1.svg").default
                }
            ],
            selected: undefined
        }
    }
    componentDidMount() {
        this.selected()
        UserApi.getGrant(res => {
            //获取当前路由index
            let tabs = res
            let reTab = []
            this.state.tab.forEach(i => {
                if (tabs.indexOf(i.name) !== -1) {
                    reTab.push(i)
                }
            })
            this.setState({
                tab: reTab
            })
            this.selected()
        })

    }

    selected() {
        this.state.tab.forEach(i => {
            if (i.url === this.props.pathname) {
                this.setState({
                    selected: i.index
                })
            }
        })
    }

    toggleManage(index) {
        this.setState({
            selected: index
        })
    }

    render() {
        return (
            <div className="NavUI" >
                <ul>
                    <li className="topIcon" >
                        <img width="125" src={require("../img/BraveDragon.png")} alt="logo" />
                        <div> WonderWorld </div>
                    </li>
                    {
                        this.state.tab.map(it => {
                            if (it.index === this.state.selected) {
                                return (
                                    <Link key={it.index}
                                        tag={Link}
                                        to={it.url}
                                        onClick={this.toggleManage.bind(this, it.index)} >
                                        <li className={it.class_sel} >
                                            <img src={it.icon_sel} alt="icon" />
                                            {it.name}
                                        </li>
                                    </Link>)
                            } else {
                                return (
                                    <Link key={it.index}
                                        tag={Link}
                                        to={it.url}
                                        onClick={this.toggleManage.bind(this, it.index)} >
                                        <li className={it.class} >
                                            <img src={it.icon} alt="icon" />
                                            {it.name}
                                        </li>
                                    </Link>)
                            }
                        })
                    }
                </ul>
            </div>
        );
    }
}