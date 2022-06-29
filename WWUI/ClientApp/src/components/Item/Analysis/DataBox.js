import React from "react";
import "../../../css/Item/DataBox.scss"

export default class DataBox extends React.Component{

    render() {
        return (
            <div className="DataBox">
                <div className="Name">{this.props.name}</div>
                <div className="Data">{this.props.data}</div>
            </div>
        )
    }
}