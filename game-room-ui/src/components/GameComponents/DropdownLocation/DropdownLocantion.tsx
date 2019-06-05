import * as React from 'react';
import './DropdownLocation.css';
export interface IDropdownLocationProps {
    displayLocationMenu: boolean;
    choosenLocation(location:string):void;
}
export default class DropdownLocation extends React.Component<IDropdownLocationProps, any>
{
    constructor(props: any) {
        super(props);
        this.state = {

        };
    };
    render() {
        return (
            <div>
                {this.props.displayLocationMenu ? (
                    <div className="dropdownLocation">
                        <ul>
                            <li><button onClick={() => {this.props.choosenLocation("Bucharest")}}>Bucharest</button></li>
                            <li><button onClick={() => {this.props.choosenLocation("Paris")}}>Paris</button></li>
                            <li><button onClick={() => {this.props.choosenLocation("London")}}>London</button></li>
                            <li><button onClick={() => {this.props.choosenLocation("Berlin")}}>Berlin</button></li>
                            <li><button onClick={() => {this.props.choosenLocation("Roma")}}>Rome</button></li>
                        </ul>

                    </div>
                ) : (null)}
            </div>
        )
    }
}