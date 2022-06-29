import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import  TopNav  from "./Item/TopNav";
import "../css/Main.scss"

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
      return (
        <div className="Main" >
          <NavMenu pathname={this.props.pathname}/>
          <div className="content">
            <TopNav></TopNav>
            <div className="children">
              {this.props.children}
            </div>
          </div>
        </div>
        );
    }
}