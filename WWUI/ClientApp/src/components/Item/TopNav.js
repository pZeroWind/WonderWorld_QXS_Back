import { Dropdown, Menu, Modal } from "antd";
import React, { Component } from "react";
import { Link, withRouter } from "react-router-dom";
import { baseUrl } from "../../api/ApiConfig";
import UserApi from "../../api/userApi";
import "../../css/TopNav.scss"


class TopNav extends Component {
    static displayName = TopNav.name;
    constructor(props) {
        super(props)
        this.state = {
            data: {
                nickName: "管理员",
                imgUrl: ""
            },
            vis: false
        }
    }

    componentDidMount() {
        this.GetUserData()
    }

    GetUserData() {
        let account = localStorage.getItem("account");
        UserApi.get(account, res => {
            if (res) {
                res.imgUrl = baseUrl + res.imgUrl
                this.setState({
                    data: res
                })
            }
        })
    }

    logout() {
        localStorage.removeItem("token");
        this.props.history.push("/login")
    }

    showModal() {
        this.setState({
            vis: !this.state.vis
        })
    }

    userList = <Menu items={
        [
            {
                label: <div className="item" onClick={this.showModal.bind(this)}>退出登录</div>
            }
        ]
    }
    />

    render() {
        var img
        if (this.state.data.imgUrl !== "") {
            img = <img alt="null" src={this.state.data.imgUrl} />
        } else {
            img = <img alt="null" src={require("../../img/icon/admin-line.svg").default} />
        }
        return (
            <div className="TopNav">
                {/* <div className="Tab">
                    <Link to="/reportCenter">反馈中心</Link>
                </div> */}
                <Dropdown overlay={this.userList} placement="bottom" className="UserData" arrow>
                    <div className="UserData">
                        {img}
                        <div>{this.state.data.nickName}</div>
                    </div>
                </Dropdown>
                <Modal
                    visible={this.state.vis}
                    title="警告"
                    onOk={this.logout.bind(this)}
                    onCancel={this.showModal.bind(this)}
                    okText="退出"
                    cancelText="返回"
                >
                    <p>是否确认退出？</p>
                </Modal>
            </div>
        )
    }
}

export default withRouter(TopNav)