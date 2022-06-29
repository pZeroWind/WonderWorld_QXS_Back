import React, { Component } from 'react';
import UserApi from '../../api/userApi';
import {message,notification} from "antd"
import "../../css/Login.scss"

export class Login extends Component {
  static displayName = Login.name;

  constructor(props) {
    super(props)
    this.state = {
      account: "",
      password: ""
    }
  }

  //登录
  Login() {
    message.loading("登录中...","login",0)
    UserApi.login(
      {
        account: this.state.account,
        password: this.state.password
      },
      res => {
        message.destroy("login")
        localStorage.setItem("token", res.data)
        localStorage.setItem("account", this.state.account)
        if (res.code === 200) {
          notification.success({
            message: "登录成功",
            placement: "bottomRight"
          })
          this.props.history.push("/analysis")
        }
      }
    )
  }

  ChangeAcconut(e) {
    this.setState({
        account:e.target.value
    })
  }

  ChangePassword(e) {
    this.setState({
        password:e.target.value
    })
  }

  render () {
    return (
      <div className='Login'>
        <div className='background'>
          <img src={require("../../img/LoginBackground.jpg")} alt="背景图" />
        </div>
        <div className='logo'>
          <img src={require("../../img/BraveDragon.png")} alt="icon" />
          <div>Wonder World轻小说</div>
        </div>
        <div className='loginWindow'>
          <div className='bg'></div>
          
          <div className='input'>
            <div>
              <img src={require("../../img/icon/admin-line.svg").default} alt="账号"/>
            </div>
            <input type="text" placeholder='请输入账号' defaultValue={this.state.account} onChange={this.ChangeAcconut.bind(this)}/>
          </div>
          <div className='input'>
            <div>
              <img src={require("../../img/icon/lock-line.svg").default} alt="密码"/>
            </div>
            <input type="password" placeholder='请输入密码' defaultValue={this.state.password} onChange={this.ChangePassword.bind(this)}/>
          </div>
          <div>
            <button className='loginBtn' onClick={this.Login.bind(this)}>进入后台</button>
          </div>
        </div>
      </div>
    );
  }
}
