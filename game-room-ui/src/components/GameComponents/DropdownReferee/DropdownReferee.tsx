import * as React from 'react';
import './DropdownReferee.css';
export interface IDropdownRefereeProps {
    displayRefereeMenu: boolean;
    choosenReferee(referee:string):void;
}
export default class DropdownReferee extends React.Component<IDropdownRefereeProps, any>
{
    constructor(props: any) {
        super(props);
        this.state = {

        };
    };
    render() {
        return (
            <div>
                {this.props.displayRefereeMenu ? (
                    <div className="dropdownReferee">
                        <ul>
                            <li><button onClick={() => {this.props.choosenReferee("Iacob Calin")}}>Iacob Calin</button></li>
                            <li><button onClick={() => {this.props.choosenReferee("Gratian")}}>Gratian</button></li>
                            <li><button onClick={() => {this.props.choosenReferee("Cosmin")}}>Cosmin</button></li>
                            <li><button onClick={() => {this.props.choosenReferee("Gabi")}}>Gabi</button></li>
                        </ul>

                    </div>
                ) : (null)}
            </div>
        )
    }
}