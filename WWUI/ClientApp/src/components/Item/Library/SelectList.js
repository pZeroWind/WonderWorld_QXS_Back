import React from "react";
import { Select } from "antd"
import "../../../css/Item/SelectList.scss"
const Option = Select.Option

export class SelectList extends React.Component {

    render() {
        return (
            <div className="SelectList">
                <label>{this.props.title}</label>
                <Select defaultValue={this.props.value} onChange={this.props.onChange} style={{ width: 120 }} bordered={false}>
                    {this.props.data.map(i => (
                        <Option key={i.id} value={i.id} >{i.name}</Option>
                    ))}
                </Select>
            </div>
        )
    }
}