import React from "react";
import "../../css/Item/AreaBox.scss"

export class AreaBox extends React.Component{
    
    render() {
        return (
            <div className={"AreaBox "+this.props.className}>
                {this.props.children}
            </div>
        )
    }
}