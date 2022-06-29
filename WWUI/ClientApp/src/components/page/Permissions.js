import { Button, Checkbox, Input, message, Modal, Radio, Spin, Tabs, Transfer } from 'antd'
import React from 'react'
import { AreaBox } from '../Item/AreaBox'
import "../../css/Page/Pers.scss"
import authApi from '../../api/authApi'

export class Permissions extends React.Component {
    state = {
        role: [],
        auth: [],
        grant: [],
        select: 1,
        load: false,
        showAddRole: false,
        showDeleteRole: false,
        name: "",
        deleteId: 0
    }

    sd() {
        this.setState({
            showDeleteRole: !this.state.showDeleteRole
        })
    }
    sa(e, a) {
        if (e) {
            if (a === "add") {
                this.setState({
                    showAddRole: !this.state.showAddRole
                })
            } else {
                this.sd()
                this.setState({
                    deleteId: e
                })
            }
        }

    }

    componentDidMount() {
        authApi.getAuth(res => {
            this.setState({
                auth: res.data
            })
        })
        this.getRole()
    }
    //获取角色权限
    getGrant(id) {
        this.setState({ load: true })
        authApi.getGrant(id, res => {
            this.setState({
                grant: res.data
            })
            this.setState({ load: false })
        })
    }

    //移除权限
    removeGrant(e) {
        if (this.state.grant.map(p3 => p3.authId).indexOf(e) != -1) {
            this.setState({
                grant: this.state.grant.filter(p => p.authId != e)
            })
        } else {
            this.state.grant.push(...this.state.auth.filter(p => p.id == e).map(p => ({ authId: p.id, role: this.state.select })))
            this.setState({
                grant: this.state.grant
            })
        }
    }
    //修改权限
    change() {
        authApi.changeGrant({
            id: this.state.select,
            items: this.state.grant.map(p => p.authId)
        }, res => {
            if (res.code == 200) {
                this.getGrant(this.state.select)
                message.success("修改成功")
            }
        })
    }

    //添加角色
    addRole() {
        authApi.addRole(this.state.name, res => {
            this.state.role.push(res.data)
            this.setState({
                role: this.state.role,
                showAddRole: false
            })
        })
    }

    getRole() {
        authApi.getRole(res => {
            this.setState({
                role: res.data
            }, () => {
                this.setState({
                    select: this.state.role[0].id
                })
                this.getGrant(this.state.role[0].id)
            })
        })
    }

    removeRole() {
        authApi.deleteRole(this.state.deleteId, res => {
            if (res.code == 200) {
                this.getRole()
                this.setState({
                    showDeleteRole: false
                })
            }
        })
    }

    changeName(e) {
        this.setState({
            name: e.target.value
        })
    }

    render() {
        const onChange = (key) => {
            this.setState({
                select: key
            }, () => {
                this.getGrant(key)
            })
        }
        var show
        if (this.state.load) {
            show = (
                <Spin
                    tip="加载中"
                >
                    <div>
                        {
                            this.state.auth.map(p2 => (
                                <div className='checkLine'>
                                    <div className='c'>
                                        <Checkbox key={p2.id}
                                            checked={this.state.grant.map(p3 => p3.authId).indexOf(p2.id) != -1}
                                        >
                                            {p2.name}
                                        </Checkbox>
                                    </div>
                                    <label>{p2.remark}</label>
                                </div>
                            ))
                        }
                    </div>
                </Spin>
            )
        } else {
            show = (
                <div>
                    {
                        this.state.auth.map(p2 => (
                            <div className='checkLine'>
                                <div className='c'>
                                    <Checkbox key={p2.id}
                                        onChange={this.removeGrant.bind(this, p2.id)}
                                        checked={this.state.grant.map(p3 => p3.authId).indexOf(p2.id) != -1}
                                    >
                                        {p2.name}
                                    </Checkbox>
                                </div>
                                <label>{p2.remark}</label>
                            </div>
                        ))
                    }
                </div>
            )
        }
        return (
            <AreaBox className="Pers">
                <div style={{ flex: "1" }}>
                    <Tabs type='editable-card' defaultActiveKey={this.state.select} onChange={onChange} onEdit={this.sa.bind(this)}>
                        {
                            this.state.role.map(p => (
                                <Tabs.TabPane tab={p.name} key={p.id}>
                                    {show}
                                </Tabs.TabPane>
                            ))
                        }
                    </Tabs>
                </div>
                <Button onClick={this.change.bind(this)}>保存修改</Button>
                <Modal
                    visible={this.state.showAddRole}
                    okText="添加"
                    cancelText="返回"
                    onCancel={this.sa.bind(this, "add", "add")}
                    onOk={this.addRole.bind(this)}
                >
                    <label>角色名</label>
                    <Input onInput={this.changeName.bind(this)}></Input>
                </Modal>
                <Modal
                    visible={this.state.showDeleteRole}
                    okText="是"
                    cancelText="否"
                    onCancel={this.sd.bind(this)}
                    onOk={this.removeRole.bind(this)}
                >
                    <label>是否确定删除</label>
                </Modal>
            </AreaBox>
        )
    }
}