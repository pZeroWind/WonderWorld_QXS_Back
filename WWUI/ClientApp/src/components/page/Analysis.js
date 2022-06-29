import React, { Component } from 'react';
import DataBox from '../Item/Analysis/DataBox';
import analysisApi from '../../api/analysisApi';
import TypeBox from '../Item/Analysis/TypeBox';
import "../../css/Page/Analysis.scss"

export class Analysis extends Component {
  static displayName = Analysis.name;

  constructor(props) {
    super(props)
    this.state = {
      data: {
        allUser: 0,
        allBook: 0,
        shelfBook: 0,
        todayShelf: 0
      }
    }
  }

  componentDidMount() {
    analysisApi.backstage(res => {
      this.setState({
        data: res,
      })
    })
    
  }

  render() {
    return (
      <div>
        <div className='DataView'>
          <DataBox name="用户总数" data={this.state.data.allUser}></DataBox>
          <DataBox name="小说总数" data={this.state.data.allBook}></DataBox>
          <DataBox name="上架总数" data={this.state.data.shelfBook}></DataBox>
          <DataBox name="今日上架" data={this.state.data.todayShelf}></DataBox>
        </div>
        <TypeBox />
      </div>
    );
  }
}
