import React from "react";
import "../../css/Item/SearchBox.scss"

export default class SearchBox extends React.Component{
    render() {
        return (
            <div className="Input">
                <input placeholder={this.props.placeholder} defaultValue={this.props.defaultValue} type="text" onInput={this.props.onChange} />
                <button onClick={this.props.onSearch}>搜索</button>
            </div>
        )
    }
} 