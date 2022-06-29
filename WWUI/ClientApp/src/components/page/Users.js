import { Button, Image, message, Modal, Pagination, Select, Spin, Table } from "antd";
import React from "react";
import { AreaBox } from "../Item/AreaBox";
import { SelectList } from "../Item/Library/SelectList";
import SearchBox from "../Item/SearchBox";
import "../../css/Page/Users.scss"
import UserApi from "../../api/userApi";
import { baseUrl } from "../../api/ApiConfig";
import { GetDate } from "../../plugins/TimeTool";
import authApi from "../../api/authApi";

export class Users extends React.Component {

    state = {
        col: [
            {
                title: "账号",
                dataIndex: "account"
            },
            {
                title: "头像",
                dataIndex: "imgUrl",
                render: (p) => (
                    <Image src={baseUrl + p} style={{ width: "50px", height: "50px" }}></Image>
                )
            },
            {
                title: "用户名",
                dataIndex: "nickName"
            },
            {
                title: "性别",
                dataIndex: "gender",
                render: (p) => (p ? '男' : '女')
            },
            {
                title: "身份",
                dataIndex: "role"
            },
            {
                title: "邮箱",
                dataIndex: "email"
            },
            {
                title: "联系电话",
                dataIndex: "tel"
            },
            {
                title: "注册时间",
                dataIndex: "registerTime",
                render: (p) => (GetDate(p))
            },
            {
                title: "操作",
                dataIndex: "account",
                render: (p) => (
                    <Button onClick={this.changeRole.bind(this, p)}>修改</Button>
                )
            }
        ],
        data: {
            page: 1,
            total: 1,
            count: 1,
            size: 5,
            type: 1,
            src: ""
        },
        dataList: [],
        select: [
            {
                id: 1,
                name: "超管"
            },
            {
                id: 2,
                name: "管理"
            },
            {
                id: 3,
                name: "作家"
            },
            {
                id: 4,
                name: "用户"
            },
        ],
        loading: true,
        show: false,
        role: "",
        account: ""
    }

    changeRole(e) {
        var role = ""
        this.state.col.forEach(i => {
            if (i.account == e) {
                role = i.role
            }
        })
        this.setState({
            account: e,
            role: role,
        }, () => {
            this.setState({
                show: true
            })
        })

    }

    showOr() {
        this.setState({
            show: !this.state.show
        })
    }

    changPage(page) {
        this.setState({
            data: {
                page: page,
                total: this.state.data.total,
                count: this.state.data.count,
                size: this.state.data.size,
                type: this.state.data.type,
                src: this.state.data.src
            },
            loading: true
        }, () => {
            UserApi.getAll(this.state.data, res => {
                this.setState({
                    data: {
                        page: res.page,
                        total: res.total,
                        count: res.count,
                        size: res.size,
                        type: this.state.data.type,
                        src: this.state.data.src
                    },
                    dataList: res.data
                }, () => {
                    this.setState({
                        loading: false
                    })
                })
            })
        })
    }

    componentDidMount() {
        this.changPage(1)
        authApi.getRole(res => {
            this.setState({
                select: res.data
            })
        })
    }

    changeSel(e) {
        this.setState({
            data: {
                page: this.state.data.page,
                total: this.state.data.total,
                count: this.state.data.count,
                size: this.state.data.size,
                type: e,
                src: this.state.data.src,
            },
            loading: true
        }, () => {
            this.changPage(1)
        })
    }

    changeSrc(value) {
        this.setState({
            data: {
                page: this.state.data.page,
                total: this.state.data.total,
                count: this.state.data.count,
                size: this.state.data.size,
                type: this.state.data.type,
                src: value.target.value
            }

        })
    }

    changeRoleOk() {
        UserApi.changeRole({
            account: this.state.account,
            role: this.state.role
        }, res => {
            if (res.code == 200) {
                message.success("修改成功")
            }
        })
    }

    render() {
        var show
        if (this.state.loading) {
            show = (
                <Spin
                    tip="加载中"
                >
                    <Table columns={this.state.col} dataSource={this.state.dataList} pagination={false}></Table>
                </Spin>
            )
        } else {
            show = (
                <Table columns={this.state.col} dataSource={this.state.dataList} pagination={false}></Table>
            )
        }
        return (
            <AreaBox className="Users">
                <div className="con">
                    <SearchBox placeholder="请输入用户账户搜索对应用户" onChange={this.changeSrc.bind(this)} onSearch={this.changPage.bind(this, 1)} />
                    <SelectList title="筛选" data={this.state.select} value={this.state.data.type} onChange={this.changeSel.bind(this)}></SelectList>
                </div>
                <div style={{ flex: "1" }}>
                    {show}
                </div>
                <div className="pageControl">
                    <Pagination onChange={this.changPage.bind(this)} current={this.state.data.page} defaultPageSize={this.state.data.size} showSizeChanger={false} total={this.state.data.total}></Pagination>
                </div>
                <Modal
                    visible={this.state.show}
                    okText="保存"
                    cancelText="返回"
                    onCancel={this.showOr.bind(this)}
                    onOk={this.changeRoleOk.bind(this)}
                >
                    <div>
                        <Select defaultValue={this.state.role} onChange={this.changeRole.bind(this)}>
                            {
                                this.state.select.map(p => (
                                    <Select.Option key={p.id} value={p.id}>{p.name}</Select.Option>
                                ))
                            }

                        </Select>
                    </div>
                </Modal>
            </AreaBox >
        )
    }
}