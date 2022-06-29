import React from "react";
import { Checkbox } from "antd";
import { baseUrl } from "../../../api/ApiConfig";
import "../../../css/Item/Book.scss"

export default class Book extends React.Component{

    render() {
        return (
            <div className="Book">
                <div className="BookImgBox">
                    <img src={baseUrl + this.props.data.cover} alt={this.props.data.title}/>
                </div>
                <div className="BookData">
                    <div className="BookDataTitle">{this.props.data.title}</div>
                    <div className="BookDataType">
                        <span>{this.props.data.type}</span>
                        <Checkbox checked={this.props.bool}></Checkbox>
                    </div>
                </div>
            </div>
        )  
    }
}