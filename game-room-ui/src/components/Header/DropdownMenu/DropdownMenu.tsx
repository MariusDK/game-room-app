import * as React from 'react';
import { Link } from 'react-router-dom';
import './DropdownMenu.css';
export interface IDropdownProps {
    displayMenu: boolean;
}
export default class DropdownMenu extends React.Component<IDropdownProps, any>
{
    constructor(props: any) {
        super(props);
        this.state = {

        };
    };
    render() {
        return (
            <div>
                {this.props.displayMenu ? (
                    <div className="dropDownMenu">
                        <ul>
                            <li><Link to="/createGameSolo">Create PvP Game</Link></li>
                            <li><Link to="/createTeamGame">Create Team Game </Link></li>
                            <li><Link to="/unfinishGames">Unfinished Game List</Link></li>
                            <li><Link to="/finishGames">Finished Game List</Link></li>
                        </ul>

                    </div>
                ) : (null)}
            </div>
        )
    }
}