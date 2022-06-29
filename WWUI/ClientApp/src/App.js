import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Analysis } from './components/page/Analysis';
import { Banner } from './components/page/Banner';
import { Library } from './components/page/Library';
import { Login } from './components/page/Login';
import { withRouter } from "react-router-dom";
import './custom.css';
import { Users } from './components/page/Users';
import { Permission, Permissions } from './components/page/Permissions';
import { Message } from './components/page/message';


class App extends Component {
  static displayName = App.name;
  componentDidMount() {
    let pathname = this.props.location.pathname;
    let token = localStorage.getItem("token")
    if (!(token || token === "")) {
      this.props.history.push("/login")
    } else if (pathname === "/") {
      this.props.history.push("/analysis")
    }
  }
  render() {
    //获取当前路由
    let pathname = this.props.location.pathname;
    //判断进入页面
    if (pathname !== "/" && pathname !== "/login" && pathname !== "/reportCenter") {

      return (
        <Layout pathname={pathname}>
          <Route exact path='/analysis' component={Analysis} />
          <Route path='/library' component={Library} />
          <Route path='/banner' component={Banner} />
          <Route path='/users' component={Users} />
          <Route path="/permissions" component={Permissions} />
          <Route path='/message' component={Message} />
        </Layout>
      );
    } else {
      return (
        <Route exact path="/login" component={Login} />
      )
    }
  }
}

export default withRouter(App)
